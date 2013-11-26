using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallNukeCard : SpellCard {


	public override int Cost {
		get {
			return 4;
		}
	}

	public override int id {
		get {
			return 16;
		}
	}

	public override string Name {
		get {
			return "Small Nuke";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public SmallNukeCard() {
		CardText += "Deals 2 damage to target unit and all adjacent units. Does not affect motherships.";
	}
	
	public override void SpellAnimation (StateObject s)
	{
		Hex hex = s.TargetUnit.Hex;
		GameObject missile = (GameObject) Object.Instantiate(((GameObject) Resources.Load("Projectiles/missiles")), s.Caster.Base.transform.localPosition, Quaternion.identity);
		Nuke nuke = missile.AddComponent<Nuke>();
		missile.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
		nuke.Launch(hex);
		DoDelayedEffect(s, 10);
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
				h.Unit.Damage(2);
			}
		}
	}
}
