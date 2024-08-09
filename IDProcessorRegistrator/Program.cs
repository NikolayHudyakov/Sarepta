
using IDProcessorRegistrator;

Console.WriteLine($"CheckHash {RegIDProcessor.CheckHash()}");
Console.WriteLine("press c - CreateHash");

if (Console.ReadKey().Key == ConsoleKey.C)
    RegIDProcessor.CreateHash();