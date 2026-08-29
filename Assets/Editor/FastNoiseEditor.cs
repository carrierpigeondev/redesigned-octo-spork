using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FastNoiseAsset))]
public class FastNoiseEditor : Editor
{
    private Texture2D previewTexture;
    private int previewSize = 128;

    public override void OnInspectorGUI()
    {
        FastNoiseAsset settings = (FastNoiseAsset)target;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        if (previewTexture != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("Preview (2D Slice)");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Preview Resolution", EditorStyles.label);
            previewSize = EditorGUILayout.IntSlider(previewSize, 16, 256);
            EditorGUILayout.EndHorizontal();

            float inspectorWidth = EditorGUIUtility.currentViewWidth - 30;
            
            GUILayout.Box(previewTexture, GUILayout.Width(inspectorWidth), GUILayout.Height(inspectorWidth));
        }

        if (EditorGUI.EndChangeCheck() || previewTexture == null)
        {
            UpdatePreview(settings);
        }
    }

    private void UpdatePreview(FastNoiseAsset settings)
    {
        if (previewTexture == null || previewSize != previewTexture.width)
        {
            previewTexture = new Texture2D(previewSize, previewSize);
            previewTexture.filterMode = FilterMode.Point;
        }

        FastNoiseLite noise = settings.GetNoiseStruct();
        Color[] colors = new Color[previewSize * previewSize];
        for (int y = 0; y < previewSize; y++)
        for (int x = 0; x < previewSize; x++)
        {
            float noiseValue = noise.GetNoise(x, y);
            noiseValue = (noiseValue + 1f) * 0.5f;
            colors[y * previewSize + x] = new Color(noiseValue, noiseValue, noiseValue);
        }
        
        previewTexture.SetPixels(colors);
        previewTexture.Apply();
    }

    private void OnDisable()
    {
        if (previewTexture != null)
        {
            DestroyImmediate(previewTexture);
        }
    }
}