using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using MultiTab.Models;

namespace MultiTab.Tests
{
    [TestClass]
    public class LoginServiceTests
    {
        private List<Angajat> angajati;
        private List<Utilizator> utilizatori;

        [TestInitialize]
        public void Setup()
        {
            // Set de date de test pentru angajați
            angajati = new List<Angajat>
            {
                new Angajat("managerTest", "parola123", "Manager", "manager@test.com"),
                new Angajat("juniorTest", "pass456", "Junior", "junior@test.com")
            };

            // Set de date de test pentru utilizatori
            utilizatori = new List<Utilizator>
            {
                new Utilizator { Username = "admin", Password = "123", Rol = "admin" },
                new Utilizator { Username = "user", Password = "abc", Rol = "user" }
            };
        }

        [TestMethod]
        public void AutentificareManager_CuDateCorecte_ReturneazaManager()
        {
            var rezultat = angajati.FirstOrDefault(a => a.NumeUtilizator == "managerTest" && a.Parola == "parola123" && a.TipAngajat == "Manager");

            Assert.IsNotNull(rezultat);
            Assert.AreEqual("managerTest", rezultat.NumeUtilizator);
        }

        [TestMethod]
        public void AutentificareAngajat_CuDateCorecte_ReturneazaJunior()
        {
            var rezultat = angajati.FirstOrDefault(a => a.NumeUtilizator == "juniorTest" && a.Parola == "pass456");

            Assert.IsNotNull(rezultat);
            Assert.AreEqual("Junior", rezultat.TipAngajat);
        }

        [TestMethod]
        public void AutentificareManager_CredentialeGresite_ReturneazaNull()
        {
            var rezultat = angajati.FirstOrDefault(a => a.NumeUtilizator == "fake" && a.Parola == "wrong");

            Assert.IsNull(rezultat);
        }

        [TestMethod]
        public void DeserializareUtilizatori_Corect_ReturneazaAdmin()
        {
            string json = JsonConvert.SerializeObject(utilizatori);
            var lista = JsonConvert.DeserializeObject<List<Utilizator>>(json);

            var rezultat = lista.FirstOrDefault(u => u.Username == "admin" && u.Password == "123");

            Assert.IsNotNull(rezultat);
            Assert.AreEqual("admin", rezultat.Rol);
        }

        [TestMethod]
        public void DeserializareUtilizatori_UserInexistent_ReturneazaNull()
        {
            string json = JsonConvert.SerializeObject(utilizatori);
            var lista = JsonConvert.DeserializeObject<List<Utilizator>>(json);

            var rezultat = lista.FirstOrDefault(u => u.Username == "ghost" && u.Password == "000");

            Assert.IsNull(rezultat);
        }
    }
}
