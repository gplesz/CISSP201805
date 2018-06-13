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

            /// El�k�sz�l�s
            /// 
            var bankszmla = new KamatozoBankszamla("teszt �gyf�l");

            /// M�velet
            /// 
            bankszmla.Jovairas(5000);

            /// Ellen�rz�s
            /// Figyelem, a kamatoz� banksz�mla j�v��r�s a polimorfizmus miatt nem �gy
            /// m�k�dik, mint a banksz�mla j�v��r�sa
            Assert.AreEqual(10000, bankszmla.Egyenleg);
        }

        [TestMethod]
        public void BankszamlaKamatszamitasJolSzamol()
        {
            ///AAA: Arrange, Act, Assert
            ///

            /// El�k�sz�l�s
            /// 
            var bankszmla = new KamatozoBankszamla("teszt �gyf�l");
            bankszmla.Jovairas(5000);

            /// M�velet
            /// 
            bankszmla.Kamatszamitas();

            /// Ellen�rz�s
            /// Figyelem, a kamatoz� banksz�mla j�v��r�s a polimorfizmus miatt nem �gy
            /// m�k�dik, mint a banksz�mla j�v��r�sa
            Assert.AreEqual(10050, bankszmla.Egyenleg);
        }
    }
}
