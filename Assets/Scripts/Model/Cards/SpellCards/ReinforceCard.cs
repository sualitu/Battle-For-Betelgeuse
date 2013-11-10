using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReinforceCard : SpellCard {
	
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
			return 10;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Reinforce";
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
		s.TargetUnit.MaxHealth += 3;

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
	
	public ReinforceCard() {
		CardText += "Increases a units health by three.";
	}
}
