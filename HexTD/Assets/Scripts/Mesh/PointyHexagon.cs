using UnityEngine;

/// Class for Hexagon mesh building
public class PointyHexagon
{
	public Mesh mesh = new Mesh();
	private Vector3[] vertices;
	private int[] triangles;
	private Vector2[] uv;

	/// Builds a hexagon mesh with 6 triangles
	public PointyHexagon(bool separateFaceVertices, float cellSize)
	{
		#region verts

		float floorLevel = 0;
		vertices = new Vector3[]
		{
			new Vector3(-1f , floorLevel, -.5f),
			new Vector3(-1f, floorLevel, .5f),
			new Vector3(0f, floorLevel, 1f),
			new Vector3(1f, floorLevel, .5f),
			new Vector3(1f, floorLevel, -.5f),
			new Vector3(0f, floorLevel, -1f)
		};

		#endregion
		mesh.vertices = vertices;

		#region triangles

		triangles = new int[]
		{
			1,5,0,
			1,4,5,
			1,2,4,
			2,3,4
		};

		#endregion
		mesh.triangles = triangles;

		#region UV

		uv = new Vector2[]
		{
			new Vector2(0,0.25f),
			new Vector2(0,0.75f),
			new Vector2(0.5f,1),
			new Vector2(1,0.75f),
			new Vector2(1,0.25f),
			new Vector2(0.5f,0),
		};

		#endregion
		mesh.uv = uv;


		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.Optimize();
	}
}