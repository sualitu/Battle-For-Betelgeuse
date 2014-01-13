using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeDistortionCard : SpellCard {
	
	public override int Cost {
		get {
			return 4;
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
		for(int i = 0; i < GameControl.gameControl.GridControl.boolMap.Count; i++) {
			for(int j = 0; j < GameControl.gameControl.GridControl.boolMap[i].Count; j++) {
				if(GameControl.gameControl.GridControl.boolMap[i][j]) {
					result.Add (GameControl.gameControl.GridControl.Map[j][i]);
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
		List<Unit> units = PathFinder.BreadthFirstSearch(s.TargetHex, GameControl.gameControl.GridControl.Map, 3, 0).FindAll(h => h.Unit != null).ConvertAll<Unit>(h => h.Unit);
		units.RemoveAll(u => u.Team == s.Caster.Team);
		effect = (GameObject) Resources.Load ("Effects/Debuff");
		if(s.TargetHex.Unit != null && s.TargetHex.Unit.Team != s.Caster.Team) units.Add(s.TargetHex.Unit);
		units.ForEach(u => u.AddBuff (new UnitBuff("Distorted", newTurn : Buff, onApplication : Buff)));
		units.ForEach(u => Effect(u.Hex.renderer.collider.bounds.center));
	}

	
	GameObject effect;
	
	void Effect(Vector3 place) {
		Object.Instantiate(effect, place, Quaternion.identity);
	}

	void Buff(Unit unit) {
		unit.Move(int.MaxValue);
	}
}
