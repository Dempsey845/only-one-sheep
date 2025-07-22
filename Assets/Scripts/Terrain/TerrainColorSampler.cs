using System;
using UnityEngine;

public class TerrainColorSampler : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private ParticleSystem walkParticles; 

    public Color[] terrainLayerColors;

    void Update()
    {
        // Get terrain texture index at the current position
        try
        {
            int textureIndex = GetMainTerrainTextureIndex(transform.position);

            // Assign corresponding colour to the particle system
            if (textureIndex >= 0 && textureIndex < terrainLayerColors.Length)
            {
                var main = walkParticles.main;
                main.startColor = terrainLayerColors[textureIndex];
            }
        }
        catch (ArgumentException)
        {
            Debug.LogWarning("Invalid argument for GetAlphaMaps");
        }
    }

    int GetMainTerrainTextureIndex(Vector3 worldPos)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        // Convert world position to splatmap coordinate
        int mapX = Mathf.FloorToInt((worldPos.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.FloorToInt((worldPos.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxMix = 0f;

        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            float mix = splatmapData[0, 0, i];
            if (mix > maxMix)
            {
                maxMix = mix;
                maxIndex = i;
            }
        }

        return maxIndex;
    }

    public void SetTerrain(Terrain terrain)
    {
        this.terrain = terrain;
    }
}
