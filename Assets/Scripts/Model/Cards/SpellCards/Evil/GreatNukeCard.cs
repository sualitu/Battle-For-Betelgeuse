using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GreatNukeCard : SpellCard {

	public override int Cost {
		get {
			return 8;
		}
	}

	public override string Name {
		get {
			return "Great Nuke";
		}
	}

	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}

	public override string Image {
		get {
			return "bignuke";
		}
	}
	
	public GreatNukeCard() {
		CardText += "Deals five damage to target unit and all units within two tiles. Does not affect motherships.";
	}
	
	public override void SpellAnimation (StateObject s)
	{
		AudioControl.PlayAudioFile("alarm");
		Hex hex = s.MainHex;
		GameObject missile = (GameObject) Object.Instantiate(((GameObject) Resources.Load("Projectiles/missiles")), s.Caster.Base.transform.localPosition, Quaternion.identity);
		Nuke nuke = missile.AddComponent<Nuke>();
		missile.transform.localScale = new Vector3(0.4f,0.4f,0.4f);
		nuke.Launch(hex);
		DoDelayedEffect(s, 10);
	} 

	public override void SpellEffect (StateObject s)
	{
		AudioControl.PlayAudioFileAt("explosions/nuke", s.MainHex.transform.localPosition);
		List<Hex> targets = new List<Hex>();
		targets = PathFinder.BreadthFirstSearch(s.MainHex, GameControl.gameControl.GridControl.Map, 2, 0);
		if(s.MainHex.Unit != s.Caster.Base && s.MainHex.Unit != s.Opponent.Base) {
			targets.Add(s.MainHex);
		}
				
		foreach(Hex h in targets) {
			if(h.Unit != null && h.Unit != s.Caster.Base && h.Unit != s.Opponent.Base) {
				h.Unit.Damage(5);
			}
		}
	}
}
