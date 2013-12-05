using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreciseMissileCard : SpellCard {

	public override int Cost {
		get {
			return 2;
		}
	}

	public override string Name {
		get {
			return "Precise Missile";
		}
	}

	public override MockUnit MockOnPlay (MockUnit mo)
	{
		mo.CurrentHealth -= 3;
		return mo;
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team != s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public PreciseMissileCard() {
		CardText += "Deals 3 damage to target unit or building.";
	}

	public override void SpellAnimation (StateObject s)
	{	
		Hex hex = s.TargetHex;
		GameObject missile = (GameObject) Object.Instantiate(((GameObject) Resources.Load("Projectiles/missiles")), s.Caster.Base.transform.localPosition, Quaternion.identity);
		Vector3 targetPos = new Vector3(hex.transform.position.x+Random.Range (-0.5f, 0.5f), hex.transform.position.y+1f, hex.transform.position.z+Random.Range (-0.5f, 0.5f));
		Vector3[] t = new Vector3[2];
		t[0] = new Vector3(targetPos.x+s.Caster.Base.transform.localPosition.x/2,s.Caster.Base.Hex.Distance(s.TargetHex),targetPos.z+s.Caster.Base.transform.localPosition.z/2);
		t[1] = targetPos;
		missile.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
		System.Object[] args = new System.Object[2];
		args[0] = missile;
		args[1] = s;
		DoDelayedEffect(s, s.Caster.Base.Hex.Distance(s.TargetHex)/2);
		iTween.MoveTo(missile.gameObject, iTween.Hash ("path", t,
			"islocal", true,
			"orienttopath", true,
			"easetype", "easeInQuad",
			"time", s.Caster.Base.Hex.Distance(s.TargetHex)/2,
			"oncomplete", "Hit"));
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.TargetHex.Unit.Damage(3);
	}
}
