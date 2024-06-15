using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorUtilities
{
	public static class Vector3Extensions
	{
		public static Vector3Int CastToNewVector3Int(this Vector3 v) => new Vector3Int((int)v.x, (int)v.y, (int)v.z);
	}
}