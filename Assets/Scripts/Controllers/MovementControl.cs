using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GameControl))]
public class MovementControl : MonoBehaviour
{
	
	
	GameControl gameControl;
	// Use this for initialization
	void Start ()
	{
		gameControl = GetComponent<GameControl>();
	}
	
	
	/*			
	private void DoMove(Unit unit, List<Hex> moves) {
		unit.Move(moves.Count);
		moves.ForEach(h => h.renderer.material.color = Color.white);
		if(moves.Count > 1) {
			List<Transform> path = moves.ConvertAll<Transform>(h => h.transform);
			iTween.MoveTo(unit.gameObject, iTween.Hash(
											"path", path.ToArray(),
											"time", moves.Count*2,
											"orienttopath", true,
											"oncomplete", "MovementDone",
											"oncompleteparams", true)); 
		} else {
			iTween.MoveTo(unit.gameObject, iTween.Hash(
											"position", moves[moves.Count-1],
											"time", 1,
											"looktarget", true)); 
		}
		Hex prevTile = unit.Hex;
		moves[moves.Count-1].Unit = unit;
		moves[moves.Count-1].Unit.Hex = moves[moves.Count-1];
		gameControl.mouseControl.selectHex(moves[moves.Count-1]);
		prevTile.Unit = null;	
	}
	
	public void MoveUnit(Unit unit, Hex hex) {
		List<Hex> path = PathFinder.DepthFirstSearch(unit.Hex, hex, gameControl.gridControl.Map, unit.MovementLeft());
		if(path.Count > 0 && (hex.Unit == null || unit.Team != hex.Unit.Team)) {
			if(hex.Unit != null) {
				gameControl.combatControl.Combat(unit, hex.Unit);
			}
			if(unit != null && (unit.CurrentHealth()) > 0 && (hex.Unit == null || unit.Id != hex.Unit.Id)) {
				if(hex.Unit != null && hex.Unit.CurrentHealth() > 0) {
					path.Remove(path[path.Count-1]);
				}
				DoMove (unit, path);
			}
		} else {
			gameControl.guiControl.ShowFloatingText(Dictionary.cannotMoveThereError, unit.transform);
		}
	}*/
	
}

