using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MadScientistCard : SpellCard
{
	public override int Cost {
		get {
			return 10;
		}
	}

	public override string Name {
		get {
			return "Mad Scientist";
		}
	}

	public override List<Hex> Targets (StateObject s)
	{
		return new System.Collections.Generic.List<Hex>();
	}
	
	public MadScientistCard() {
		CardText += "Destroys all units and buildings, except the motherships.";
	}

	public override void SpellEffect (StateObject s)
	{
		foreach(Unit u in s.Units) {
			if(u != s.Caster.Base && u != s.Opponent.Base) {
				u.Damage(int.MaxValue);
			}
		}
	}
}

