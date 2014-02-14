using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallNukeCard : SpellCard {


	public override int Cost {
		get {
			return 4;
		}
	}

	public override string Name {
		get {
			return "Small Nuke";
		}
	}

	public override string Image {
		get {
			return "nuke";
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
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}
	
	public override void SpellAnimation (StateObject s)
	{
		//AudioControl.PlayAudioFile("alarm");
		Hex hex = s.MainHex;
		GameObject missile = (GameObject) Object.Instantiate(((GameObject) Resources.Load("Projectiles/missiles")), s.Caster.Base.transform.localPosition, Quaternion.identity);
		Nuke nuke = missile.AddComponent<Nuke>();
		missile.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
		nuke.Launch(hex);
		DoDelayedEffect(s, 10);
	}

	public override void SpellEffect (StateObject s)
	{
		//AudioControl.PlayAudioFile("explosions/nuke");
		List<Hex> targets = new List<Hex>();
		if(s.MainHex.Unit != s.Caster.Base && s.MainHex.Unit != s.Opponent.Base) {
			targets.Add(s.MainHex);
		}
		
		s.MainHex.Adjacent(GameControl.gameControl.GridControl.Map).ForEach(h => targets.Add(h));
		
		foreach(Hex h in targets) {
			if(h.Unit != null && h.Unit != s.Caster.Base && h.Unit != s.Opponent.Base) {
				h.Unit.Damage(2);
			}
		}
	}
}
