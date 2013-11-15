using UnityEngine;
using System.Collections;

public class MockUnit 
{
	public int Attack;
	public int CurrentHealth;
	public int MaxHealth;
	public int Movement;
	public int Team;
	public Hex Hex;
	
	public MockUnit(Unit unit) {
		Attack = unit.Attack;
		CurrentHealth = unit.CurrentHealth();
		MaxHealth = unit.MaxHealth;
		Movement = unit.MovementLeft();
		Team = unit.Team;
		Hex = unit.Hex;
	}
	
	public int Value() {
		int result = 0;
		result += Attack;
		if(CurrentHealth == 0) {
			result -= MaxHealth;
		} else {
			result += CurrentHealth;
		}
		result += MaxHealth;
		result += Movement;
		
		return result;
	}
}

