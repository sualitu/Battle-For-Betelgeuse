using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BuildingCard : EntityCard
{
	public BuildingCard() : base() {
		cardType = CardType.BUILDING;
	}
	
	public override int Movement {
		get {
			return 0;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> targets = new List<Hex>();

		GameControl.gameControl.Flags.FindAll(f => f.OwnerTeam == s.Caster.Team).ForEach(f => targets.AddRange(f.Hexs));

		targets.RemoveAll(h => h.Unit != null);
		return targets;
	}
}

