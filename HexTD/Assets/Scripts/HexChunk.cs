using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexChunk
{
	public static int size = 16;

	public int chunkXPos;
	public int chunkZPos;

	static float xOffset = 0.866f;
	static float zOffset = 0.75f;

	public GameObject chunkObject;

	public ChunkState_e chunkState = ChunkState_e.DESPAWNED;
	public enum ChunkState_e
	{
		SPAWNED,
		DESPAWNED
	}

	public Dictionary<Hex, HexObject> grid;
	public GameObject[,] gridObjects;

	public HexChunk(int chunkXPos, int chunkZPos)
	{
		this.chunkXPos = chunkXPos;
		this.chunkZPos = chunkZPos;

		GameObject goChunk = new GameObject("Chunk " + chunkXPos + "_" + chunkZPos);
		goChunk.transform.position = new Vector3((chunkXPos * HexChunk.size), 0, (chunkZPos * HexChunk.size));
		goChunk.transform.rotation = Quaternion.identity;
		goChunk.transform.SetParent(GridManager.I.transform);
		goChunk.tag = "Chunk";

		chunkObject = goChunk;

		grid = new Dictionary<Hex, HexObject>();
		gridObjects = new GameObject[size, size];
	}

	public void GenerateGrid()
	{
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{

				float xPos = x * xOffset;
				float zPos = y * zOffset;

				// Are we on an odd row?
				if (y % 2 == 1)
				{
					xPos += xOffset / 2f;
				}

				InitHexObject(x, y, xPos, zPos);
			}
		}
	}

	public void InitHexObject(int x, int z, float xPos, float zPos)
	{
		GameObject goHex = new GameObject("Hex " + x + "_" + z);
		goHex.transform.parent = chunkObject.transform;
		goHex.transform.localPosition = new Vector3(xPos, zPos);

		HexObject hexObject = goHex.AddComponent<HexObject>();
		hexObject.InitializeHexObject(new Hex(x, z, xPos, zPos));

		grid.Add(hexObject.hex, hexObject);
	}

}
