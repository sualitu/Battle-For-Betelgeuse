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
		s.Caster.Base.Hex.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(h => h.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(he => targets.Add(he)));
		HashSet<Hex> hsTargets = new HashSet<Hex>(targets);
		foreach(Hex hex in hsTargets) {
			targets.AddRange(PathFinder.BreadthFirstSearch(hex, GameControl.gameControl.gridControl.Map, 4, s.Caster.Team));
		}
		targets.RemoveAll(h => h.Unit != null);
		return targets;
	}
}

