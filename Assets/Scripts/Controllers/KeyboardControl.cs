using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKey(KeyCode.Escape)) {
			NGUITools.SetActive(Assets.Instance.MainMenu, true);
			/*
			GameControl.gameControl.NetworkControl.QuitGame();
			Application.LoadLevel (0);
			*/
		}
		if(Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
			GameControl.gameControl.EndTurnClicked();
		}
	}
}

