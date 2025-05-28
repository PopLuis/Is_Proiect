using MultiTab.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json; // Schimbăm din System.Text.Json în Newtonsoft.Json

namespace MultiTab
{
    public partial class ServiceWindow : Window
    {
        private const string CaleFisier = "C:\\Users\\Pop\\source\\repos\\IS_Project\\cereri_service.json";

        public ServiceWindow()
        {
            InitializeComponent();
            dpData.SelectedDate = DateTime.Now.AddDays(1);
        }

        private void TrimiteCererea_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validare câmpuri
                if (string.IsNullOrWhiteSpace(txtNume.Text) ||
                    string.IsNullOrWhiteSpace(txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtProblema.Text))
                {
                    MessageBox.Show("Completează toate câmpurile!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Citim cererile existente
                List<CerereService> cereri = File.Exists(CaleFisier) ?
                    JsonConvert.DeserializeObject<List<CerereService>>(File.ReadAllText(CaleFisier)) :
                    new List<CerereService>();

                // Adăugăm cererea nouă
                cereri.Add(new CerereService
                {
                    NumeClient = txtNume.Text,
                    Email = txtEmail.Text,
                    DescriereProblema = txtProblema.Text,
                    DataCerere = DateTime.Now,
                    DataProgramare = dpData.SelectedDate ?? DateTime.Now.AddDays(1),
                    Status = "În așteptare"
                });

                // Salvăm
                File.WriteAllText(CaleFisier, JsonConvert.SerializeObject(cereri, Formatting.Indented));

                MessageBox.Show("Cererea a fost trimisă cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}