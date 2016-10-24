using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Assets.Programming.MatchMaking;
using System.Text;
using Assets.Programming;

public class GameController : MonoBehaviour, INetworkBroadcastListener
{
    public GameObject usernameCaptureMenu;
    public GameObject searchForOrHostMatchMenu;
    public GameObject searchForMatchMenu;
    public GameObject hostMatchMenu;
    public GameObject gameMenu;

    public GameObject opponentListContainer;
    public GameObject opponentListItemPrefab;

    public Text playerNameInput;
    public Text wtfText;

    public string butts = "muffin nuffin";

    private string _playerName = "Lazy, No-Named Player";
    private OverriddenNetworkDiscovery _networkDiscovery;
    private GameObject _currentMenu;

    private Dictionary<string, NearbyOpponentMatch> _uniqueGamesFound = new Dictionary<string, NearbyOpponentMatch>();
    private IEnumerator _opponentListProcessorCoroutine;

    private HandController _handController;

    // Use this for initialization
    void Start () {
        _currentMenu = usernameCaptureMenu;

        _networkDiscovery = FindObjectOfType<OverriddenNetworkDiscovery>();
        _networkDiscovery.AddListener(this);
        _networkDiscovery.broadcastData = _playerName;

        _handController = FindObjectOfType<HandController>();

        //TransitionToGame();
    }

    // Update is called once per frame
    void Update () {

	}

    public void TransitionToSearchOrHostChoiceMenu()
    {
        if (NetworkManager.singleton.isNetworkActive)
        {
            Debug.LogWarning("stopping networkManager");
            NetworkManager.singleton.StopHost();
        }
        if (_networkDiscovery.running)
        {
            Debug.LogWarning("stopping networkDiscovery");
            _networkDiscovery.StopBroadcast();
            if (_opponentListProcessorCoroutine != null)
            {
                Debug.LogWarning("stopping networkDiscovery, stop coroutine to process broadcasts");
                StopCoroutine(_opponentListProcessorCoroutine);
            }
        }

        _playerName = playerNameInput.text;
        _currentMenu.SetActive(false);
        _currentMenu = searchForOrHostMatchMenu;
        _currentMenu.SetActive(true);
        Debug.LogWarning("setting broadcast data");
        _networkDiscovery.broadcastData = _playerName;
        Debug.LogWarning("initializing networkDiscovery");
        _networkDiscovery.Initialize();
    }

    public void TransitionToSearchForMatchMenu()
    {
        _currentMenu.SetActive(false);
        _currentMenu = searchForMatchMenu;
        _currentMenu.SetActive(true);
        Debug.LogWarning("starting networkDiscovery, listen");
        _networkDiscovery.StartAsClient();
        _opponentListProcessorCoroutine = COProcessOpponentList();
        Debug.LogWarning("starting networkDiscovery, start coroutine to process broadcasts");
        StartCoroutine(_opponentListProcessorCoroutine);
    }

    public void TransitionToHostMatchMenu()
    {
        //_currentMenu.SetActive(false);
        //_currentMenu = hostMatchMenu;
        //_currentMenu.SetActive(true);
        Debug.LogWarning("starting networkManager");
        NetworkManager.singleton.StartHost();
        Debug.LogWarning("starting networkDiscovery, broadcast");
        _networkDiscovery.StartAsServer();

        TransitionToGame();
    }

    public void TransitionToUsernameCatpureMenu()
    {
        _currentMenu.SetActive(false);
        _currentMenu = usernameCaptureMenu;
        _currentMenu.SetActive(true);
    }

    public void TransitionToGame() { 

        _currentMenu.SetActive(false);
        _currentMenu = gameMenu;
        _currentMenu.SetActive(true);
    }

