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
	public GUIText selUnitName;
	public GUIText playerStats;
	public GUIText enemyStats;
	GUIText selUnitLabelObject;
	GUIText selUnitValueObject;
	GUIText selUnitNameObject;
	GUIText playerStatsObject;
	GUIText enemyStatsObject;
	Vector2 popUp = new Vector2(0,0);
	public GUISkin skin = null;
	Hex oldHex = null;
	EndTurn et;
	GUITexture selUnitObject;
	Texture2D selUnitTexture;
	GUICard guiCard;
	Rect imageRect = new Rect(34,47,154,91);
	public GameObject CardPrefab;

	void Start() {
		gameControl = GetComponent<GameControl>();
		et = GameObject.Find("EndTurn").GetComponent<EndTurn>();
		et.title = "Random Deck";
		selUnitLabelObject = (GUIText) Instantiate(selUnitLabels);
		selUnitValueObject = (GUIText) Instantiate(selUnitValues);
		selUnitNameObject = (GUIText) Instantiate(selUnitName);
		selUnitObject = (GUITexture) Instantiate(selUnitBox);
		playerStatsObject = (GUIText) Instantiate(playerStats);
		enemyStatsObject = (GUIText) Instantiate(enemyStats);
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
		selUnitTexture = (Texture2D) Resources.Load("GUI/Cards/images/" + unit.Card.Image);
		selUnitNameObject.text = unit.UnitName;
		selUnitLabelObject.text = "Attack:\nHealth:\nMovement:\n";
		selUnitValueObject.text = unit.Attack + "\n" + unit.CurrentHealth() + " / " + unit.MaxHealth + "\n" + (unit.MovementLeft() < 1 ? "0" : (unit.MovementLeft()).ToString()) + " / " + unit.MaxMovement.ToString();
		unit.Buffs.ForEach(b => selUnitLabelObject.text += b.Name + ", ");
	}

	void HideSelUnitInfo() {
		selUnitLabelObject.text = "";
		selUnitValueObject.text = "";
		selUnitNameObject.text = "";
	}

	void ShowCardOver() {

	}

	void OnGUI() {
		enemyStatsObject.text = "Opponent\nCards in Deck: " + gameControl.enemyPlayer.Deck.Count  + 
			"\nCards in Hand: " + gameControl.enemyPlayer.Hand.Count + 
				"\nMana: " + gameControl.enemyPlayer.ManaLeft() + " / " + gameControl.enemyPlayer.MaxMana + 
				"\nVictory Points: " + gameControl.enemyPlayer.Points + " / " + Settings.VictoryRequirement;

		playerStatsObject.text = "You\nCards in Deck: " + gameControl.thisPlayer.Deck.Count  + 
			"\nCards in Hand: " + gameControl.thisPlayer.Hand.Count + 
				"\nMana: " + gameControl.thisPlayer.ManaLeft() + " / " + gameControl.thisPlayer.MaxMana + 
				"\nVictory Points: " + gameControl.thisPlayer.Points + " / " + Settings.VictoryRequirement;

		if(gameControl.mouseControl.selectedUnit != null && gameControl.mouseControl.selectedUnit.Team != 0) {
			ShowSelUnitBox();
			ShowSelUnitInfo(gameControl.mouseControl.selectedUnit);
			GUI.DrawTexture(imageRect, selUnitTexture);
			if(imageRect.Contains(Event.current.mousePosition)) {
				if(guiCard == null) {
					guiCard = ((GameObject) Object.Instantiate(CardPrefab)).GetComponent<GUICard>();
					guiCard.SetInfo(gameControl.mouseControl.selectedUnit.Card, gameControl.mouseControl.selectedUnit.Team == 1 ? gameControl.thisPlayer : gameControl.enemyPlayer);
					guiCard.ForcePlaceCard(Event.current.mousePosition.x, Event.current.mousePosition.y); 
					guiCard.HandCard = false;
				}  else {
					guiCard.SetPosition(Event.current.mousePosition.x, Event.current.mousePosition.y);
				}
			} else {
				if(guiCard != null) guiCard.Kill();
			}		
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
