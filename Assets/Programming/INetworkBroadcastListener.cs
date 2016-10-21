public interface INetworkBroadcastListener
{
    void OnReceivedBroadcast(string fromAddress, string data);
}