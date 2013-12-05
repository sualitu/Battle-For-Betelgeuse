using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeepSpaceExplorationCard : SpellCard {
	
	public override int Cost {
		get {
			return 3;
		}
	}
	
	public override string Name {
		get {
			return "Deep Space Exploration";
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
	
	public DeepSpaceExplorationCard() {
		CardText += "Draw two cards.";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.DrawCard();
		s.Caster.DrawCard();
	}

}

