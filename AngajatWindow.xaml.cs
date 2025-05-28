using MultiTab.Data;
using MultiTab.Models;
using System;
using System.Windows;

namespace MultiTab
{
    /// <summary>
    /// Interaction logic for AngajatWindow.xaml
    /// </summary>
    public partial class AngajatWindow : Window
    {
        private readonly DataManager _dataManager;
        private readonly Angajat _angajatCurent;

        public AngajatWindow(DataManager dataManager, Angajat angajat)
        {
            InitializeComponent();
            _dataManager = dataManager;
            _angajatCurent = angajat;

            // Dezactivează butonul de adăugare produs pentru juniori
            if (!_angajatCurent.TipAngajat.Equals("Senior", StringComparison.OrdinalIgnoreCase))
            {
                AdaugaProdusButton.IsEnabled = false;
            }
        }

        private void AdaugaProdusButton_Click(object sender, RoutedEventArgs e)
        {
            // Verificare redundantă, butonul e deja dezactivat pentru juniori
            if (!_angajatCurent.TipAngajat.Equals("Senior", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    "Doar angajații Senior pot adăuga produse/piese.",
                    "Acces interzis",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Deschide fereastra de adăugare produs/piesă
            var addWin = new AdvancedFeaturesWindow(_angajatCurent.TipAngajat);
            addWin.Owner = this;
            addWin.ShowDialog();
        }

        private void SchimbaStatusButton_Click(object sender, RoutedEventArgs e)
        {
            // Aici deschizi fereastra de modificare status comenză/service
            // sau înserezi logica direct
            MessageBox.Show("Deschide fereastra de modificare status...");
        }
        private void VizualizareComenziButton_Click(object sender, RoutedEventArgs e)
        {
            var statusWin = new StatusServiceWindow();
            statusWin.Owner = this;
            statusWin.ShowDialog();
        }
        private void VizualizareComenziServiceButton_Click(object sender, RoutedEventArgs e)
        {
            // Deschide fereastra de gestionare a cererilor de service
            var statusWin = new StatusServiceWindow();
            statusWin.Owner = this;
            statusWin.ShowDialog();
        }

        private void VizualizareComenziNormaleButton_Click(object sender, RoutedEventArgs e)
        {
            // Deschide fereastra pentru gestionarea comenzilor de produse
            var comenziProduseWin = new StatusComenziProduseWindow();
            comenziProduseWin.Owner = this;
            comenziProduseWin.ShowDialog();
        }

    }
}