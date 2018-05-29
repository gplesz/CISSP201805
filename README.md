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

#### Memory
- ROM: Read Only Memory
- PROM: Programmably Read Only Memory
- EPROM: Erasable Programmable Read Only Memory
- EEPROM: Electronically Erasable Programmable Memory
- Flash: blokkonként törölhető EEPROM
- RAM: Random Access Memory: áram kell neki, hogy megtartsa a beleírt adatot
  - dinamic: kapacitásokból áll, ami idővel elveszíti a töltését, így a processzornak frissítenie kell időről időre a tartalmat.
  - static: flip-flop áramkörökből áll, amíg áram alatt van nem felejt, a processzornak nem kell frissítést végeznie. Drágább és gyorsabb eszköz. Cache memória: az L1 a processzoron van, az L" pedig static RAM.

```
   Egy lap 256 helyet tartalmaz
+--------------------------------+
|                                |
|     RAM                        |
|                                |
|                                |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|  | 0|255 bináris szám     |    |
|  +------------------------+    |
|                                |
+--------------------------------+
```

Ha például 8 bites rendszerünk van, akkor egy memóriahelyre (imt önállóan lehet címezni, tehát bele írni és belőle olvasni) 256 féle érték kerülhet. Így 256 féle címet tartalmazhat. Ha több, mint 256 helyünk van, akkor a memóriát 256 egységes **lapokra** osztjuk, és a címek két részből állnak: a lap címéből, és a lapon belül a memóriahely címéből.

#### Registers
Spceiális memóriák, a processzor végrehajtási egységével (ALU: Arithmetic-logical unit) szinkron sebességgel dolgoznak. Rajtuk keresztül kapja a processzor az adatokat, a segítségükkel végzi a műveletet.

- **Register addressing:** amikor a processzor a regisztert célozza (címzi). Például: **register 1**
- **Inmediate addressing:** közvetlen műveletvégzés, register segítségével Például: **add 2 to register 1**
- **Direct addressing:** a regiszterben a memóriahely címe van, amivel dolgozni kell. **Ezen a memóriahelyen van az adat** a számoláshoz. Ez a cím a végrehajtandó utasítással egy lapon van. (különben két rész kéne a címhez: lapcím (base) és azon belül a memória címe (offset))
- **Indirect addressing:** a register által megcímzett memóriahely nem adat van, hanem egy újabb cím, ami az adatot tartalmazza a művelethez. (A cím egy lapon van a paranccsal.)
- **Base+Offset addressing:** amikor az adat nincs azonos lapn a művelettel, akkor agy a lapcím + lapon belüli memóriacím párossal jutunk el az adatig.

#### Primary, Secondary storage
- **Primary:** olyan memória, amit regiszter műveletekkel elérünk. Ez a "valódi" memória. *Gyors és drága*
- **Secondary:** először be kell tölteni a primary storage -be használathoz. *Lassú és olcsó*.

- **Virtual memory**, **virtual storage**: az operációs rendszer kezeli, segítségével elsődlegesnek látszik.

#### I/O devices
- Monitor
- Printer
- Keyboard/Mouse
- Modems
  - Memory-mapped I/O
  - IRQ
  - DMA
- Firmware
  - BIOS
  - Device firmware

## Adatbázisok (Databases)
Az adatok kiegészítve a hozzáférés módjával. Vagyis az adatok mellett az alkalmazás is, ami a felhasználó számára lehetővé teszi, hogy az adatokat lekérdezze, létrehozza, módosítsa, törölje. 

DBMS: Database Managamant Systems
- a program, ami a felhasználó számára lehetővé teszi, hogy az adatokat lekérdezze, létrehozza, módosítsa, törölje az adatokat.
- adatok mentése és visszatöltése (Backup and Recovery)
- lekérdező nyelv biztosítása (SQL)
    - SQL/DQL: Data Query Language
    - SQL/DML: Data Manipulation Language
- az adatbázis struktúrájának a kezelő nyelve (SQL/DDL: Data Definition Language)
- az adatok hozzáférésének a szabályozása (SQL/DCL: Data Control language)

### Hierarchikus adatbázisok (Hierarchical Databases)
A hierarchikus adatbázis fa struktúrájú adatok nyilvántartására készült.

Példa:
```
                <-------------------+  Vezérigazgató +------------------->
                |                            +                           |
                |                            |                           |
                v                            v                           v
          Értékesítés                 <-+ Számlázás +->+             Logisztika
               +                      |                |                 +
               |                      |                |                 |
               v                      v                v                 v
Értékesítési vezető                 Számlázó 1   Számlázó 2          Raktáros

```
Másik példa a DNS: Domain Name System.

felső szintű domainek: .hu, .com, stb.

www.netacademia.hu 

Minden csomópontnak egy szülője és tetszőleges számú gyermek csomópontja lehet, vagyis, egy-a-többhöz adatokat kezel.

### Elosztott adatbázisok (Distributed Databases)
A felhasználó számára egy rendszernek látszik, az adatok egymástól elválasztva több fizikai helyszínen vannak. Az egyes objektumoknak több szülője és több gyermeke is lehet, vagyis több-a-többhöz adatok nyilvántartására szolgál. A faceboook ismerősi hálózata például ilyen adatbázis.

