using UnityEngine;
using System.Collections;

public class SaboteurCard : UnitCard
{
	public override int Attack {
		get {
			return 0;
		}
	}

	public override int Cost {
		get {
			return 4;
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
			return "Saboteur";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/saboteur";
		}
	}
	
	public SaboteurCard() {
		StandardSpecials.Add(new StandardSpecial.DeathTouch());
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

	public override string Image {
		get {
			return "saboteur";
		}
	}
}

