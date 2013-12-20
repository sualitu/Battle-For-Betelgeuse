using UnityEngine;
using System.Collections.Generic;

public class DestructiveLoadCard : SpellCard
{
	public override int Cost {
		get {
			return 5;
		}
	}
	
	public override string Name {
		get {
			return "Destructive Load";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public DestructiveLoadCard() {
		CardText += "Gives target unit Death Touch.";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.TargetHex.Unit.AddBuff(new DeathTouchBuff());
	}
}

