using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject opponentListContainer;
    public GameObject opponentListItemPrefab;

    private string _playerName = "Lazy, No-Named Player";
    private OverriddenNetworkDiscovery _networkDiscovery;

    //private HashSet<string> _uniqueOpponentsEncountered = new HashSet<string>();

	// Use this for initialization
	void Start () {
        _networkDiscovery = FindObjectOfType<OverriddenNetworkDiscovery>();
        _networkDiscovery.Initialize();
        _networkDiscovery.broadcastData = _playerName;
        _networkDiscovery.StartAsServer();
	}
	
	// Update is called once per frame
	void Update () {
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
}
