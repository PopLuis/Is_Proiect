using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MultiTab.Models;

namespace MultiTab.Tests.Tests
{
    [TestClass]
    public class ComenzileMeleTests
    {
        private List<Comanda> toateComenzile;
        private List<CerereService> toateCererile;

        [TestInitialize]
        public void Init()
        {
            toateComenzile = new List<Comanda>
            {
                new Comanda { Username = "florian", Status = "In asteptare", Produse = new List<Produs> {
                    new Produs { Nume = "Laptop" } } },
                new Comanda { Username = "altuser", Status = "Trimisa", Produse = new List<Produs> {
                    new Produs { Nume = "Mouse" } } }
            };

            toateCererile = new List<CerereService>
            {
                new CerereService { NumeClient = "florian", Problema = "Nu pornește" },
                new CerereService { NumeClient = "altuser", Problema = "Ecran spart" }
            };
        }

        [TestMethod]
        public void FiltrareComenzi_PentruUtilizator_ReturneazaCorect()
        {
            var comenziFlorian = toateComenzile
                .Where(c => c.Username == "florian")
                .SelectMany(c => c.Produse)
                .ToList();

            Assert.AreEqual(1, comenziFlorian.Count);
            Assert.AreEqual("Laptop", comenziFlorian[0].Nume);
        }

        [TestMethod]
        public void FiltrareService_PentruUtilizator_ReturneazaCorect()
        {
            var cereri = toateCererile.Where(c => c.NumeClient == "florian").ToList();

            Assert.AreEqual(1, cereri.Count);
            Assert.AreEqual("Nu pornește", cereri[0].Problema);
        }

        [TestMethod]
        public void FiltrareService_PentruUtilizatorInexistent_ReturneazaGol()
        {
            var cereri = toateCererile.Where(c => c.NumeClient == "necunoscut").ToList();

            Assert.AreEqual(0, cereri.Count);
        }

        [TestMethod]
        public void VerificareStatusComanda_Corespunzator()
        {
            var status = toateComenzile.First(c => c.Username == "florian").Status;
            Assert.AreEqual("In asteptare", status);
        }

        [TestMethod]
        public void ComandaFaraProduse_EsteInvalida()
        {
            var comanda = new Comanda { Username = "florian", Produse = new List<Produs>() };
            Assert.AreEqual(0, comanda.Produse.Count);
        }
    }
}
