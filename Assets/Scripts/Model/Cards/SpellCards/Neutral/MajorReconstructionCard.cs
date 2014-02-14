using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MajorReconstructionCard : SpellCard {
	

	public override int Cost {
		get {
			return 4;
		}
	}

	public override string Name {
		get {
			return "Major Reconstruction";
		}
	}

	public override int MockOnPlay (MockUnit mo, HexEvaluator he)
	{
		if(mo.CurrentHealth < mo.MaxHealth) {
			if(mo.MaxHealth - mo.CurrentHealth < 5) {
				mo.CurrentHealth = mo.MaxHealth;
			} else {
				mo.CurrentHealth += 5;
			}
		}
		return mo.Value(he);
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public MajorReconstructionCard() {
		CardText += "Heals target unit or building for five.";
	}

	public override void SpellEffect (StateObject s)
	{
		s.MainHex.Unit.Heal(5);
	}
}
