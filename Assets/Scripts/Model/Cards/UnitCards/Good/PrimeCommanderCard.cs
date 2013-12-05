using UnityEngine;
using System.Collections.Generic;

public class PrimeCommanderCard : UnitCard
{
	public override int Attack {
		get {
			return 1;
		}
	}
	
	public override int Cost {
		get {
			return 7;
		}
	}
	
	public override int Health {
		get {
			return 4;
		}
	}
	
	public override int Movement {
		get {
			return 6;
		}
	}
	
	public override string Name {
		get {
			return "Prime Commander";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/commandership";
		}
	}
	
	public PrimeCommanderCard() {
		CardText += "Adjacent units and buildings gain +1/+3.";
	}
	
	public override void OnPlay (StateObject s)
	{
		new AuraBuff("Commander Presence", s.TargetHex.Unit, 1, OnRemove, OnAdd);
	}
	
	void OnRemove(Unit unit) {
		unit.MaxHealth -= 3;
		unit.Attack -= 1;
	}
	
	void OnAdd(Unit unit) {
		unit.MaxHealth += 3;
		unit.Attack += 1;
	}
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
}

