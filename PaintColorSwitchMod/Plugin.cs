using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx;
using HarmonyLib;
using PaintColorSwitchMod.Patches;

namespace PaintColorSwitchMod
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PaintColorSwitchModBase : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "Silvision.PaintColorSwitchMod";
        private const string PLUGIN_NAME = "Paint Color Switch Mod";
        private const string PLUGIN_VERSION = "1.0.0";
        
        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            harmony.PatchAll(typeof(PaintColorSwitchModBase));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
        }
    }
}