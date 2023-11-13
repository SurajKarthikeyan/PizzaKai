using UnityEngine;

/// <summary>
/// Contains extensions for paths.
/// 
/// <br/>
/// 
/// Authors: Ryan Chang (2023)
/// </summary>
public static class PathExt
{
	#region Methods
	/// <summary>
	/// Removes all verticies between two other verticies that don't add any
	/// informantion to the overall path.
	/// </summary>
	/// <param name="path">The <see cref="Path{T}<"/> to prune.</param>
	public static void PruneNodes(this Path<Vector3Int> path)
	{
		Vertex<Vector3Int> next = path.End;

		while (next != path.Start)
		{

		}
	}
	#endregion
}