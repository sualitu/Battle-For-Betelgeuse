using UnityEngine;
using System.Collections;

public class MindControlCard : SpellCard
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
			return 12;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Mind Control";
		}
	}

	

	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		s.TargetUnit.Team = s.Caster.Team;
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

	public override System.Collections.Generic.List<Hex> Targets (StateObject s)
	{
		return s.Units.FindAll(u => u.Team != s.Caster.Team && u != s.Opponent.Base).ConvertAll<Hex>(u => u.Hex);
	}
	
	
	public MindControlCard() {
		CardText += "Take control over an enemy unit or building.";
	}
}

