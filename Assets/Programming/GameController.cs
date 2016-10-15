using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameController : MonoBehaviour {

    public GameObject usernameCaptureMenu;
    public GameObject searchForOrHostMatchMenu;
    public GameObject searchForMatchMenu;
    public GameObject hostMatchMenu;
    public GameObject gameMenu;

    public GameObject opponentListContainer;
    public GameObject opponentListItemPrefab;

    public Text playerNameInput;

    private string _playerName = "Lazy, No-Named Player";
    private OverriddenNetworkDiscovery _networkDiscovery;
    private GameObject _currentMenu;

	// Use this for initialization
	void Start () {
        _currentMenu = usernameCaptureMenu;

        _networkDiscovery = FindObjectOfType<OverriddenNetworkDiscovery>();
        _networkDiscovery.broadcastData = _playerName;
	}
	
	// Update is called once per frame
	void Update () {
        //foreach(var opponentListItem in opponentListContainer.GetComponentsInChildren<Button>())
        //{
        //    Destroy(opponentListItem.gameObject);
        //}

        if (_networkDiscovery.broadcastsReceived == null)
        {
            return;
        }

        var optionNameIndex = 0;
        foreach(var addressAndDataPair in _networkDiscovery.broadcastsReceived)
        {
            var opponentListItemInstance = Instantiate<GameObject>(opponentListItemPrefab);
            opponentListItemInstance.transform.SetParent(opponentListContainer.transform);
            opponentListItemInstance.name = (optionNameIndex++).ToString();
            var opponentName = System.Text.Encoding.Default.GetString(addressAndDataPair.Value.broadcastData);
            opponentListItemInstance.GetComponentInChildren<Text>().text = opponentName;
            //			EventTrigger trigger = quizOptionUIInstance.GetComponent<EventTrigger>();
            //			EventTrigger.Entry entry = new EventTrigger.Entry();
            //			entry.eventID = EventTriggerType.PointerClick;
            //			entry.callback.AddListener( (eventData) => { QuizOptionSelected(quizOptionUIInstance); } );
            //			trigger.triggers.Add(entry);
            //var quizOptionUIButton = opponentListItemInstance.GetComponent<Button>();
            //quizOptionUIButton.onClick.AddListener(() => { _gameController.QuizOptionSelected(opponentListItemInstance); });
            //UpdateButtonStyling(quizOptionUIButton, quizOption);
        }
	}

    public void TransitionToSearchOrHostChoiceMenu()
    {
        if(NetworkManager.singleton.isNetworkActive)
        {
            NetworkManager.singleton.StopServer();
        }
        if(_networkDiscovery.isServer || _networkDiscovery.isClient)
        {
            _networkDiscovery.StopBroadcast();
        }

        _playerName = playerNameInput.text;
        _currentMenu.SetActive(false);
        _currentMenu = searchForOrHostMatchMenu;
        _currentMenu.SetActive(true);
        _networkDiscovery.broadcastData = _playerName;
        _networkDiscovery.Initialize();
    }

    public void TransitionToSearchForMatchMenu()
    {
        _currentMenu.SetActive(false);
        _currentMenu = searchForMatchMenu;
        _currentMenu.SetActive(true);
        _networkDiscovery.StartAsClient();
    }

    public void TransitionToHostMatchMenu()
    {
        _currentMenu.SetActive(false);
        _currentMenu = hostMatchMenu;
        _currentMenu.SetActive(true);
        NetworkManager.singleton.StartServer();
        _networkDiscovery.StartAsServer();
    }

    public void TransitionToUsernameCatpureMenu()
    {
        _currentMenu.SetActive(false);
        _currentMenu = usernameCaptureMenu;
        _currentMenu.SetActive(true);
    }
}
