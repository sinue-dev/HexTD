using UnityEngine;
using System.Collections;

public class HexObject : MonoBehaviour {

	public enum EdgeCell { middle, west, nw_corner, northwest, north, n_corner, northeast, ne_corner, east, se_corner, southeast, south, s_corner, southwest, sw_corner };

	public enum CellType { EMPTY, WORLD }

	public Hex hex;

	public MeshFilter meshFilter;
	public MeshCollider meshCollider;
	public MeshRenderer meshRenderer;

	public void InitializeHexObject(Hex hex)
	{
		this.hex = hex;

		this.meshFilter = gameObject.AddComponent<MeshFilter>();
		this.meshFilter.sharedMesh = GridManager.I.pointyHexagonMesh.mesh;

		this.meshCollider = gameObject.AddComponent<MeshCollider>();
		this.meshRenderer = gameObject.AddComponent<MeshRenderer>();

		GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load("Materials/HexGrass", typeof(Material));
	}


}
