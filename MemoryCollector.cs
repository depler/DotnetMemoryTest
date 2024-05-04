using System.Diagnostics;

namespace DotnetTest3;

public static class MemoryCollector
{
    public static void Run()
    {
        Task.Factory.StartNew(CollectGarbage, TaskCreationOptions.LongRunning);
    }

    private static void CollectGarbage()
    {
        Thread.Sleep(2000);
        Console.Clear();

        ShowConfig();
        Console.WriteLine("=====Tracking memory=====");

        while (true)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            using var process = Process.GetCurrentProcess();

            var heapMb = GC.GetTotalMemory(false) / 1_000_000;
            var processMb = process.PrivateMemorySize64 / 1_000_000;

            Console.WriteLine($"Memory MB: process {processMb}, heap {heapMb}");

            Thread.Sleep(1000);
        }
    }

    private static void ShowConfig()
    {
        var gc = GC.GetConfigurationVariables();
        var memory = GC.GetGCMemoryInfo();

        Console.WriteLine("=====GC config=====");

        foreach (var (key, value) in gc)
            Console.WriteLine($"{key}={value}");

        Console.WriteLine("=====System info=====");
        Console.WriteLine($"Processor count: {Environment.ProcessorCount}");
        Console.WriteLine($"Available memory: {memory.TotalAvailableMemoryBytes / 1_000_000} MB");
    }
}
