using System;
using UnityEngine;

public class UnitBuff {

	public int Duration  { get; set; }
	public string Name { get; set; }
	protected UnitBuffAction newTurnDel = DoNothing;
	protected UnitBuffAction onRemove = DoNothing;
	protected UnitBuffAction onApplication = DoNothing;

	public delegate void UnitBuffAction(Unit unit);

	public static void DoNothing(Unit unit) {
	}

	public virtual bool HasEffect {
		get {
			return false;
		}
	}

	public UnitBuff(UnitBuffAction newTurn = null) {
		newTurnDel = newTurn ?? DoNothing;
	}

	public UnitBuff(string name, 
	                UnitBuffAction newTurn = null, 
	                UnitBuffAction onRemove = null,
	                UnitBuffAction onApplication = null,
	                int duration = 1) {
		Name = name;
		newTurnDel = newTurn ?? DoNothing;
		this.onRemove = onRemove ?? DoNothing;
		this.onApplication = onApplication ?? DoNothing;
		Duration = duration;
	}


	public virtual void OnRemoved(Unit unit) {
		onRemove(unit);
	}

	public virtual void OnDamaged(Unit unit) {

	}

	public virtual void OnApplication(Unit unit) {
		onApplication(unit);
	}

	public virtual void OnNewTurn(Unit unit) {
		newTurnDel(unit);
		--Duration;
		if(Duration == 0) {
			unit.RemoveBuff(this);
		}
	}
}