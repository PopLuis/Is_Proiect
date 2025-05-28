using MultiTab.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MultiTab
{
    public partial class StatusServiceWindow : Window
    {
        private string caleFisier = "C:\\Users\\Pop\\source\\repos\\IS_Project\\cereri_service.json";
        private List<CerereService> cereri = new List<CerereService>();

        public StatusServiceWindow()
        {
            InitializeComponent();
            IncarcaCereri();
        }

        private void IncarcaCereri()
        {
            if (File.Exists(caleFisier))
            {
                string json = File.ReadAllText(caleFisier);
                cereri = JsonSerializer.Deserialize<List<CerereService>>(json) ?? new List<CerereService>();
                CereriGrid.ItemsSource = cereri;
            }
        }

        private void ActualizeazaStatus_Click(object sender, RoutedEventArgs e)
        {
            if (CereriGrid.SelectedItem is CerereService cerere)
            {
                string statusNou = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(statusNou))
                {
                    cerere.Status = statusNou;
                    File.WriteAllText(caleFisier, JsonSerializer.Serialize(cereri, new JsonSerializerOptions { WriteIndented = true }));
                    CereriGrid.Items.Refresh();
                    MessageBox.Show("Status actualizat!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selectează o cerere!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
