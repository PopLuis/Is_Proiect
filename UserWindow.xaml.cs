using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using MultiTab.Data;
using MultiTab.Models;

namespace MultiTab
{
    public partial class UserWindow : Window
    {
        private List<Produs> produse = new List<Produs>();
        private List<Promotie> promotii = new List<Promotie>();
        private List<Produs> cosCumparaturi = new List<Produs>();
        private DataManager dataManager = new DataManager();
        private Utilizator utilizator;

        public UserWindow(Utilizator utilizatorConectat)
        {
            InitializeComponent();
            utilizator = utilizatorConectat;
            IncarcaProduse();
            promotii = dataManager.Promotii ?? new List<Promotie>();
        }

        private void IncarcaProduse()
        {
            try
            {
                string caleFisier = "C:\\Users\\Florian\\Desktop\\MultiTab-master\\produse.json";
                if (File.Exists(caleFisier))
                {
                    string json = File.ReadAllText(caleFisier);
                    produse = JsonSerializer.Deserialize<List<Produs>>(json);
                    ListaProduseUser.ItemsSource = produse;
                }
                else
                {
                    MessageBox.Show("Fișierul produse.json nu a fost găsit.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea catalogului: " + ex.Message);
            }
        }

        private void Filtru_Click(object sender, RoutedEventArgs e)
        {
            string categorie = (sender as Button)?.Tag.ToString();

            if (categorie == "toate")
            {
                ListaProduseUser.ItemsSource = produse;
            }
            else if (categorie == "promotii")
            {
                DateTime acum = DateTime.Now;
                var promotiiActive = promotii.Where(p => acum >= p.DataStart && acum <= p.DataSfarsit).ToList();
                var produsePromo = new List<Produs>();

                foreach (var promotie in promotiiActive)
                {
                    foreach (var numeProdus in promotie.ArticoleIncluse)
                    {
                        var produsOriginal = produse.FirstOrDefault(p => p.Nume == numeProdus);
                        if (produsOriginal != null)
                        {
                            var produsRedus = new Produs
                            {
                                Nume = produsOriginal.Nume,
                                Categorie = produsOriginal.Categorie,
                                Descriere = produsOriginal.Descriere,
                                Pret = Math.Round(produsOriginal.Pret * (1 - (decimal)(promotie.DiscountProcent / 100)), 2)
                            };
                            produsePromo.Add(produsRedus);
                        }
                    }
                }

                if (produsePromo.Count == 0)
                {
                    MessageBox.Show("Nu există promoții active în acest moment.");
                }

                ListaProduseUser.ItemsSource = produsePromo;
            }
            else
            {
                var filtrate = produse.Where(p => p.Categorie?.ToLower() == categorie.ToLower()).ToList();
                ListaProduseUser.ItemsSource = filtrate;
            }
        }

        private void AdaugaInCos_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var produs = button?.DataContext as Produs;
            if (produs != null)
            {
                cosCumparaturi.Add(produs);
                MessageBox.Show($"Produsul \"{produs.Nume}\" a fost adăugat în coș.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void VeziCos_Click(object sender, RoutedEventArgs e)
        {
            CosWindow cosWindow = new CosWindow(cosCumparaturi, utilizator);
            cosWindow.ShowDialog();
        }

        private void DeschideService_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow fereastraService = new ServiceWindow();
            fereastraService.ShowDialog();
        }
        private void ComenzileMele_Click(object sender, RoutedEventArgs e)
        {
            ComenzileMeleWindow fereastra = new ComenzileMeleWindow(utilizator.Username);
            fereastra.ShowDialog();
        }
    }
}
