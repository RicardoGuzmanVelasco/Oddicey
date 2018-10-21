using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Directions;

namespace Utils.Extensions
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

        #region Vector2Direction
        /// <summary>
        /// Sidescrolling vector direction to Direction enum type.
        /// </summary>
        /// <returns>Right if unknown.</returns>
        public static Direction ToDirection(this Vector2 vector)
        {
            if (vector == Vector2.right)
                return Direction.right;
            if (vector == Vector2.left)
                return Direction.left;

            return Direction.right;
        }
        #endregion
    }

    public static class GameObjectExtensions
    {
        #region Clone
        /// <summary>
        /// Duplicates the object using <see cref="GameObject.Instantiate"/>.
        /// </summary>
        /// <param name="pos"> The ABSOLUTE position of the clone. Same that parent by default.</param>
        /// <param name="name">The name of the clone. Namesake by default.</param>
        /// <returns>The clone.</returns>
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
        /// <para>The clone will take the same absolute position and transform sibling index.</para>
        /// </summary>
        /// <param name="name">The name of the clone. By default, namesake.</param>
        /// <returns>The clone.</returns>
        public static GameObject Clone(this GameObject gameobject, string name = null)
        {
            return gameobject.Clone(gameobject.transform.position, name);
        }
        #endregion

        #region Fork
        /// <summary>
        /// <para>Create a child of the given game object.</para>
        /// <para>The child will take the same absolute position (local origin).</para>
        /// </summary>
        /// <param name="name">The name of the clone.</param>
        /// <returns>The child created.</returns>
        public static GameObject CreateChild(this GameObject gameobject, string name)
        {
            GameObject child = new GameObject(name);
            child.transform.parent = gameobject.transform;
            child.transform.position = gameobject.transform.position;
            return child;
        }
        #endregion
    }

    public static class TransformExtensions
    {
        #region Destroy
        /// <summary>
        /// Destroy all children with <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Name of the children in hierarchy.</param>
        public static void DestroyChildren(this Transform transform, string name)
        {
            //Cast & reverse prevent destroying on iterating array.
            foreach (Transform child in transform.Cast<Transform>().Reverse())
                if (child.name == name)
                    GameObject.DestroyImmediate(child.gameObject);
        }

        /// <summary>
        /// Destroy all children with <paramref name="name"/> but one of them.
        /// </summary>
        /// <param name="name">Name of the children in hierarchy.</param>
        /// <returns>The survivor child, in order to check that existed children with <paramref name="name"/>.</returns>
        public static GameObject DestroyDuplicatedChildren(this Transform transform, string name)
        {
            GameObject template = transform.Find(name).gameObject;
            template.name = "name" + "_Temp";
            transform.DestroyChildren(name);
            template.name = name;
            return template;
        }
        #endregion
    }

    public static class EnumExtensions
    {
        /// <summary>
        /// Overrides* static <see cref="Enum"/>.HasFlag() functionality.
        /// <para>
        /// *This method was added in .NET framework 4.0 and higher,
        /// but returns odd values when involving enumerators with 0 as internal value. 
        /// </para>
        /// </summary>
        /// <remarks>
        /// Note that <see cref="Enum"/> type must has <see cref="FlagsAttribute"/>.
        /// <para>
        /// This function works as a <see cref="List{T}.Contains(T)"/> applied to flags.
        /// </para>
        /// </remarks>
        /// <param name="flag">Any of <paramref name="enumerators"/> type, concatenated with logic operators.</param>
        /// <returns>
        /// <see langword="true"/> if all <paramref name="flag"/> contained,
        /// as long as <paramref name="flag"/> and <paramref name="enumerators"/> are different to zero.
        /// </returns>
        public static bool HasFlag(this Enum notifications, Enum flag)
        {
            ulong keys = Convert.ToUInt64(notifications);
            ulong flags = Convert.ToUInt64(flag);
            return (keys & flags) == flags &&
                   (flags != 0 && keys != 0);
        }
    }

    public static class ListExtensions
    {
        /// <summary>
        /// Returns a random item from the list.
        /// Based on some useful extensions by omgwtfgames@GitHub. 
        /// </summary>
        /// <remarks><exception cref="System.IndexOutOfRangeException">Trown when size of the list is zero.</exception></remarks>
        /// <returns> </returns>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list.Count == 0)
                throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list.");
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}