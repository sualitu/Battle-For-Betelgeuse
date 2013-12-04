using UnityEngine;
using System.Collections.Generic;

public class IntergalacticPoliticsCard : SpellCard {
	
	public override int Cost {
		get {
			return 9;
		}
	}
	
	public override int id {
		get {
			return 23;
		}
	}
	
	public override string Name {
		get {
			return "Intergalactic Diplomacy";
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
	
	public IntergalacticPoliticsCard() {
		CardText += "Grants all players 150 victory points.";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.Points += 150;
		s.Opponent.Points += 150;
	}
}


