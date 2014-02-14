using UnityEngine;
using System.Collections;

public class StackableBuff : UnitBuff
{
	int stacks = 1;

	public void IncreaseStack(Unit unit) {
		onApplication(unit);
		stacks++;
	}

	public void DecreaseStack(Unit unit) {
		onRemove(unit);
		stacks--;
	}

	public override void OnRemoved (Unit unit)
	{
		while(stacks > 0) {
			onRemove(unit);
			stacks--;
		}
	}

	public override string Name {
		get {
			return name + " " + stacks;
		}
	}

	public StackableBuff(string name, 
	                UnitBuffAction newTurn = null, 
	                UnitBuffAction onRemove = null,
	                UnitBuffAction onApplication = null,
	                int duration = 1) : base(name, newTurn, onRemove, onApplication, duration) {}
}

