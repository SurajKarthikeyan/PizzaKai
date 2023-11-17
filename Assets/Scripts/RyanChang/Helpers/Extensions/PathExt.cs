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
	/// Removes all vertices between two other vertices that don't add any
	/// information to the overall path.
	/// </summary>
	/// <param name="path">The <see cref="Path{T}<"/> to prune.</param>
	public static void PruneNodes(this Path<Vector3Int> path)
	{
		var left = path.Start;
		var curr = path.Next(left);

		while (curr != path.End)
		{
			var right = path.Next(curr);

			// Check if the left, right, and current lie on the same x/y
			// coordinate.
			if (NumericalExt.AllEqual(left.id.x, curr.id.x, right.id.x) ||
				NumericalExt.AllEqual(left.id.y, curr.id.y, right.id.y))
			{
				// Can remove this one.
				path.TryRemove(curr, left);
			}

			left = curr;
			curr = right;
		}
	}
	#endregion
}