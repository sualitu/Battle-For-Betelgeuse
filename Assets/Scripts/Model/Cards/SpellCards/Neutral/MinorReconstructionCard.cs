using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinorReconstructionCard : SpellCard {
	


	public override int Cost {
		get {
			return 1;
		}
	}

	public override string Name {
		get {
			return "Minor Reconstruction";
		}
	}

	public override int MockOnPlay (MockUnit mo, HexEvaluator he)
	{
		if(mo.CurrentHealth < mo.MaxHealth) {
			if(mo.MaxHealth - mo.CurrentHealth < 2) {
				mo.CurrentHealth = mo.MaxHealth;
			} else {
				mo.CurrentHealth += 2;
			}
		}
		return mo.Value (he);
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}

	public override string Image {
		get {
			return "repair";
		}
	}
	
	public MinorReconstructionCard() {
		CardText += "Heals target unit or building for two.";
	}

	public override void SpellEffect (StateObject s)
	{
		s.MainHex.Unit.Heal(2);
	}
}
