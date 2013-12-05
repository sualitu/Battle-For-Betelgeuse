using UnityEngine;
using System.Collections.Generic;

public class BarrierShipCard : UnitCard
{
	public override int Attack {
		get {
			return 0;
		}
	}
	
	public override int Cost {
		get {
			return 3;
		}
	}
	
	public override int Health {
		get {
			return 4;
		}
	}
	
	public override int Movement {
		get {
			return 4;
		}
	}
	
	public override string Name {
		get {
			return "Barrier Ship";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/shieldship";
		}
	}
	
	public BarrierShipCard() {
		CardText += "Increases the health of units and buildings within two tiles by two.";
	}
	
	public override void OnPlay (StateObject s)
	{
		new AuraBuff("Shield Barrier", s.TargetHex.Unit, 2, OnRemove, OnAdd);
	}

	void OnRemove(Unit unit) {
		unit.MaxHealth -= 2;
	}

	void OnAdd(Unit unit) {
		unit.MaxHealth += 2;
	}

	public override string Image {
		get {
			return "shieldship";
		}
	}
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}

}

