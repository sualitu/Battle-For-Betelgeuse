using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class CombatControl : MonoBehaviour
{	
	/// <summary>
	/// Combat the specified attacker and defender.
	/// </summary>
	/// <param name='attacker'>
	/// Attacker.
	/// </param>
	/// <param name='defender'>
	/// Defender.
	/// </param>
	public void Combat(Unit attacker, Unit defender) {
		defender.Damage(attacker.Attack);
		if(!attacker.IsRanged()) {
			attacker.Damage(defender.Attack);
		}
		attacker.Move(attacker.MovementLeft());
	}
}

