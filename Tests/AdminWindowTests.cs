using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MultiTab;

namespace MultiTab.Tests
{
    [TestClass]
    public class AdminWindowTests
    {
        private readonly string testFile = "produse_test.json";

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(testFile))
                File.Delete(testFile);
        }

        [TestMethod]
        public void SalvareProdus_CreeazaFisier_VerificaContinut()
        {
            // Arrange
            var produse = new List<Produs>
            {
                new Produs { Nume = "Laptop", Pret = 3000, Descriere = "Gaming", Categorie = "Electronice" }
            };

            // Act
            var json = JsonSerializer.Serialize(produse, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(testFile, json);

            // Assert
            Assert.IsTrue(File.Exists(testFile));
            string continut = File.ReadAllText(testFile);
            Assert.IsTrue(continut.Contains("Laptop"));
            Assert.IsTrue(continut.Contains("Gaming"));
        }

        [TestMethod]
        public void IncarcareProduse_DinFisier_CorectDeserializat()
        {
            // Arrange
            var produse = new List<Produs>
            {
                new Produs { Nume = "Monitor", Pret = 1200, Descriere = "Full HD", Categorie = "Electronice" }
            };
            File.WriteAllText(testFile, JsonSerializer.Serialize(produse));

            // Act
            var json = File.ReadAllText(testFile);
            var lista = JsonSerializer.Deserialize<List<Produs>>(json);

            // Assert
            Assert.IsNotNull(lista);
            Assert.AreEqual(1, lista.Count);
            Assert.AreEqual("Monitor", lista[0].Nume);
            Assert.AreEqual(1200, lista[0].Pret);
        }

        [TestMethod]
        public void AdaugareProdus_CorectInitializat()
        {
            // Act
            var produs = new Produs
            {
                Nume = "Mouse",
                Pret = 150,
                Descriere = "Wireless",
                Categorie = "Accesorii"
            };

            // Assert
            Assert.AreEqual("Mouse", produs.Nume);
            Assert.AreEqual(150, produs.Pret);
            Assert.AreEqual("Wireless", produs.Descriere);
            Assert.AreEqual("Accesorii", produs.Categorie);
        }

        [TestMethod]
        public void PretInvalid_DetecteazaTextCaInvalid()
        {
            // Act
            bool rezultat = decimal.TryParse("abc123", out _);

            // Assert
            Assert.IsFalse(rezultat);
        }

        [TestMethod]
        public void SalvareFisier_NuAruncaExceptie_PeCaleValida()
        {
            // Arrange
            var produse = new List<Produs>
            {
                new Produs { Nume = "Tastatura", Pret = 100, Descriere = "Mecanica", Categorie = "Accesorii" }
            };

            // Act & Assert
            try
            {
                var json = JsonSerializer.Serialize(produse);
                File.WriteAllText(testFile, json);
                Assert.IsTrue(File.Exists(testFile));
            }
            catch
            {
                Assert.Fail("Nu trebuia să apară excepție la salvare pe cale validă.");
            }
        }
    }
}
