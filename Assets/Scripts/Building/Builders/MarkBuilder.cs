using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MarkType
{
    Default,
    Lazy,
    Sleepy
}

[ExecuteInEditMode]
public class MarkBuilder : ObstacleBuilder<Mark>
{
    /// <summary>
    /// Target side of the mark when play starts.
    /// 0 means random side.
    /// </summary>
    [Tooltip("0 = randomize on play")]
    [SerializeField]
    [Range(0, 6)]
    int sideRequired = 0;
    /// <summary>
    /// What type the mark to build will be.
    /// </summary>
    [SerializeField]
    MarkType type = MarkType.Default;
    /// <summary>
    /// Sprites list the mark to build will have.
    /// </summary>
    /// <remarks>
    /// Must have size=6 and sprites orderer from 1 side to 6 side skins.
    /// </remarks>
    [SerializeField]
    Sprite[] sprites = new Sprite[6];
    /// <summary>
    /// Sprite to show in editor in case the mark will be randomized.
    /// </summary>
    [SerializeField]
    Sprite randomSpriteInEditor;

    #region Build assembly line
    protected override void UpdateAttachedTypeName()
    {
        AttachedTypeName = type.ToString();
    }

    protected override void AfterUpdateAttached()
    {
        attached.sprites = sprites;

        attached.randomSide = sideRequired == 0;
        attached.sideRequired = sideRequired;

        //Draw on edit mode if no random side and random sprite to perform in editor exists.
        Sprite newSprite = null;
        if(sideRequired != 0)
            newSprite = sprites[sideRequired - 1];
        else if (randomSpriteInEditor != null)
            newSprite = randomSpriteInEditor;
        attached.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
    #endregion
}
