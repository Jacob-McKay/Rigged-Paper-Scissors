using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Programming;
using UnityEngine.Networking;

public class HandController : MonoBehaviour {
    public Button leftButton;
    public Button rightButton;

    public Text resultText;

    private Hands _hands;

    private Animator _leftButtonAnimator;
    private Animator _rightButtonAnimator;

    private Hand _selectedHand;
    private Hand _leftHand;
    private Hand _rightHand;

    //[SyncVar]
    private Hand _serverHand;

    //[SyncVar]
    private Hand _clientHand;

    // Use this for initialization
    void Start () {
        _leftButtonAnimator = leftButton.GetComponent<Animator>();
        _rightButtonAnimator = rightButton.GetComponent<Animator>();

        _hands = FindObjectOfType<Hands>();
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void InitializeNewGame()
    {
        _leftHand = _hands.GetRandomHand();
        leftButton.image.sprite = _leftHand.image;
        _rightHand = _hands.GetRandomHand();
        rightButton.image.sprite = _rightHand.image;
    }

    public void OptionSelected(Button buttonSelected)
    {
        Debug.Log("Button: " + buttonSelected + " was clickd brahhhhhhhhhh");
        _selectedHand = GetButtonHand(buttonSelected);

        //if(isServer)
        //{
        //    _serverHand = _selectedHand;
        //} else
        //{
        //    _clientHand = _selectedHand;
        //}

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

    private IEnumerator COCheckForWinAfter3Seconds()
    {
        yield return new WaitForSeconds(3f);
        var result = _serverHand.Against(_clientHand);
        resultText.enabled = true;
        if (result == Outcome.Win)
        {
            resultText.text = "SERVER WINS";
        }
        if (result == Outcome.Loss)
        {
            resultText.text = "SERVER WINS";
        }
        if (result == Outcome.Stalemate)
        {
            resultText.text = "YOUR BOTH LOSERS";
        }
    }
}
