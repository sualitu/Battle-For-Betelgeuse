using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MadScientistCard : SpellCard
{
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 10;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 20;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Mad Scientist";
		}
	}


	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		foreach(Unit u in s.Units) {
			if(u != s.Caster.Base && u != s.Opponent.Base) {
				u.Damage(int.MaxValue);
			}
		}
	}

	public override string PrefabPath {
		get {
			return "Effects/Heal";
		}
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}

	public override List<Hex> Targets (StateObject s)
	{
		return new System.Collections.Generic.List<Hex>();
	}
	
	public MadScientistCard() {
		CardText += "Destroys all units and buildings, except the motherships.";
	}

}