### Objektumorientált/Objektumrelációs adatbázisok (Object oriented Databases)
Az objektum orientált programozási alapelvek összekombinálása az adatok nyilvántartásával, ennek köszönhetően az adatok egységbezárása és leszármaztatása az adatbázison belül megvalósítható. A kód újrafelhasználhatóságát és a hibakeresést is jól támogatja. Sokkal alkalmasabb multimédiás, CAD, grafikai és szakértői rendszerek támogatásához.

### Reláció adatbázisok (Relational Databases)
IBM, 1970, Codd. SEQUEL: Structured English Query Language. 

A relációs adatbázis un. relációkat (relation) (adattáblákat) kezel.

Például egy partnereket és a hozzájuk tartozó pénzmozgásokat nyilvántartó táblázat

---
|Név|Cím|Partner név|Partner cím|Összeg|
|-|-|-|-|-|
|Gipsz Jakab|1000 Budapest|Nagy Zoltán|2000 Szentendre|+5000|
|Gipsz Jakab|1000 Budapest|Kiss Tamás |1200 Budapest|+7000|
|Nagy Zoltán|2000 Szentendre|Kiss Tamás |1200 Budapest|-3000|
|Kiss Tamás|1000 Budapest|Gipsz Jakab |1200 Budapest|+4000|

#### SQL Szolgáltatásai
- adatok táblázatban
    - vizszintesen: sorok, rekordok (row=record=tuple)
    - függőlegesen: oszlopok, mezők (column=field=attribute)
- vizszintes irányban a táblázat struktúrája ritkán változik, függőleges irányban pedig állandóan. Az adatbáziskezelő munkájának nagy része a sorok létrehozásával, módosításával, lekérdezésével és törlésével telik.

ebből az adathalmazból a relációs adatbázis világban a következő struktúrát gyártjuk:

Partner tábla
---
|Kulcs|Név|Cím|
|-|-|-|
|1|Gipsz Jakab|1000 Budapest|
|2|Nagy Zoltán|2000 Szentendre|
|3|Kiss Tamás|1200 Budapest|

##### Kulcsok (Keys)
- **Candidate Key** (lehetséges kulcsmező): a rendelkezésünkre álló adatok alapján képes egy sort azonosítani.
- **Primary Key (PK)** (elsődleges kulcs): ami egyértelműen azonosítja a sort.
  - lehet Identity (nővekvő egész szám), ezt az adatbázis adja
  - lehet (GUID: Globally Uniq IDentifier): egy speciálisan számolt érték, az a lényege, hogy minden GUILD a világon egyedüliként jön létre. Ezt számolhatja a kliens és az adatbázis is.
  - az azonosításhoz használhatunk több mező kombinációját is (Composite Key)
- **Foreign Key (FK)** (távoli kulcs): ami egy másik táblában egy PK mezőre "mutat". 

Összegek tábla
---
|Partner 1| Partner 2| Összeg |
|-|-|-|
|1|2|+5000|
|1|3|+7000|
|2|3|-3000|
|3|1|+4000|

##### Adatbázisok normalizálása
Azt jelenti, hogy az adatok redundanciájának csökkententése illetve az integritásának növelése céljából alakítjuk az adattáblák struktúráját. A folyamat szabványosított, normálformák írják le, hogy milyen feltételeknek kell az adatbázisban érvényesülni, hogy azt mondhassuk, hogy 1NF->2NF->3NF->4NF.

