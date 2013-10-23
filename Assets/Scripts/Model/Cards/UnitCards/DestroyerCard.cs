using UnityEngine;
using System.Collections;

public class DestroyerCard : UnitCard
{
	public override int Attack {
		get {
			return 8;
		}
	}

	public override int Cost {
		get {
			return 6;
		}
	}

	public override int Health {
		get {
			return 3;
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
			return "Units/destroyer";
		}
	}

	public override int id {
		get {
			return 1;
		}
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		return;
	}
}

