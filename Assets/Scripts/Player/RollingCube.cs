using System.Collections;
using System.Linq;
using UnityEngine;
using Utils.Directions;
using Utils.Extensions;

public class RollingCube : MonoBehaviour
{
    bool falling = false;

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
    public float RollingDistanceFromGround()
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
        if(!falling) //Prevents snap 1 frame after begin falling.
            Snap();
        rolling = false;
    }

    public void Snap()
    {
        floor = Mathf.RoundToInt(floor); //Avoids accumulated error.
        transform.position = transform.position.XY(Mathf.RoundToInt(transform.position.x), floor);
        transform.eulerAngles = transform.eulerAngles.Snap(90);
    }

    /// <summary>
    /// Fast fall, as on a gravity-based environment. 
    /// </summary>
    /// <remarks>
    /// The fall towards floor below or BOTTOM boundary takes exactly
    /// 1 <see cref="Notification.Beep"/>, regardless the total distance of the fall.
    /// </remarks>
    /// <example>
    /// Fall from y=0 to y=-112 takes 1 <see cref="Notification.Beep"/>.
    /// Fall from y=0 to y=-4 also takes 1 <see cref="Notification.Beep"/>.
    /// Fall to BOTTOM boundary, wherever it is, also takes 1 <see cref="Notification.Beep"/>.
    /// </example>
    /// <exception cref="System.InvalidOperationException">
    /// When current position of <see cref="RollingCube"/> is lower than the target floor.
    /// </exception>
    IEnumerator Fall()
    {
        float distance = Mathf.RoundToInt(transform.position.y - LookForGround().y);
        if(distance < 0)
            throw new System.InvalidOperationException("Falling upwards in such an oxymoron.");

        float speed = distance / rollingTime;
        float fallAdvance = speed * Time.fixedDeltaTime;

        do
        {
            distance -= fallAdvance; //Remaining distance decreases as long as fall advances.
            if(distance < 0) //Last iteration.
                fallAdvance += distance; //Prevents die beneath the target floor after last advance.

            transform.Translate(Vector2.down * fallAdvance, Space.World);

            yield return new WaitForFixedUpdate();
        } while(falling);
        Snap();
    }

    //TODO: static in utilities class.
    Vector2 LookForGround()
    {
        Vector2 ground = new Vector2();
        bool found = false;

        //The order of collisions is not guaranteed by Unity.
        RaycastHit2D[] hitsOrderedByDistance = Physics2D.RaycastAll(transform.position, Vector2.down);
        hitsOrderedByDistance = hitsOrderedByDistance.OrderBy(h => h.distance).ToArray();

        foreach(var hit in hitsOrderedByDistance)
            if(hit.collider.gameObject.CompareTag("Boundary")) //BOTTOM. No platform below die. So fall until death.
            {
                //TODO: if bottom, then camera must stop its Y following. Could be made by a scene trigger that is created yet.
                found = true;
                ground = hit.collider.transform.position;
            }
            else if(hit.collider.gameObject.CompareTag("Floor")) //First platform below die. It is, die grounding goal.
            {
                found = true;
                ground = hit.collider.transform.position;
                break;
            }

        if(!found)
            throw new MissingReferenceException("No BOTTOM boundary neither any platform below die were found.");

        return ground;
    }

    /// <summary>
    /// Simple falling at <see cref="Notification.Beep"/> rhythm.
    /// </summary>
    /// <remarks>
    /// One <see cref="Notification.Beep"/> means one <see cref="Builder.SquareSize> down translation.
    /// </remarks>
    IEnumerator FallOnRhythm()
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