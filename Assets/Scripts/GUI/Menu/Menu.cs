using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	MenuOption currentMenu;
	// Use this for initialization
	void Start () {
		MenuOption main = new MenuOption("Main", null, null);
		MenuOption play = new MenuOption("Play", main, null);
		MenuOption settings = new MenuOption("Settings", main, null);
		MenuOption quit = new MenuOption("Quit", main, null);
		MenuOption[] mainSub = new MenuOption[3];
		mainSub[0] = play;
		mainSub[1] = settings;
		mainSub[2] = quit;
		main.Children = mainSub;
		MenuOption singlePlayer = new MenuOption("Single Player", play, null);
		MenuOption multiPlayer = new MenuOption("Multi Player", play, null);
		MenuOption playBack = new MenuOption("Back", play, null);
		MenuOption[] playSub = new MenuOption[3];
		playSub[0] = singlePlayer;
		playSub[1] = multiPlayer;
		playSub[2] = playBack;
		play.Children = playSub;
		MenuOption rndOppo = new MenuOption("Random Opponent", multiPlayer, null);
		MenuOption specificOppo = new MenuOption("Specific Opponent", multiPlayer, null);
		MenuOption multiBack = new MenuOption("Back", multiPlayer, null);
		MenuOption[] multSub = new MenuOption[3];
		multSub[0] = rndOppo;
		multSub[1] = specificOppo;
		multSub[2] = multiBack;
		multiPlayer.Children = multSub;		
		
		currentMenu = main;
		
		GameObject.Find("Menu Item 1").GetComponent<MenuBox>().menuOption = play;
		GameObject.Find("Menu Item 2").GetComponent<MenuBox>().menuOption = settings;
		GameObject.Find("Menu Item 3").GetComponent<MenuBox>().menuOption = quit;
	}

	
	public void Clicked(int i) {
		
		int j = 1;
		currentMenu = currentMenu.Children[i-1];
		if(currentMenu.Title == "Quit") { Application.Quit(); return; }
		else if(currentMenu.Title == "Back") { currentMenu = currentMenu.Parent.Parent; }
		else if(currentMenu.Title == "Random Opponent") {GameControl.IsMulti = true; LoadingScreen.show (); iTween.Stop ();Application.LoadLevel(1); return; }
		else if(currentMenu.Title == "Single Player") { GameControl.IsMulti = false; LoadingScreen.show (); iTween.Stop ();Application.LoadLevel(1); return; }
		foreach(Transform child in transform.transform) {
			MenuBox menu = child.gameObject.GetComponent<MenuBox>();
			if(!menu.clicked) {
				
				menu.SetMenuOption(currentMenu.Children[j-1]);
				menu.index = j;
				iTween.MoveTo(child.gameObject, iTween.Hash("y", -30,
												  "time", 1,
												  "delay", j*0.1,
												  "onstart", "ToggleMoving",
												  "oncomplete", "MoveBack",
												  "oncompleteparams", 1,
												  "islocal", true));
				j++;
			}
		}
	}
}
