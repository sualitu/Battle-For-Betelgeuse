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
		Object.Instantiate((GameObject) Resources.Load ("Effects/Debuff"), s.TargetHex.collider.bounds.center , Quaternion.identity);
		SpellEffect(s);
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.Caster.DrawCards(s.TargetHex.Unit.Buffs.Count);
		s.TargetHex.Unit.Buffs.ForEach(b => s.TargetHex.Unit.RemoveBuff(b));

	}
}
