using UnityEngine;
using System.Collections;

public class EndTurn : MonoBehaviour {

	public string title = "Start Game";
	public GUISkin skin = null;
	public Rect position;
	int x = 154;
	int y = 55;
	GameControl gameControl;
	public bool IsMouseOver = false;
	bool started = false;
	
	void Start() {
		gameControl = GameControl.gameControl;
		position = new Rect (Screen.width - 200, 190,x,y);
		StartCoroutine(Started());
	}

	public void OnGUI() {
		if(started) {
			if(!IsMouseOver && position.Contains(Event.current.mousePosition)) {
				gameControl.AudioControl.PlayAudio(Assets.Instance.ButtonHoverSound);
			}
			IsMouseOver = position.Contains(Event.current.mousePosition);
			GUI.skin = skin;
			if(GUI.Button(position, title)) {
				gameControl.AudioControl.PlayAudio(Assets.Instance.ButtonClickSound);
				gameControl.EndTurnClicked();
			}
		}
	}

	IEnumerator Started() {
		yield return new WaitForSeconds(2);
		started = true;
	}
}
