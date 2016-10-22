using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Programming;

public class HandController : MonoBehaviour {

    public Button leftButton;
    public Button rightButton;

    private Animator _leftButtonAnimator;
    private Animator _rightButtonAnimator;

    private Hand _hand;

	// Use this for initialization
	void Start () {
  
        _leftButtonAnimator = leftButton.GetComponent<Animator>();
        _rightButtonAnimator = rightButton.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OptionSelected(Button buttonSelected)
    {
        Debug.Log("Button: " + buttonSelected + " was clickd brahhhhhhhhhh");
        GetButtonAnimator(buttonSelected).SetTrigger("Selected");
        GetButtonAnimator(OtherButton(buttonSelected)).SetTrigger("Unselected");
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
}
