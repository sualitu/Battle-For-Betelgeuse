using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UnitCard : EntityCard
{
	public UnitCard() : base() {
		cardType = CardType.UNIT;
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> targets = new List<Hex>();
		s.Caster.Base.Hex.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(h => h.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(he => targets.Add(he)));
		targets.RemoveAll(h => h.Unit != null);
		return targets;
	}
	
}

