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
			return 7;
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

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
	}

	public override string Image {
		get {
			return "explorer";
		}
	}	
	
	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

