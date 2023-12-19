using System;
using Unity.Netcode;
using UnityEngine;

namespace PaintColorSwitchMod {
    public class PaintColorSwitchNetworkHandler : NetworkBehaviour {
        
        public static PaintColorSwitchNetworkHandler Instance { get; private set; }
        public static event Action<NetworkObjectReference> ColorChangeEvent;

        public override void OnNetworkSpawn() {
            ColorChangeEvent = null;
            
            if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer) {
                if (Instance != null && Instance.gameObject.GetComponent<NetworkObject>() != null) {
                    Instance.gameObject.GetComponent<NetworkObject>().Despawn();
                }
            }
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
            ColorChangeEvent?.Invoke(sprayPaintItemNOR);
        }

    }
}