    #region Pull this networking stuff outta here!
    public void OnReceivedBroadcast(string fromAddress, string data)
    {
        //var stringBuilder = new StringBuilder();
        //stringBuilder.Append(fromAddress + " : " + data + "\n");
        //stringBuilder.Append(wtfText.text);
        //var buildText = stringBuilder.ToString();
        //Debug.LogError("existing text: " + wtfText.text);
        //var incommingText = fromAddress + ":" + data.TrimEnd('\0');
        //var s = "hello";
        //s = string.Format("{0} {1}", s, "world");
        //Debug.LogError(s);
        //Debug.LogErrorFormat("{0} , {1} , {2}", wtfText.text, incommingText, UnityEngine.Random.value);

        //Debug.LogError("incoming text: " + incommingText);
        //var newText = string.Format("{0} , {1}", incommingText, wtfText.text);
        //Debug.LogError("new      text: " + newText);
        //Debug.LogError(newText);
        //var number = UnityEngine.Random.insideUnitCircle;
        //var concatted = string.Concat(incommingText, wtfText.text);
        //Debug.LogError("concatted text: " + concatted);
        ////wtfText.text = buildText;
        //Debug.LogWarning("Data.length: " + data.TrimEnd('\0'));
        //wtfText.text = string.Format("{0} \n{1}", incommingText, wtfText.text);
        ////wtfText.text = butts;
        //Debug.LogWarning(BitConverter.ToString(Encoding.ASCII.GetBytes(data)));
        ProcessBroadcastMessage(fromAddress, data);
    }

    private void ProcessBroadcastMessage(string fromAddress, string data)
    {
        NearbyOpponentMatch oldData = null;
        var isGameAlreadyProcessed = _uniqueGamesFound.TryGetValue(fromAddress, out oldData);
        if(isGameAlreadyProcessed)
        {
            ProcessBroadcastExistingGameFound(fromAddress, data, oldData);
        } else
        {
            ProcessBroadcastNewGameFound(fromAddress, data);
        }
    }

    private void ProcessBroadcastNewGameFound(string fromAddress, string data)
    {
        Debug.Log("Should add new game to the list: " + fromAddress);
        var opponentListItemInstance = Instantiate(opponentListItemPrefab);
        opponentListItemInstance.transform.SetParent(opponentListContainer.transform);
        opponentListItemInstance.name = fromAddress;
        opponentListItemInstance.GetComponentInChildren<Text>().text = data;

        //Why do I need to do this?  lame
        opponentListItemInstance.transform.localScale = new Vector3(1, 1, 1);

        opponentListItemInstance.GetComponent<Button>().onClick.AddListener(() =>
        {
            JoinGame(fromAddress, data);
        });

        _uniqueGamesFound.Add(fromAddress, new NearbyOpponentMatch() {
            PlayerName = data,
            FromAddress = fromAddress,
            OpponentExpireTime = Time.time + 1.5f,
            OpponentListItemInstance = opponentListItemInstance
        });
    }

    private void JoinGame(string fromAddress, string data)
    {
        NetworkManager.singleton.networkAddress = fromAddress;
        NetworkManager.singleton.StartClient();
        TransitionToGame();
    }

    private void ProcessBroadcastExistingGameFound(string fromAddress, string data, NearbyOpponentMatch oldData)
    {
        if (data != oldData.PlayerName)
        {
            oldData.OpponentListItemInstance.GetComponentInChildren<Text>().text = data;
            oldData.PlayerName = data;
        }
        Debug.Log("Should reset expiration from existing host: " + fromAddress);
        oldData.OpponentExpireTime = Time.time + 1.5f;
        oldData.OpponentListItemInstance.SetActive(true);
    }

    private IEnumerator COProcessOpponentList()
    {
        while (true)
        {
            foreach (var opponentByHostAddress in _uniqueGamesFound)
            {
                if (opponentByHostAddress.Value.OpponentExpireTime <= Time.time)
                {
                    opponentByHostAddress.Value.OpponentListItemInstance.SetActive(false);
                }
            }
            yield return new WaitForSeconds(0.33f);
        }
    }
    #endregion
}
