using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class MouseControl : MonoBehaviour {
		
	public Unit selectedUnit;
	GameControl gameControl;
	GameObject selectorObject;
	GameObject mouseOverAnimation;
	public bool PlayModeOn = true;

	bool lastWasCircle = true;
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
				selectedUnit.Movable.ForEach(t => { 
					t.renderer.material.color = Settings.StandardTileColour; 
					t.renderer.material.mainTexture = Assets.Instance.BaseHex;
				} );
			}
			selHex.IsSelected = false;
			selHex.renderer.material.color = Settings.StandardTileColour;
			selHex = null;
			selectedUnit = null;
			if(selectorObject != null) Destroy(selectorObject);
		}
	}
	
	public void SelectHex(Hex hex) {
		if( selHex != null ) {
			DeselectHex();
		}
		selHex = hex;
		hex.IsSelected = true;
		if(hex.Unit != null && hex.Unit.Team != Team.NEUTRAL) {
			selectedUnit = hex.Unit;
			selectorObject = (GameObject) Instantiate(Assets.Instance.Selector, hex.renderer.collider.bounds.center, Quaternion.identity);
		}
		if(selectedUnit != null && selectedUnit.Team == gameControl.ThisPlayer.Team) {
			hex.Unit.Movable = PathFinder.BreadthFirstSearch(hex, gameControl.GridControl.Map, hex.Unit.MovementLeft(), hex.Unit.Team);
		}
	}
	
	bool clickedLastSec = false;
	int i = 0;

	void DestroyMouseOver() {
		if(mouseOverAnimation != null) {
			iTween.Stop (mouseOverAnimation);
			Destroy(mouseOverAnimation);
		} 
	}
	
	void Update () {
		gameControl.Flags.ForEach(f => f.ColourizeHexs());
		if(clickedLastSec) {
			i++;
			if(i > 24) {
				clickedLastSec = false;
				i = 0;
			}
		}
		if(!gameControl.GuiControl.MouseIsOverGUI()) {
			if(Input.GetMouseButton(1)) {
				// Right click
				DeselectHex();
			}
			ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
			if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {
				Hex hex = null;
				try { hex = rayHit.collider.GetComponent<Hex>(); } catch {}
				if(hex == null) { return; }

				bool isNew = false;
				if(mouseOverHex == null) {
					mouseOverHex = hex;
				} else if (mouseOverHex.GridPosition != hex.GridPosition) {
					if(gameControl.ThisPlayer.targets.Contains(mouseOverHex)) {
						mouseOverHex.renderer.material.color = Settings.MovableTileColour;
					}
					else {
						mouseOverHex.renderer.material.color = Settings.StandardTileColour;
					}
					mouseOverHex = hex;
					isNew = true;
				}
				hex.renderer.material.color = Settings.MouseOverTileColour;


				if(selectedUnit != null && selectedUnit.Movable.Contains(hex)) {
					if(mouseOverAnimation == null || lastWasCircle) {
						if(lastWasCircle) 
							DestroyMouseOver();
						mouseOverAnimation = (GameObject) Instantiate (Assets.Instance.MouseOver);
						Hover hover = mouseOverAnimation.AddComponent<Hover>();
						hover.MoveTo(hex.renderer.collider.bounds.center);
						hover.Hex = hex;
						lastWasCircle = false;
					}  else { 
						if(isNew) {
							mouseOverAnimation.GetComponent<Hover>().Hex = hex;
							mouseOverAnimation.GetComponent<Hover>().MoveTo(hex.renderer.collider.bounds.center);
						} 
					}
				} else {
					lastWasCircle = true;
					if(mouseOverAnimation == null) {
						mouseOverAnimation = (GameObject) Instantiate(Assets.Instance.MouseOverCircle, new Vector3(hex.renderer.collider.bounds.center.x, 0.25f, hex.renderer.collider.bounds.center.z), Quaternion.identity);
					} else {
						if(isNew) {
							DestroyMouseOver();
							mouseOverAnimation = (GameObject) Instantiate(Assets.Instance.MouseOverCircle, new Vector3(hex.renderer.collider.bounds.center.x, 0.25f, hex.renderer.collider.bounds.center.z), Quaternion.identity);
						}
					}
				}
				if(!clickedLastSec && Input.GetMouseButton(0)) {
					clickedLastSec = true;
					// Left click
					if(gameControl.ThisPlayer.selectedCard != null) {
						// A card is selected
						if(gameControl.ThisPlayer.targets.Contains(hex) && gameControl.State == State.MYTURN) {
							if(GameControl.IsMulti) {
								gameControl.NetworkControl.PlayNetworkCardOn(gameControl.ThisPlayer.selectedCard, hex);
							} else {
								gameControl.PlayCardOnHex(gameControl.ThisPlayer.selectedCard, hex, System.Guid.NewGuid().ToString());
							}
							DeselectHex();
							gameControl.ThisPlayer.DeselectCard();
						} else {
							gameControl.GuiControl.ShowSmallSplashText(Dictionary.CannotMoveThereError);
						}
					} else if(selectedUnit != null && selectedUnit.Team == gameControl.ThisPlayer.Team && selectedUnit.Hex.GridPosition != hex.GridPosition) {
						// A unit is selected
						if((hex.Unit == null || hex.Unit.Team != gameControl.ThisPlayer.Team) && gameControl.State == State.MYTURN) {
							if(GameControl.IsMulti) {
								gameControl.NetworkControl.MoveNetworkUnit(selectedUnit, hex);
							} else {
								selectedUnit.PrepareMove (hex);
							}
						} else {
							DeselectHex();
							SelectHex(hex);
						}
					} else if(selectedUnit == null || (hex.Unit != null && hex.Unit.Team == Team.ME)) {
						// Select a unit
						SelectHex(hex);
					}
				}
			} else {
				DestroyMouseOver();
			}
		} else {
			DestroyMouseOver();
		}
	}
}