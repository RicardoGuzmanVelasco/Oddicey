using System;
using System.Collections;
using UnityEngine;
using Utils.Extensions;
using Utils.StaticExtensions;

/// <summary>
/// Any builder is a helper with square, tiles info
/// which will be deleted when playing.
/// </summary>
[ExecuteInEditMode]
public abstract class ObstacleBuilder<T> : Builder
{
    /// <summary>
    /// <see cref="T"/> component to build.
    /// </summary>
    protected T attached;
    /// <summary>
    /// Name of the runtime <see cref="Type"/> of <see cref="attached"/>.
    /// Needs to be set by child builder.
    /// </summary>
    /// <example>
    /// <see cref="VaneBuilder"/>:<see cref="ObstacleBuilder"/>{<see cref="Vane"/>}
    /// will have an "<see cref="AttachedTypeName"/> = <see cref="VaneBuilder.type"/>".
    /// </example>
    string attachedTypeName;
    /// <summary>
    /// On set, handles the name received, supposing it's a child builder type enumerator.
    /// </summary>
    /// <example>
    /// If it receives <see cref="VaneType.Flipping"/>, <see cref="T"/> type will be <see cref="Vane"/>,
    /// so it concatenates both strings to get the actual type.
    /// It uses <see cref="StringExtensions.Trim(string, string)"/> with "Default" string from the final name.
    /// </example>
    public string AttachedTypeName
    {
        get
        {
            return attachedTypeName;
        }

        set
        {
            attachedTypeName = value.Trim("Default")+TypeExtensions.GetStaticType<T>(attached);
        }
    }

    #region Build assembly line
    protected override void Build()
    {
        base.Build();

        UpdateAttachedTypeName();

        BeforeUpdateAttached();
        UpdateAttached();
        AfterUpdateAttached();
        UpdateName();
    }

    /// <summary>
    /// Abstract because it needs <see cref="attachedTypeName"/> to be updated.
    /// </summary>
    /// <example>
    /// <seealso cref="VaneBuilder.type"/>, <seealso cref="VaneBuilder.UpdateAttachedTypeName"/>.
    /// </example>
    abstract protected void UpdateAttachedTypeName();
    virtual protected void BeforeUpdateAttached() { }
    void UpdateAttached()
    {
        DestroyImmediate(attached as UnityEngine.Object);

        Type type = TypeExtensions.GetStaticType<T>(attached).GetSubClass(attachedTypeName);
        // If this operation return null, type is the obstacle's class itself, not a subclass.
        if(type == null)
            type = TypeExtensions.GetStaticType<T>(attached);
        gameObject.AddComponent(type);

        attached = GetComponent<T>();
    }
    virtual protected void AfterUpdateAttached() { }
    #endregion

    void OnValidate()
    {
        attached = GetComponent<T>();
        rebuild = true;
    }

    #region Name in hierarchy
    ///<summary>
    ///<seealso cref="Builder.UpdateName"/>.
    ///In this case, instead of builder type it uses <see cref="attached"/> type.
    ///</summary>
    protected override string BaseName()
    {
        return attached.GetType().ToString();
    }
    #endregion
}