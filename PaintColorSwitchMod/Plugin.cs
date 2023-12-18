using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BepInEx;
using HarmonyLib;
using PaintColorSwitchMod.Patches;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public static GameObject PaintColorSwitchNetworkHandlerPrefab;

        
        private readonly Harmony harmony = new Harmony(PLUGIN_GUID);

        // Taken from: [https://github.com/EvaisaDev/UnityNetcodeWeaver]
        private static void NetCodeWeaver()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
        
        private void Awake()
        {
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            
            // AssetBundle mainAssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("PaintColorSwitchMod.Properties.Resources.asset"));
            // PaintColorSwitchNetworkHandlerPrefab = mainAssetBundle.LoadAsset<GameObject>("PaintColorSwitchNetworkHandler");
            // mainAssetBundle.Unload(false);
            
            NetCodeWeaver();
            
            if (Instance == null)
            {
                Instance = this;
            }
            
            harmony.PatchAll(typeof(PaintColorSwitchModBase));
            harmony.PatchAll(typeof(NetworkObjectManager));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            // harmony.PatchAll(typeof(SprayPaintItemPatch));
            // harmony.PatchAll(typeof(GameNetworkManagerPatch));
        }
        
        
        
    }
    
    [HarmonyPatch]
    public class NetworkObjectManager
    {
        static GameObject networkPrefab;
        
        [HarmonyPostfix, HarmonyPatch(typeof(GameNetworkManager), "Start")]
        public static void Init()
        {
            if (networkPrefab != null)
                return;
            
            AssetBundle mainAssetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("PaintColorSwitchMod.Properties.Resources.asset"));
            networkPrefab = mainAssetBundle.LoadAsset<GameObject>("PaintColorSwitchNetworkHandler");
            mainAssetBundle.Unload(false);
            networkPrefab.AddComponent<PaintColorSwitchNetworkHandler>();
        
            NetworkManager.Singleton.AddNetworkPrefab(networkPrefab);
        }
    
        [HarmonyPostfix, HarmonyPatch(typeof(StartOfRound), "Awake")]
        static void SpawnNetworkHandler() {
            if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsServer) return;
            var networkHandlerHost = Object.Instantiate(networkPrefab, Vector3.zero, Quaternion.identity);
            networkHandlerHost.GetComponent<NetworkObject>().Spawn();
        }
    
    }
}