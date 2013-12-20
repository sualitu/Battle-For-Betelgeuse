using UnityEngine;
using System.Collections;

public class ShieldedCollierCard : UnitCard
{
	public override int Attack {
		get {
			return 3;
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}
	
	public override int Cost {
		get {
			return 5;
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

	public override string Image {
		get {
			return "collier";
		}
	}
	
	public override string Name {
		get {
			return "Shielded Collier";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/collier";
		}
	}
	
	public ShieldedCollierCard() {
		StandardSpecials.Add(new StandardSpecial.ForceField());
		setStandardCardText();
	}
	
	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
	}
	
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}	
}

