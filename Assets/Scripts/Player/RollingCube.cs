using System.Collections;
using UnityEngine;
using Utils.Directions;
using Utils.Extensions;

public class RollingCube : MonoBehaviour
{
    private bool falling = false;

    Vector2 pivot;

    /// <remarks>
    /// The collider, not the sprite, determines the actual size of the figure.
    /// </remarks>
    Vector2 extents;
    public float floor; //'y' value before roll.
    public bool rolling = false;

    [SerializeField]
    float currentRollRotation = 0f;

    const float error = 0.95f;
    /// <summary>
    /// Inverse of speed with error constant. Time to do a single roll.
    /// </summary>
    float rollingTime;

    /// <summary>
    /// Rolling direction. By default, it's assumed that <see cref="Direction.right"/> is the forward direction.
    /// </summary>
    public Direction direction = Direction.right;

    public bool grounding = false;
    /// <summary>
    /// Range within <see cref="grounding>"/>, so <see cref="Die.Flip(int)"/> is considered available.
    /// </summary>
    float threshold = 30; //Min degrees difference to consider grounding.

    #region Properties
    public float RollingTime
    {
        get
        {
            return rollingTime;
        }

        set
        {
            rollingTime = value * error;
        }
    }

    public bool Falling
    {
        get
        {
            return falling;
        }

        set
        {
            if(value && !falling)
                StartCoroutine(Fall());
            falling = value;
        }
    }
    #endregion

    void Awake()
    {
        extents = GetComponent<Collider2D>().bounds.extents;
    }

    /// <summary>
    /// How much the current rotation is far from the threshold.
    /// It is symmetrical, both late or early from grounding-considered.
    /// </summary>
    /// <remarks>
    /// A positive return means far from ground, so it's a negative result.
    /// But a negative return means into ground zone, so it's a positive result.
    /// Zero is the perfect result.
    /// </remarks>
    /// <returns>
    /// If positive, how much the rolling cube is out of ground <see cref="threshold"/>.
    /// If negative, how much the rolling cube is out of absolute ground, it is 0º rotation.
    /// If zero, the rolling cube is in absolute ground.
    /// </returns>
    public float DistanceFromGround()
    {
        if(currentRollRotation == 90 || currentRollRotation == 0)
            return 0;
        if(currentRollRotation < 90 - threshold)
            return OutOfTimeDistance();
        else
            return currentRollRotation - 90;
    }

    float OutOfTimeDistance()
    {
        //May this two OPTIONS can depend on the difficulty.
        //OPTION A) The later distances, the more punishment.
        //That's a great way of punish the late result, so risk a perfect result is such a hazard.
        //return 90 - threshold - currentRollRotation;
        //OPTION B) Punish equally late and early distances.
        //That's the real distance from grounding, but slightly punishes the late fail,
        //so trying to get closer to the perfect result takes a lower risk, maybe bad game design.
        if(currentRollRotation > (90 - threshold) / 2)
            return 90 - threshold - currentRollRotation;
        else
            return currentRollRotation;
    }

    public void Roll()
    {
        if(falling)
            return;

        if(rolling)
        {
            Debug.LogError("LOST BEEP");
            return;
        }

        bool reverse = direction != Direction.right;
        float pivotX = transform.position.x + extents.x * (reverse ? -1 : 1);
        float y = transform.position.y - extents.y;
        pivot = new Vector2(pivotX, y);
        StartCoroutine(Roll(reverse ? Vector3.back : Vector3.forward));
    }

    /// <summary>
    /// Coroutine which does a full square-based roll. It is, 90º roll.
    /// </summary>
    /// <remarks>
    /// Using <see cref="Time.fixedDeltaTime"/> because the roll is split into little chunks before start rolling.
    /// </remarks>
    /// <param name="axis">
    /// Although it only depends on <see cref="direction"/>, if coroutine just takes the attribute
    /// every yield instead of take it by parameter, it will not fix the axis of its rotation,
    /// and therefore the axis can change within the execution of a single roll and cause erratic behaviors.
    /// </param>
    IEnumerator Roll(Vector3 axis)
    {
        float instants = Mathf.Ceil(RollingTime / Time.fixedDeltaTime);
        float instantAngle = 90f / instants;

        //Set, reset and take values.
        rolling = true;
        currentRollRotation = 0f;
        floor = transform.position.y;

        grounding = false;
        for(int i = 0; i < instants; i++)
        {
            transform.RotateAround(pivot, axis, -instantAngle); //Left-hand rule.
            currentRollRotation += instantAngle;

            if(instantAngle * i >= 90 - threshold)
                grounding = true;
            yield return new WaitForFixedUpdate();
        }

        //Reset, free and snap properly.
        pivot = transform.position;
        Snap();
        rolling = false;
    }

    public void Snap()
    {
        transform.position = transform.position.XY(Mathf.RoundToInt(transform.position.x), floor);
        transform.eulerAngles = transform.eulerAngles.Snap(90);
    }

    IEnumerator Fall()
    {
        do
        {
            transform.Translate(Vector2.down * Builder.ToUnits(error / rollingTime) * Time.fixedDeltaTime, Space.World);
            yield return new WaitForFixedUpdate();
        } while(falling);
    }

    public void Turn()
    {
        direction = direction.Reverse();
        FindObjectOfType<Notifier>().NotificateTurn();
    }

    public void Turn(Vector2 newDir)
    {
        direction = newDir.ToDirection();
        FindObjectOfType<Notifier>().NotificateTurn();
    }
}