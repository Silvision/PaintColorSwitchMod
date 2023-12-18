using System;
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
        public void EventServerRPC()
        {
            Debug.Log("Client sent a server RPC... EventServerRPC Fired!");
            EventClientRpc();
        }

        [ClientRpc]
        public void EventClientRpc() {
            Debug.Log("Server sent a client RPC... EventClientRpc Fired!");
        }

    }
}