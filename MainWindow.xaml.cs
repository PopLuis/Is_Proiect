using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using MultiTab.Data;
using MultiTab.Models;
using System.Globalization;

namespace MultiTab
{
    public partial class MainWindow : Window
    {
        private List<Produs> produse = new List<Produs>();
        private List<Promotie> promotii = new List<Promotie>();
        private DataManager dataManager = new DataManager();

        public MainWindow()
        {
            InitializeComponent();
            IncarcaProduseDinFisier();
   
            promotii = dataManager.Promotii ?? new List<Promotie>();
        }

        private void IncarcaProduseDinFisier()
        {
            try
            {
                if (File.Exists("C:\\Users\\Pop\\source\\repos\\IS_Project\\produse.json"))
                {
                    string json = File.ReadAllText("C:\\Users\\Pop\\source\\repos\\IS_Project\\produse.json");
                    produse = JsonSerializer.Deserialize<List<Produs>>(json);
                    AfiseazaProduse(produse);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea produselor: " + ex.Message);
            }
        }

        private void AfiseazaProduse(List<Produs> lista)
        {
            ListaProduseMain.Items.Clear();
            foreach (var p in lista)
                ListaProduseMain.Items.Add($"{p.Nume} ({p.Categorie}) - {p.Pret} RON");
        }

        private void Button_LogIn(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        private void Button_CreareCont(object sender, RoutedEventArgs e)
        {
            CreareCont creareCont = new CreareCont();
            creareCont.ShowDialog();
        }

        private void Filtru_Click(object sender, RoutedEventArgs e)
        {
            string categorie = (sender as Button).Tag.ToString();

            if (categorie == "toate")
            {
                AfiseazaProduse(produse);
            }
            else if (categorie == "promotii")
            {
                DateTime acum = DateTime.Now;
                var promotiiActive = promotii.Where(p => acum >= p.DataStart && acum <= p.DataSfarsit).ToList();
                var produsePromo = new List<string>();

                foreach (var promotie in promotiiActive)
                {
                    foreach (var numeProdus in promotie.ArticoleIncluse)
                    {
                        var produsOriginal = produse.FirstOrDefault(p => p.Nume == numeProdus);
                        if (produsOriginal != null)
                        {
                            decimal pretRedus = Math.Round(produsOriginal.Pret * (1 - (decimal)(promotie.DiscountProcent / 100)), 2);
                            string textProdus = $"{produsOriginal.Nume} ({produsOriginal.Categorie}) - {pretRedus.ToString("0.00", CultureInfo.InvariantCulture)} RON (redus din {produsOriginal.Pret.ToString("0.00", CultureInfo.InvariantCulture)} RON)";
                            produsePromo.Add(textProdus);
                        }
                    }
                }

                ListaProduseMain.Items.Clear();
                if (produsePromo.Count == 0)
                {
                    ListaProduseMain.Items.Add("Nu există promoții active în acest moment.");
                }
                else
                {
                    foreach (var item in produsePromo)
                    {
                        ListaProduseMain.Items.Add(item);
                    }
                }
            }
            else
            {
                var filtrate = produse.Where(p => p.Categorie.ToLower() == categorie.ToLower()).ToList();
                AfiseazaProduse(filtrate);
            }
        }
    }
}
