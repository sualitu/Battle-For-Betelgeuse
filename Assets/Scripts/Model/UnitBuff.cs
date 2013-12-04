using System;


public class UnitBuff {

	public int Duration  { get; set; }
	public string Name { get; set; }
	UnitBuffAction newTurnDel;
	
	public delegate void UnitBuffAction(Unit unit);

	public void DoNothing(Unit unit) {
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


	public void OnNewTurn(Unit unit) {
		newTurnDel(unit);
		--Duration;
	}
}