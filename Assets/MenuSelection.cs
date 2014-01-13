using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {


	public void Quit() {
		Application.Quit();
	}

	public void SinglePlayer() {
		GameControl.IsMulti = false; 
		LoadingScreen.show ();
		Application.LoadLevel(Settings.GameMode);
	}

	public void MultiPlayer() {
		GameControl.IsMulti = true; 
		LoadingScreen.show ();
		Application.LoadLevel(Settings.GameMode);
	}
}
