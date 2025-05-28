using System.Linq; // Adaugă acest using
using System.Windows;
using MultiTab.Data;
using MultiTab.Models;
using System.Collections.Generic;
using System;
using System.Windows.Controls;

namespace MultiTab
{
    public partial class ManagerWindow : Window
    {
        private DataManager _dataManager;

        public ManagerWindow(DataManager dataManager)
        {
            InitializeComponent();
            _dataManager = dataManager;
            IncarcaAngajati();
            IncarcaPromotii();
            // Setăm implicit tipul de angajat la "Junior" sau "Senior"
            cmbTipAngajat.SelectedIndex = 0; // Selectează "Junior" by default
        }

        private void IncarcaAngajati()
        {
            lvAngajati.ItemsSource = null; // Golește lista curentă
            lvAngajati.ItemsSource = _dataManager.Angajati.Where(a => a.TipAngajat != "Manager").ToList(); // Afiseaza doar juniori/seniori
        }

        private void IncarcaPromotii()
        {
            lvPromotii.ItemsSource = null; // Golește lista curentă

       
            var promotiiPentruAfisare = _dataManager.Promotii.Select(p => new
            {
                p.NumePromotie,
                p.Descriere,
                ArticoleIncluseString = string.Join(", ", p.ArticoleIncluse), // Convertește lista în string
                p.DataStart,
                p.DataSfarsit,
                p.DiscountProcent
            }).ToList();

            lvPromotii.ItemsSource = promotiiPentruAfisare;
        }


        private void BtnAdaugaAngajat_Click(object sender, RoutedEventArgs e)
        {
            string numeUtilizator = txtNumeUtilizatorAngajat.Text.Trim();
            string parola = pwdParolaAngajat.Password.Trim();
            string tipAngajat = (cmbTipAngajat.SelectedItem as ComboBoxItem)?.Content.ToString();
            string email = txtEmailAngajat.Text.Trim();

            if (string.IsNullOrWhiteSpace(numeUtilizator) || string.IsNullOrWhiteSpace(parola) || string.IsNullOrWhiteSpace(tipAngajat) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Completează toate câmpurile pentru angajat!");
                return;
            }

            Angajat angajatNou = new Angajat(numeUtilizator, parola, tipAngajat, email);

            if (_dataManager.AdaugaAngajat(angajatNou))
            {
                MessageBox.Show("Angajat adăugat cu succes!");
                IncarcaAngajati();
                txtNumeUtilizatorAngajat.Clear();
                pwdParolaAngajat.Clear();
                txtEmailAngajat.Clear();
                cmbTipAngajat.SelectedIndex = 0; // Resetează selectia
            }
            else
            {
                MessageBox.Show("Numele de utilizator al angajatului există deja!");
            }
        }

        private void BtnStergeAngajat_Click(object sender, RoutedEventArgs e)
        {
            string numeUtilizator = txtNumeUtilizatorAngajatStergere.Text.Trim();

            if (string.IsNullOrWhiteSpace(numeUtilizator))
            {
                MessageBox.Show("Introdu numele de utilizator al angajatului de șters!");
                return;
            }

            if (_dataManager.StergeAngajat(numeUtilizator))
            {
                MessageBox.Show("Angajat șters cu succes!");
                IncarcaAngajati();
                txtNumeUtilizatorAngajatStergere.Clear();
            }
            else
            {
                MessageBox.Show("Angajatul nu a fost găsit!");
            }
        }

        private void BtnReimprospataAngajati_Click(object sender, RoutedEventArgs e)
        {
            IncarcaAngajati();
        }

        private void BtnAdaugaPromotie_Click(object sender, RoutedEventArgs e)
        {
            string numePromotie = txtNumePromotie.Text.Trim();
            string descriere = txtDescrierePromotie.Text.Trim();
            List<string> articoleIncluse = txtArticolePromotie.Text.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
            DateTime? dataStart = dpDataStartPromotie.SelectedDate;
            DateTime? dataSfarsit = dpDataSfarsitPromotie.SelectedDate;

            if (string.IsNullOrWhiteSpace(numePromotie) || string.IsNullOrWhiteSpace(descriere) || articoleIncluse.Count == 0 || dataStart == null || dataSfarsit == null)
            {
                MessageBox.Show("Completează toate câmpurile pentru promoție!");
                return;
            }

            if (dataStart > dataSfarsit)
            {
                MessageBox.Show("Data de start nu poate fi după data de sfârșit!");
                return;
            }

            // Conform cerintei, discount-ul este de 10%
            Promotie promotieNoua = new Promotie(numePromotie, descriere, articoleIncluse, dataStart.Value, dataSfarsit.Value, 10.0);

            if (_dataManager.AdaugaPromotie(promotieNoua))
            {
                MessageBox.Show("Promoție adăugată cu succes!");
                IncarcaPromotii();
                txtNumePromotie.Clear();
                txtDescrierePromotie.Clear();
                txtArticolePromotie.Clear();
                dpDataStartPromotie.SelectedDate = null;
                dpDataSfarsitPromotie.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Numele promoției există deja!");
            }
        }

        private void BtnStergePromotie_Click(object sender, RoutedEventArgs e)
        {
            string numePromotie = txtNumePromotieStergere.Text.Trim();

            if (string.IsNullOrWhiteSpace(numePromotie))
            {
                MessageBox.Show("Introdu numele promoției de șters!");
                return;
            }

            if (_dataManager.StergePromotie(numePromotie))
            {
                MessageBox.Show("Promoție ștearsă cu succes!");
                IncarcaPromotii();
                txtNumePromotieStergere.Clear();
            }
            else
            {
                MessageBox.Show("Promoția nu a fost găsită!");
            }
        }

        private void BtnReimprospataPromotii_Click(object sender, RoutedEventArgs e)
        {
            IncarcaPromotii();
        }
    }
}