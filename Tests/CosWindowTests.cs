using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MultiTab.Models;

namespace MultiTab.Tests.Tests
{
    [TestClass]
    public class CosWindowTests
    {
        private List<Produs> produse;
        private Utilizator utilizator;

        [TestInitialize]
        public void Setup()
        {
            utilizator = new Utilizator { Username = "luis" };
            produse = new List<Produs>
            {
                new Produs { Nume = "Mouse", Pret = 100, Cantitate = 1, Categorie = "periferice" },
                new Produs { Nume = "Laptop", Pret = 3000, Cantitate = 1, Categorie = "laptop" }
            };
        }

        [TestMethod]
        public void TotalFaraTaxaDacaNuExistaPreasamblate()
        {
            var produseFaraPreasamblate = new List<Produs>
            {
                new Produs { Nume = "Mouse", Pret = 50, Cantitate = 2, Categorie = "accesorii" }
            };

            decimal total = produseFaraPreasamblate.Sum(p => p.Pret * p.Cantitate);
            Assert.AreEqual(100, total);
        }

        [TestMethod]
        public void TotalCuTaxaCandExistaPreasamblate()
        {
            decimal total = produse.Sum(p => p.Pret * p.Cantitate);
            bool continePreasamblate = produse.Any(p => new[] { "laptop", "desktop", "imprimanta", "periferice" }
                                                        .Contains(p.Categorie.ToLower()));
            if (continePreasamblate)
                total += 100;

            Assert.AreEqual(3200, total);
        }

        [TestMethod]
        public void VerificaExistentaPreasamblatelor()
        {
            bool are = produse.Any(p => new[] { "laptop", "desktop", "imprimanta", "periferice" }
                                        .Contains(p.Categorie.ToLower()));
            Assert.IsTrue(are);
        }

        [TestMethod]
        public void VerificaTaxaNuSeAplicaLaCategoriiObisnuite()
        {
            var produseSimple = new List<Produs>
            {
                new Produs { Nume = "Cablu HDMI", Pret = 20, Cantitate = 1, Categorie = "cablu" }
            };

            bool continePreasamblate = produseSimple.Any(p => new[] { "laptop", "desktop", "imprimanta", "periferice" }
                                                              .Contains(p.Categorie.ToLower()));
            Assert.IsFalse(continePreasamblate);
        }

        [TestMethod]
        public void CalculCorectPentruMultipleProduse()
        {
            produse.Add(new Produs { Nume = "Monitor", Pret = 500, Cantitate = 2, Categorie = "periferice" });
            decimal total = produse.Sum(p => p.Pret * p.Cantitate);
            if (produse.Any(p => new[] { "laptop", "desktop", "imprimanta", "periferice" }
                                 .Contains(p.Categorie.ToLower())))
                total += 100;

            Assert.AreEqual(4200, total);
        }
    }
}
