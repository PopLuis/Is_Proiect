using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using MultiTab.Models;

namespace MultiTab
{
    public partial class ComenzileMeleWindow : Window
    {
        private readonly string username;
        private readonly string caleComenzi = "C:\\Users\\Pop\\source\\repos\\IS_Project\\comenzi.json";
        private readonly string caleService = "C:\\Users\\Pop\\source\\repos\\IS_Project\\cereri_service.json";

        public ComenzileMeleWindow(string utilizatorUsername)
        {
            InitializeComponent();
            username = utilizatorUsername;
            IncarcaComenziSiService();
        }

        private void IncarcaComenziSiService()
        {
            // COMENZI
            if (File.Exists(caleComenzi))
            {
                try
                {
                    string jsonComenzi = File.ReadAllText(caleComenzi);
                    var toateComenzile = JsonSerializer.Deserialize<List<Comanda>>(jsonComenzi) ?? new List<Comanda>();

                    var comenziUser = toateComenzile
                        .Where(c => c.Username == username)
                        .SelectMany(c => c.Produse.Select(p => new
                        {
                            NumeProdus = p.Nume,
                            DataComenzii = DateTime.Now, // Dacă ai data în modelul tău, folosește-o aici
                            Status = c.Status
                        }))
                        .ToList();

                    GridComenzi.ItemsSource = comenziUser;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la citirea comenzilor: " + ex.Message);
                }
            }

            // CERERI SERVICE
            if (File.Exists(caleService))
            {
                try
                {
                    string jsonService = File.ReadAllText(caleService);
                    var toateServiciile = JsonSerializer.Deserialize<List<CerereService>>(jsonService) ?? new List<CerereService>();

                    var serviceUser = toateServiciile.Where(s => s.NumeClient == username).ToList();

                    ListaService.ItemsSource = serviceUser;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la citirea cererilor de service: " + ex.Message);
                }
            }
        }
    }
}
