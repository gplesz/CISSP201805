using System;

namespace ObjectOrientedDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var cegesBankszamla = new KamatozoBankszamla("Abroncs és Fiai Kft.");

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");
            cegesBankszamla.Jovairas(5000);

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");
            cegesBankszamla.Terheles(2000);

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");
            cegesBankszamla.Terheles(4000);

            //ezt nem lehet végrehajtani, az egyenleg kívülről csak olvasható, nem írható
            //cegesBankszamla.Egyenleg = 3000;

            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");

            cegesBankszamla.Kamatszamitas();
            System.Console.WriteLine($"Egyenleg: {cegesBankszamla.Egyenleg}");

            System.Console.WriteLine($"Bankszámla neve: {cegesBankszamla.Nev}");
            

        }
    }

    class KamatozoBankszamla : Bankszamla
    {
        public KamatozoBankszamla(string name) : base(name) {}

        public void Kamatszamitas()
        {
            egyenleg = egyenleg + 50;
        }

        public new void Jovairas(int osszeg)
        {
            egyenleg = egyenleg + 2*osszeg;
        }
    }

    class Bankszamla
    {
        private string name;
        public object Nev 
        { 
            get { return name; }
        }

        protected int egyenleg;
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

        public void Jovairas(int osszeg)
        {
            egyenleg = egyenleg + osszeg;
        }

        public void Terheles(int osszeg)
        {
            egyenleg = egyenleg - osszeg;
        }
    }
}
