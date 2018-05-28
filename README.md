# CISSP201805
A NetAcademia 2018 májusi CISSP tanfolyam kiegészítő kódtára

## Szoftverfejlesztés és adatbázisok

## Computing Systems
A biztonsági kérdéseket fontos áttekinteni a számítógépes architektúra területén.

### Hardware
#### Processor
CPU Central Processing Unit

- Korlátozott számítási és logikai műveletre képes
- Az összetett feladatok elvégzéséért az OS és a magasszintű program fordító/futtató környezet a felelős.
- Moore törvénye (Gordon Moore)
  - 1965: a köetkező 10 évben a tranzisztorok **sűrűsége** meg fog duplázódni az integrált áramkörökön.
  - 1975: újraértékelt, és a törvényt a következő 10 évre is kiterjeszti
  - David House (Intel): a mennyiség mellett a sebesség növekedést is figyelembe véve azt mondta, hogy a processzorok teljesítménye 18 havonta meg fog duplázódni.

- konkrét példa: a Lotus-123 és az Excel párharca.

- A processzorok teljesítménye előre kiszámíthatóan korlátos marad, ezzel szemben a felhasználók igénye gyakorlatilag határtalan. Ezért erős az igény a párhuzamos feldolgozás kiszolgálására. Hogy lehetne két dolgot egy időben elvégezni?

#### Végrehajtási típusok (Execution Types)

##### több feladat egyidejű elvégzése (multitasking)
A legtöbb rendszer erre nem képes, az operációs rendszer ad egy olyan szolgáltatást, aminek a segítségével több feladatot apró részekre bontva, majd ezeket az apró részeket sűrűn felváltva a processzor látszólag minden feladatot egyszerre hajt végre. Valójában a CDP idejét bontjuk apró szeletekre, és egy egy szeletet különböző feladatnak engedünk.

##### több folyamat egyidejű elvégzése (multiprocessing)
Folyamatok párhuzamos végrehajtásra képes hardware.

- SMP: Symmetric Multiprocessing
  egy számítógép több processzort tartalmaz, de az OS, a memóriahasználat és az adathozzáférés közös. Ez általában 2-16 processzort jelent számítógépenként.

- MPP: Massive Parallel Processing
  több száz vagy akár több ezer processzor, processzoronként saját OS, memória és adathozzáférés. Például: particionált adatok.

Következő generációs (SMP) multiprocessing: többmagos processzorok segítségével egy CPU-n belül több műveletvégrehajtás történhet egyidejűleg. 

##### párhuzamos programozás (multiprogramming)
Az alkalmazást írjuk úgy, hogy képes legyen több feladat egyidejű számolására. Ez inkább a régi mainframe-ek sajátja.

##### töbszálú feladat végrehajtás (multithreading)
A modern operációs rendszerek (Windows/Linux) képes egy folyamaton belül (process) párhozamos "szálakat" (thread) szolgáltatni, aminek az az értelme, hogy thread-ek közötti feladatváltás sokkal kevesebb processzorutasításból oldható meg (50 utasítás) mint az egyes processzek közötti váltás (1000 utasítás).


```
+----------------------+                        +----------------------+
|    Alkalmazás        |                        |    Alkalmazás        |
|                      |                        |                      |
| +----+  +----+ +-----+                        | +----+  +----+ +-----+
| |Szál|  |Szál| |Szál |                        | |Szál|  |Szál| |Szál |
| |    |  |    | |     |                        | |    |  |    | |     |
| +--+-+  +--+-+ +---+-+                        | +--+-+  +--+-+ +---+-+
|    |       |       | |                        |    |       |       | |
|    |       |       | |                        |    |       |       | |
|    |       |       | |                        |    |       |       | |
|    |       |       | |                        |    |       |       | |
|    |       |       | |     1000 utasítás      |    |       |       | |
|    |       |       | | +------------------->  |    |       |       | |
|    |  50   |   50  | |                        |    | 50    |  50   | |
|    v       v       v |                        |    v       v       v |
|                      |                        |                      |
|                      |                        |                      |
|                      |                        |                      |
+----------------------+                        +----------------------+
```

A process saját memóriával rendelkezik, a thread-eknek nincs saját memóriája, de minden thread-nek saját hívási verme van (call stack).

```


        Dokumentumkezelés                          Dokumentum betöltése                    Állománykezelés
     +----------------------+                 +----------------------------+            +----------------------+
     |                      |           +>--> |                            |      +>--> |                      |
     |  Utasítások          |           |     |                            |      |     |  Utasítások          |
     |      +               |           |     |   Utasítások               |      |     |      +               |
     |      |               |           |     |       +                    |      |     |      |               |
     |      +---------------------------+     |       |                    |      |     |      |               |
     |      | ^-------------------------+     |       | +------------------------->     |      |               |
     |      |               |           ^     |       | ^-------------------------+     |      |               |
     |      |               |           |     |       |                    |      |     |      |               |
     |      |               |           |     |       |                    |      |     |      |               |
     |      |               |           |     |       |                    |      |     |      |               |
     |      |               |           |     |       |                    |      |     |      |               |
     |      |               |           |     |       |                    |      |     |      |               |
     |      v               |           |     |       |                    |      |     |      v               |
     |                      |           |     |       v                    |      |     |                      |
     +----------------------+           |     |                            |      <---- +----------------------+
                                        <--+  +----------------------------+
             ^                                                                                    ^
             |                                        ^                                           |
             |                                        |                                           |
             |                                        |                                           |
             |                                        |                                           |
             |                                        |                                           |
             |                                        |                                           |
             <------------------------------------------------------------------------------------+
                                                                                                  +
                                                                                                  Call Stack
```

