using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{
	public const int mapChunkSize = 241;
	[Range(0, 6)]
	public int levelOfDetail;
	public float noiseScale;
	[Range(0, 15)]
	public int octaves;
	[Range(0, 1)]
	public float persistance;
	[Range(1, 100)]
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	public TerrainType[] regions;
	//
	public float minHeight
	{
		get
		{
			return noiseScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
		}
	}

	public float maxHeight
	{
		get
		{
			return noiseScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
		}
	}
	//
	public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
		//float[,] noiseMap = Diamond_square.Generate();
		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++)
		{
			for (int x = 0; x < mapChunkSize; x++)
			{
				float currentHeight = noiseMap[x, y];
				for (int i = 0; i < regions.Length; i++)
				{
					if (currentHeight <= regions[i].height)
					{
						colourMap[y * mapChunkSize + x] = regions[i].colour;
						break;
					}
				}
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay>();
		
		display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));

	}

	void OnValidate()
	{
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
	}
}

[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public Color colour;
}