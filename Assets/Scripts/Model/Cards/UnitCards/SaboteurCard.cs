using UnityEngine;
using System.Collections;

public class SaboteurCard : UnitCard
{
	public override int Attack {
		get {
			return 0;
		}
	}

	public override int Cost {
		get {
			return 4;
		}
	}

	public override int Health {
		get {
			return 2;
		}
	}

	public override int Movement {
		get {
			return 5;
		}
	}

	public override string Name {
		get {
			return "Saboteur";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/saboteur";
		}
	}

	public override int id {
		get {
			return 22;
		}
	}
	
	public SaboteurCard() {
		CardText += "Instantly kills any unit or building it attacks.";
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
	}

	public override void OnAttack (StateObject s)
	{
		s.TargetUnit.Attack = int.MaxValue;
		s.TargetUnit.MaxHealth = 1;
		s.TargetUnit.ResetStats();
	}

	public override bool OnAttacked (StateObject s)
	{
		return base.OnAttacked (s);
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
	public override void OnNewTurn (StateObject s)
	{
		s.TargetUnit.Attack = 0;
		s.TargetUnit.MaxHealth = 2;
	}
}