###### Multithread programozás demo

```csharp
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
```
###### Multithread programozás demo 2.

```csharp
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
```
A program futtatásakor látszik, hogy az első négy szál azonnal elindul (mivel a gép ahol futtatom négymagos processzor köré épült), majd szépen lassan a többi szál is elindul, de annak idő kell.

Megjegyzések:

- kód párhozamos futtatásához nem kell semmilyen különleges programozási módszertan, a futtatókörnyezet (.NEt + OS) szolgáltatja.

- a párhuzamosság miatt csak akkor kell párhuzamos helyzeteket megoldani, ha a szálaknak együtt kell működnie.s

#### Folyamat típusok (Processing Types)
A magas biztonságú rendszerek különböző biztonsági szinteket állapítanak meg, ennek segítségével szabályozzák a hozzáférést. Például:

- besorolás nélküli
- érzékeny
- bizalmas
- titkos
- szigorúan titkos

A feladat az az, hogy az egyes biztonsági szintet megkövetelő lehetőségekhez csak az adott besorolású személyek személyek férjenek hozzá. 

##### Single state
Ha nem akarjuk technikailag beépíteni a hozzáférési szabályozást minden rendszerbe, akkor megtehetjük, hogy **single state** típusként üzemeltetünk. Egyszerre csak egy féle hozzáférést enged a rendszer, vagyis hétfőn csak szigorúan titkos, kedden csak titkos, szerdán csak bizalmas, csütörtökön csak érzékeny és pénteken besorolás nélküli használatot engedünk.

##### Multi State
Egyszerre többféle besorolásnak megfelelő hozzáférést képes kezelni a rendszer. Ez azért nehéz ügy, mert a szabályoknak megfelelően a különböző szintek adatai semmilyen formában nem keveredhetnek egymással. 
Ez a mechanizmus nagyon költséges, és az ilyen rendszerek nagyon drágák. Az MPP rendszerek közül egyes drága rendszerekben van olyan indok, ami értelmessé teszi ezt a kialakítást.

#### Protection rings
1963, Multics operációs rendszer készítésekor a MIT, a Bell Laboratórium és a General Electric fejlesztése.
A Unix operációs rendszer fejlesztéséhez is ez az alapelv, és modern OS-ek alapelve.

```
+-----------------------------------------------+
|                                               |
|      User-level programs and applications     |
|      (Ring 3)                                 |
|                                               |
|    +-------------------------------------+    |
|    |      Drivers, protocols             |    |
|    |      (Rind 2)                       |    |
|    |                                     |    |
|    |   +---------------------------+     |    |
|    |   |  Other OS komonents       |     |    |
|    |   |  (Ring 1)                 |     |    |
|    |   |                           |     |    |
|    |   | +--------------------+    |     |    |
|    |   | |                    |    |     |    |
|    |   | |  OS kernel/memory  |    |     |    |
|    |   | |  (Ring 0)          |    |     |    |
|    |   | |                    |    |     |    |
|    |   | +--------------------+    |     |    |
|    |   |                           |     |    |
|    |   |                           |     |    |
|    |   +---------------------------+     |    |
|    |                                     |    |
|    +-------------------------------------+    |
|                                               |
+-----------------------------------------------+
```
Addig, amíg alacsonyabb szinten van végrehajtandó folyamat, addig a magasabb szinten lévő folyamat nem kap végrehajtási lehetőséget.

- Ring 0-2: Privileged 
- Ring 3: User

#### Folyamat állapotok (Process states)

```
                                                                        +----------+
                    Process needs another time slice                    |          |
+-------------+   <------------------<------------------+               |  Stopped |
|             |   |                                     |               |          |
| New process |   |                                     |               +-----+----+
|             |   |                                     |                     ^
+---+---------+   |                                     |        When process |
    |             v                                     +        finished or  |
    |                                                            terminated   |
    |         +-------+   If CPU is available      +---------+                |
    |         |       |                            |         |                |
    +-------->+ Ready +--------------------------->+ Running +---------------->
              |       |                            |         |
              +-------+                  +-------> +----+----+
                                         |              |
                             Unblocked   |              |
                                         |              |  block for I/O,
                                         |              |  resources
                                         |              |
                                         +              |
                                                        |
                                    +---------+         |
                                    |         |         |
                                    | Waiting |  <------+
                                    |         |
                                    +---------+

```

- **Ready:** (a folyamat) végrehajtásra kész. Ha lesz szabad processzor, akkor ezt a folyamatot végre is hajthatja. A hozzárendelt erőforrások ki vannak osztva neki.
- **Waiting:** Külső erőforrásra vár. A futása addig blokkolódik, amíg az erőforrás hozzárendelése megtörténik.
- **Running:** A folyamatot a CPU végrehajtja. Addig tart, amíg a) folyamot véget ér, vagy b) le nem jár az időszelete, vagy c) a folyamat erőforrásigény miatt blokkolt állapotba kerül.
- **Stopped:** A folyamat véget ért. Vagy azért, mert végzett, vagy mert meg kellett szakítani (hiba, esetleg nem elérhető erőforrás miatt). Ilyenkor a folyamatunkhoz hozzárendelt erőforrások elvonhatók és újraoszthatók.
- **Supervisory:** Amikor a folyamat végrehajtásához magasabb jogok kellenek (egy körrel beljebb kell hozzá kerülni).






## Adatbázisok

## Szoftverfejlesztés