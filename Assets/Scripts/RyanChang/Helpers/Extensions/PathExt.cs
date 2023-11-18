using System;
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
		var right = path.Next(left);

		while (right != path.End)
		{
			// Mystery code. 1 = X is same, 2 = Y is same, 3 = X/Y are both same
			// (error), 0 = none are same.
			short code = 0;

			// Check if the left, right, and current lie on the same x/y
			// coordinate.
			if (NumericalExt.AllEqual(left.id.x, right.id.x))
				code++;
			if (NumericalExt.AllEqual(left.id.y, right.id.y))
				code += 2;

			if (code > 2)
			{
				// X and Y are the same. Should not happen.
				throw new InvalidOperationException("X and Y coords are the same " +
					$"for {left} and {right}!");
			}

			var next = path.Next(right);

			while (next != path.End &&
				((NumericalExt.AllEqual(left.id.x, next.id.x) && code == 1) ||
				(NumericalExt.AllEqual(left.id.y, next.id.y) && code == 2)))
			{
				// Shimmy to the right.
				right = next;
				next = path.Next(right);
			}

			if (code > 0)
			{
				// Bridge the two nodes together.
				path.Intrasect(left, right);
			}

			// Move both nodes. Avoid infinite loop.
			left = path.Next(left);
			right = path.Next(left);
		}
	}
	#endregion
}