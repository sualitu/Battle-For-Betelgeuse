using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretCard : BuildingCard
{
	public override int Attack {
		get {
			return 2;
		}
	}

	public override int Cost {
		get {
			return 1;
		}
	}

	public override int Health {
		get {
			return 5;
		}
	}

	public override int id {
		get {
			return 6;
		}
	}

	public override string Name {
		get {
			return "Turret";
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
		List<Unit> targets = new List<Unit>();
		targets.AddRange(s.TargetUnit.Hex.Adjacent(GameControl.gameControl.gridControl.Map).FindAll(h => h.Unit != null && h.Unit.Team != s.TargetUnit.Team).ConvertAll<Unit>(h => h.Unit));
		if(targets.Count > 0) {
			s.TargetUnit.attacking = targets[0];
			s.TargetUnit.AttackTarget(targets[0], 0);
		}
	}

	public override void OnPlay (StateObject s)
	{
		base.OnPlay (s);
	}

	public override string PrefabPath {
		get {
			return "Buildings/turret";
		}
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

