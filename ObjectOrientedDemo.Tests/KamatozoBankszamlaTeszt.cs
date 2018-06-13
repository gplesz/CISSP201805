using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectOrientedDemo.Tests
{
    [TestClass]
    public class KamatozoBankszamlaTeszt
    {
        [TestMethod]
        public void BankszamlaJovairasJolSzamol()
        {
            ///AAA: Arrange, Act, Assert
            ///

            /// Elõkészülés
            /// 
            var bankszmla = new KamatozoBankszamla("teszt ügyfél");

            /// Mûvelet
            /// 
            bankszmla.Jovairas(5000);

            /// Ellenõrzés
            /// Figyelem, a kamatozó bankszámla jóváírás a polimorfizmus miatt nem úgy
            /// mûködik, mint a bankszámla jóváírása
            Assert.AreEqual(10000, bankszmla.Egyenleg);
        }

        [TestMethod]
        public void BankszamlaKamatszamitasJolSzamol()
        {
            ///AAA: Arrange, Act, Assert
            ///

            /// Elõkészülés
            /// 
            var bankszmla = new KamatozoBankszamla("teszt ügyfél");
            bankszmla.Jovairas(5000);

            /// Mûvelet
            /// 
            bankszmla.Kamatszamitas();

            /// Ellenõrzés
            /// Figyelem, a kamatozó bankszámla jóváírás a polimorfizmus miatt nem úgy
            /// mûködik, mint a bankszámla jóváírása
            Assert.AreEqual(10050, bankszmla.Egyenleg);
        }
    }
}
