using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class GUIControl : MonoBehaviour {
	
	public GUIText unitNameLabel;
	public GUIText attackLabel;
	public GUIText healthLabel;
	public GUIText movementLabel;
	public GameObject textPrefab;
	public GUIText TextSplashPrefab;
	private GameControl gameControl;
	private Texture2D texture;

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
		
	void Start() {
		gameControl = GetComponent<GameControl>();
	}
	
	public void clearUnitGui() {
		unitNameLabel.text = "";
		attackLabel.text = "";
		healthLabel.text = "";
		movementLabel.text = "";
	}
		
	public void ShowFloatingText(string s, Transform tar) {
		GameObject text = (GameObject) Instantiate(textPrefab, new Vector3(tar.position.x + 1, 1, tar.position.z+1), Quaternion.Euler(new Vector3(50f,0f,0f)));
		TextMesh tm = text.GetComponent<TextMesh>();
		tm.text = s;
		tm.fontSize = 10;
	}
	
	public void UpdateGUI() {
		setUnitGUI(gameControl.mouseControl.selectedUnit);
	}
	
	public bool MouseIsOverGUI() {
		return false;	
	}
	
	public void ShowSplashText(string s) {
		TextSplashPrefab.text = s;
		Instantiate (TextSplashPrefab);
	}
}
