using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class MouseControl : MonoBehaviour {
	
	public GameObject cube;
	public Transform selection;
	
	public Unit selectedUnit;
	private Color initColor;
	private GameControl gameControl;
	public GameObject terrain;
	public bool PlayModeOn = true;
	
	private Hex selHex;
	private Hex mouseOverHex;
	private Ray ray;
	private RaycastHit rayHit;
	private List<Hex> moves;
			
	void Start () {
		gameControl = GetComponent<GameControl>();
	}
			
	public void deselectHex() {
		if(selHex != null) {
			selHex.IsSelected = false;
			selHex.renderer.material.color = Color.white;
			selHex = null;
			selectedUnit = null;
			gameControl.guiControl.clearUnitGui();
			if(moves != null) {
				moves.ForEach(t => t.renderer.material.color = Color.white);
			}
		}
	}
	
	public void selectHex(Hex hex) {
		if( selHex != null ) {
			selHex.renderer.material.color = Color.white;
			selHex.IsSelected = false;
		}
		selHex = hex;
		hex.IsSelected = true;
		selectedUnit = hex.Unit;
		gameControl.guiControl.setUnitGUI(selectedUnit);
	}
	

	
	void Update () {
		ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
		if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {
			try {
				Vector2 coords = rayHit.collider.GetComponent<Hex>().GridPosition;
				Hex hex = gameControl.gridControl.Map[Mathf.FloorToInt(coords.x)][Mathf.FloorToInt(coords.y)];
				if(PlayModeOn) {
					if(mouseOverHex == null) {
						mouseOverHex = hex;
						mouseOverHex.renderer.material.color = Color.red;
					} else if (mouseOverHex.GridPosition != hex.GridPosition) {
						mouseOverHex.renderer.material.color = Color.white;
						mouseOverHex = hex;
						if(selectedUnit != null && selectedUnit.Team == gameControl.thisPlayer.Team) {
							if(moves != null) {
								moves.ForEach(t => t.renderer.material.color = Color.white);
							}
							
							moves = PathFinder.DepthFirstSearch(selectedUnit.Hex, hex, gameControl.gridControl.Map, selectedUnit.MovementLeft()); 
	
						}
						hex.renderer.material.color = Color.red;
					}
					
					if(Input.GetMouseButtonDown(0)){
						if(!gameControl.guiControl.MouseIsOverGUI() && gameControl.state == State.MYTURN) {
							if(selectedUnit == null) {
								selectHex(hex);
							} else {
								if(selectedUnit.Team == gameControl.thisPlayer.Team && gameControl.state == State.MYTURN) {								
									if(GameControl.IsMulti) {
										gameControl.networkControl.MoveNetworkUnit(selectedUnit, hex);
									} else {
										selectedUnit.PrepareMove (hex);
									}
								}
							}
						} 
						gameControl.guiControl.UpdateGUI();	
					} else if( Input.GetMouseButtonDown(1) ) {
						deselectHex();
					}
				} else {
					if(mouseOverHex == null) {
						mouseOverHex = hex;
						mouseOverHex.renderer.material.color = Color.red;
					} else if (mouseOverHex.GridPosition != hex.GridPosition) {
						if(gameControl.thisPlayer.targets.Contains(mouseOverHex)) {
							mouseOverHex.renderer.material.color = Color.yellow;
						} else {
							mouseOverHex.renderer.material.color = Color.white;
						}
						mouseOverHex = hex;
						hex.renderer.material.color = Color.red;
					}
					if(Input.GetMouseButton(0)) {
						if(gameControl.thisPlayer.targets.Contains(hex) && gameControl.state == State.MYTURN) {
							// Play Card
							if(GameControl.IsMulti) {
								gameControl.networkControl.PlayNetworkCardOn(gameControl.thisPlayer.selectedCard, hex);
							} else {
								gameControl.PlayCardOnHex(gameControl.thisPlayer.selectedCard, hex, System.Guid.NewGuid().ToString());
							}
						}
					}
				}
			} catch  {
			}
		} else {
		}
		if(Input.GetMouseButtonDown(0)){

		}
	}
}
