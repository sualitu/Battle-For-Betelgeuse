using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class CombatControl : MonoBehaviour
{	
	public void Combat(Unit attacker, Unit defender) {
		defender.Damage(attacker.Attack);
		attacker.Damage(defender.Attack);
		attacker.Move(attacker.MovementLeft());
	}
}

