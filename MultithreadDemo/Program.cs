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
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);
            ThreadPool.QueueUserWorkItem(Adatfeldolgozas);

            Console.ReadLine();
            Console.WriteLine("Alkalmazás véget ért!");
        }

        private static void Adatfeldolgozas(object state)
        {
            System.Console.WriteLine("- -> Adatfeldolgozás elindult");
            Thread.Sleep(5000);
            System.Console.WriteLine("- -> Adatfeldolgozás véget ért");
        }
    }
}
