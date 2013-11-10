using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class MouseControl : MonoBehaviour {
	
	public GameObject cube;
	public Transform selection;
	
	public Unit selectedUnit;
	Color initColor;
	GameControl gameControl;
	public GameObject terrain;
	public bool PlayModeOn = true;
	
	Hex selHex;
	public Hex mouseOverHex;
	Ray ray;
	RaycastHit rayHit;
			
	void Start () {
		gameControl = GetComponent<GameControl>();
	}
			
	public void DeselectHex() {
		if(selHex != null) {
			if(selectedUnit != null) {
				selectedUnit.movable.ForEach(t => t.renderer.material.color = Color.white);
			}
			selHex.IsSelected = false;
			selHex.renderer.material.color = Color.white;
			selHex = null;
			selectedUnit = null;
		}
	}
	
	public void SelectHex(Hex hex) {
		if( selHex != null ) {
			selHex.renderer.material.color = Color.white;
			selHex.IsSelected = false;
		}
		selHex = hex;
		hex.IsSelected = true;
		selectedUnit = hex.Unit;
		if(selectedUnit != null) {
			hex.Unit.movable = PathFinder.BreadthFirstSearch(hex, gameControl.gridControl.Map, hex.Unit.MovementLeft(), hex.Unit.Team);
		}
	}
	
	void Update () {
		
		if(!gameControl.guiControl.MouseIsOverGUI()) {
			if(Input.GetMouseButton(1)) {
				// Right click
				DeselectHex();
			}
			ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
			if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {
				Hex hex = null;
				try { hex = rayHit.collider.GetComponent<Hex>(); } catch {}
				if(hex == null) { return; }
				
				if(mouseOverHex == null) {
					mouseOverHex = hex;
				} else if (mouseOverHex.GridPosition != hex.GridPosition) {
					if(gameControl.thisPlayer.targets.Contains(mouseOverHex))
						mouseOverHex.renderer.material.color = Color.green;
					else
						mouseOverHex.renderer.material.color = Color.white;
					mouseOverHex = hex;
				}
				hex.renderer.material.color = Color.red;
				
				if(Input.GetMouseButton(0) && gameControl.state == State.MYTURN) {
					// Left click
					if(gameControl.thisPlayer.selectedCard != null) {
						// A card is selected
						if(gameControl.thisPlayer.targets.Contains(hex)) {
							if(GameControl.IsMulti) {
								gameControl.networkControl.PlayNetworkCardOn(gameControl.thisPlayer.selectedCard, hex);
							} else {
								gameControl.PlayCardOnHex(gameControl.thisPlayer.selectedCard, hex, System.Guid.NewGuid().ToString());
							}
							DeselectHex();
							gameControl.thisPlayer.DeselectCard();
						} else {
							gameControl.guiControl.ShowSmallSplashText(Dictionary.cannotMoveThereError);
						}
					} else if(selectedUnit != null && selectedUnit.Hex.GridPosition != hex.GridPosition) {
						// A unit is selected
						if(GameControl.IsMulti) {
							gameControl.networkControl.MoveNetworkUnit(selectedUnit, hex);
						} else {
							selectedUnit.PrepareMove (hex);
						}
					} else if(selectedUnit == null) {
						// Select a unit
						SelectHex(hex);
					}
				}
			}
		} 		
	}
}
