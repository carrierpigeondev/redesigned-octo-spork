using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public static class Meshing
{
    private static readonly int[] QuadIndices = { 0, 1, 2, 2, 1, 3 };
    
    public static void CopyMeshUInt32( ref Mesh.MeshDataArray meshDataArray, ref NativeList<uint> indices, 
        ref NativeList<Vertex> vertices, ref Bounds bounds
    )
    {
        Mesh.MeshData meshData = meshDataArray[0];
        
        NativeArray<VertexAttributeDescriptor> layout = Vertex.VertexLayout(Allocator.Persistent);
        meshData.SetVertexBufferParams(vertices.Length, layout);
        layout.Dispose();
        meshData.GetVertexData<Vertex>().CopyFrom(vertices.AsArray());
        
        meshData.SetIndexBufferParams(indices.Length, IndexFormat.UInt32);
        meshData.GetIndexData<uint>().CopyFrom(indices.AsArray());

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, indices.Length)
        {
            bounds =  bounds,
            vertexCount = vertices.Length,
        }, MeshUpdateFlags.DontRecalculateBounds);
    }
    
    public static void CreateQuadUInt32(byte blockID, NativeList<Vertex> vertices, NativeList<uint> indices, int side, float3 offset)
    {
        foreach (int quadIndex in QuadIndices) indices.Add((uint)vertices.Length + (uint)quadIndex);

        int2 uvAxes = Tables.UVAxes[side];
        
        for (int vertexIndex = 0; vertexIndex < 4; vertexIndex++)
        {
            float3 voxelVertex = Tables.Vertices[Tables.QuadVertices[side][vertexIndex]] + offset;
            float2 uv = new float2(voxelVertex[uvAxes.x], voxelVertex[uvAxes.y]) * ChunkBehavior.BlockSize;
            
            vertices.Add(new Vertex
            {
                position = voxelVertex * ChunkBehavior.BlockSize,
                normal = Tables.Normals[side],
                texCoord0 = new float3(uv, blockID)
            });
        }
    }

    public static void CreateCubeUInt32(byte blockID, Mesh.MeshDataArray meshDataArray, Allocator allocator)
    {
        NativeList<Vertex> vertices = new NativeList<Vertex>(allocator);
        NativeList<uint> indices = new NativeList<uint>(allocator);

        Bounds bounds = new Bounds(Vector3.one / 2, Vector3.one);

        for (int side = 0; side < 6; side++) CreateQuadUInt32(blockID, vertices, indices, side, new float3(0, 0, 0));
        
        CopyMeshUInt32(ref meshDataArray, ref indices, ref vertices, ref bounds);

        vertices.Dispose();
        indices.Dispose();
    }

    public static void CreateChunkUInt32(Volume volume, Mesh.MeshDataArray meshDataArray, Allocator allocator)
    {
        NativeList<Vertex> vertices = new NativeList<Vertex>(allocator);
        NativeList<uint> indices = new NativeList<uint>(allocator);
        
        Vector3 size = new Vector3(volume.Resolution, volume.Height, volume.Resolution) * ChunkBehavior.BlockSize;
        Bounds bounds = new Bounds(size / 2, size);
        
        for (int x = 0; x < volume.Resolution; x++)
        for (int y = 0; y < volume.Height; y++)
        for (int z = 0; z < volume.Resolution; z++)
        {
            float3 offset = new float3(x, y, z);
            byte blockID = volume[x, y, z];

            if (IsAir(volume, x, y, z)) continue;
            
            for (int side = 0; side < 6; side++) 
                if (NeighborIsAir(volume, x, y, z, side))
                    CreateQuadUInt32(blockID, vertices, indices, side, offset);
        }
        
        CopyMeshUInt32(ref meshDataArray, ref indices, ref vertices, ref bounds);
        
        vertices.Dispose();
        indices.Dispose();
    }

    public static bool IsAir(Volume volume, int x, int y, int z)
    {
        return volume[x, y, z] == 0;
    }

    public static bool NeighborIsAir(Volume volume, int x, int y, int z, int side)
    {
        int3 offset = Tables.Neighbors[side];
        return IsAir(volume, x + offset.x, y + offset.y, z + offset.z);
    }
}