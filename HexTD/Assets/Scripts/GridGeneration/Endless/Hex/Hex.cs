using UnityEngine;
using System.Collections;

public class Hex
{
	public readonly int x;
	public readonly int z;
	public float xPos;
	public float zPos;

	public HexType_e hexType;

	public enum HexType_e
	{
		NONE,
		GRASS,
		SNOW,
		STONE,
		WATER
	}

	public Hex(int x, int z, float xPos, float zPos, HexType_e hexType)
	{
		this.x = x;
		this.z = z;
		this.xPos = xPos;
		this.zPos = zPos;

		this.hexType = hexType;
	}

	public override bool Equals(object obj)
	{
		if (obj == null) return false;
		if (this.GetType() != obj.GetType()) return false;

		Hex hex = (Hex)obj;
		return (this.x == hex.x) && (this.z == hex.z);
	}

	public override int GetHashCode()
	{
		return this.x.GetHashCode() ^ this.z.GetHashCode();
	}

	public Hex[] GetNeighbours()
	{

		// So if we are at x, y -- the neighbour to our left is at x-1, y
		GameObject leftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + z);

		// Right neighbour is also easy to find.
		GameObject right = GameObject.Find("Hex_" + (x + 1) + "_" + z);

		// The problem is that our neighbours to our upper-left and upper-right
		// might be at x-1 and x, OR they might be at x and x+1, depending
		// on whether we our on an even or odd row (i.e. if y % 2 is 0 or 1)

		return null;
	}

}