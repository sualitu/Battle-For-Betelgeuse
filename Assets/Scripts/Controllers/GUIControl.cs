using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class GUIControl : MonoBehaviour {
	
	public GUIText unitNameLabel;
	public GUIText attackLabel;
	public GUIText healthLabel;
	public GUIText movementLabel;
	public GUIText SmallTextSplashPrefab;
	public GUIText TextSplashPrefab;
	private GameControl gameControl;
	private Texture2D texture;
	public GUIText cardNameLabel;
	public GUIText cardAttackLabel;
	public GUIText cardHealthLabel;
	public GUIText cardMovementLabel;
	public GUIText cardCostLabel;
	public GUIText cardTextLabel;

	public void setUnitGUI(Unit unit) {
		if(unit != null) {
			unitNameLabel.text = (unit.Team == gameControl.thisPlayer.Team ? "Your " : "Enemy ") + unit.UnitName;
			attackLabel.text = unit.Attack.ToString();
			healthLabel.text = (unit.CurrentHealth() < 1 ? "0" : (unit.CurrentHealth()).ToString()) + " / " + unit.MaxHealth.ToString();
			movementLabel.text = (unit.MovementLeft() < 1 ? "0" : (unit.MovementLeft()).ToString()) + " / " + unit.MaxMovement.ToString();
		} else {
			clearUnitGui();
		}
	}
	
	public void setCardGui(Card card) {
		if(card != null) {
			cardNameLabel.text = card.Name;
			cardAttackLabel.text = card.Attack.ToString();
			cardCostLabel.text = card.Cost.ToString();
			cardHealthLabel.text = card.Health.ToString();
			cardMovementLabel.text = card.Movement.ToString();
			cardTextLabel.text = card.CardText;
		}
	}
		
	void Start() {
		gameControl = GetComponent<GameControl>();
	}
	
	public void clearUnitGui() {
		unitNameLabel.text = "";
		attackLabel.text = "";
		healthLabel.text = "";
		movementLabel.text = "";
	}
			
	public void UpdateGUI() {
		setUnitGUI(gameControl.mouseControl.selectedUnit);
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
}
