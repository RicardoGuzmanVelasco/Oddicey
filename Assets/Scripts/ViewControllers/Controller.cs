using System;
using UnityEngine;

public class Controller : MonoBehaviour
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

    public virtual void Event(string trigger, bool state)
    {
        throw new NotImplementedException();
    }
}
