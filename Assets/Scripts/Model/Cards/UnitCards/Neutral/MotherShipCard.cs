using UnityEngine;
using System.Collections;

public class MothershipCard : UnitCard
{
	public override int Attack {
		get {
			return 0;
		}
	}

	public override int Cost {
		get {
			return 0;
		}
	}

	public override int Health {
		get {
			return 100;
		}
	}

	public override int Movement {
		get {
			return 0;
		}
	}

	public override string Name {
		get {
			return "Mothership";
		}
	}

	public override string PrefabPath {
		get {
			return "Buildings/spacestation";
		}
	}
	
	public MothershipCard() {
		StandardSpecials.Add(new StandardSpecial.Defenseless());
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

