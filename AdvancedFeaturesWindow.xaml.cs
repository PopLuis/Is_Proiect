using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using MultiTab.Models;

namespace MultiTab
{
    public partial class AdvancedFeaturesWindow : Window
    {
        private List<Produs> produse = new List<Produs>();
        private string caleFisier = "C:\\Users\\Pop\\source\\repos\\IS_Project\\produse.json";
        private string tipAngajat = "Junior"; // default, dar va fi setat prin constructor

        public AdvancedFeaturesWindow(string tipAngajat)
        {
            InitializeComponent();
            this.tipAngajat = tipAngajat;
            IncarcaProduseInitiale();
            VerificaPermisiuni();
        }

        private void VerificaPermisiuni()
        {
            if (tipAngajat.ToLower() == "junior")
            {
                txtNume.IsEnabled = false;
                txtCategorie.IsEnabled = false;
                txtDescriere.IsEnabled = false;
                txtPret.IsEnabled = false;
                BtnAdaugaProdus.IsEnabled = false;
                BtnStergeProdus.IsEnabled = false;

                MessageBox.Show("Ești angajat Junior. Poți doar vizualiza produsele.", "Acces restricționat", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void IncarcaProduseInitiale()
        {
            try
            {
                if (File.Exists(caleFisier))
                {
                    string json = File.ReadAllText(caleFisier);
                    produse = JsonSerializer.Deserialize<List<Produs>>(json) ?? new List<Produs>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea produselor: " + ex.Message);
                produse = new List<Produs>();
            }

            ActualizeazaLista();
        }

        private void SalveazaProduseInFisier()
        {
            try
            {
                string json = JsonSerializer.Serialize(produse, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(caleFisier, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvarea produselor: " + ex.Message);
            }
        }

        private void ActualizeazaLista()
        {
            ListaProduse.ItemsSource = null;
            ListaProduse.ItemsSource = produse;
        }

        private void AdaugaProdus_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtCategorie.Text) || string.IsNullOrWhiteSpace(txtPret.Text))
            {
                MessageBox.Show("Completează toate câmpurile obligatorii!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPret.Text, out decimal pret))
            {
                MessageBox.Show("Prețul trebuie să fie un număr valid!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Produs produsNou = new Produs
            {
                Nume = txtNume.Text.Trim(),
                Categorie = txtCategorie.Text.Trim(),
                Descriere = txtDescriere.Text.Trim(),
                Pret = pret
            };

            produse.Add(produsNou);
            SalveazaProduseInFisier();
            ActualizeazaLista();
            GolesteCampuri();

            MessageBox.Show($"Produsul \"{produsNou.Nume}\" a fost adăugat cu succes.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GolesteCampuri()
        {
            txtNume.Text = "";
            txtCategorie.Text = "";
            txtDescriere.Text = "";
            txtPret.Text = "";
        }

        private void StergeProdus_Click(object sender, RoutedEventArgs e)
        {
            if (ListaProduse.SelectedItem == null)
            {
                MessageBox.Show("Selectează un produs din listă pentru a-l șterge.", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ListaProduse.SelectedItem is Produs produsSelectat)
            {
                var result = MessageBox.Show($"Sigur vrei să ștergi produsul \"{produsSelectat.Nume}\"?", "Confirmare", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    produse.Remove(produsSelectat);
                    SalveazaProduseInFisier();
                    ActualizeazaLista();
                    MessageBox.Show("Produsul a fost șters.", "Informație", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
