using UnityEngine;
using System.Collections;

public class FighterSquadCard : UnitCard
{
	public override int Attack {
		get {
			return 2;
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
			return 5;
		}
	}

	public override string Name {
		get {
			return "Fighter Squad";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/fighter_squad";
		}
	}

	public override int id {
		get {
			return 2;
		}
	}
	
	public FighterSquadCard() {
		StandardSpecials.Add(new StandardSpecial.Boost(3));
		setStandardCardText();
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		return;
	}
	
	public override string Image {
		get {
			return "fighter_squad";
		}
	}
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

