using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaultyLaunchersCard : SpellCard {

	public override int Cost {
		get {
			return 3;
		}
	}

	public override int id {
		get {
			return 14;
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
		s.TargetHex.Unit.Attack = 1;
	}
}
