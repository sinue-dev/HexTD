using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexChunk
{
	public static int size = 16;

	public static float xOffset = 2; //0.866f;
	public static float zOffset = 1.5f; //0.75f;

	public GameObject goChunk;
	public ChunkObject chunkObject;

	public ChunkState_e chunkState = ChunkState_e.DESPAWNED;
	public enum ChunkState_e
	{
		SPAWNED,
		DESPAWNED
	}

	public Dictionary<Hex, HexObject> grid;

	public HexChunk()
	{
		// Grid Dictionary initialisieren
		grid = new Dictionary<Hex, HexObject>();
	}

	public void InitializeChunk(int x, int z)
	{
		float xPos = x * (HexChunk.xOffset * HexChunk.size);
		float zPos = z * (HexChunk.zOffset * HexChunk.size);

		goChunk = new GameObject("Chunk " + x + "_" + z);
		goChunk.transform.position = new Vector3(xPos, 0, zPos);
		goChunk.transform.rotation = Quaternion.identity;
		goChunk.transform.SetParent(GridManager.I.transform);
		goChunk.tag = "Chunk";

		chunkObject = goChunk.AddComponent<ChunkObject>();
		chunkObject.InitializeChunkObject(new Chunk(x, z, xPos, zPos));
	}

	public void GenerateGrid()
	{
		for (int x = 0; x < size; x++)
		{
			for (int z = 0; z < size; z++)
			{

				float xPos = x * xOffset;
				float zPos = z * zOffset;

				// Are we on an odd row?
				if (z % 2 == 1)
				{
					xPos += xOffset / 2f;
				}

				InitHexObject(x, z, xPos, zPos, Hex.HexType_e.GRASS);
			}
		}
	}

	public void InitHexObject(int x, int z, float xPos, float zPos, Hex.HexType_e hexType)
	{
		//GameObject goHex = new GameObject("Hex " + x + "_" + z);
		//goHex.transform.parent = chunkObject.transform;
		//goHex.transform.localPosition = new Vector3(xPos, zPos);

		GameObject goHex = new GameObject("Hex " + x + "_" + z);
		goHex.transform.SetParent(goChunk.transform);
		goHex.transform.localPosition = new Vector3(xPos, 0, zPos);
		goHex.transform.rotation = Quaternion.identity;		
		goHex.tag = "Hex";
		goHex.AddComponent<BoxCollider>();

		HexObject hexObject = goHex.AddComponent<HexObject>();
		hexObject.InitializeHexObject(new Hex(x, z, xPos, zPos, hexType));

		if (!grid.ContainsKey(hexObject.hex))
			grid.Add(hexObject.hex, hexObject);
		else
			grid[hexObject.hex] = hexObject;
	}

}
