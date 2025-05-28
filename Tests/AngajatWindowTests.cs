using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiTab.Models;

namespace MultiTab.Tests
{
    [TestClass]
    public class AngajatWindowTests
    {
        [TestMethod]
        public void Angajat_Junior_NuPoateAdaugaProdus()
        {
            var angajat = new Angajat("junior1", "1234", "Junior", "mail@test.com");
            bool poateAdauga = angajat.TipAngajat.Equals("Senior", System.StringComparison.OrdinalIgnoreCase);
            Assert.IsFalse(poateAdauga);
        }

        [TestMethod]
        public void Angajat_Senior_PoateAdaugaProdus()
        {
            var angajat = new Angajat("senior1", "1234", "Senior", "senior@mail.com");
            bool poateAdauga = angajat.TipAngajat.Equals("Senior", System.StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(poateAdauga);
        }

        [TestMethod]
        public void TipAngajat_NuEsteCaseSensitive()
        {
            var angajat = new Angajat("senior2", "pw", "sEnIoR", "x@test.com");
            bool poate = angajat.TipAngajat.Equals("Senior", System.StringComparison.OrdinalIgnoreCase);
            Assert.IsTrue(poate);
        }

        [TestMethod]
        public void Angajat_CampuriCorecte()
        {
            var angajat = new Angajat("nume", "pass", "Junior", "a@x.com");
            Assert.AreEqual("nume", angajat.NumeUtilizator);
            Assert.AreEqual("pass", angajat.Parola);
            Assert.AreEqual("Junior", angajat.TipAngajat);
            Assert.AreEqual("a@x.com", angajat.Email);
        }

        [TestMethod]
        public void Angajat_FaraEmail_NuEsteValid()
        {
            var angajat = new Angajat("test", "pw", "Junior", "");
            bool valid = !string.IsNullOrWhiteSpace(angajat.Email);
            Assert.IsFalse(valid);
        }
    }
}
