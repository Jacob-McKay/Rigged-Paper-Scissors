using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class OverriddenNetworkDiscovery : NetworkDiscovery
{
    private List<INetworkBroadcastListener> _listeners = new List<INetworkBroadcastListener>();

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        base.OnReceivedBroadcast(fromAddress, data);
        foreach (var listener in _listeners)
        {
            listener.OnReceivedBroadcast(fromAddress, data);
        }

        Debug.LogWarning(fromAddress + "    " + data);

        // for now, don't join the first game you see
        //networkmanager.singleton.networkaddress = fromaddress;
        // networkmanager.singleton.startclient();
    }

    public void AddListener(INetworkBroadcastListener newListener)
    {
        _listeners.Add(newListener);
    }
}