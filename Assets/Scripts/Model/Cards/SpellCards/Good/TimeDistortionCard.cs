using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeDistortionCard : SpellCard {
	
	public override int Cost {
		get {
			return 4;
		}
	}

	public override int id {
		get {
			return 21;
		}
	}

	public override string Name {
		get {
			return "Time Distortion";
		}
	}

	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = new List<Hex>();
		for(int i = 0; i < GameControl.gameControl.gridControl.boolMap.Count; i++) {
			for(int j = 0; j < GameControl.gameControl.gridControl.boolMap[i].Count; j++) {
				if(GameControl.gameControl.gridControl.boolMap[i][j]) {
					result.Add (GameControl.gameControl.gridControl.Map[j][i]);
				}
			}
		}
		return result;
	}
	
	public TimeDistortionCard() {
		CardText += "Units within three tiles cannot attack or move next turn.";
	}

	public override void SpellAnimation (StateObject s) {
		Object.Instantiate((GameObject) Resources.Load ("Effects/Splash"), s.TargetHex.collider.bounds.center, Quaternion.identity);
		SpellEffect(s);
	}

	public override void SpellEffect (StateObject s)
	{
		List<Unit> units = PathFinder.BreadthFirstSearch(s.TargetHex, GameControl.gameControl.gridControl.Map, 3, 0).FindAll(h => h.Unit != null).ConvertAll<Unit>(h => h.Unit);
		units.ForEach(u => u.buffs.Add (new UnitBuff("Distorted", Buff, Buff, unit : u)));
	}

	void Buff(Unit unit) {
		unit.Move(int.MaxValue);
	}
}