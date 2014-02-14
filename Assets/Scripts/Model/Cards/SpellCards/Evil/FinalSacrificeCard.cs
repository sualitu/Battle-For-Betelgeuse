using UnityEngine;
using System.Collections;

public class FinalSacrificeCard : SpellCard
{
	public override int Cost {
		get {
			return 0;
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
		CardText += "Sacrifice a unit and draw cards equal to the lowest of its health and attack.";
	}
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}

	public override void SpellEffect (StateObject s)
	{
		Unit target = s.MainHex.Unit;
		int draw = Mathf.Min(target.Attack, target.MaxHealth);
		target.Damage(int.MaxValue);
		for(int i = 0; i < draw; i++) {
			s.Caster.DrawCard();
		}
	}
}

