using System;
using UnityEngine;

/// <summary>
/// Represents a 6-faced NUMERICAL die.
/// </summary>
/// <remark>
/// Don't forget this class hasn't real side distribution, but numeric.
/// In any actual die, X'=(sides+1-X), where X' means X opposite side.
/// Thus, standard 6-faced dice have X'=(7-X). [1,6][2,5][3,4] are tuples.
/// But in this numerical-faced die, X'=(X+sides/2)%sides. [1,4][2,5][3,6] are tuples.
/// </remark>
public class Die : MonoBehaviour
{
	int side = 1;
	public Sprite[] sprites;
	SpriteRenderer spriteRenderer;

	RollingCube cube;

	#region Properties

	public int Side
	{
		get { return side; }
	}

	#endregion

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[side - 1]; //Internal value smashes inspector change.
		cube = GetComponent<RollingCube>();
	}

    /// <summary>
    /// Relatively sets die <see cref="side"/>. 
    /// </summary>
    /// <remarks>
    /// Don't forget <see cref="Die"/> hasn't real side distribution, but numeric.
    /// </remarks>
    /// <param name="increment">[-2|-1|+1|+2] adds to die current number. 0 flips over.</param>
    /// <example>
    /// <para><see cref="side"/> == 4, and <paramref name="increment"/> == -2, then <see cref="side"/> == 2.</para>
    /// <para><see cref="side"/> == 6, and <paramref name="increment"/> ==  0, then <see cref="side"/> == 1.</para>
    /// </example>
    /// <returns>Whether or not flip was effective. It is, in time.</returns>
    public bool Flip(int increment = 0)
	{
		if (Mathf.Abs(increment) > 2)
			throw new ArgumentOutOfRangeException("Unable to flip an increment of " + increment + "sides.");

        //Check whether flip is enabled or disable it.
        if(!cube.grounding)
            return false;
        cube.grounding = false;

		if (increment == 0)
			increment = 3;

		side += increment;
		if (side < 1)
			side += 6;
		else if (side > 6)
			side -= 6;
		spriteRenderer.sprite = sprites[side - 1];

		return true;
	}
}
