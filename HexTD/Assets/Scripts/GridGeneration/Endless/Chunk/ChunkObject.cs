using UnityEngine;
using System.Collections;

public class ChunkObject : MonoBehaviour {

	public Chunk chunk;

	public void InitializeChunkObject(Chunk chunk)
	{
		this.chunk = chunk;
	}
}
