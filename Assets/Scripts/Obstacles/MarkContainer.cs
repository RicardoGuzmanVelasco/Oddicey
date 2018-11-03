using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkContainer : MonoBehaviour
{
    /// <summary>
    /// The <see cref="Mark"/> component this GameObject contains as child.
    /// </summary>
    public Mark Mark { get; private set; }

    void Awake()
    {
        Mark = GetComponentInChildren<Mark>();
    }
}
