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
		CardText += "Deals five damage to target unit and all unist within 2 tiles. Does not affect motherships.";
	}
	
	public override void SpellAnimation (StateObject s)
	{
		Hex hex = s.TargetUnit.Hex;
		GameObject missile = (GameObject) Object.Instantiate(((GameObject) Resources.Load("Projectiles/missiles")), s.Caster.Base.transform.localPosition, Quaternion.identity);
		Nuke nuke = missile.AddComponent<Nuke>();
		missile.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
		nuke.Launch(hex);
		DoDelayedEffect(s, 10);
	} 

	public override void SpellEffect (StateObject s)
	{
		List<Hex> targets = new List<Hex>();
		targets = PathFinder.BreadthFirstSearch(s.TargetUnit.Hex, GameControl.gameControl.gridControl.Map, 2, 0);
		if(s.TargetUnit != s.Caster.Base && s.TargetUnit != s.Opponent.Base) {
			targets.Add(s.TargetUnit.Hex);
		}
				
		foreach(Hex h in targets) {
			if(h.Unit != null && h.Unit != s.Caster.Base && h.Unit != s.Opponent.Base) {
				h.Unit.Damage(5);
			}
		}
	}
}
