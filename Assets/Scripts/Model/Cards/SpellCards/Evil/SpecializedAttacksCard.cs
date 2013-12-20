using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpecializedAttacksCard : SpellCard {
	
	public override int Cost {
		get {
			return 2;
		}
	}
	
	public override string Name {
		get {
			return "Specialized Attacks";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public SpecializedAttacksCard() {
		CardText += "Increases target unit or buildings attack by two.";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.TargetHex.Unit.AddBuff(new UnitBuff("Specialized Attacks", onRemove : OnRemove, onApplication : OnApply));
	}

	void OnApply(Unit unit) {
		unit.Attack += 2;
	}

	void OnRemove(Unit unit) {
		unit.Attack -= 2;
	}
}
