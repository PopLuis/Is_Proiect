using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MultiTab
{
    public partial class AdminWindow : Window
    {
        private List<Produs> produse = new List<Produs>();
        private readonly string caleFisier = "C:\\Users\\Pop\\source\\repos\\IS_Project\\produse.json";

        public AdminWindow()
        {
            InitializeComponent();
            IncarcaProduseDinFisier();
        }

        private void AdaugaProdus_Click(object sender, RoutedEventArgs e)
        {
            string nume = NumeTextBox.Text;
            string descriere = DescriereTextBox.Text;
            string categorie = (CategorieComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (decimal.TryParse(PretTextBox.Text, out decimal pret))
            {
                Produs p = new Produs
                {
                    Nume = nume,
                    Pret = pret,
                    Descriere = descriere,
                    Categorie = categorie
                };

                produse.Add(p);
                ListaProduse.Items.Add($"{p.Nume} - {p.Pret} RON - {p.Categorie}");

                NumeTextBox.Clear();
                PretTextBox.Clear();
                DescriereTextBox.Clear();
                CategorieComboBox.SelectedIndex = 0;

                SalveazaProduseInFisier();
            }
            else
            {
                MessageBox.Show("Prețul introdus nu este valid!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalveazaProduseInFisier()
        {
            try
            {
                var json = JsonSerializer.Serialize(produse, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(caleFisier, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvarea produselor: " + ex.Message);
            }
        }

        private void IncarcaProduseDinFisier()
        {
            try
            {
                if (File.Exists(caleFisier))
                {
                    string json = File.ReadAllText(caleFisier);
                    produse = JsonSerializer.Deserialize<List<Produs>>(json) ?? new List<Produs>();

                    foreach (var p in produse)
                    {
                        ListaProduse.Items.Add($"{p.Nume} - {p.Pret} RON - {p.Categorie}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea produselor: " + ex.Message);
            }
        }
    }
}