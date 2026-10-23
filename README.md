\# CrossApp



Наскрізний проєкт з крос-платформного програмування.



Предметна область: Склад. Сутності: Product (товар), StockBatch (партія), 

&#x09;			   Warehouse (склад), Movement (переміщення).



ризначення: застосунок призначений для обліку залишків товарів по партіях.



\##Запуск

dotnet build

dotnet run --project src/Cli



\## Середовище

.NET SDK 10.0, Windows 10 x64

## Self-contained publish



Порівняння розміру каталогів 'publish':



| RID | Розмір |
|---|---:|
| `win-x64` | 76.83 MB |
| `linux-x64` | 78.79 MB |


\## Структура solution

CrossApp/
    CrossApp.sln
    README.md
    .gitignore
    src/
        Core/
            Core.csproj
            EnvironmentInfo.cs
            EnvironmentReport.cs
        Cli/
            Cli.csproj
             Program.cs

\## Build

dotnet build

\## Run

dotnet run --project src/Cli

\## Publish

Self-contained: dotnet publish src/Cli -c Release -r win-x64 --self-contained true

Framework-dependent: dotnet publish src/Cli -c Release -r win-x64 --self-contained false

\## Порівняння


| RID      | Режим                | Розмір   | Потрібен .NET Runtime |
|----------|----------------------|----------|------------------------|
| win-x64  | Self-contained       | 76.8 MB  | Ні                     |
| win-x64  | Framework-dependent  | 194 KB   | Так                    |
| linux-x64| Self-contained       | 78.79 MB | Ні                     |


## Інваріанти домену

## Інваріанти домену

| Правило | Тип винятку | Метод |
|---|---|---|
| Ідентифікатор товару не може бути порожнім | `ArgumentException` | `Product.Create()` |
| SKU товару не може бути порожнім | `ArgumentException` | `Product.Create()` |
| Назва товару не може бути порожньою | `ArgumentException` | `Product.Create()` |
| Одиниця виміру не може бути порожньою | `ArgumentException` | `Product.Create()` |
| Початковий залишок товару не може бути від'ємним | `ArgumentOutOfRangeException` | `Product.Create()` |
| DTO не може бути `null` | `ArgumentNullException` | `Product.FromDto()` |
| SKU, назва, одиниця виміру та кількість у DTO не можуть бути `null` | `ArgumentException` | `Product.FromDto()` |
| Кількість приходу має бути більшою за нуль | `ArgumentOutOfRangeException` | `Product.RegisterArrival()` |
| Кількість видачі має бути більшою за нуль | `ArgumentOutOfRangeException` | `Product.Issue()` |
| Не можна видати більше товару, ніж є на складі | `InvalidOperationException` | `Product.Issue()` |
| Ідентифікатор замовлення не може бути порожнім | `ArgumentException` | `Order()` |
| Замовлення можна підтвердити або скасувати лише зі стану `Draft` | `InvalidOperationException` | `Order.GetNextStatus()` |
| Ідентифікатор примірника книги не може бути порожнім | `ArgumentException` | `BookCopy()` |
| Не можна видати вже виданий примірник | `InvalidOperationException` | `BookCopy.IssueTo()` |
| Не можна повернути невиданий примірник | `InvalidOperationException` | `BookCopy.Return()` |
| Ідентифікатор читача не може бути порожнім | `ArgumentException` | `Reader()` |
| Ім'я читача не може бути порожнім | `ArgumentException` | `Reader()` |
| Читач не може мати більше ніж 5 відкритих видач | `InvalidOperationException` | `LibraryService.IssueCopy()` |
| Примірник може повернути лише читач, якому його видано | `InvalidOperationException` | `LibraryService.ReturnCopy()` |
| Не можна зменшити кількість відкритих видач, якщо вона дорівнює нулю | `InvalidOperationException` | `Reader.CloseLoan()` |
| Читач і примірник не можуть бути `null` під час видачі або повернення | `ArgumentNullException` | `LibraryService.IssueCopy()`, `LibraryService.ReturnCopy()` |

