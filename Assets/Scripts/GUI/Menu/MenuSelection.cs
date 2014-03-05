using UnityEngine;
using System.Collections;

public class MenuSelection : MonoBehaviour {

	public GameObject Fade;
	public GameObject GameMode;

	public void SelectEasy() {
		GameControl.Difficulty = Difficulty.EASY;
	}

	public void SelectHard() {
		GameControl.Difficulty = Difficulty.HARD;
	}

	public void SelectControl() {
		GameControl.Deck = Card.ControlDeck();
	}

	public void SelectProtection() {
		GameControl.Deck = Card.ProtectionDeck();
	}

	public void SelectDestruction() {
		GameControl.Deck = Card.DestructionDeck();
	}

	public void Quit() {
		Application.Quit();
	}

	public void SinglePlayer() {
		if(GameControl.Deck == null || GameControl.Difficulty == null) return;
		GameMode.GetComponent<UITweener>().Play(false);
		GameControl.IsMulti = false; 
		StartCoroutine(StartGame ());
	}

	public void MultiPlayer() {
		GameControl.IsMulti = true; 
		StartCoroutine(StartGame ());
	}

	void FadeOut() {
		TweenAlpha blackTween = Fade.AddComponent<TweenAlpha>();
		blackTween.from = 0f;
		blackTween.to = 1f;
		blackTween.Reset();
		blackTween.Play(true);

		TweenAlpha menuTween = gameObject.AddComponent<TweenAlpha>();
		menuTween.from = 1f;
		menuTween.to = 0f;
		menuTween.Reset();
		menuTween.Play(true);
	}
	
	IEnumerator StartGame(){
		FadeOut();
		yield return new WaitForSeconds (1);
		LoadingScreen.show ();
		Application.LoadLevel(Settings.GameMode);
	}
}
