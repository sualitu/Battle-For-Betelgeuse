using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellCard : Card
{
	public SpellCard() : base() {
		cardType = CardType.SPELL;
	}
	
	public virtual List<Hex> Targets(StateObject s) {
		return new List<Hex>();
	}
}

