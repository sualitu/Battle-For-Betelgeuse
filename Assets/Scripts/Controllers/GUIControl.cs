using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class GUIControl : MonoBehaviour {
	
	public GUIText SmallTextSplashPrefab;
	public GUIText TextSplashPrefab;
	private GameControl gameControl;
	Vector2 popUp = new Vector2(0,0);
	public GUISkin skin = null;
		
	void Start() {
		gameControl = GetComponent<GameControl>();
	}
			
	public void UpdateGUI() {
	}
	
	public bool MouseIsOverGUI() {
		return gameControl.et.IsMouseOver || gameControl.thisPlayer.GuiHand.Exists(g => g.IsMouseOver);
	}
	
	public void ShowSplashText(string s) {
		TextSplashPrefab.text = s;
		Instantiate (TextSplashPrefab);
	}
	
	public void ShowSmallSplashText(string s) {
		SmallTextSplashPrefab.text = s;
		Instantiate (SmallTextSplashPrefab);
	}
	
	string ConstructMouseOverString(Unit unit) {
		return unit.UnitName + "\nAttack: " + unit.Attack + 
			"\nHealth: " + (unit.CurrentHealth() < 1 ? "0" : (unit.CurrentHealth()).ToString()) + " / " + unit.MaxHealth.ToString() + 
				"\nMovement: " + (unit.MovementLeft() < 1 ? "0" : (unit.MovementLeft()).ToString()) + " / " + unit.MaxMovement.ToString();
	}
	
	void OnGUI() {
		if(gameControl.mouseControl.mouseOverHex != null && gameControl.mouseControl.mouseOverHex.Unit != null) {
			popUp.x = Input.mousePosition.x;
			popUp.y = -(Input.mousePosition.y-Screen.height);
			GUI.skin = skin;
			GUI.Box (new Rect(popUp.x+20, popUp.y-30, 150, 100), ConstructMouseOverString(gameControl.mouseControl.mouseOverHex.Unit));
		}
	}
	
}
