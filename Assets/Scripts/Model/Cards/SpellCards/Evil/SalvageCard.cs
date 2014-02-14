using UnityEngine;
using System.Collections;

public class SalvageCard : SpellCard
{
	public override int Cost {
		get {
			return 8;
		}
	}
	
	public override string Name {
		get {
			return "Salvage";
		}
	}
	
	public override System.Collections.Generic.List<Hex> Targets (StateObject s)
	{
		return s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
	}
	
	public SalvageCard() {
		CardText += "Sacrifice a unit and gain victory points equal to its health and attack times 10";
	}
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.Points += (s.MainHex.Unit.Attack+s.MainHex.Unit.CurrentHealth() * 10);
		s.MainHex.Unit.Damage(int.MaxValue);
	}
}

