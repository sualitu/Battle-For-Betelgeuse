using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaultyLaunchersCard : SpellCard {

	public override int Cost {
		get {
			return 3;
		}
	}

	public override string Name {
		get {
			return "Faulty Launchers";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public FaultyLaunchersCard() {
		CardText += "Sets target unit's attack to 1.";
	}

	public override void SpellEffect (StateObject s)
	{
		int oldAttack = s.MainHex.Unit.Attack;
		s.MainHex.Unit.AddBuff(
			new UnitBuff("Faulty Launcher", 
			             onRemove: (unit => OnRemove (unit, oldAttack)), 
			             onApplication: OnAdd, 
			             duration: -1)
			);
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}

	public override int MockOnPlay (MockUnit mo, HexEvaluator he)
	{
		mo.Attack = 1;
		return mo.Value(he);
	}

	void OnAdd(Unit u) {
		u.Attack = 1;
	}

	void OnRemove(Unit u, int i) {
		u.Attack = i;
	}
}
