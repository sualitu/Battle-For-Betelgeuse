using UnityEngine;
using System.Collections;

public class BattleCruiserCard : UnitCard
{
	public override int Attack {
		get {
			return 11;
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
			return 8;
		}
	}

	public override int Movement {
		get {
			return 4;
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

