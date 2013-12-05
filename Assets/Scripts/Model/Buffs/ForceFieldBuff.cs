using UnityEngine;
using System.Collections;

public class ForceFieldBuff : UnitBuff
{
	public ForceFieldBuff() {
		Duration = -1;
		Name = "Force Field";
	}

	public override bool HasEffect {
		get {
			return true;
		}
	}

	public override void OnDamaged (Unit unit)
	{
		unit.RemoveBuff(this);
	}

	public override void OnApplication (Unit unit)
	{
		unit.AddEffect(this, (GameObject) Resources.Load("Effects/force_field"));
	}
}

