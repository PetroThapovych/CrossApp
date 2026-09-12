
using System.Text.Json;
using Core;


var report = EnvironmentInfo.Collect();

var info = new
{
    OSDescription = report.OsDescription,
    Architecture = report.ProcessArchitecture,
    Runtime = report.FrameworkDescription,
    DetectedRid = report.DetectedRid,
    ReportedRid = report.ReportedRid,
    ApplicationDirectory = report.BaseDirectory,
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
    Console.WriteLine($"Архiтектура : {info.Architecture}");
    Console.WriteLine($"Runtime : {info.Runtime}");
    Console.WriteLine($"RID (визначено програмою): {info.DetectedRid}");
    Console.WriteLine($"RID (повiдомлено .NET): {info.ReportedRid}");
    Console.WriteLine($"Каталог застосунку : {info.ApplicationDirectory}");

    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"Предметна область: {info.Domain}");
}