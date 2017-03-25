using UnityEngine;

namespace Extensions
{
	public static class VectorExtensions
	{
		public static Vector3 Snap(this Vector3 vector, float snap)
		{
			vector.x = Mathf.Round(vector.x / snap) * snap;
			vector.y = Mathf.Round(vector.y / snap) * snap;
			vector.z = Mathf.Round(vector.z / snap) * snap;
			return vector;
		}
	}
}