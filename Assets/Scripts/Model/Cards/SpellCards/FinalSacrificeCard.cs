using UnityEngine;
using System.Collections;

public class FinalSacrificeCard : SpellCard
{
	public override int Cost {
		get {
			return 4;
		}
	}

	public override int id {
		get {
			return 18;
		}
	}

	public override string Name {
		get {
			return "Final Sacrifice";
		}
	}

	public override System.Collections.Generic.List<Hex> Targets (StateObject s)
	{
		return s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
	}
	
	public FinalSacrificeCard() {
		CardText += "Deals damage to enemy mothership equal to target units attack. Unit then dies.";
	}

	public override void SpellEffect (StateObject s)
	{
		s.Opponent.Base.Damage(s.TargetUnit.Attack);
		s.TargetUnit.Damage(int.MaxValue);	
	}
}

