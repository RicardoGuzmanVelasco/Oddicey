using System.Linq;
using UnityEngine;

namespace Utils
{
	namespace Extensions
	{
		public static class VectorExtensions
		{
			#region Snap
			/// <summary>
			/// Rounds <paramref name="vector"/> components to <paramref name="vector"/> coordinates respectively.
			/// </summary>
			/// <param name="snaps">Coordinates to round.</param>
			/// <returns></returns>
			public static Vector3 Snap(this Vector3 vector, Vector3 snaps)
			{
				vector.x = Mathf.Round(vector.x / snaps.x) * snaps.x;
				vector.y = Mathf.Round(vector.y / snaps.y) * snaps.y;
				vector.z = Mathf.Round(vector.z / snaps.z) * snaps.z;
				return vector;
			}

			/// <summary>
			/// Rounds <paramref name="vector"/> components to <paramref name="x"/>, <paramref name="y"/>, <paramref name="z"/> respectively.
			/// </summary>
			/// <param name="x">x round target.</param>
			/// <param name="y">y round target.</param>
			/// <param name="z">z round target.</param>
			/// <returns></returns>
			public static Vector3 Snap(this Vector3 vector, float x, float y, float z)
			{
				return vector.Snap(new Vector3(x, y, z));
			}

			/// <summary>
			/// Rounds <paramref name="vector"/> components to <paramref name="snap"/>.
			/// </summary>
			/// <param name="snap">Round target.</param>
			/// <returns></returns>
			public static Vector3 Snap(this Vector3 vector, float snap)
			{
				return vector.Snap(new Vector3(snap, snap, snap));
			}
			#endregion

			#region Coords
			/// <summary>
			/// Returns same vector with 'x' changed to parameter <paramref name="x"/>.
			/// </summary>
			/// <param name="x">New 'x' coordinate.</param>
			/// <returns></returns>
			public static Vector3 X(this Vector3 vector, float x)
			{
				return new Vector3(x, vector.y, vector.z);
			}

			/// <summary>
			/// Returns same vector with 'y' changed to parameter <paramref name="y"/>.
			/// </summary>
			/// <param name="y">New 'y' coordinate.</param>
			/// <returns></returns>
			public static Vector3 Y(this Vector3 vector, float y)
			{
				return new Vector3(vector.x, y, vector.z);
			}

			/// <summary>
			/// <para>Returns same vector with
			/// 'x' changed to parameter <paramref name="x"/> and 
			/// 'y' changed to parameter <paramref name="y"/>.
			/// </para>
			/// <para>Frequently used in 2D environments.</para>
			/// </summary>
			/// <param name="x">New 'x' coordinate.</param>
			/// <param name="y">New 'y' coordinate</param>
			/// <returns></returns>
			public static Vector3 XY(this Vector3 vector, float x, float y)
			{
				return new Vector3(x, y, vector.z);
			}

			/// <summary>
			/// Returns same vector with 'z' changed to parameter <paramref name="z"/>.
			/// </summary>
			/// <param name="z">New 'z' coordinate.</param>
			/// <returns></returns>
			public static Vector3 Z(this Vector3 vector, float z)
			{
				return new Vector3(vector.x, vector.y, z);
			}
			#endregion
		}

		public static class GameObjectExtensions
		{
			#region Clone
			/// <summary>
			/// Duplicates the object using <see cref="GameObject.Instantiate"/>.
			/// </summary>
			/// <param name="pos"> The position of the clone. By default, the same position.</param>
			/// <param name="name">The name of the clone. By default, namesake.</param>
			public static GameObject Clone(this GameObject gameobject, Vector3 pos, string name = null)
			{
				if (name == null)
					name = gameobject.name;

				GameObject clone = GameObject.Instantiate(gameobject, gameobject.transform.parent);
				clone.name = name;
				clone.transform.position = pos;

				return clone;
			}

			/// <summary>
			/// <para>Duplicates the object using <see cref="GameObject.Instantiate"/>.</para>
			/// <para>The clone will take the same position.</para>
			/// </summary>
			/// <param name="name">The name of the clone. By default, namesake.</param>
			public static GameObject Clone(this GameObject gameobject, string name = null)
			{
				return gameobject.Clone(gameobject.transform.position, name);
			}
			#endregion
		}

		public static class TransformExtensions
		{
			/// <summary>
			/// Destroy all childs with <paramref name="name"/>.
			/// </summary>
			/// <param name="name">String pattern.</param>
			public static void DestroyChildren(this Transform transform, string name)
			{
				//Cast & reverse prevent destroying on iterating array.
				foreach (var child in transform.Cast<Transform>().Reverse())
					if (child.name == name)
						GameObject.DestroyImmediate(child.gameObject);
			}
		}
	}
}