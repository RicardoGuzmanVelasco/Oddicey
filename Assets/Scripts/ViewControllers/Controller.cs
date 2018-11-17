using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Event(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
