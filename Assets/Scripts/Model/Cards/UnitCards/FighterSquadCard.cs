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
			return 1;
		}
	}

	public override int Health {
		get {
			return 1;
		}
	}

	public override int Movement {
		get {
			return 4;
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
		setCardText();
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		return;
	}
	
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

