using UnityEngine;

namespace Utils.Directions
{
    /// <summary>
    /// 2D sidescrolling direction.
    /// </summary>
    public enum Direction
    {
        right,
        left
    }

    static class DirectionExtensions
    {
        /// <summary>
        /// Conversion to Vector2 sidescrolling direction.
        /// </summary>
        /// <returns>
        /// Vector2 on behalf of the direction.
        /// </returns>
        public static Vector2 ToVector2(this Direction dir)
        {
            switch (dir)
            {
                case Direction.left:
                    return Vector2.left;
                //break;
                case Direction.right:
                    return Vector2.right;
                    //break;
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Semantic turning around. 
        /// </summary>
        public static Direction Reverse(this Direction dir)
        {
            switch (dir)
            {
                case Direction.left:
                    dir = Direction.right;
                    break;
                case Direction.right:
                    dir = Direction.left;
                    break;
            }
            return dir;
        }
    }
}