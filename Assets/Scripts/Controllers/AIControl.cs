using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIControl : MonoBehaviour
{
	Player player;
	GameControl gameControl;
	
	List<Hex> unitTargets = new List<Hex>();
	
	public AIControl SetAI(Player player, GameControl gameControl) {
		player.Ai = true;
		this.player = player;
		this.gameControl = gameControl;
		return this;
	}
	
	bool MyTurn() {
		return gameControl.state == State.ENEMYTURN;
	}
	
	bool MovesInProgress() {
		return iTween.tweens.Count > 1;
	}
	
	bool PlayCard() {
		
		// Choose a card
		Card card = player.Hand.Find(c => c.Cost <= player.ManaLeft());
		if(card == null) { return false; }
		// Choose a tile
		Hex targetHex = null;
		List<Hex> targets = new List<Hex>();
		player.Base.Hex.Adjacent(gameControl.gridControl.Map).ForEach(h => h.Adjacent(gameControl.gridControl.Map).ForEach(he => targets.Add(he)));
		targets.RemoveAll(h => h.Unit != null);
		if(targets.Count > 1) {
			targetHex = targets[Random.Range(0, targets.Count-1)];
		} else if(targets.Count == 1) {
			targetHex = targets[0];
		} else {
			return false;
		}
		// Play card
		gameControl.EnemyCardPlayed(card);
		gameControl.PlayCardOnHex(card, targetHex, System.Guid.NewGuid().ToString());
		player.SpendMana(card.Cost);
		player.Hand.Remove(card);
		MoveUnits();
		return true;
	}
	
	List<Unit> MyUnits() {
		return gameControl.units.FindAll(u => u.Team == player.Team);
	}
	
	List<Hex> getAllPossibleMovesForUnit(Unit unit) {
		int i = unit.MovementLeft();
		List<Hex> targets = new List<Hex>();
		getMoves(unit.Hex, i, targets);
		return targets;
	}
	
	void getMoves(Hex hex, int i, List<Hex> acc) {
		if(i < 1) {
			return;
		} else {
			List<Hex> adj = hex.Adjacent(gameControl.gridControl.Map).FindAll(h => !acc.Contains(h) && (h.Unit == null || h.Unit.Team != player.Team));
			acc.AddRange(adj);
			adj.ForEach(h => getMoves(h, --i, acc));
		}
	}
	
	float GetDist(Hex source, Hex sink) {
		return Mathf.Sqrt(Mathf.Pow(source.GridPosition.x-sink.GridPosition.x,2)+Mathf.Pow(source.GridPosition.y-sink.GridPosition.y,2));
	}
		
	bool MoveUnits() {
		// Find a unit
		List<Unit> unitsWithMoves = MyUnits().FindAll(u => u.MovementLeft() > 0);
		Unit unit = null;
		if(unitsWithMoves.Count > 1) {
			unit = unitsWithMoves[Random.Range(0, unitsWithMoves.Count-1)];
		} else if(unitsWithMoves.Count == 1) {
			unit = unitsWithMoves[0];
		} else {
			return false;
		}
		// Move unit
		Hex targetHex = null;
		unitTargets = new List<Hex>();
		unitTargets = getAllPossibleMovesForUnit(unit);
		targetHex = unitTargets.Find(h => h.Unit != null && h.Unit.Team != player.Team);
		if(targetHex == null) {
			if(unitTargets.Count > 1) {
				
				unitTargets.ForEach(h => targetHex = (targetHex == null || GetDist(unit.Hex, targetHex) < GetDist(unit.Hex, h)) ? h : targetHex);
			} else if(unitTargets.Count == 1) {
				targetHex = unitTargets[0];
			} else {
				return false;
			}
		}
		unit.PrepareMove(targetHex);
		return true;
	}
	
	
	void EndTurn() {
		gameControl.EnemeyEndTurn();
	}
	
	void Update() {
		if(MyTurn() && !MovesInProgress()) {
			if(!PlayCard() && !MoveUnits()) {
				EndTurn();
			}
		}
	}
}

