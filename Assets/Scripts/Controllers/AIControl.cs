using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIControl : MonoBehaviour
{
	Player player;
	GameControl gameControl;
	Dictionary<Unit, List<Hex>> unitMoves = new Dictionary<Unit, List<Hex>>();
	Dictionary<Unit, List<Hex>> unitBlacklist = new Dictionary<Unit, List<Hex>>();	
	Hex currentHex;
	
	public void DoTurn() {
	}
	
	/// <summary>
	/// Calculates the hex value.
	/// </summary>
	/// <returns>
	/// The hex value.
	/// </returns>
	/// <param name='hex'>
	/// Hex.
	/// </param>
	int CalculateHexValue(Hex hex) {
		if(hex.Unit != null) {
			if(hex.Unit.Team == player.Team) {
				return int.MinValue;
			} else {
				return int.MaxValue - hex.Unit.Attack;
			}
		} else {
			return 10000 - Mathf.FloorToInt(hex.Distance(gameControl.thisPlayer.Base.Hex));
		}
	}
	
	/// <summary>
	/// Sets AI.
	/// </summary>
	/// <returns>
	/// The AIControl for the player.
	/// </returns>
	/// <param name='player'>
	/// Player.
	/// </param>
	/// <param name='gameControl'>
	/// Game control.
	/// </param>
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
		return iTween.tweens.Count > 2;
	}
	
	List<Unit> MyUnits() {
		return gameControl.units.FindAll(u => u.Team == player.Team);
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
	
	bool PlayCard() {
		// TODO When adding different card types, improve this.
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
		Unit unit = gameControl.PlayCardOnHex(card, targetHex, System.Guid.NewGuid().ToString());
		player.SpendMana(card.Cost);
		player.Hand.Remove(card);
		unitMoves[unit] = StandardList();
		unitBlacklist[unit] = new List<Hex>();
		return true;
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
        unitMoves[unit] = SortHexList(unitMoves[unit]);
		if(unitMoves[unit].Count > 0) {
			int i = 0;
			try {
				while(PathFinder.DepthFirstSearch(unit.Hex, unitMoves[unit][i], gameControl.gridControl.Map, unit.MovementLeft()).Count < 1) {
					i++;
				}
			} catch {
				return false;
			}
			targetHex = unitMoves[unit][i];
		} else {
			return false;
		}
		unitBlacklist[unit] = new List<Hex>();
		unitMoves[unit] = StandardList();
        unit.PrepareMove(targetHex);
        return true;
	}
	
	
	void EndTurn() {
		gameControl.EnemeyEndTurn();
	}
	
	Hex GetNextHex() {
		int x;
		int y;
		if(currentHex == null) {
			x = 0;
			y = 0;
		} else {
			x = (int) currentHex.GridPosition.x;
			y = (int) currentHex.GridPosition.y;
		}
		while(!gameControl.gridControl.boolMap[x][y]) {
			if(x < gameControl.gridControl.MapSize.x) {
				x++;
			} else {
				x = 0;
				y++;
			}
		}
		return gameControl.gridControl.Map[x][y];
	}
	
	List<Hex> SortHexList(List<Hex> hexs) {
		IEnumerable<Hex> k = from h in hexs
				orderby CalculateHexValue(h) descending
				select h;
		return new List<Hex>(k);
	}
	
	List<Hex> StandardList() {
		List<Hex> returnList = new List<Hex>();
		foreach(List<Hex> row in gameControl.gridControl.Map) {
			foreach(Hex h in row) {
				if(h != null) {
					returnList.Add(h);
				}
			}
		}
		return SortHexList(returnList);
	}
	
	void Update() {
		currentHex = GetNextHex();
		foreach(Unit unit in MyUnits()) {
			if(!unitMoves.ContainsKey(unit)) {
				unitMoves[unit] = new List<Hex>();
				unitBlacklist[unit] = new List<Hex>();
			}
			if(!unitMoves[unit].Contains(currentHex)) {
				List<Hex> path = PathFinder.DepthFirstSearch(unit.Hex, currentHex, gameControl.gridControl.Map, unit.MovementLeft());
				if(path.Count > 0) {
					unitMoves[unit].Add(currentHex);
				} else {
					unitBlacklist[unit].Add(currentHex);	
				}
			}
		}
		if(MyTurn() && !MovesInProgress()) {
			if(!PlayCard() && !MoveUnits()) {
				EndTurn();
			}
		}
	}
}

