using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class GUIControl : MonoBehaviour {
	
	public GUIText SmallTextSplashPrefab;
	public GUIText TextSplashPrefab;
	private GameControl gameControl;
	public GUITexture selUnitBox;
	public GUIText selUnitLabels;
	public GUIText selUnitValues;
	GUIText selUnitLabelObject;
	GUIText selUnitValueObject;
	Vector2 popUp = new Vector2(0,0);
	public GUISkin skin = null;
	Hex oldHex = null;
	EndTurn et;
	GUITexture selUnitObject;

	void Start() {
		gameControl = GetComponent<GameControl>();
		et = GameObject.Find("EndTurn").GetComponent<EndTurn>();
		et.title = "Random Deck";
		selUnitLabelObject = (GUIText) Instantiate(selUnitLabels);
		selUnitValueObject = (GUIText) Instantiate(selUnitValues);
		selUnitObject = (GUITexture) Instantiate(selUnitBox);
		HideSelUnitBox();
		HideSelUnitInfo();
	}

	public void SetButton(string s) {
		et.title = s;
	}
			
	public void UpdateGUI() {
	}
	
	public bool MouseIsOverGUI() {
		return gameControl.state > State.PREGAME &&  (et.IsMouseOver || gameControl.thisPlayer.GuiHand.Exists(g => g.IsMouseOver));
	}
	
	public void ShowSplashText(string s) {
		TextSplashPrefab.text = s;
		Instantiate (TextSplashPrefab);
	}
	
	public void ShowSmallSplashText(string s) {
		SmallTextSplashPrefab.text = s;
		Instantiate (SmallTextSplashPrefab);
	}
	
	string GetMouseOverString(Unit unit) {
		return unit.ConstructTooltip();
	}

	void ShowSelUnitBox() {
		if(selUnitObject == null) 
			selUnitObject = (GUITexture) Instantiate(selUnitBox);
		else 
			selUnitObject.enabled = true;
	}

	void HideSelUnitBox() {
		selUnitObject.enabled = false;
	}
	
	void ShowSelUnitInfo(Unit unit) {
		selUnitLabelObject.text = unit.UnitName + "\nAttack:\nHealth:\nMovement:";
		selUnitValueObject.text = unit.Attack + "\n" + unit.CurrentHealth() + " / " + unit.MaxHealth + "\n" + unit.MovementLeft() + " / " + unit.MaxMovement;
	}

	void HideSelUnitInfo() {
		selUnitLabelObject.text = "";
		selUnitValueObject.text = "";
	}

	void OnGUI() {
		if(gameControl.mouseControl.selectedUnit != null) {
			ShowSelUnitBox();
			ShowSelUnitInfo(gameControl.mouseControl.selectedUnit);
		} else {
			HideSelUnitBox();
			HideSelUnitInfo();
		}
		if(gameControl.mouseControl.mouseOverHex != null && gameControl.mouseControl.mouseOverHex.Unit != null) {
			if(oldHex == null || oldHex != gameControl.mouseControl.mouseOverHex) {
				oldHex = gameControl.mouseControl.mouseOverHex;
				popUp.x = Camera.main.WorldToScreenPoint(oldHex.transform.position).x;
				popUp.y = -(Camera.main.WorldToScreenPoint(oldHex.transform.position).y-Screen.height);
			}
			GUI.skin = skin;
			GUI.Box (new Rect(popUp.x+30, popUp.y-30, 150, 100), GetMouseOverString(gameControl.mouseControl.mouseOverHex.Unit));
		}
	}
	
}
