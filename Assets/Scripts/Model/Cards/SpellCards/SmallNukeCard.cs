using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallNukeCard : SpellCard {
	
	public override int Attack {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int Cost {
		get {
			return 3;
		}
	}

	public override int Health {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override int id {
		get {
			return 16;
		}
	}

	public override int Movement {
		get {
			throw new System.NotImplementedException ();
		}
	}

	public override string Name {
		get {
			return "Small Nuke";
		}
	}


	public override void OnNewTurn (StateObject s)
	{
		base.OnNewTurn (s);
	}

	public override void OnPlay (StateObject s)
	{
		List<Hex> targets = new List<Hex>();
		if(s.TargetUnit != s.Caster.Base && s.TargetUnit != s.Opponent.Base) {
			targets.Add(s.TargetUnit.Hex);
		}
		
		s.TargetUnit.Hex.Adjacent(GameControl.gameControl.gridControl.Map).ForEach(h => targets.Add(h));
		
		foreach(Hex h in targets) {
			if(h.Unit != null && h.Unit != s.Caster.Base && h.Unit != s.Opponent.Base) {
				h.Unit.Damage(2);
			}
		}

		Object.Instantiate(Prefab, s.TargetUnit.transform.position, Quaternion.identity);
	}
	
	public override string PrefabPath {
		get {
			return "Effects/Heal";
		}
	}

	public override string Projectile {
		get {
			return "missiles";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public SmallNukeCard() {
		CardText += "Deals 2 damage to target unit and all adjacent units. Does not affect motherships.";
	}
}
