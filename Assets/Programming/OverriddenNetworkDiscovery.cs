using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OverriddenNetworkDiscovery : NetworkDiscovery
{

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.LogWarning(fromAddress + "    " + data);
        NetworkManager.singleton.networkAddress = fromAddress;

        // For now, don't join the first game you see
        // NetworkManager.singleton.StartClient();
    }
}