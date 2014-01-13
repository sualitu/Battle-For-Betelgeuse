using UnityEngine;
using System.Collections;

public class SelfDestructCard : SpellCard
{
	public override int Cost {
		get {
			return 4;
		}
	}

	public override string Name {
		get {
			return "Self-Destruct";
		}
	}

	public override System.Collections.Generic.List<Hex> Targets (StateObject s)
	{
		return s.Units.FindAll(u => u.Team == s.Caster.Team).ConvertAll<Hex>(u => u.Hex);
	}
	
	public SelfDestructCard() {
		CardText += "Blows up the unit dealing damage equal to its remaining health to all adjacent units.";
	}
	
	public override void SpellAnimation (StateObject s)
	{
		Object.Instantiate((GameObject) Resources.Load("Effects/Nuke"), s.TargetHex.collider.bounds.center, Quaternion.identity);
		DoDelayedEffect(s, 0.5f);
	}

	public override void SpellEffect (StateObject s)
	{
		int damage = s.TargetHex.Unit.CurrentHealth();
		foreach(Hex h in s.TargetHex.Adjacent(GameControl.gameControl.GridControl.Map)) {
			if(h.Unit != null) {
				h.Unit.Damage(damage);
			}
		}
		
		s.TargetHex.Unit.Damage(int.MaxValue);	
	}
}

