using UnityEngine;
using Utils.Extensions;

public class SquareTransform : MonoBehaviour
{
	public int X
	{
		get
		{
			return (int)Builder.ToSquares(transform.position.x);
		}

		set
		{
			transform.position = transform.position.X(Builder.ToUnits(value));
		}
	}

	public int Y
	{
		get
		{
			return (int)Builder.ToSquares((int)transform.position.y);
		}

		set
		{
			transform.position = transform.position.Y(Builder.ToUnits(value));
		}
	}

    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }
    }

    public override string ToString()
    {
        return "[" + (int)X + "," + (int)Y + "]";
    }
}
