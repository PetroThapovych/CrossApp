\# CrossApp



Наскрізний проєкт з крос-платформного програмування.



Предметна область: Склад. Сутності: Product (товар), StockBatch (партія), 

&#x09;			   Warehouse (склад), Movement (переміщення).



ризначення: застосунок призначений для обліку залишків товарів по партіях.



\##Запуск

dotnet build

dotnet run --project src/Cli



\## Середовище

.NET SDK 10.0, Windows 11 x64

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