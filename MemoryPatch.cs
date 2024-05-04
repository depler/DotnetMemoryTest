using HarmonyLib;
using System.Buffers;
using System.Reflection;

namespace DotnetTest3;

public static class MemoryPatch
{
    private static readonly Harmony Harmony = new(nameof(MemoryPatch));

    public static void Install()
    {
        AppDomain.CurrentDomain.AssemblyLoad += AssemblyLoad;
    }

    private static void AssemblyLoad(object? sender, AssemblyLoadEventArgs args)
    {
        var type = args.LoadedAssembly.GetType("System.Buffers.PinnedBlockMemoryPoolFactory");
        if (type == null)
            return;

        var method = type.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);
        Harmony.Patch(method, new HarmonyMethod(CreateMemoryPool));
    }

    private static bool CreateMemoryPool(ref MemoryPool<byte> __result)
    {
        __result = new MemoryBlockPool();
        return false;
    }
}
