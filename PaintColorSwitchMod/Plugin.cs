using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using PaintColorSwitchMod.Patches;
using UnityEngine;

namespace PaintColorSwitchMod
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PaintColorSwitchModBase : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "Silvision.PaintColorSwitchMod";
        private const string PLUGIN_NAME = "Paint Color Switch Mod";
        private const string PLUGIN_VERSION = "1.0.0";
        
        public static PaintColorSwitchModBase Instance;

        
        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        // Taken from: [https://github.com/EvaisaDev/UnityNetcodeWeaver]
        // private void NetCodeWeaver()
        // {
        //     var types = Assembly.GetExecutingAssembly().GetTypes();
        //     foreach (var type in types)
        //     {
        //         var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        //         foreach (var method in methods)
        //         {
        //             var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
        //             if (attributes.Length > 0)
        //             {
        //                 method.Invoke(null, null);
        //             }
        //         }
        //     }
        // }
        
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            
            // NetCodeWeaver();
            
            if (Instance == null)
            {
                Instance = this;
            }
            
            harmony.PatchAll(typeof(PaintColorSwitchModBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
        }
    }
}