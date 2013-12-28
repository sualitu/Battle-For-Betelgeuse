using UnityEngine;
using System.Collections;

public class DestroyerCard : UnitCard
{
	public override int Attack {
		get {
			return 4;
		}
	}

	public override int Cost {
		get {
			return 6;
		}
	}

	public override int Health {
		get {
			return 6;
		}
	}

	public override int Movement {
		get {
			return 5;
		}
	}

	public override string Name {
		get {
			return "Destroyer";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/Destroyer";
		}
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		return;
	}

	public override string Image {
		get {
			return "destroyer";
		}
	}
	
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

