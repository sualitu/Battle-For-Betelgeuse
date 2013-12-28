using UnityEngine;
using System.Collections;

public class ImperialFighterCard : UnitCard
{
	public override int Attack {
		get {
			return 1;
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}
	
	public override int Cost {
		get {
			return 2;
		}
	}
	
	public override int Health {
		get {
			return 2;
		}
	}
	
	public override int Movement {
		get {
			return 6;
		}
	}
	
	public override string Name {
		get {
			return "Imperial Fighter";
		}
	}

	public override string Image {
		get {
			return "imperialfighter";
		}
	}
	
	public override string PrefabPath {
		get {
			return "Units/imperialfighter";
		}
	}
	
	public ImperialFighterCard() {
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

