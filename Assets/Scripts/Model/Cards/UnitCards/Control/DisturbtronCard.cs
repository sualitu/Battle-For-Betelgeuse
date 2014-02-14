using UnityEngine;
using System.Collections.Generic;

public class DisturbtronCard : UnitCard
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
			return 2;
		}
	}
	
	public override int Movement {
		get {
			return 4;
		}
	}
	
	public override string Name {
		get {
			return "Magnetic Disturber";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/magnetic_disturber";
		}
	}

	public override string Image {
		get {
			return "disturbtron";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}
	
	public DisturbtronCard() {
		CardText += "Adjacent enemy units gain -2 attack and +1 health.";
	}
	
	public override void OnPlay (StateObject s)
	{
		new AuraBuff("Disturbed", s.MainHex.Unit, 1, OnRemove, OnAdd, true);
	}
	
	void OnRemove(Unit unit) {
		unit.Attack += 2;
		unit.MaxHealth -= 1;
	}
	
	void OnAdd(Unit unit) {
		unit.Attack -= 2;
		unit.MaxHealth += 1;
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
}

