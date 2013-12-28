using UnityEngine;
using System.Collections.Generic;

public class NuclearWeaponsCard : SpellCard
{
	public override int Cost {
		get {
			return 1;
		}
	}
	
	public override string Name {
		get {
			return "Nuclear Weapons";
		}
	}
	
	public override Faction Faction {
		get {
			return Faction.EVIL;
		}
	}
	
	public override List<Hex> Targets (StateObject s)
	{
		List<Hex> result = s.Units.FindAll (u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
		
		return result;
	}
	
	public NuclearWeaponsCard() {
		CardText += "Gives target unit +5 attack. Dies at the start of your next turn.";
	}
	
	public override void SpellEffect (StateObject s)
	{
		s.TargetHex.Unit.AddBuff(new UnitBuff("Nuclear Weapons", onRemove : OnRemove, onApplication : OnApply, duration : 1));
	}
	
	void OnApply(Unit unit) {
		unit.Attack += 5;

	}
	
	void OnRemove(Unit unit) {
		unit.Damage(int.MaxValue);
	}
}

