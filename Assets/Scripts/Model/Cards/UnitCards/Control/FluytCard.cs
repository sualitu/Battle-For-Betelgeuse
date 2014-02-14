using UnityEngine;
using System.Collections;

public class FluytCard : UnitCard
{
	public override int Attack {
		get {
			return 1;
		}
	}
	
	public override int Cost {
		get {
			return 4;
		}
	}
	
	public override int Health {
		get {
			return 5;
		}
	}
	
	public override int Movement {
		get {
			return 4;
		}
	}
	
	public override string Name {
		get {
			return "Fluyt";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/fluyt";
		}
	}

	public override string Image {
		get {
			return "fluyt";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}

	public FluytCard() {
		CardText += "Lowers the attack of the enemy by one on combat.";
	}
	
	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		return;
	}

	public override void OnAttack (StateObject s)
	{
		AddBuff (s.SecondaryHex.Unit);
	}

	public override bool OnAttacked (StateObject s)
	{
		AddBuff(s.MainHex.Unit);
		return true;
	}

	void AddBuff(Unit u) {
		u.AddBuff(new UnitBuff("Fluyted", onRemove: OnRemove, onApplication: OnAdd, duration: -1));
	}

	void OnAdd(Unit u) {
		u.Attack -= 1;
	}

	void OnRemove(Unit u) {
		u.Attack += 1;
	}
	
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

