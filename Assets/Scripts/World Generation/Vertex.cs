using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public float3 position;
    public float3 normal;
    public float3 tangent;
    public Color color;
    public float3 texCoord0;

    public static NativeArray<VertexAttributeDescriptor> VertexLayout(Allocator allocator)
    {
        int vertexAttributeCount = 5;
        NativeArray<VertexAttributeDescriptor> vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
            vertexAttributeCount, allocator, NativeArrayOptions.UninitializedMemory
        );

        vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, dimension: 3);
        vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, dimension: 3);
        vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, dimension: 3);
        vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.Color, dimension: 4);
        vertexAttributes[4] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 3);
        
        return vertexAttributes;
    }
}