using UnityEngine;
using System.Collections;

public class FinalSacrificeCard : SpellCard
{
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 4;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 18;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Final Sacrifice";
		}
	}

	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		s.Opponent.Base.Damage(s.TargetUnit.Attack);
		s.TargetUnit.Damage(int.MaxValue);		
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
		return s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
	}
	
	public FinalSacrificeCard() {
		CardText += "Deals damage to enemy mothership equal to target units attack. Unit then dies.";
	}
}

