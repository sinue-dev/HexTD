using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GridManager : Singleton<GridManager>
{
	// WIDTH = 16
	// HEIGHT = 16
	public PointyHexagon pointyHexagonMesh;

	public int viewDistance = 1;

	public GameObject playerPrefab;
	public GameObject player;

	Dictionary<Chunk, HexChunk> chunks;

	Dictionary<Hex.HexType_e, string> dictMaterials;

	void Start()
	{
		chunks = new Dictionary<Chunk, HexChunk>();

		LoadHexMaterials();

		pointyHexagonMesh = new PointyHexagon();

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

		List<Chunk> chunksKeys;

		for (int x = playerChunk[0] - viewDistance; x <= playerChunk[0] + viewDistance; x++)
		{
			for(int z = playerChunk[1] - viewDistance; z <= playerChunk[1] + viewDistance; z++)
			{
				bool spawn = true;

				chunksKeys = new List<Chunk>(chunks.Keys);
				foreach (Chunk key in chunksKeys)
				{
					Chunk chunk = key;
					HexChunk hexChunk = chunks[key];

					if (hexChunk == null) continue;

					if (hexChunk.chunkObject.chunk.x == x && hexChunk.chunkObject.chunk.z == z)
					{
						if (hexChunk.chunkState == HexChunk.ChunkState_e.DESPAWNED)
						{
							hexChunk.InitializeChunk(x, z);
							hexChunk.GenerateGrid();
							SpawnChunk(hexChunk);
						}
						spawn = false;
					}
				}

				if (spawn)
				{
					HexChunk newHexChunk = new HexChunk();
					newHexChunk.InitializeChunk(x, z);
					newHexChunk.GenerateGrid();
					SpawnChunk(newHexChunk);
					chunks.Add(newHexChunk.chunkObject.chunk, newHexChunk);
				}
			}
		}

		chunksKeys = new List<Chunk>(chunks.Keys);
		foreach (Chunk key in chunksKeys)
		{
			Chunk chunk = key;
			HexChunk hexChunk = chunks[key];

			if ((hexChunk.chunkObject.chunk.x < playerChunk[0] - viewDistance || hexChunk.chunkObject.chunk.x > playerChunk[0] + viewDistance) || (hexChunk.chunkObject.chunk.z < playerChunk[1] - viewDistance || hexChunk.chunkObject.chunk.z > playerChunk[1] + viewDistance))
			{
				if (hexChunk.chunkState == HexChunk.ChunkState_e.SPAWNED) DespawnChunk(hexChunk);
			}
		}
		
	}

	#region MaterialDatabase Functions

	private void LoadHexMaterials()
	{
		dictMaterials = new Dictionary<Hex.HexType_e, string>();

		dictMaterials.Add(Hex.HexType_e.GRASS, "Materials/HexGrass");
		dictMaterials.Add(Hex.HexType_e.SNOW, "Materials/HexSnow");
		dictMaterials.Add(Hex.HexType_e.STONE, "Materials/HexStone");
		dictMaterials.Add(Hex.HexType_e.WATER, "Materials/Water");
	}

	private Material LoadFromHexMaterials(Hex.HexType_e hexType)
	{
		if(dictMaterials.ContainsKey(hexType))
		{
			string sMaterialPath = "";
			dictMaterials.TryGetValue(hexType, out sMaterialPath);

			if(sMaterialPath != "")
			{
				Material material = Resources.Load(sMaterialPath, typeof(Material)) as Material;
				return material;
			}			
		}
		return null;
	}

	public Hex.HexType_e GetHexMaterialType(string sMaterialName)
	{
		return (Hex.HexType_e)dictMaterials.FirstOrDefault(x => x.Value.Contains(sMaterialName)).Key;
	}

	private void SyncCurrentMaterial(HexObject hexObject)
	{
		Material material = hexObject.GetComponent<Renderer>().material;
		if (material != null)
		{
			string sMaterialName = material.name;
			hexObject.hex.hexType = GetHexMaterialType(sMaterialName);
		}		
	}

	#endregion

	private void SpawnChunk(HexChunk chunk)
	{
		//chunk.InitializeChunk()

		List<Hex> hexKeys = new List<Hex>(chunk.grid.Keys);
		foreach (Hex key in hexKeys)
		{
			Hex hex = key;

			if (hex != null)
			{
				if (chunk.grid.ContainsKey(key))
				{
					HexObject hexObject = chunk.grid[key];

					if (hexObject != null)
					{
						hexObject.meshFilter = hexObject.gameObject.AddComponent<MeshFilter>();
						hexObject.meshFilter.sharedMesh = GridManager.I.pointyHexagonMesh.mesh;

						hexObject.meshCollider = hexObject.gameObject.AddComponent<MeshCollider>();
						hexObject.meshRenderer = hexObject.gameObject.AddComponent<MeshRenderer>();

						hexObject.GetComponent<Renderer>().sharedMaterial = LoadFromHexMaterials(hexObject.hex.hexType);
					}
				}
			}
		}

		chunk.chunkState = HexChunk.ChunkState_e.SPAWNED;
	}

	private void DespawnChunk(HexChunk chunk)
	{
		List<Hex> hexKeys = new List<Hex>(chunk.grid.Keys);
		foreach (Hex key in hexKeys)
		{
			Hex hex = key;
			HexObject hexObject = chunk.grid[key];

			if (hex != null && hexObject != null)
			{
				SyncCurrentMaterial(hexObject);

				GameObject.Destroy(hexObject.transform.gameObject);
			}
		}

		GameObject.Destroy(chunk.chunkObject.transform.gameObject);
		chunk.goChunk = null;

		chunk.chunkState = HexChunk.ChunkState_e.DESPAWNED;
	}
}
