using UnityEngine;
using System.Collections;

public class BattleCruiserCard : UnitCard
{
	public override int Attack {
		get {
			return 15;
		}
	}

	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}

	public override int Cost {
		get {
			return 12;
		}
	}

	public override int Health {
		get {
			return 10;
		}
	}

	public override int Movement {
		get {
			return 5;
		}
	}

	public override string Name {
		get {
			return "Battle Cruiser";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/battlecruiser";
		}
	}

	public override string Image {
		get {
			return "battlecruiser";
		}
	}
	
	public BattleCruiserCard() {
		StandardSpecials.Add(new StandardSpecial.Ranged());
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

