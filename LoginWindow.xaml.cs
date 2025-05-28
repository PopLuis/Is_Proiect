using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using MultiTab.Data;
using MultiTab.Models;
using Newtonsoft.Json;

namespace MultiTab
{
    public partial class LoginWindow : Window
    {
        private readonly DataManager _dataManager;
        private readonly List<Utilizator> _utilizatoriExterni;

        // Constructor aplicație reală
        public LoginWindow() : this(new DataManager(), null) { }

        // Constructor pentru testare
        public LoginWindow(DataManager dataManager, List<Utilizator> utilizatoriTest = null)
        {
            InitializeComponent();
            _dataManager = dataManager;
            _utilizatoriExterni = utilizatoriTest;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                AfiseazaEroare("Completează toate câmpurile!");
                return;
            }

            // Autentificare ca Manager
            var manager = _dataManager.AutentificaManager(username, password);
            if (manager != null)
            {
                AfiseazaSucces($"Autentificare reușită ca Manager: {manager.NumeUtilizator}");
                DeschideFereastraNoua(new ManagerWindow(_dataManager));
                return;
            }

            // Autentificare ca Angajat
            var angajat = _dataManager.AutentificaAngajat(username, password);
            if (angajat != null)
            {
                AfiseazaSucces($"Bun venit, {angajat.TipAngajat} {angajat.NumeUtilizator}!");
                DeschideFereastraNoua(new AngajatWindow(_dataManager, angajat));
                return;
            }

            // Autentificare ca Utilizator
            List<Utilizator> utilizatori = _utilizatoriExterni ?? CitesteUtilizatoriDinFisier();

            var utilizator = utilizatori.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (utilizator != null)
            {
                Window fereastra;
                if (utilizator.Rol == "admin")
                    fereastra = new AdminWindow();
                else
                    fereastra = new UserWindow(utilizator);

                fereastra.Show();
                this.Close();
            }
            else
            {
                AfiseazaEroare("Nume de utilizator sau parolă incorecte!");
            }
        }

        private List<Utilizator> CitesteUtilizatoriDinFisier()
        {
            try
            {
                string json = File.ReadAllText("C:\\Users\\Pop\\source\\repos\\IS_Project\\ulizatori.json");
                return JsonConvert.DeserializeObject<List<Utilizator>>(json);
            }
            catch
            {
                return new List<Utilizator>();
            }
        }

        // Aceste metode pot fi suprascrise în testare
        protected virtual void AfiseazaSucces(string mesaj) =>
            MessageBox.Show(mesaj, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);

        protected virtual void AfiseazaEroare(string mesaj) =>
            MessageBox.Show(mesaj, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);

        protected virtual void DeschideFereastraNoua(Window window)
        {
            window.Show();
            this.Close();
        }
    }
}
