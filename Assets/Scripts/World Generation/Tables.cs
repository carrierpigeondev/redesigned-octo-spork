using Unity.Mathematics;

public static class Tables
    {
        /// <summary>
        /// UVs for a quad
        /// </summary>
        public static readonly float2[] UVs = new float2[4]
        {
            new float2(0, 0),
            new float2(0, 1),
            new float2(1, 0),
            new float2(1, 1),
        };

        /// <summary>
        /// All 8 possible vertices for a voxel
        /// </summary>
        public static readonly float3[] Vertices = new float3[8]
        {
            new float3(0.0f, 0.0f, 0.0f),
            new float3(1.0f, 0.0f, 0.0f),
            new float3(1.0f, 1.0f, 0.0f),
            new float3(0.0f, 1.0f, 0.0f),
            new float3(0.0f, 0.0f, 1.0f),
            new float3(1.0f, 0.0f, 1.0f),
            new float3(1.0f, 1.0f, 1.0f),
            new float3(0.0f, 1.0f, 1.0f),
        };

        /// <summary>
        /// right, left, up, down, front, back
        /// </summary>
        public static readonly float3[] Normals = new float3[6]
        {
            new float3(1.0f, 0.0f, 0.0f),
            new float3(-1.0f, 0.0f, 0.0f),
            new float3(0.0f, 1.0f, 0.0f),
            new float3(0.0f, -1.0f, 0.0f),
            new float3(0.0f, 0.0f, 1.0f),
            new float3(0.0f, 0.0f, -1.0f),
        };

        /// <summary>
        /// Indices to build all 6 quads
        /// </summary>
        public static readonly int[][] QuadVertices = new int[6][]
        {
            // quad order
            // right, left, up, down, front, back

            // 0 1 2 2 1 3 <- triangle numbers

            // quads
            new int[4] {1, 2, 5, 6}, // right quad
            new int[4] {4, 7, 0, 3}, // left quad

            new int[4] {3, 7, 2, 6}, // up quad
            new int[4] {1, 5, 0, 4}, // down quad

            new int[4] {5, 6, 4, 7}, // front quad
            new int[4] {0, 3, 1, 2}, // back quad
        };


        /// <summary>
        /// Helper to get neighboring voxels
        /// right0, left1, up2, down3, front4, back5
        /// </summary>
        public static readonly int3[] Neighbors = new int3[6]
        {
            new int3(1,  0, 0),
            new int3(-1, 0, 0),
            new int3(0,  1, 0),
            new int3(0, -1, 0),
            new int3(0, 0,  1),
            new int3(0, 0, -1),
        };
    }