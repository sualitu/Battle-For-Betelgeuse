using UnityEngine;
using System.Collections;

public abstract class UnitCard : Card
{
	public UnitCard() : base() {
		cardType = CardType.UNIT;
	}
}

