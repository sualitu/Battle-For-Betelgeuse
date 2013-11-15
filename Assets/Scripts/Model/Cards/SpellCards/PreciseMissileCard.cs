using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreciseMissileCard : SpellCard {
	
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 2;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 11;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Precise Missile";
		}
	}


	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		s.TargetUnit.Damage(3);

		Object.Instantiate(Prefab, s.TargetUnit.transform.position, Quaternion.identity);
	}

	public override MockUnit MockOnPlay (MockUnit mo)
	{
		mo.CurrentHealth -= 3;
		return mo;
	}
	public override string PrefabPath {
		get {
			return "Effects/Heal";
		}
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team != s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public PreciseMissileCard() {
		CardText += "Deals 3 damage to target unit or building.";
	}
}
