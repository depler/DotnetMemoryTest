using System.Buffers;
using System.Collections.Concurrent;

namespace DotnetTest3;

public sealed class MemoryBlock : IMemoryOwner<byte>
{
    public Memory<byte> Memory => _data;

    private MemoryBlockPool _pool;
    private byte[]? _data;

    public MemoryBlock(MemoryBlockPool pool, int length)
    {
        _pool = pool;
        _data = GC.AllocateUninitializedArray<byte>(length);
    }

    public void Dispose()
    {
        if (!_pool.TryReturn(this))
            _data = null;
    }
}

public sealed class MemoryBlockPool : MemoryPool<byte>
{
    private const int MaxMemory = 1024 * 1024 * 100; //100mb
    private const int MaxBlockSize = 1024 * 128; //128kb
    private const int MaxBlockCount = MaxMemory / MaxBlockSize; 

    private static readonly ConcurrentQueue<MemoryBlock> Blocks = new();

    public override int MaxBufferSize => MaxBlockSize;

    public override IMemoryOwner<byte> Rent(int size)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(size, MaxBufferSize);

        return Blocks.TryDequeue(out var block) ? block : new MemoryBlock(this, MaxBufferSize);
    }

    public bool TryReturn(MemoryBlock block)
    {
        if (Blocks.Count < MaxBlockCount)
        {
            Blocks.Enqueue(block);
            return true;
        }

        return false;
    }

    protected override void Dispose(bool disposing)
    {

    }
}