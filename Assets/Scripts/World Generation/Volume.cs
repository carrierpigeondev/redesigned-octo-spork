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
        this.data = new byte[resolution * resolution * height];
    }
    
    public Volume(int resolution) : this(resolution, resolution) {}

    public byte this[int x, int y, int z]
    {
        get
        {
            #if UNITY_EDITOR
            CheckBounds(x, y, z);
            #endif
            
            return data[XyzToIndex(x, y, z, resolution, height)];
        }
        set
        {
            #if UNITY_EDITOR
            CheckBounds(x, y, z);
            #endif
            
            data[XyzToIndex(x, y, z, resolution, height)] = value;
        }
    }
    
    #if UNITY_EDITOR
    private void CheckBounds(int x, int y, int z)
    {
        if (x < 0 || x >= resolution || y < 0 || y >= height || z < 0 || z >= resolution)
            throw new IndexOutOfRangeException($"Index [{x},{y},{z}] out of range [{resolution}, {height}, {resolution}]");
    }
    #endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int XyzToIndex(int x, int y, int z, int resolution, int height)
    {
        return z * resolution * height + y * resolution + x;
    }

    public async Task SetForeach(Func<int3, byte> BlockAt)
    {
        if (data.Length == 0) return;
        Volume volume = this;

        await Task.Factory.StartNew(() =>
        {
            for (int x = 0; x < volume.Resolution; x++)
            for (int y = 0; y < volume.Height; y++)
            for (int z = 0; z < volume.Resolution; z++)
            {
                volume.data[XyzToIndex(x, y, z, volume.Resolution, volume.Height)] = BlockAt(new int3(x, y, z));
            }
        });
    }
}