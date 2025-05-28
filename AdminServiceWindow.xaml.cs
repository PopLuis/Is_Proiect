using MultiTab.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MultiTab
{
    public partial class AdminServiceWindow : Window
    {
        private List<CerereService> cereri = new List<CerereService>();
        private string caleFisier = "C:\\Users\\Pop\\source\\repos\\IS_Project\\cereri_service.json";

        public AdminServiceWindow()
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
                dgCereri.ItemsSource = cereri; // Folosește dgCereri (numele din XAML)
            }
        }

        private void ActualizeazaStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgCereri.SelectedItem is CerereService cerereSelectata) // Folosește dgCereri
            {
                cerereSelectata.Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString(); // Folosește cmbStatus
                File.WriteAllText(caleFisier, JsonSerializer.Serialize(cereri, new JsonSerializerOptions { WriteIndented = true }));
                dgCereri.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Selectează o cerere din listă!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}