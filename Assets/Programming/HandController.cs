using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Programming;
using UnityEngine.Networking;

public class HandController : NetworkBehaviour {
    public Button leftButton;
    public Button rightButton;

    private Hands _hands;

    private Animator _leftButtonAnimator;
    private Animator _rightButtonAnimator;

    private Hand _selectedHand;
    private Hand _leftHand;
    private Hand _rightHand;

    public ValidHand _serverHand;
    public ValidHand _clientHand;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        leftButton = GameObject.FindGameObjectWithTag("LeftHandChoiceButton").GetComponent<Button>();
        rightButton = GameObject.FindGameObjectWithTag("RightHandChoiceButton").GetComponent<Button>();
        _leftButtonAnimator = leftButton.GetComponent<Animator>();
        _rightButtonAnimator = rightButton.GetComponent<Animator>();

        leftButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("LISTENERS INVOKED");
            OptionSelected(leftButton);
        });

        rightButton.onClick.AddListener(() =>
        {
            Debug.LogWarning("LISTENER INVOKED");
            OptionSelected(rightButton);
        });
        Debug.LogWarning("LISTENERS ADDED");

        InitializeNewGame();
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if(isLocalPlayer)
        {
            if (!isServer && isClient)
            {
                Debug.Log("I'm a friggin client, I swear");
            }

            Debug.Log("Client Hand: " + _clientHand);
            Debug.Log("Server Hand: " + _serverHand);

            if (isServer && GameState.ServerHandShot != ValidHand.Abstain && GameState.ClientHandShot != ValidHand.Abstain)
            {
                var serverOutcome = GameState.ServerOutcome;
                var clientOutcome = GameState.ClientOutcome;
                RpcDisplayOutcome(serverOutcome, clientOutcome);
            }
        }
    }

    public void InitializeNewGame()
    {
        if (isLocalPlayer)
        {
            _hands = FindObjectOfType<Hands>();

            _leftHand = _hands.GetRandomHand();
            leftButton.image.sprite = _leftHand.image;
            _rightHand = _hands.GetRandomHand();
            rightButton.image.sprite = _rightHand.image;
        }
    }

    [ClientRpc]
    void RpcDisplayOutcome(Outcome serverOutcome, Outcome clientOutcome)
    {
        if(isServer)
        {
            Debug.LogError("I'm the server, and I: " + serverOutcome);
        } else
        {
            Debug.LogError("I'm only a client, and I: " + clientOutcome);
        }
    }

    [Command]
    void CmdShoot(bool shotByServer, ValidHand handShot)
    {
        var from = shotByServer ? "server" : "client";
        Debug.Log("holy butts we got a message from: " + from + " with hand: " + handShot);
        Debug.Log("holy butts isLocalPlayer: " + isLocalPlayer + " isServer: " + isServer);
        if(isServer)
        {
            Debug.Log("MCKAY LOOK FOR THIS MESSAGE!!!!");
            if(shotByServer)
            {
                GameState.ServerDidShoot = true;
                GameState.ServerHandShot = handShot;
            }
            if(!shotByServer)
            {
                GameState.ClientDidShoot = true;
                GameState.ClientHandShot = handShot;
            }
        }
    }

    public void OptionSelected(Button buttonSelected)
    {
        if(!isLocalPlayer)
        {
            Debug.LogWarning("It's not us man, don't touch it");
            return;
        }
        Debug.Log("Button: " + buttonSelected + " was clickd brahhhhhhhhhh");
        _selectedHand = GetButtonHand(buttonSelected);

        CmdShoot(isServer, _selectedHand.hand);

        GetButtonAnimator(buttonSelected).SetTrigger("Pressed");
        GetButtonAnimator(OtherButton(buttonSelected)).SetTrigger("Unselected");

        Debug.Log("Current hand in play is: " + _selectedHand.name);
    }

    private Hand GetButtonHand(Button buttonSelected)
    {
        if (leftButton == buttonSelected)
        {
            return _leftHand;
        }

        if (rightButton == buttonSelected)
        {
            return _rightHand;
        }

        throw new Exception("Wtf button did you click on?");
    }

    private Animator GetButtonAnimator(Button buttonSelected)
    {
        if (leftButton == buttonSelected)
        {
            return _leftButtonAnimator;
        }

        if (rightButton == buttonSelected)
        {
            return _rightButtonAnimator;
        }

        throw new Exception("Wtf button did you click on?");
    }

    private Button OtherButton(Button buttonSelected)
    {
        if(leftButton == buttonSelected)
        {
            return rightButton;
        }

        if(rightButton == buttonSelected)
        {
            return leftButton;
        }

        throw new Exception("Wtf button did you click on?");
    }

    //private IEnumerator COCheckForWinAfter3Seconds()
    //{
    //    yield return new WaitForSeconds(3f);
    //    var result = Hands.Against(_serverHand, _clientHand);

    //    if (result == Outcome.Win)
    //    {
    //        Debug.LogError("SERVER WINS");
    //    }
    //    if (result == Outcome.Loss)
    //    {
    //        Debug.LogError("SERVER WINS");
    //    }
    //    if (result == Outcome.Stalemate)
    //    {
    //        Debug.LogError("YOUR BOTH LOSERS");
    //    }
    //}
}
