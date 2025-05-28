using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using MultiTab.Models;

namespace MultiTab
{
    public partial class StatusComenziProduseWindow : Window
    {
        private readonly string caleComenzi = "C:\\Users\\Pop\\source\\repos\\IS_Project\\comenzi.json";
        private List<Comanda> toateComenzile;

        public StatusComenziProduseWindow()
        {
            InitializeComponent();
            IncarcaComenzile();
        }

        private void IncarcaComenzile()
        {
            if (!File.Exists(caleComenzi))
            {
                MessageBox.Show("Fișierul comenzilor nu a fost găsit.");
                return;
            }

            try
            {
                string json = File.ReadAllText(caleComenzi);
                toateComenzile = JsonSerializer.Deserialize<List<Comanda>>(json) ?? new List<Comanda>();

                GridComenziProduse.ItemsSource = toateComenzile.Select(c => new ComandaAfisare
                {
                    Username = c.Username,
                    Produse = string.Join(", ", c.Produse.Select(p => p.Nume)),
                    Status = c.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la citirea comenzilor: " + ex.Message);
            }
        }

        private void SalveazaModificari_Click(object sender, RoutedEventArgs e)
        {
            if (GridComenziProduse.SelectedItem is ComandaAfisare selectie)
            {
                string statusNou = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(statusNou))
                {
                    var comanda = toateComenzile.FirstOrDefault(c =>
                        c.Username == selectie.Username &&
                        string.Join(", ", c.Produse.Select(p => p.Nume)) == selectie.Produse);

                    if (comanda != null)
                    {
                        comanda.Status = statusNou;

                        try
                        {
                            File.WriteAllText(caleComenzi, JsonSerializer.Serialize(toateComenzile, new JsonSerializerOptions { WriteIndented = true }));
                            MessageBox.Show("Status actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                            IncarcaComenzile();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Eroare la salvare: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Selectează o comandă din listă!");
            }
        }

        private class ComandaAfisare
        {
            public string Username { get; set; }
            public string Produse { get; set; }
            public string Status { get; set; }
        }
    }
}
