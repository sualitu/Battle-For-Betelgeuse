using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImproveThrustersCard : SpellCard {

	public override int Cost {
		get {
			return 4;
		}
	}

	public override string Name {
		get {
			return "Improve Thrusters";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public ImproveThrustersCard() {
		CardText += "Doubles target unit's movement.";
	}

	public override void SpellEffect (StateObject s)
	{
		s.TargetHex.Unit.MaxMovement *= 2;
	}
}
