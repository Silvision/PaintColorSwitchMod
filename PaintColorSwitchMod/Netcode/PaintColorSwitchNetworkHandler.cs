using System;
using System.Reflection;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace PaintColorSwitchMod {
    public class PaintColorSwitchNetworkHandler : NetworkBehaviour {
        
        public static PaintColorSwitchNetworkHandler Instance { get; private set; }

        public override void OnNetworkSpawn() {
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
                Instance?.gameObject.GetComponent<NetworkObject>().Despawn();
            Instance = this;
            
            base.OnNetworkSpawn();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void EventServerRpc(NetworkObjectReference sprayPaintItemNOR)
        {
            Debug.Log("Client sent a server RPC... EventServerRPC Fired!");
            EventClientRpc(sprayPaintItemNOR);
        }

        [ClientRpc]
        public void EventClientRpc(NetworkObjectReference sprayPaintItemNOR) {
            Debug.Log("Server sent a client RPC... EventClientRpc Fired!");
            
            // Code to alter SprayPaint variables below
            if (sprayPaintItemNOR.TryGet(out NetworkObject networkObject)) {
                SprayPaintItem sprayPaintItem = networkObject.GetComponent<SprayPaintItem>();

                FieldInfo fieldInfo = AccessTools.Field(typeof(SprayPaintItem), "sprayCanMatsIndex");
                int colorIndex = (int)fieldInfo.GetValue(sprayPaintItem);
                
                if ( colorIndex < sprayPaintItem.sprayCanMats.Length - 1) {
                    colorIndex++;
                } else {
                    colorIndex = 0;
                }
                fieldInfo.SetValue(sprayPaintItem, colorIndex);
                sprayPaintItem.sprayParticle.GetComponent<ParticleSystemRenderer>().material = sprayPaintItem.particleMats[colorIndex];
                sprayPaintItem.sprayCanNeedsShakingParticle.GetComponent<ParticleSystemRenderer>().material = sprayPaintItem.particleMats[colorIndex];
                
                
            }
            
        }

    }
}