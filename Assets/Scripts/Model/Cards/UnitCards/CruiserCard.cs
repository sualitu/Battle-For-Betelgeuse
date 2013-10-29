using UnityEngine;
using System.Collections;

public class CruiserCard : UnitCard
{
	public override int Attack {
		get {
			return 3;
		}
	}

	public override int Cost {
		get {
			return 3;
		}
	}

	public override int Health {
		get {
			return 3;
		}
	}

	public override int Movement {
		get {
			return 3;
		}
	}

	public override string Name {
		get {
			return "Star Cruiser";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/star_cruiser";
		}
	}

	public override int id {
		get {
			return 4;
		}
	}
	
	public CruiserCard() {
		StandardSpecials.Add(new StandardSpecial.Ranged(1));
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

