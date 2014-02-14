using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PurgeCard : SpellCard {
	
	public override int Cost {
		get {
			return 2;
		}
	}
	
	public override string Name {
		get {
			return "Purge";
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public PurgeCard() {
		CardText += "Remove all buffs from target unit. Draw one card per buff removed.";
	}

	public override void SpellAnimation (StateObject s)
	{
		Object.Instantiate((GameObject) Resources.Load ("Effects/Debuff"), s.MainHex.collider.bounds.center , Quaternion.identity);
		SpellEffect(s);
	}

	public override int MockOnPlay (MockUnit mo, HexEvaluator he)
	{
		int draw = mo.Buffs.Count * 3;
		mo.Buffs = new List<UnitBuff>();
		return mo.Value(he) - draw;
	}
	
	public override Faction Faction {
		get {
			return Faction.CONTROL;
		}
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.DrawCards(s.MainHex.Unit.Buffs.Count);
		List<UnitBuff> buffs = new List<UnitBuff>(s.MainHex.Unit.Buffs);
		foreach(UnitBuff buff in buffs) {
			s.MainHex.Unit.RemoveBuff(buff);
		}
	}
}
