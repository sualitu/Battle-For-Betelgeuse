using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReinforceCard : SpellCard {
	

	public override int Cost {
		get {
			return 4;
		}
	}

	public override int id {
		get {
			return 10;
		}
	}

	public override string Name {
		get {
			return "Reinforce";
		}
	}

	public override MockUnit MockOnPlay (MockUnit mo)
	{
		mo.CurrentHealth += 3;
		mo.MaxHealth += 3;
		return mo;
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public ReinforceCard() {
		CardText += "Increases a units health by three.";
	}

	public override void SpellEffect (StateObject s)
	{
		s.TargetUnit.MaxHealth += 3;
	}
}