[Ebben a cikkben](https://www.lifewire.com/database-normalization-basics-1019735) röviden az egyes normálformákról lehet olvasni.

##### Gyakorlati példa

Ha helyi SQL adatbázison dologzunk, akkor létre kell hozni az adatbázist és utána ki is kell választani, így:

```sql
--adatbázis létrehozása
create database CISSP2018
go

--a létrehozott adatbázist kiválasztjuk
use CISSP2018
go
```

ez után, vagy pedig ha az [SQLFiddle](http://sqlfiddle.com) oldalon próbáljuk ki a kódot, akkor nem kell létrehozás, csak ami most jön:

```sql
--két tábla létrehozása (DDL: Data Definition Language)
create table Partnerek (
	Kulcs int not null primary key clustered,
	Nev nvarchar(50),
	Cim nvarchar(50)
)
go

create table Osszegek (
	Kulcs int not null primary key clustered,
	Partner1 int,
	Partner2 int,
	Osszeg int,
	constraint fk_osszegek_partnerek1 foreign key (Partner1) references Partnerek (Kulcs),
	constraint fk_osszegek_partnerek2 foreign key (Partner2) references Partnerek (Kulcs)
)
go
```

Figyelem, ezt a két scriptet egyszerre kell az SQLFiddle-ben használni.

```sql
--ezt követően fel kell tölteni adatokkal (DML: Data manipulation Language)
insert Partnerek values (1, 'Gipsz Jakab', '1000 Budapest')
go

insert Partnerek values (2, 'Nagy Zoltán', '2000 Szentendre')
go

insert Partnerek values (3, 'Kiss Tamás', '1200 Budapest')
go

insert Osszegek values (1,1,2,5000)
go

insert Osszegek values (2,1,3,7000)
go

insert Osszegek values (3,2,3,-3000)
go

insert Osszegek values (4,3,1,4000)
go
```

Példa az eredeti adatcsomag lekérdezhetőségére:

```sql
--lekérdezzük az adatokat DQL: Data Query language
select
  Partnerek1.Nev,
  Partnerek1.Cim,
  Partnerek2.Nev,
  Partnerek2.Cim,
  Osszegek.Osszeg
from
  Osszegek
  inner join Partnerek Partnerek1 on Partnerek1.Kulcs = Partner1
  inner join Partnerek Partnerek2 on Partnerek2.Kulcs = Partner2
  
```

```sql
--Kis Tamás elköltözik Kecskemétre (DML)
update 
  Partnerek
set
  Cim='6000 Kecskemét'
where
  Kulcs=3
```

##### Az SQL adatbázis védelmi képességei

**Ezek azt szolgálják, hogy ne kerülhessen az adatbázisba érvénytelen információ**

- Entity integrity: garantálja, hogy minden sornak van PK-ja
- Referential integrity: az adott táblában lévő FK mező információja létező PK-ra mutat. (Létrehozáskor, módosításkor és törléskor) Például:

  ```sql

    delete Partnerek where Kulcs = 1

    ---eredménye: 
    --The DELETE statement conflicted with the REFERENCE constraint "fk_osszegek_partnerek1". The conflict occurred in database "db_18_d2dc4", table "dbo.Osszegek", column 'Partner1'.

  ```

- Semantic integrity: biztosítja, hogy a strukturális és szemantikai szabályok ki legyenek kényszerítve
  - not null
  ```sql
  insert Partnerek values (null, 'Tüdő Pál', '1000 Budapest')

  --eredménye:
  --Cannot insert the value NULL into column 'Kulcs', table 'CISSP2018.dbo.Partnerek'; column does not allow nulls. INSERT fails.
  ```
  - check constraint
    például: Összeg>0 nem enged 0-t vagy negatív számot felvinni.

###### SQL nézetek

Példa (az eredeti schema után még ezt is bemásolni az SQLFiddle-be):

```sql

create view EredetiAdathalmaz
as
select
  Partnerek1.Nev Nev1,
  Partnerek1.Cim Cim1,
  Partnerek2.Nev Nev2,
  Partnerek2.Cim Cim2,
  Osszegek.Osszeg
from
  Osszegek
  inner join Partnerek Partnerek1 on Partnerek1.Kulcs = Partner1
  inner join Partnerek Partnerek2 on Partnerek2.Kulcs = Partner2

```
Egy olyan virtuális táblát generál (egy olyan felületet ad a felhasználó felé) ami elrejti a mögötte lévő mechanizmust és a nyers adatokat, és saját jogosultságokat lehet hozzá megadni.

Lehetővé teszi, hogy az így létrehozott virtuális tábla megsértse a normalizálási szabályokat.

###### Database Schema
Definiálja az adatbázis felépítését
- Táblák (Tables)
- Táblák közti kapcsolatok (Relationships), FK
- Business rules
- Domains

###### Adatbázis integritása: a tranzakciók (Database integrity operations: transactions)
Minden adatbázis műveletre jellemzőek ezek a szabályok
- **A**tomic: nem részekre bontható, az egyes műveletek vagy együtt végrehajtódnak, vagy egyik sem hajtódik végre.
- **C**onsistency: az adatbázis műveletei megtartják a konzisztenciát, vagyis az integritási szabályoknak megfelelő állapotból a művelet után csak az integritási szabályíoknak szintén megfelelő állapotba kerülhet.
- **I**solation: amit tranzakcióban végzünk, azt a többiek addig nem láthatják, amíg nem végeztünk.
- **D**urable: ha a tranzakció végetért, akkor az eredménye tartósan megmarad.

```sql
begin transaction 

insert Partnerek values (4, 'Tüdő Pál', '1200 Budapest')

select * from Partnerek

insert Osszegek values (5,1,4,1000)

select * from Osszegek

--rollback

commit transaction
```

###### Konkurencia (Concurrency)
Az adatok szerkesztésének a felügyelete (edit control) egy megelőző mechanizmus, arra törekszik, hogy az adatbázisban tárolt információk helyesek legyenek, integritás és rendelkezésreállás szempontjából. Maga a védelem egy zárolási (lock mechanism) funkciót használ, hogy a módosítás konzisztens legyen: egy felhasználó módosítását engedélyezi, a többi hozzáférést megtagadja.

A módosítást követően a feloldás visszaállítja a többi felhasználó hozzáférését.

Bizonyos esetekben, ha a rendszergazda integrálja a konkurrens hozzáférés szabályozást naplózással, a módosítások nyomon köveésére is alkalmas.

Amikor pedig a rögzített adatokat vizsgálják, a konkurrencia nyomozói eszközzé (detective control) válik.

## Szoftverfejlesztés