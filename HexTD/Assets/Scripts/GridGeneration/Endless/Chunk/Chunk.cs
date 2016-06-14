using UnityEngine;
using System.Collections;

public class Chunk
{
	// Our coordinates in the map array
	public int x;
	public int z;
	public float xPos;
	public float zPos;

	public Chunk(int x, int z, float xPos, float zPos)
	{
		this.x = x;
		this.z = z;
		this.xPos = xPos;
		this.zPos = zPos;
	}

	public override bool Equals(object obj)
	{
		if (obj == null) return false;
		if (this.GetType() != obj.GetType()) return false;

		Chunk hex = (Chunk)obj;
		return (this.x == hex.x) && (this.z == hex.z);
	}

	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ this.z.GetHashCode();
	}
}
