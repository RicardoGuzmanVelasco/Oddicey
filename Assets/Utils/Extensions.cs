using UnityEngine;

namespace Utils
{
	namespace Extensions
	{
		namespace VectorExtensions
		{
			public static class VectorExtensions
			{
				#region Snap
				public static Vector3 Snap(this Vector3 vector, Vector3 snaps)
				{
					vector.x = Mathf.Round(vector.x / snaps.x) * snaps.x;
					vector.y = Mathf.Round(vector.y / snaps.y) * snaps.y;
					vector.z = Mathf.Round(vector.z / snaps.z) * snaps.z;
					return vector;
				}

				public static Vector3 Snap(this Vector3 vector, float x, float y, float z)
				{
					return vector.Snap(new Vector3(x, y, z));
				}

				public static Vector3 Snap(this Vector3 vector, float snap)
				{
					return vector.Snap(new Vector3(snap, snap, snap));
				}
				#endregion

				#region Coords
				public static Vector3 X(this Vector3 vector, float x)
				{
					return new Vector3(x, vector.y, vector.z);
				}

				public static Vector3 Y(this Vector3 vector, float y)
				{
					return new Vector3(vector.x, y, vector.z);
				}

				public static Vector3 XY(this Vector3 vector, float x, float y)
				{
					return new Vector3(x, y, vector.z);
				}

				public static Vector3 Z(this Vector3 vector, float z)
				{
					return new Vector3(vector.x, vector.y, z);
				}
				#endregion
			}
		}
	}
}