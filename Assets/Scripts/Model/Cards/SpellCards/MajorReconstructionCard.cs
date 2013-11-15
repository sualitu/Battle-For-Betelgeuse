using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MajorReconstructionCard : SpellCard {
	
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 4;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 8;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Major Reconstruction";
		}
	}


	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		if(s.TargetUnit.CurrentHealth() < s.TargetUnit.MaxHealth) {
			if(s.TargetUnit.MaxHealth - s.TargetUnit.CurrentHealth() < 5) {
				s.TargetUnit.Damage(-(s.TargetUnit.MaxHealth - s.TargetUnit.CurrentHealth()));
			} else {
				s.TargetUnit.Damage(-5);
			}
		}
		Object.Instantiate(Prefab, s.TargetUnit.transform.position, Quaternion.identity);
	}

	public override MockUnit MockOnPlay (MockUnit mo)
	{
		if(mo.CurrentHealth < mo.MaxHealth) {
			if(mo.MaxHealth - mo.CurrentHealth < 5) {
				mo.CurrentHealth = mo.MaxHealth;
			} else {
				mo.CurrentHealth += 5;
			}
		}
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
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public MajorReconstructionCard() {
		CardText += "Heals target unit or building for five.";
	}

}
