using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

public struct Volume
{
    private byte[] data;
    private readonly int resolution;
    private readonly int height;

    public int Length => data.Length;
    public int Resolution => resolution;
    public int Height => height;

    public Volume(int resolution, int height)
    {
        if (resolution < 0 || height < 0) throw new ArgumentException("Dimensions must be non-negative.");

        this.resolution = resolution;
        this.height = height;
        this.data = new byte[(resolution + 2) * (resolution + 2) * (height + 2)];
    }
    
    public Volume(int resolution) : this(resolution, resolution) {}

    public byte this[int x, int y, int z]
    {
        get
        {
            #if UNITY_EDITOR
            CheckBounds(x + 1, y + 1, z + 1);
            #endif
            
            return data[XyzToIndex(x + 1, y + 1, z + 1, resolution + 2, height + 2)];
        }
        set
        {
            #if UNITY_EDITOR
            CheckBounds(x + 1, y + 1, z + 1);
            #endif
            
            data[XyzToIndex(x + 1, y + 1, z + 1, resolution + 2, height + 2)] = value;
        }
    }
    
    #if UNITY_EDITOR
    private void CheckBounds(int x, int y, int z)
    {
        if (x < 0 || x >= resolution + 2 || y < 0 || y >= height + 2 || z < 0 || z >= resolution + 2)
            throw new IndexOutOfRangeException($"Index [{x},{y},{z}] out of range [{resolution}, {height}, {resolution}]");
    }
    #endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int XyzToIndex(int x, int y, int z, int resolution, int height)
    {
        return z * resolution * height + y * resolution + x;
    }

    public Task SetForeach(Func<int3, byte> BlockAt)
    {
        if (data.Length == 0) return Task.CompletedTask;
        Volume volume = this;

        return Task.Run(() =>
        {
            for (int x = -1; x < volume.Resolution + 1; x++)
            for (int y = -1; y < volume.Height + 1; y++)
            for (int z = -1; z < volume.Resolution + 1; z++)
            {
                volume.data[XyzToIndex(x + 1, y + 1, z + 1, volume.Resolution + 2, volume.Height + 2)] = BlockAt(new int3(x, y, z));
            }
        });
    }
}