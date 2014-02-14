using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MadScientistCard : SpellCard
{
	public override int Cost {
		get {
			return 11;
		}
	}

	public override string Name {
		get {
			return "Armageddon";
		}
	}

	public override List<Hex> Targets (StateObject s)
	{
		return new System.Collections.Generic.List<Hex>();
	}

	public override string Image {
		get {
			return "armageddon";
		}
	}

	public override bool IsTargetless {
		get {
			return true;
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
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

