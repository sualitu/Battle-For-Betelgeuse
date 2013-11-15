using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FaultyLaunchersCard : SpellCard {
	
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 3;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 14;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Faulty Launchers";
		}
	}


	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		s.TargetUnit.Attack = 1;

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
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public FaultyLaunchersCard() {
		CardText += "Sets target unit's attack to 1.";
	}

}
