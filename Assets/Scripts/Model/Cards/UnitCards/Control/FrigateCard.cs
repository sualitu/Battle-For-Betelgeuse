using UnityEngine;
using System.Collections.Generic;

public class FrigateCard : UnitCard
{
	public override int Attack {
		get {
			return 5;
		}
	}
	
	public override int Cost {
		get {
			return 7;
		}
	}
	
	public override int Health {
		get {
			return 3;
		}
	}
	
	public override int Movement {
		get {
			return 5;
		}
	}
	
	public override string Name {
		get {
			return "Frigate";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/frigate";
		}
	}

	public override string Image {
		get {
			return "frigate";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}
	
	public FrigateCard() {
		StandardSpecials.Add(new StandardSpecial.Ranged());
		setStandardCardText();
		CardText += "Can only lose one health at a time.";
	}

	public override int OnDamaged (StateObject s, int d)
	{
		return 1;
	}
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
}

