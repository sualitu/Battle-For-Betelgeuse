using System;
using UnityEngine;

public class UnitBuff {

	public int Duration  { get; set; }
	protected string name;
	protected UnitBuffAction newTurnDel = DoNothing;
	protected UnitBuffAction onRemove = DoNothing;
	protected UnitBuffAction onApplication = DoNothing;

	public virtual string Name {
		get {
			return name;
		}
	}

	public delegate void UnitBuffAction(Unit unit);

	public static void DoNothing(Unit unit) {
	}

	public virtual bool HasVisualEffect {
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
		this.name = name;
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