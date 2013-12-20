using UnityEngine;
using System.Collections.Generic;

public class InstantServiceCard : SpellCard
{
	public override int Cost {
		get {
			return 5;
		}
	}
	
	public override string Name {
		get {
			return "Instant Service";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		return new List<Hex>();
	}
	
	public InstantServiceCard() {
		CardText += "Heals up to three random friendly damaged buildings or units.";
	}
	
	public void HealUnit(Unit u) {
		StandardAnimation(u.Hex.collider.bounds.center);
		u.Damage(-2);
	}
	
	public override void SpellEffect (StateObject s)
	{
		List<Unit> units = s.Units.FindAll(u => u.CurrentHealth() < u.MaxHealth && u.Team == s.Caster.Team && s.Caster.Base != u && s.Opponent.Base != u); 
		if(units.Count <= 3) {
			units.ForEach(u => HealUnit(u));
		} else {
			for(int i = 0; i < 3; i++) {
				int rnd = Random.Range (0, units.Count);
				HealUnit(units[rnd]);
				units.RemoveAt(rnd);
			}
		}
	}
}

