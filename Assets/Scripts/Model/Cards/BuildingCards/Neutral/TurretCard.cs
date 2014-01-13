using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretCard : BuildingCard
{
	public override int Attack {
		get {
			return 2;
		}
	}

	public override int Cost {
		get {
			return 1;
		}
	}

	public override int Health {
		get {
			return 3;
		}
	}

	public override string Name {
		get {
			return "Turret";
		}
	}

	public override void OnNewTurn (StateObject s)
	{
		List<Unit> targets = new List<Unit>();
		targets = PathFinder.BreadthFirstSearch(s.TargetHex, GameControl.gameControl.GridControl.Map, 2, s.TargetHex.Unit.Team).ConvertAll<Unit>(h => h.Unit);
		targets = targets.FindAll(u => u != null);
		targets = targets.FindAll(u => u.Team != s.TargetHex.Unit.Team);
		if(targets.Count > 0) {
			s.TargetHex.Unit.attacking = targets[0];
			s.TargetHex.Unit.AttackTarget(targets[0], 0);
		}
	}
	
	public TurretCard() {
		CardText += "At the beginning of your turn this building will attack a random unit within two tiles";
	}

	public override void OnPlay (StateObject s)
	{
		base.OnPlay (s);
	}

	public override string PrefabPath {
		get {
			return "Buildings/turret";
		}
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}

	public override string Image {
		get {
			return "turret";
		}
	}
}

