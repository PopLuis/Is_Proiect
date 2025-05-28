using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiTab.Data;
using MultiTab.Models;
using System.Collections.Generic;
using System.Linq;

namespace MultiTab.Tests
{
    [TestClass]
    public class ManagerWindowTests
    {
        private DataManager _dataManager;

        [TestInitialize]
        public void Setup()
        {
            _dataManager = new DataManager();
        }

        [TestMethod]
        public void AdaugaAngajat_NouAngajat_ReturneazaTrue()
        {
            var angajat = new Angajat("nouUser", "parola", "Junior", "email@test.com");
            bool rezultat = _dataManager.AdaugaAngajat(angajat);
            Assert.IsTrue(rezultat);
        }

        [TestMethod]
        public void AdaugaAngajat_Duplicat_ReturneazaFalse()
        {
            var angajat = new Angajat("user1", "parola", "Junior", "email@test.com");
            _dataManager.AdaugaAngajat(angajat);
            bool rezultat = _dataManager.AdaugaAngajat(angajat); // adaugat din nou
            Assert.IsFalse(rezultat);
        }

        [TestMethod]
        public void StergeAngajat_Existent_ReturneazaTrue()
        {
            var angajat = new Angajat("deSters", "pw", "Senior", "mail@x.com");
            _dataManager.AdaugaAngajat(angajat);
            bool rezultat = _dataManager.StergeAngajat("deSters");
            Assert.IsTrue(rezultat);
        }

        [TestMethod]
        public void AdaugaPromotie_Valida_ReturneazaTrue()
        {
            var promotie = new Promotie("Promo10", "Reducere la produse", new List<string> { "prod1", "prod2" },
                System.DateTime.Now, System.DateTime.Now.AddDays(5), 10.0);
            bool rezultat = _dataManager.AdaugaPromotie(promotie);
            Assert.IsTrue(rezultat);
        }

        [TestMethod]
        public void StergePromotie_Exista_ReturneazaTrue()
        {
            var promotie = new Promotie("PromoX", "Text", new List<string> { "x" },
                System.DateTime.Now, System.DateTime.Now.AddDays(1), 10);
            _dataManager.AdaugaPromotie(promotie);
            bool rezultat = _dataManager.StergePromotie("PromoX");
            Assert.IsTrue(rezultat);
        }
    }
}
