﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GenerateTerrain : MonoBehaviour{

	/* Parameters to conrol terrain generation */
	[Range(30,50)]
	public int mapSizeSetting = 50;
	public int seed = 1;
	[Range(1,100)]
	public float scale = 1f;
	[Range(0,10)]
	public int numOctaves = 2;
	[Range(0,1)]
	public float persistance = 0.5f;
	[Range(0,10)]
	public float lacunarity = 0.5f;
	public bool useTerrainColors = true;
	public bool autoGenerate = false;
	[Range(10,100)]
	public int maxMapHeight = 30;
	[Range(0,3)]
	public int chunkRenderDistance;

	private PerlinNoise noiseGenerator;
	private int mapSize = 5;
	private TerrainType[] terrains; 
	private ProceduralTerrain terrain;

	GenerateTerrain()
	{
		/* Define the different types of terrain based on height */
		terrains = new TerrainType[5];

		Color32[] seaColors = new Color32[1];
		seaColors[0] = new Color32(10, 50, 255, 255);

		Color32[] beachColors = new Color32[2];
		beachColors[0] = new Color32(255, 255, 0, 255);
		beachColors[1] = new Color32(255, 255, 51, 255);

		Color32[] landColors = new Color32[4];
		/* Each terrain type can get multiple colours to add variety to it */
		landColors[0] = new Color32(5, 128, 50, 255);
		landColors[1] = new Color32(15, 74, 29, 255);
		landColors[2] = new Color32(11, 150, 87, 255);
		landColors[3] = new Color32(22, 102, 17, 255);

		Color32[] mountainColors = new Color32[2];
		mountainColors[0] = new Color32(103, 110, 93, 255);
		mountainColors[1] = new Color32(79, 87, 71, 255);

		Color32[] mountainCapColors = new Color32[1];
		mountainColors[0] = new Color32(207, 207, 207, 255);

		terrains[0] = new TerrainType(seaColors, 0.35f);
		terrains[1] = new TerrainType(beachColors, 0.4f);
		terrains[2] = new TerrainType(landColors, 0.75f);
		terrains[3] = new TerrainType(mountainColors, 0.9f);
		terrains[4] = new TerrainType(mountainCapColors, 1.0f);
	}

	public void Start()
	{
		drawMap();
	}

	public void ClearMap()
	{
		object[] allRootObjects = GameObject.FindObjectsOfType(typeof (GameObject));
    	GameObject[] terrainsToDelete = new GameObject[allRootObjects.Length];

    	int terrainsFound = 0;

		foreach (object currentObject in allRootObjects)
		{
		    GameObject currentGameObject = (GameObject) currentObject;

		    if(currentGameObject.name == ProceduralTerrain.OBJECTTYPENAME)
		    {
		    	terrainsToDelete[terrainsFound] = currentGameObject;
		    	terrainsFound++;
		    }
		}

		allRootObjects = null;

		for(int i = terrainsFound - 1; i >= 0; i--)
			GameObject.DestroyImmediate(terrainsToDelete[i]);

		terrainsToDelete = null;

		Debug.Log(terrainsFound + " " + ProceduralTerrain.OBJECTTYPENAME + "'s deleted from scene");
	}

	public void drawMap()
	{
		mapSize = mapSizeSetting * 5;
		noiseGenerator = new PerlinNoise(mapSize, mapSize, seed, scale, numOctaves, persistance, lacunarity);
		transform.localScale = new Vector3(5, 1, 5);

		ClearMap();

		terrain = new ProceduralTerrain(noiseGenerator, chunkRenderDistance, terrains, maxMapHeight);
		terrain.Render(useTerrainColors);
	}
}
