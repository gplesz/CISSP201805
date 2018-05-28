using System;
using System.Threading;

namespace MultithreadDemo
{
    /// <summary>
    /// a .NET (dotnet) a Microsoft programozási paradigmája, válasz a Java-ra
    /// közös tulajdonságuk, hogy nem a processzor által azonnal futtatható (un. gépi kódot)
    /// készít a fordító, hanem egy saját közös nyelre fordít le mindent (Intermediate Language-IL)
    /// a magas szintű nyelvekről (VB.NET/C#/PHP).
    /// 
    /// Ezt az IL nyelvű kódot, az adott operációs rendszer .NET környezete futtatja:
    /// menet közben lefordítja gépi kódra, és azt már az OS tudja a processzornak adni.
    /// 
    /// Ezzel biztosítja (egyebek mellett) a multiplatformosságot.
    /// 
    /// (a Java-nál byte kód és virtual machine a megfelelő fogalmak nevei.)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Alkalmazás elindult!");

            //a dotnet beépített thread kezelése
            //ezen keresztül kérhetünk egy futtatási szálat.
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Chatkiszolgalas);

            Console.ReadLine();
            Console.WriteLine("Alkalmazás véget ért!");
        }

        /// <summary>
        /// Saját szálon futó folyamat, nem kell törődnöm azzal, hogy 
        /// a program többi része hol tart.
        /// </summary>
        /// <param name="state"></param>
        private static void Chatkiszolgalas(object state)
        {
            System.Console.WriteLine("- - -> Chatkiszolgálás elindult");
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(200);
                System.Console.WriteLine("- - -> Chatkiszolgálás folyamatban");
            }
            System.Console.WriteLine("- - -> Chatkiszolgálás véget ért");
        }

        private static void Adatfeldolgozas(object state)
        {
            System.Console.WriteLine("- -> Adatfeldolgozás elindult");
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(500);
                System.Console.WriteLine("- -> Adatfeldolgozás folyamatban");
            }
            System.Console.WriteLine("- -> Adatfeldolgozás véget ért");
        }
    }
}
