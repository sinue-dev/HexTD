using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : Singleton<GridManager>
{
	// WIDTH = 16
	// HEIGHT = 16
	public PointyHexagon pointyHexagonMesh;

	public int viewDistance = 2;

	public GameObject playerPrefab;
	public GameObject player;

	Dictionary<float[], HexChunk> chunks;

	void Start()
	{
		chunks = new Dictionary<float[], HexChunk>();

		if (player == null)
		{
			player = GameObject.Instantiate(playerPrefab, new Vector3(0, 0), Quaternion.identity) as GameObject;
		}
	}
	
	void Update()
	{
		if (player == null) return;

		int[] playerChunk = new int[2];
		playerChunk[0] = Mathf.FloorToInt(player.transform.position.x / HexChunk.size);
		playerChunk[1] = Mathf.FloorToInt(player.transform.position.z / HexChunk.size);

		for(int x = playerChunk[0] - viewDistance; x < playerChunk[0] + viewDistance; x++)
		{
			for(int z = playerChunk[1] - viewDistance; z < playerChunk[1] + viewDistance; z++)
			{
				bool spawn = true;
				foreach (KeyValuePair<float[], HexChunk> item in chunks)
				{
					float[] pos = item.Key;
					HexChunk chunk = item.Value;

					if (chunk == null) continue;

					if (chunk.chunkXPos == playerChunk[0] && chunk.chunkZPos == playerChunk[1])
					{
						if (chunk.chunkState == HexChunk.ChunkState_e.DESPAWNED)
						{
							SpawnChunk(chunk);
						}
						spawn = false;
					}
				}

				if (spawn)
				{
					HexChunk newChunk = new HexChunk(x, z);
					newChunk.GenerateGrid();
					SpawnChunk(newChunk);
					chunks.Add(new float[] { newChunk.chunkXPos, newChunk.chunkZPos }, newChunk);
				}
			}
		}
		
	}

	private void SpawnChunk(HexChunk chunk)
	{
		foreach(KeyValuePair<Hex, HexChunk> item in chunk)
		{

		}

		for(int x = 0; x < HexChunk.size; x++)
		{
			for(int z = 0; z < HexChunk.size; z++)
			{
				if(chunk.grid[x, z] != null)
				{
					GameObject goHex = new GameObject("Hex " + x + "_" + z);
					goHex.transform.position = new Vector3((chunk.chunkXPos * HexChunk.size) + x, 0, (chunk.chunkZPos * HexChunk.size) + z);
					goHex.transform.rotation = Quaternion.identity;
					goHex.transform.SetParent(chunk.chunkObject.transform);
					goHex.tag = "Hex";

					chunk.gridObjects[x, z] = goHex;

					goHex.AddComponent<BoxCollider>();
				}

			}
		}

		chunk.chunkState = HexChunk.ChunkState_e.SPAWNED;
	}

	

}
