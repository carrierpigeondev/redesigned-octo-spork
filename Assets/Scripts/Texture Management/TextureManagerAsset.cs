using System;
using System.Collections;
using System.Collections.Concurrent;
using Unity.Collections;
using UnityEngine;

[Serializable]
public struct BlockType
{
    public string NameID;
    public Texture2D Texture;
}

[CreateAssetMenu(menuName = "TextureManager", fileName = "TextureManager")]
public class TextureManagerAsset : ScriptableObject, ISerializationCallbackReceiver
{
    public BlockType[] BlockTypes;
    public Shader Shader;
    public int TextureSize = 16;
    public FilterMode FilterMode = FilterMode.Point;

    private ConcurrentDictionary<string, byte> _blockTypes;
    public byte this[string blockName]
    {
        get
        {
            if (_blockTypes.TryGetValue(blockName, out byte index)) return index;
            else
            {
                Debug.LogWarning($"no block of name {blockName}");
                return 0;
            }
        }
    }

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize()
    {
        _blockTypes = new ConcurrentDictionary<string, byte>();
        _blockTypes = GetBlockMap();
    }

    public ConcurrentDictionary<string, byte> GetBlockMap()
    {
        for (int i = 0; i < BlockTypes.Length; i++)
            _blockTypes.TryAdd(BlockTypes[i].NameID, (byte)i);

        return _blockTypes;
    }

    public Material BlockMaterial
    {
        get
        {
            Material mat = new Material(Shader);
            mat.SetTexture("_TextureArray", GetTexture2DArray());
            return mat;
        }
    }

    private Texture2DArray GetTexture2DArray()
    {
        if (BlockTypes == null || BlockTypes.Length == 0)
        {
            Debug.LogError("No block typoes defined in TextureManager");
            return null;
        }

        Texture2DArray textureArray = new Texture2DArray(TextureSize, TextureSize, BlockTypes.Length,
            TextureFormat.RGBA32, true);

        for (int i = 0; i < BlockTypes.Length; i++)
        {
            if (BlockTypes[i].Texture == null) continue;
            if (BlockTypes[i].Texture.width != TextureSize ||
                BlockTypes[i].Texture.height != TextureSize)
            {
                Debug.LogError($"Texture size mismatch {BlockTypes[i].NameID}");
                continue;
            }

            Texture2D formattedTexture = FixTextureFormat(BlockTypes[i].Texture, TextureFormat.RGBA32);
            Graphics.CopyTexture(formattedTexture, 0, 0, textureArray, i, 0);
            if (formattedTexture != BlockTypes[i].Texture)
            {
                Destroy(formattedTexture);
            }
        }

        textureArray.Apply();
        textureArray.filterMode = FilterMode;
        return textureArray;
    }

    private Texture2D FixTextureFormat(Texture2D sourceTexture, TextureFormat desiredFormat)
    {
        if (sourceTexture.format == desiredFormat) return sourceTexture;

        RenderTexture rt = RenderTexture.GetTemporary(
            sourceTexture.width, sourceTexture.height, 0, RenderTextureFormat.ARGB32);

        Graphics.Blit(sourceTexture, rt);

        Texture2D convertedTexture = new Texture2D(
            sourceTexture.width, sourceTexture.height, desiredFormat, false);

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        convertedTexture.ReadPixels(new Rect(0, 0, sourceTexture.width, sourceTexture.height), 0, 0);
        convertedTexture.Apply();

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return convertedTexture;
    }
}