using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum CheckerMsg
{
    Starting,
    Error,
    Finished
}

public class SceneChecker
{
    [MenuItem("Tools/Scene Checks/Find floating actors %#&f")]
    public static void FindFloatingActors()
    {
        LogCheck(CheckerMsg.Starting, "Find floating actors in scene");

        HashSet<GameObject> actors = new HashSet<GameObject>();
        HashSet<Vector2> floors = new HashSet<Vector2>();

        //Get all applicable actors in scene (builders and die atm).
        foreach(var builder in GameObject.FindObjectsOfType<Builder>())
            if(!IsPlatform(builder))
                actors.Add(builder.gameObject);
        actors.Add(GameObject.FindObjectOfType<Die>().gameObject);

        //Get all floor positions.
        foreach(var gameObject in GameObject.FindGameObjectsWithTag("Floor"))
            floors.Add(gameObject.transform.position);

        //Check if all applicable actors has any floor just below.
        foreach(var actor in actors)
            if(!floors.Contains(actor.transform.position))
                LogCheck(CheckerMsg.Error, "", "Actor " + actor + " is floating in scene.");

        LogCheck(CheckerMsg.Finished, "Find floating actors in scene");
    }

    static void LogCheck(CheckerMsg type, string checkTest, string msg = "")
    {
        msg = "[" + checkTest + "] " + msg;
        switch(type)
        {
            case CheckerMsg.Error:
                Debug.LogError(msg);
                break;
            case CheckerMsg.Starting:
            case CheckerMsg.Finished:
                Debug.Log(msg + type.ToString() + ".");
                break;
            default:
                break;
        }
    }

    static bool IsPlatform(Builder builder)
    {
        return builder.GetComponentInParent<PlatformBuilder>();
    }

}
