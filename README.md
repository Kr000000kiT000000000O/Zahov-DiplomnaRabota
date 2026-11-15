# BikeStore – дипломeн проект, Етап 1

**Тема:** Разработване на уеб приложение за магазин за велосипеди  
**Автор:** Кристиян Викторов Захов, 12Д клас  
**Ръководител:** Иванжела Динчева  
**Статус:** Етап 1 – концепция, проучване, начална структура и начален модел на данните

## Какво има в това хранилище

- `docs/` – концепция, проучване на 3 приложения, изисквания, архитектура, ER модел, план и инструкции.
- `BikeStore/` – работна **начална структура** на ASP.NET Core MVC приложение с публичен каталог, детайли, списък на велосипеди за наем, вход/регистрация и роли.
- `BikeStore.Tests/` – начални unit тестове на калкулацията за наем.
- `.github/workflows/ci.yml` – автоматична проверка на build и тестове при push/PR.
- `database/01_initial_schema.sql` – **алтернативен** SQL скрипт за SSMS. Не го комбинирайте с EF миграцията за една и съща БД.
- `Dokumentacia_Etap1_BikeStore.docx` – документация за предаване (включена в ZIP).

> Важна граница: етап 1 **не е завършен онлайн магазин**. Покупките, записването на резервации, реалните плащания, цялостният административен CRUD, 65% покритие и публикуването онлайн са планирани за следващите етапи.

## Изисквания

- Windows + Visual Studio **2022, минимум 17.8** с workload **ASP.NET and web development**.
- .NET **8 SDK** (съвместим с VS 2022, но с приключила поддръжка; за реално внедряване е нужна миграция към .NET 10 + VS 2026).
- SQL Server LocalDB (инсталира се често с VS) или SQL Server Express / Developer + SSMS.
- Интернет при първоначално възстановяване на NuGet пакетите.

## Стъпка 1 – Отваряне

1. Изтеглете ZIP и разархивирайте.
2. Копирайте файловете **в корена на вече съществуващото GitHub хранилище**, без да копирате папка `.git` и без да създавате ново хранилище.
3. Отворете `BikeStore.sln` с Visual Studio 2022.
4. Изчакайте NuGet Restore. Задайте `BikeStore` като Startup Project.
5. В `BikeStore/appsettings.json` променете `DefaultConnection` само ако SQL Server не е LocalDB:
   - LocalDB (по подразбиране): `Server=(localdb)\MSSQLLocalDB;Database=BikeStoreDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true`
   - Пример SQLEXPRESS: `Server=.\SQLEXPRESS;Database=BikeStoreDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true`

## Стъпка 2 – Създаване на началната база данни (препоръчително)

**Изберете само един от двата начина. Препоръчителен е A.**

### Начин A – Entity Framework Core Code First (препоръчително)

В Visual Studio: **Tools → NuGet Package Manager → Package Manager Console**. Изберете **Default project: BikeStore**. Преди първото стартиране изпълнете:

```powershell
Add-Migration InitialCreate
Update-Database
```

Тези команди генерират миграция и създават таблиците `AspNetUsers`, `AspNetRoles`, `Categories`, `Bicycles`, `Orders`, `OrderItems`, `RentalReservations` и свързаните Identity таблици. `Migrations/` трябва да бъде качена в GitHub след генерирането. После стартирайте с **F5**. `DbSeeder` въвежда 4 категории, 3 фиктивни велосипеда и роли `Client`, `Employee`, `Admin`.

Ако командата не е разпозната, проверете NuGet Restore, startup/default проект и версията на SDK. **Не изпълнявайте** `EnsureCreated`, когато използвате миграции.

### Начин B – SQL скрипт в SSMS (алтернатива)

Ако преподавателят иска да вижда базата веднага в SSMS, отворете `database/01_initial_schema.sql`, изпълнете го на локалния SQL Server и настройте connection string към него. Скриптът е за **нова/празна** база `BikeStoreDb`. След този начин **не изпълнявайте** `Update-Database` с новата `InitialCreate` миграция върху същата вече изградена база: ще има конфликт. За следващите етапи предпочетете чиста база и миграции.

## Стъпка 3 – Вход и роли

- Влезте в приложението и използвайте **Регистрация** (стандартните ASP.NET Core Identity страници). Новите профили могат да разглеждат собствените си поръчки, но **още не получават автоматично ролята `Client`** – това е планирано за следващия етап.
- Създайте първи администратор безопасно с **User Secrets** (десен бутон върху проекта `BikeStore` → **Manage User Secrets**):

```json
{
  "Bootstrapping": {
    "AdminEmail": "your-private-admin@example.com",
    "AdminPassword": "Use-A-Strong-Unique-Password-123!"
  }
}
```

Заменете примерните стойности с лични стойности. Не копирайте този файл в GitHub. При следващо стартиране на приложението акаунтът ще бъде създаден (или ще получи роля Admin). Служебният панел е `/Admin`.

## Стъпка 4 – Проверки

От началния екран трябва да работят:

- `/` – начална страница;
- `/Bicycles` – публичен каталог и търсене;
- `/Bicycles/Details/1` – детайли на примерен велосипед;
- `/Rentals` – велосипеди, налични за наем (без форма за резервация на този етап);
- `/Orders` – защитена страница за поръчки;
- `/Admin` – само за Admin/Employee, начална статистика.

В **Test Explorer** пуснете `BikeStore.Tests` или от терминал:

```bash
dotnet restore BikeStore.sln
dotnet build BikeStore.sln
dotnet test BikeStore.sln
```

## Стъпка 5 – Качване в твоето готово GitHub хранилище

В терминала **в корена на вече клонираното хранилище**, след копиране на съдържанието:

```bash
git add .
git commit -m "Stage 1: concept, research, database design and MVC starter"
git push
```

Ако ползваш Visual Studio: **Git Changes → Stage All → Commit All → Push**. GitHub Actions ще се появи в таб **Actions**, след като workflow файлът бъде качен. Проверете за зелена успешна проверка. Кодът **не е качен автоматично** в GitHub от този пакет.

## Документация

- `docs/RESEARCH.md` – проверено сравнително проучване (15.11.2026).
- `docs/REQUIREMENTS.md` – цели, обхват, потребителски роли, критерии за приемане.
- `docs/ARCHITECTURE.md` – слоеве, компоненти, API/UI и защита.
- `docs/DATABASE.md` – ER диаграма, ограничения и начални таблици.
- `docs/ROADMAP.md` – етапи, рискове, план за реализация и тестване.

## Техническа бележка

Текущият проект е **кодова начална основа**, а не сертифицирана компилирана версия: пакетът е генериран в среда без инсталиран .NET SDK и SQL Server. След `Restore/Build` във Visual Studio направете локална проверка. Миграцията се генерира и БД се създава **на твоя компютър** – няма как да бъдат създадени дистанционно в локалния ти SSMS без достъп.