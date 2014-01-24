using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorsCard : SpellCard {
	
	public override int Cost {
		get {
			return 2;
		}
	}
	
	public override string Name {
		get {
			return "Generators";
		}
	}
	
	public GeneratorsCard() {
		CardText += "Gain one mana permanently";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.MaxMana += 1;
		s.Caster.ManaSpend += 1;
	}
}
