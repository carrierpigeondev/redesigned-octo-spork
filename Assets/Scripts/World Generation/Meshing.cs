using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

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
    
    public static void CreateQuadUInt32(NativeList<Vertex> vertices, NativeList<uint> indices, int side, float3 offset)
    {
        foreach (int quadIndex in QuadIndices) indices.Add((uint)vertices.Length + (uint)quadIndex);
        
        for (int vertexIndex = 0; vertexIndex < 4; vertexIndex++) vertices.Add(new Vertex {
            position = Tables.Vertices[Tables.QuadVertices[side][vertexIndex]] + offset,
            normal = Tables.Normals[side],
        });
    }

    public static void CreateCubeUInt32(Mesh.MeshDataArray meshDataArray, Allocator allocator)
    {
        NativeList<Vertex> vertices = new NativeList<Vertex>(allocator);
        NativeList<uint> indices = new NativeList<uint>(allocator);

        Bounds bounds = new Bounds(Vector3.one / 2, Vector3.one);

        for (int side = 0; side < 6; side++) CreateQuadUInt32(vertices, indices, side, new float3(0, 0, 0));
        
        CopyMeshUInt32(ref meshDataArray, ref indices, ref vertices, ref bounds);

        vertices.Dispose();
        indices.Dispose();
    }

    public static void CreateChunkUInt32(int resolution, int height, Mesh.MeshDataArray meshDataArray,
        Allocator allocator
    )
    {
        NativeList<Vertex> vertices = new NativeList<Vertex>(allocator);
        NativeList<uint> indices = new NativeList<uint>(allocator);
        
        Vector3 size = new Vector3(resolution, height, resolution);
        Bounds bounds = new Bounds(size / 2, size);
        
        for (int x = 0; x < resolution; x++)
        for (int y = 0; y < height; y++)
        for (int z = 0; z < resolution; z++)
        {
            float3 offset = new float3(x, y, z);
            for (int side = 0; side < 6; side++) CreateQuadUInt32(vertices, indices, side, offset);
        }
        
        CopyMeshUInt32(ref meshDataArray, ref indices, ref vertices, ref bounds);
        
        vertices.Dispose();
        indices.Dispose();
    }
}