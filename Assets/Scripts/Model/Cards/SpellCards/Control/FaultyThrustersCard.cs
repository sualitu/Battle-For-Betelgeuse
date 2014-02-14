using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaultyThrustersCard : SpellCard {

	public override int Cost {
		get {
			return 1;
		}
	}

	public override string Name {
		get {
			return "Faulty Thrusters";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public FaultyThrustersCard() {
		CardText += "Sets target unit's movement to one.";
	}

	public override void SpellEffect (StateObject s)
	{
		int oldAttack = s.MainHex.Unit.MaxMovement;
		s.MainHex.Unit.AddBuff(
			new UnitBuff("Faulty Thrusters", 
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
		mo.Movement = 1;
		return mo.Value(he);
	}
	
	void OnAdd(Unit u) {
		u.MaxMovement = 1;
	}
	
	void OnRemove(Unit u, int i) {
		u.MaxMovement = i;
	}
}
