using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils.Extensions;
using Utils.StaticExtensions;

namespace Utils.EditorExtensions
{
    enum CheckerMsg
    {
        Starting,
        Error,
        Warning,
        Finished
    }

    public class SceneChecker
    {
        /// <summary>
        /// Check all tests hereinafter.
        /// </summary>
        /// <hotkey>CTRL+SHIFT+ALT+A</hotkey>
        [MenuItem(MenuItems.CheckAll + " " + MenuItems.HotkeyCtrlShiftAlt + "a", false, MenuItems.Priority)]
        public static void CheckAll()
        {
            LogCheck(CheckerMsg.Starting, "ALL CHECKINGS ARE RUNNING");
            FindFloatingActors();
            FindOverlappingObstacles();
            LogCheck(CheckerMsg.Finished, "ALL CHECKINGS FINISHED");
        }


        /// <summary>
        /// Check all the applicable actors are on the ground (it is, a platform), not floating in the void.
        /// </summary>
        /// <hotkey>CTRL+SHIFT+ALT+F</hotkey>
        [MenuItem(MenuItems.FindFloatingActors + " " + MenuItems.HotkeyCtrlShiftAlt + "f")]
        public static void FindFloatingActors()
        {
            LogCheck(CheckerMsg.Starting, "Find floating actors in scene");

            HashSet<GameObject> actors = new HashSet<GameObject>();
            HashSet<Vector2> floors = new HashSet<Vector2>();

            //Get all applicable actors in scene (no platforms, just builders and die).
            foreach(var builder in GameObject.FindObjectsOfType<Builder>())
                if(!builder.GetComponentInParent<PlatformBuilder>()) //Not applicable if platform.
                    actors.Add(builder.gameObject);
            actors.Add(GameObject.FindObjectOfType<Die>().gameObject);

            //Get all floor positions.
            foreach(var gameObject in GameObject.FindGameObjectsWithTag("Floor"))
                floors.Add(gameObject.transform.position);

            //Check if all applicable actors has any floor just below.
            foreach(var actor in actors)
                if(!floors.Contains(actor.transform.position))
                    LogCheck(CheckerMsg.Error, "Actor " + actor.name + " is floating in scene");

            LogCheck(CheckerMsg.Finished, "Find floating actors in scene");
        }

        /// <summary>
        /// Check all squares are populated by a single applicable actor at once.
        /// </summary>
        /// <hotkey>CTRL+SHIFT+ALT+O</hotkey>
        [MenuItem(MenuItems.FindOverlappingObstacles + " " + MenuItems.HotkeyCtrlShiftAlt + "o")]
        public static void FindOverlappingObstacles()
        {
            LogCheck(CheckerMsg.Starting, "Find overlapping obstacles in scene");

            HashSet<Vector2> actors = new HashSet<Vector2>();

            //Get all positions of applicable builders in scene (neither platforms nor marks).
            foreach(var builder in GameObject.FindObjectsOfType<Builder>())
                if(!builder.GetComponent<MarkBuilder>() && !builder.GetComponentInParent<PlatformBuilder>())
                    if(!actors.Add(builder.transform.position)) //If set already owned this position, an overlapping exists.
                        LogCheck(CheckerMsg.Error, "Overlap exists in " + (Vector2)builder.transform.position);

            LogCheck(CheckerMsg.Finished, "Find overlapping obstacles in scene");
        }

        #region Log
        static void LogCheck(CheckerMsg type, string msg)
        {
            msg += ".";
            switch(type)
            {
                case CheckerMsg.Warning:
                    Debug.LogWarning(msg);
                    break;
                case CheckerMsg.Error:
                    Debug.LogError(msg);
                    break;
                case CheckerMsg.Starting:
                case CheckerMsg.Finished:
                    Debug.Log(type.ToString() + ": " + msg);
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}