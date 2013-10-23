using UnityEngine;
using System.Collections;

public class CowCard : UnitCard
{
	public override int Attack {
		get {
			return 10;
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
			return "Carrier";
		}
	}

	public override string PrefabPath {
		get {
			return "Units/cow";
		}
	}

	public override int id {
		get {
			return 5;
		}
	}
	
	public CowCard() {
		StandardSpecials.Add(new StandardSpecial.Boost(5));
	}

	public override void OnPlay (StateObject s)
	{
		StandardOnPlay(s);
		AudioControl.PlayAudioFile("moo");
	}
}

