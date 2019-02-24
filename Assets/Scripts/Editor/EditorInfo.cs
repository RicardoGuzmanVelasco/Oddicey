using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace Utils.EditorExtensions
{
    public static class Tags
    {

    }

    public static class SortingLayers
    {

    }

    public static class Layers
    {

    }

    public static class MenuItems
    {
        /// <summary>
        /// Base multiplier to add a separator between menu items.
        /// </summary>
        /// <remarks>
        /// To pin upper any menu item, just multiply this const by the required priority (...3 > 2 > 1).
        /// </remarks>
        public const int Priority = 50;

        public const string Tools = "Tools/";
            public const string SceneChecker = Tools + "Scene checks/";
                public const string CheckAll = SceneChecker + "Check all";
                public const string FindFloatingActors = SceneChecker + "Find floating actors";
                public const string FindOverlappingObstacles = SceneChecker + "Find overlapping obstacles";
        #region Hotkeys
        public const string HotkeyCtrl = "%";
        public const string HotkeyShift = "#";
        public const string HotkeyAlt = "&";
        public const string HotkeyNoCommands = "_";
        public const string HotkeyCtrlShift = HotkeyCtrl + HotkeyShift;
        public const string HotkeyCtrlAlt = HotkeyCtrl + HotkeyAlt;
        public const string HotkeyCtrlShiftAlt = HotkeyCtrl + HotkeyShift + HotkeyAlt;

        /// <summary>
        /// Hotkey character combinations are added to the end of the menu item path, preceded by a space.
        /// This function is only for mnemonic purposes. It couldn't be used as consts can't use methods to be defined.
        /// </summary>
        /// <remarks>
        /// <paramref name="hotkeys"/> must contain only elements declared in this class as Hotkey- consts.
        /// </remarks>
        /// <param name="key"> Character representing the keyboard key to attach the hotkey.</param>
        /// <param name="hotkeys">Optional sequence of commands to attach the hotkey.</param>
        /// <seealso cref="https://unity3d.com/es/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items."/>
        public static string Hotkey(char key, params string[] hotkeys)
        {
            string commands = " ";
            foreach(var command in hotkeys)
                commands += command;

            return (commands.HasContent() ? commands : "_") + key; 
        }
        #endregion
    }
}