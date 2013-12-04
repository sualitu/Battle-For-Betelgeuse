using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinorReconstructionCard : SpellCard {
	


	public override int Cost {
		get {
			return 1;
		}
	}


	public override int id {
		get {
			return 9;
		}
	}


	public override string Name {
		get {
			return "Minor Reconstruction";
		}
	}

	public override MockUnit MockOnPlay (MockUnit mo)
	{
		if(mo.CurrentHealth < mo.MaxHealth) {
			if(mo.MaxHealth - mo.CurrentHealth < 2) {
				mo.CurrentHealth = mo.MaxHealth;
			} else {
				mo.CurrentHealth += 2;
			}
		}
		return mo;
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public MinorReconstructionCard() {
		CardText += "Heals target unit or building for two.";
	}

	public override void SpellEffect (StateObject s)
	{
		if(s.TargetHex.Unit.CurrentHealth() < s.TargetHex.Unit.MaxHealth) {
			if(s.TargetHex.Unit.MaxHealth - s.TargetHex.Unit.CurrentHealth() < 2) {
				s.TargetHex.Unit.Damage(-(s.TargetHex.Unit.MaxHealth - s.TargetHex.Unit.CurrentHealth()));
			} else {
				s.TargetHex.Unit.Damage(-2);
			}
		}
	}
}
