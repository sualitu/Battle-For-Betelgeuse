using UnityEngine;
using System.Collections.Generic;

public class EmptySpaceCard : SpellCard
{
	public override int Cost {
		get {
			return 7;
		}
	}
	
	public override string Name {
		get {
			return "Empty Space";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		return new List<Hex>();
	}
	
	public EmptySpaceCard() {
		CardText += "Removes all buffs from up to three random enemy buildings or units.";
	}

	void TinyExplosion(Vector3 place) {
		Object.Instantiate((GameObject) Resources.Load ("Effects/tinyexpl"), place, Quaternion.identity);
	}

	public void RemoveBuffsFromUnit(Unit u) {
		//TinyExplosion(u.Hex.collider.bounds.center);
		List<UnitBuff> buffs = u.Buffs;
		buffs.ForEach(b => u.RemoveBuff(b));
	}
	
	public override void SpellEffect (StateObject s)
	{
		List<Unit> units = s.Units.FindAll(u => u.Team != s.Caster.Team && s.Caster.Base != u && s.Opponent.Base != u); 
		if(units.Count <= 3) {
			units.ForEach(u => RemoveBuffsFromUnit(u));
		} else {
			for(int i = 0; i < 3; i++) {
				int rnd = Random.Range (0, units.Count);
				RemoveBuffsFromUnit(units[rnd]);
				units.RemoveAt(rnd);
			}
		}
	}
}

