using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinorReconstructionCard : SpellCard {
	
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 1;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 9;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Minor Reconstruction";
		}
	}

	public override void OnAttack ()
	{
		base.OnAttack ();
	}

	public override bool OnAttacked ()
	{
		return base.OnAttacked ();
	}

	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		if(s.TargetUnit.CurrentHealth() < s.TargetUnit.MaxHealth) {
			if(s.TargetUnit.MaxHealth - s.TargetUnit.CurrentHealth() < 2) {
				s.TargetUnit.Damage(-(s.TargetUnit.MaxHealth - s.TargetUnit.CurrentHealth()));
			} else {
				s.TargetUnit.Damage(-2);
			}
		}
		Object.Instantiate(Prefab, s.TargetUnit.transform.position, Quaternion.identity);
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
		result.ForEach(h => h.renderer.material.color = Color.green);
		return result;
	}
	
	public MinorReconstructionCard() {
		CardText += "Heals target unit or building for two.";
	}
}
