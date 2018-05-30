using System;

namespace ObjectOrientedDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var cegesBankszamla = new Bankszamla("Abroncs és Fiai Kft.");

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");
            cegesBankszamla.Jovairas(5000);

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");
            cegesBankszamla.Terheles(2000);

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");
            cegesBankszamla.Terheles(4000);

            //ezt nem lehet végrehajtani, az egyenleg kívülről csak olvasható, nem írható
            //cegesBankszamla.Egyenleg = 3000;

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");

        }
    }

    internal class Bankszamla
    {
        private string name;
        private int egyenleg;
        public object Egyenleg 
        { 
            get 
            { 
                return egyenleg; 
            } 
        }

        public Bankszamla(string name)
        {
            this.name = name;
        }

        internal void Jovairas(int osszeg)
        {
            egyenleg = egyenleg + osszeg;
        }

        internal void Terheles(int osszeg)
        {
            egyenleg = egyenleg - osszeg;
        }
    }
}
