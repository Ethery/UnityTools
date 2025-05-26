using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorUtilities
{
	public static class Vector2Extensions
	{
		public static Vector2Int ToNewVector2Int(this Vector2 v) => new Vector2Int((int)v.x, (int)v.y);

		public static Vector3 ToNewVector3XZ(this Vector2 v) => new Vector3((int)v.x, 0, (int)v.y);

		public static Vector3 ToNewVector3XY(this Vector2 v) => new Vector3((int)v.x, (int)v.y, 0);
	}
}