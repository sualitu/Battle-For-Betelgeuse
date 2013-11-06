using UnityEngine;
using System.Collections;

public abstract class BuildingCard : Card
{
	public BuildingCard() : base() {
		
	}
	
	public override int Movement {
		get {
			return 0;
		}
	}
}

