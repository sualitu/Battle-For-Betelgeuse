using UnityEngine;
using System.Collections;

public class ExplorerCard : UnitCard
{
	public override int Attack {
		get {
			return 1;
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
			return 10;
		}
	}

	public override string Name {
		get {
			return "Explorer";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/explorer";
		}
	}

	public override int id {
		get {
			return 3;
		}
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
	}
}

