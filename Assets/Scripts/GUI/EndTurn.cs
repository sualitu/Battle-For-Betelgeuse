using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {

	public string title = "End turn";
	public GUISkin skin = null;
	public Rect position;
	int x = 175;
	int y = 75;
	GameControl gameControl;
	
	float sizeRatio = 175f/75f;
	float ratio = Screen.width/Settings.NativeResolution.x;
	
	void Start() {
		gameControl = GameControl.gameControl;
		position = new Rect (Screen.width - 400, Screen.height - 200,x,y);
	}
	
	public bool IsMouseOver = false;
	
	public void OnGUI() {
		IsMouseOver = position.Contains(Event.current.mousePosition);
		GUI.skin = skin;
		if(GUI.Button(position, title)) {
			gameControl.EndTurnClicked();
		}
	}
}
