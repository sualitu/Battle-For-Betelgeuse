using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReinforceCard : SpellCard {
	

	public override int Cost {
		get {
			return 4;
		}
	}

	public override string Name {
		get {
			return "Reinforce";
		}
	}

	public override Faction Faction {
		get {
			return Faction.GOOD;
		}
	}

	public override MockUnit MockOnPlay (MockUnit mo)
	{
		mo.CurrentHealth += 3;
		mo.MaxHealth += 3;
		return mo;
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		return result;
	}
	
	public ReinforceCard() {
		CardText += "Increases a units health by three.";
	}

	public override void SpellEffect (StateObject s)
	{
		Debug.Log ("Applied Reinforce");
		s.TargetHex.Unit.AddBuff(new UnitBuff("Reinforced", duration : -1, onRemove : OnRemove, onApplication : OnApply));
	}

	void OnApply(Unit unit) {
		unit.MaxHealth += 2;
	}
	
	void OnRemove(Unit unit) {
		unit.MaxHealth -= 2;
	}
}
