using UnityEngine;
using System.Collections.Generic;

public class ForceFieldCard : SpellCard
{
	public override int Cost {
		get {
			return 2;
		}
	}
	
	public override string Name {
		get {
			return "Force Field";
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
	
	public ForceFieldCard() {
		CardText += "Sourrounds target unit in a powerful force field.";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.MainHex.Unit.AddBuff(new ForceFieldBuff());
	}
}

