using UnityEngine;
using System.Collections;

public class SelfDestructCard : SpellCard
{
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
			return 17;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Self-Destruct";
		}
	}



	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		int damage = s.TargetUnit.CurrentHealth();
		foreach(Hex h in s.TargetUnit.Hex.Adjacent(GameControl.gameControl.gridControl.Map)) {
			if(h.Unit != null) {
				h.Unit.Damage(damage);
			}
		}
		
		s.TargetUnit.Damage(int.MaxValue);		
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

	public override System.Collections.Generic.List<Hex> Targets (StateObject s)
	{
		return s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
	}
	
	public SelfDestructCard() {
		CardText += "Blows up the unit dealing damage equal to its remaining health to all adjacent units.";
	}
}

