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
	
	public virtual MockUnit MockOnPlay(MockUnit mo) {
		Debug.LogError("MockOnPlay attempted called on card without it implemented. This probably means an AI is trying to use a card that is not meant for AI");
		return mo;
	}
}

