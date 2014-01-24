using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class GUIControl : MonoBehaviour {

	GUIText selUnitLabelObject;
	GUIText selUnitValueObject;
	GUIText selUnitNameObject;
	GUIText playerStatsObject;
	GUIText enemyStatsObject;
	GameControl gameControl;
	Vector2 popUp = new Vector2(0,0);
	Hex oldHex = null;
	EndTurn et;
	GUITexture selUnitObject;
	Texture2D selUnitTexture;
	GUICard guiCard;
	Rect imageRect = new Rect(34,47,154,91);

	void Start() {
		gameControl = GetComponent<GameControl>();
		et = GameObject.Find("EndTurn").GetComponent<EndTurn>();
		et.title = "Random Deck";
		selUnitLabelObject = (GUIText) Instantiate(Assets.Instance.SelUnitLabels);
		selUnitValueObject = (GUIText) Instantiate(Assets.Instance.SelUnitValues);
		selUnitNameObject = (GUIText) Instantiate(Assets.Instance.SelUnitName);
		selUnitObject = (GUITexture) Instantiate(Assets.Instance.SelUnitBox);
		playerStatsObject = (GUIText) Instantiate(Assets.Instance.PlayerStats);
		enemyStatsObject = (GUIText) Instantiate(Assets.Instance.EnemyStats);
		HideSelUnitBox();
		HideSelUnitInfo();
	}

	public void SetMainButton(string s) {
		et.title = s;
	}
			
	public void UpdateGUI() {
	}
	
	public bool MouseIsOverGUI() {
		try {
			return gameControl.State > State.PREGAME &&  (et.IsMouseOver || gameControl.ThisPlayer.GuiHand.Exists(g => g.IsMouseOver));
		} catch {
			Debug.LogWarning("MouseIsOverGUI failed.");
			return false;
		}
	}
	
	public void ShowSplashText(string s) {
		Assets.Instance.TextSplashPrefab.text = s;
		Instantiate (Assets.Instance.TextSplashPrefab);
	}
	
	public void ShowSmallSplashText(string s) {
		Assets.Instance.SmallTextSplashPrefab.text = s;
		Instantiate (Assets.Instance.SmallTextSplashPrefab);
	}
	
	string GetMouseOverString(Unit unit) {
		return unit.ConstructTooltip();
	}

	void ShowSelUnitBox() {
		if(selUnitObject == null) 
			selUnitObject = (GUITexture) Instantiate(Assets.Instance.SelUnitBox);
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
		GUI.depth = 1;
		enemyStatsObject.text = "Opponent\nCards in Deck: " + gameControl.EnemyPlayer.Deck.Count  + 
			"\nCards in Hand: " + gameControl.EnemyPlayer.Hand.Count + 
				"\nMana: " + gameControl.EnemyPlayer.ManaLeft() + " / " + gameControl.EnemyPlayer.MaxMana + 
				"\nVictory Points: " + gameControl.EnemyPlayer.Points + " / " + Settings.VictoryRequirement;

		playerStatsObject.text = "You\nCards in Deck: " + gameControl.ThisPlayer.Deck.Count  + 
			"\nCards in Hand: " + gameControl.ThisPlayer.Hand.Count + 
				"\nMana: " + gameControl.ThisPlayer.ManaLeft() + " / " + gameControl.ThisPlayer.MaxMana + 
				"\nVictory Points: " + gameControl.ThisPlayer.Points + " / " + Settings.VictoryRequirement;

		if(gameControl.MouseControl.selectedUnit != null && gameControl.MouseControl.selectedUnit.Team != Team.NEUTRAL) {
			ShowSelUnitBox();
			ShowSelUnitInfo(gameControl.MouseControl.selectedUnit);
			GUI.DrawTexture(imageRect, selUnitTexture);
			if(imageRect.Contains(Event.current.mousePosition)) {
				if(guiCard == null) {
					guiCard = ((GameObject) Object.Instantiate(Assets.Instance.CardPrefab)).GetComponent<GUICard>();
					guiCard.SetInfo(gameControl.MouseControl.selectedUnit.Card, gameControl.MouseControl.selectedUnit.Team == Team.ME ? gameControl.ThisPlayer : gameControl.EnemyPlayer);
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
		if(gameControl.MouseControl.mouseOverHex != null && gameControl.MouseControl.mouseOverHex.Unit != null && gameControl.MouseControl.mouseOverHex.Unit.Team != 0) {
			if(oldHex == null || oldHex != gameControl.MouseControl.mouseOverHex) {
				oldHex = gameControl.MouseControl.mouseOverHex;
				popUp.x = Camera.main.WorldToScreenPoint(oldHex.transform.position).x;
				popUp.y = -(Camera.main.WorldToScreenPoint(oldHex.transform.position).y-Screen.height);
			}
			GUI.skin = Assets.Instance.Skin;
			GUI.Box (new Rect(popUp.x+30, popUp.y-30, 150, 100), GetMouseOverString(gameControl.MouseControl.mouseOverHex.Unit));
		}
	}
	
}
