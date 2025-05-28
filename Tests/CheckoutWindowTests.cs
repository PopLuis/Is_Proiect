using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MultiTab.Models;

namespace MultiTab.Tests
{
    [TestClass]
    public class CheckoutWindowTests
    {
        [TestMethod]
        public void TelefonValid_TrebuieSaFie10Cifre()
        {
            string telefon = "0740123456";
            bool esteValid = Regex.IsMatch(telefon, @"^\d{10}$");
            Assert.IsTrue(esteValid);
        }

        [TestMethod]
        public void TelefonInvalid_TrebuieSaFieRespins()
        {
            string telefon = "0740ABC456";
            bool esteValid = Regex.IsMatch(telefon, @"^\d{10}$");
            Assert.IsFalse(esteValid);
        }

        [TestMethod]
        public void CardNumberValid_TrebuieSaFie16Cifre()
        {
            string card = "1234567812345678";
            bool esteValid = Regex.IsMatch(card, @"^\d{16}$");
            Assert.IsTrue(esteValid);
        }

        [TestMethod]
        public void DataExpirare_ValidFormat()
        {
            string data = "05/27";
            bool esteValid = Regex.IsMatch(data, @"^(0[1-9]|1[0-2])\/\d{2}$");
            Assert.IsTrue(esteValid);
        }

        [TestMethod]
        public void CVV_InvalidFormat()
        {
            string cvv = "99"; // trebuie 3 cifre
            bool esteValid = Regex.IsMatch(cvv, @"^\d{3}$");
            Assert.IsFalse(esteValid);
        }
    }
}
