using UnityEngine;
using System.Collections.Generic;

public class DeckOption : MonoBehaviour
{
	public string title = "Deck";
	public GUISkin skin = null;
	public Rect position;
	int x = 154;
	int y = 55;
	public int h = 0;
	GameControl gameControl;
	public int index = 0;
	public List<DeckOption> dos = new List<DeckOption>();	
	public bool IsMouseOver = false;
	
	void Start() {
		gameControl = GameControl.gameControl;
		position = new Rect (Screen.width + h, Screen.height-200,x,y);
	}
	
	
	public void OnGUI() {
		GUI.skin = skin;		
		if(!IsMouseOver && position.Contains(Event.current.mousePosition)) {
			gameControl.AudioControl.PlayAudio(Assets.Instance.ButtonHoverSound);
		}
		IsMouseOver = position.Contains(Event.current.mousePosition);
		if(GUI.Button(position, title)) {
			gameControl.AudioControl.PlayAudio(Assets.Instance.ButtonClickSound);
			gameControl.ChooseDeck(index);
			dos.ForEach(d => Destroy (d));
			Destroy (this);
		}
	}
}

