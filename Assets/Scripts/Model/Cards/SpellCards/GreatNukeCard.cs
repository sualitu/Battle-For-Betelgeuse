using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreatNukeCard : SpellCard {

	public override int Cost {
		get {
			return 8;
		}
	}

	public override int id {
		get {
			return 19;
		}
	}


	public override string Name {
		get {
			return "Great Nuke";
		}
	}
	
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public GreatNukeCard() {
		CardText += "Deals five damage to target unit and all adjacent units. Does not affect motherships.";
	}

	public override void SpellEffect (StateObject s)
	{
		List<Hex> targets = new List<Hex>();
		if(s.TargetUnit != s.Caster.Base && s.TargetUnit != s.Opponent.Base) {
			targets.Add(s.TargetUnit.Hex);
		}
		
		s.TargetUnit.Hex.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(h => targets.Add(h));
		
		foreach(Hex h in targets) {
			if(h.Unit != null && h.Unit != s.Caster.Base && h.Unit != s.Opponent.Base) {
				h.Unit.Damage(5);
			}
		}
	}
}
