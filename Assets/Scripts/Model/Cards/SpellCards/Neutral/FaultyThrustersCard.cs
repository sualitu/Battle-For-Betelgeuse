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
		s.TargetHex.Unit.MaxMovement = 1;
	}
}
