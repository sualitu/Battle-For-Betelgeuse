using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {

	public string title = "End turn";
	public GUISkin skin = null;
	public Rect position;
	int x = 154;
	int y = 55;
	GameControl gameControl;
	public bool IsMouseOver = false;
	
	
	void Start() {
		gameControl = GameControl.gameControl;
		position = new Rect (Screen.width - 200, 190,x,y);
	}
	
	
	public void OnGUI() {
		IsMouseOver = position.Contains(Event.current.mousePosition);
		GUI.skin = skin;
		if(GUI.Button(position, title)) {
			gameControl.EndTurnClicked();
		}
	}
}
