using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplorationRocketCard : SpellCard {
	
	public override int Cost {
		get {
			return 2;
		}
	}
	
	public override string Name {
		get {
			return "Exploration Rocket";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = new List<Hex>();
		
		return result;
	}

	
	public override bool IsTargetless {
		get {
			return true;
		}
	}
	
	public ExplorationRocketCard() {
		CardText += "Deals two damage to a random enemy unit. Draw a card.";
	}

	public override void SpellAnimation (StateObject s)
	{	
		Hex hex = s.Units.FindAll(u => u.Team != s.Caster.Team).RandomElement().Hex;
		s.MainHex = hex;
		GameObject missile = (GameObject) Object.Instantiate(((GameObject) Resources.Load("Projectiles/missiles")), s.Caster.Base.transform.localPosition, Quaternion.identity);
		Vector3 targetPos = new Vector3(hex.transform.position.x+Random.Range (-0.5f, 0.5f), hex.transform.position.y+1f, hex.transform.position.z+Random.Range (-0.5f, 0.5f));
		Vector3[] t = new Vector3[2];
		t[0] = new Vector3(targetPos.x+s.Caster.Base.transform.localPosition.x/2,s.Caster.Base.Hex.Distance(s.MainHex),targetPos.z+s.Caster.Base.transform.localPosition.z/2);
		t[1] = targetPos;
		missile.transform.localScale = new Vector3(0.15f,0.15f,0.15f);
		System.Object[] args = new System.Object[2];
		args[0] = missile;
		args[1] = s;
		DoDelayedEffect(s, s.Caster.Base.Hex.Distance(s.MainHex)/2);
		iTween.MoveTo(missile.gameObject, iTween.Hash ("path", t,
		                                               "islocal", true,
		                                               "orienttopath", true,
		                                               "easetype", "easeInQuad",
		                                               "time", s.Caster.Base.Hex.Distance(s.MainHex)/2,
		                                               "oncomplete", "Hit"));
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.DrawCard();
		s.MainHex.Unit.Damage(2);
	}
}
