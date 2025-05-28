using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace MultiTab
{
    public partial class CreareCont : Window
    {
        private const string FilePath = "C:\\Users\\Pop\\source\\repos\\IS_Project\\ulizatori.json";

        public CreareCont()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string parola = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(parola))
            {
                MessageBox.Show("Completează toate câmpurile!");
                return;
            }

            Utilizator utilizatorNou = new Utilizator
            {
                Username = username,
                Password = parola
            };

            List<Utilizator> utilizatori = new List<Utilizator>();

            if (File.Exists("C:\\Users\\Florian\\Desktop\\MultiTab-master\\utilizatori.json"))
            {
                string json = File.ReadAllText("C:\\Users\\Florian\\Desktop\\MultiTab-master\\utilizatori.json");
                if (!string.IsNullOrWhiteSpace(json))
                {
                    utilizatori = System.Text.Json.JsonSerializer.Deserialize<List<Utilizator>>(json) ?? new List<Utilizator>();
                }
            }

            // Verificăm dacă username-ul există deja
            if (utilizatori.Exists(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Acest username există deja!");
                return;
            }

            utilizatori.Add(utilizatorNou);

            string jsonNou = System.Text.Json.JsonSerializer.Serialize(utilizatori, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonNou);

            MessageBox.Show("Cont creat cu succes!");
            this.Close();
        }
    }
}
