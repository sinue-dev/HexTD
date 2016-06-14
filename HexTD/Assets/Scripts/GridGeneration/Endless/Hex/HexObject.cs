using UnityEngine;
using System.Collections;

public class HexObject : MonoBehaviour {

	public Hex hex;

	public MeshFilter meshFilter;
	public MeshCollider meshCollider;
	public MeshRenderer meshRenderer;

	public void InitializeHexObject(Hex hex)
	{
		this.hex = hex;
	}


}
