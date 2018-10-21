using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Format a GameObject name as a separator.
/// </summary>
/// <remarks>
/// It takes the GameObject name when attached or enabled.
/// Then it formats with 2*<see cref="level"/> concatenated <see cref="separator"/> characters.
/// Lastly, it set GameObject actived to <see langword="false"/>.
/// </remarks>
[ExecuteInEditMode]
public class HierarchySeparator : MonoBehaviour
{
    const int minLevel = 1, maxLevel = 5;
    const char separator = '=';

    [SerializeField]
    string hierarchyName = "NONAME";

    //TODO: autoformat bool attribute. If true, level is maxlevel - HierarchySeparatorParentsCount.

    [SerializeField]
    [Range(minLevel,maxLevel)]
    [Tooltip("Greater level gets more hierarchy relevance.")]
    int level = maxLevel;

    private void Start()
    {
        throw new System.TypeLoadException("A Separator gameobject mustn't be actived.");
    }

    private void OnEnable()
    {
        if (transform.childCount > 0)
            throw new System.FormatException("A separator can't have any child.");

            hierarchyName = gameObject.name;
    }

    void OnValidate()
    {
        gameObject.name = new string(separator, level * 2) + " " + hierarchyName;
        gameObject.SetActive(false);
    }
}
