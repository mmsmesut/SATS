using DbUp;
using System.Reflection;

var connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=mms;Search Path=mlshop";

var assembly = Assembly.GetExecutingAssembly();
var resourceNames = assembly.GetManifestResourceNames();

Console.WriteLine("Embedded resources found:");
foreach (var resourceName in resourceNames)
{
    Console.WriteLine(resourceName);
}

var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(assembly)
    .LogScriptOutput() // This will log the content of the scripts as they are executed
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;
