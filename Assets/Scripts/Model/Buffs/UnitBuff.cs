using System;
using UnityEngine;

public class UnitBuff {

	public int Duration  { get; set; }
	public string Name { get; set; }
	protected UnitBuffAction newTurnDel;

	public delegate void UnitBuffAction(Unit unit);

	public void DoNothing(Unit unit) {
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
	                UnitBuffAction onPlay = null, 
	                Unit unit = null,
	                int duration = 1) {
		Name = name;
		newTurnDel = newTurn ?? DoNothing;
		Duration = duration;
		if(unit != null && onPlay != null) {
			onPlay(unit);
		} 
	}

	public virtual void OnDamaged(Unit unit) {

	}

	public virtual void OnApplication(Unit unit) {
	}

	public virtual void OnNewTurn(Unit unit) {
		newTurnDel(unit);
		--Duration;
	}
}