using System.Runtime.InteropServices;
using System.Text.Json;

var info = new
{
    OSDescription = RuntimeInformation.OSDescription,
    OSVersion = Environment.OSVersion.ToString(),
    Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
    DotNetVersion = Environment.Version.ToString(),
    Runtime = RuntimeInformation.FrameworkDescription,
    ApplicationDirectory = AppContext.BaseDirectory,
    CurrentDirectory = Environment.CurrentDirectory,
    Domain = "Склад (товари, партії, залишки, переміщення)"
};

if (args.Contains("--json"))
{
    Console.WriteLine(JsonSerializer.Serialize(info));
}
else
{
    Console.WriteLine("CrossApp – практикум з крос-платформного програмування");
    Console.WriteLine("Студент: Цапович Петро, група ФЕІ-35");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"ОС : {info.OSDescription}");
    Console.WriteLine($"Версія ОС : {info.OSVersion}");
    Console.WriteLine($"Архітектура : {info.Architecture}");
    Console.WriteLine($"Версія .NET : {info.DotNetVersion}");
    Console.WriteLine($"Runtime : {info.Runtime}");
    Console.WriteLine($"Каталог застосунку : {info.ApplicationDirectory}");
    Console.WriteLine($"Поточний каталог : {info.CurrentDirectory}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"Предметна область: {info.Domain}");
}