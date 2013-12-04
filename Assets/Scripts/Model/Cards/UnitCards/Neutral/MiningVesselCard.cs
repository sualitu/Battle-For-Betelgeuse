using UnityEngine;
using System.Collections;

public class MiningVesselCard : UnitCard
{
	public override int Attack {
		get {
			return 1;
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
			return 4;
		}
	}

	public override string Name {
		get {
			return "Mining Vessel";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/mining_vessel";
		}
	}

	public override int id {
		get {
			return 21;
		}
	}
	
	public MiningVesselCard() {
		CardText += "When this card is played, draw one card.";
	}

	public override void OnPlay (StateObject s)
	{
		s.Caster.DrawCard();
		
	}

	public override string Image {
		get {
			return "mining_vessel";
		}
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}
}

