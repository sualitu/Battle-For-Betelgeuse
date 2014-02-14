using UnityEngine;
using System.Collections.Generic;

public class CongreveShipCard : UnitCard
{
	public override int Attack {
		get {
			return 3;
		}
	}
	
	public override int Cost {
		get {
			return 5;
		}
	}
	
	public override int Health {
		get {
			return 7;
		}
	}
	
	public override int Movement {
		get {
			return 5;
		}
	}
	
	public override string Name {
		get {
			return "Congreve Ship";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/congreve_ship";
		}
	}

	public override string Image {
		get {
			return "congreve";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}
	
	public CongreveShipCard() {
		StandardSpecials.Add(new StandardSpecial.Ranged());
		setStandardCardText();
	}
		
	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
}

