using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class CombatControl : MonoBehaviour
{
	
	private GameControl gameControl;

	// Use this for initialization
	void Start ()
	{
		gameControl = GetComponent<GameControl>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void Combat(Unit attacker, Unit defender) {
		defender.Damage(attacker.Attack);
		gameControl.guiControl.ShowFloatingText("-" + attacker.Attack, defender.transform);
		attacker.Damage(defender.Attack);
		gameControl.guiControl.ShowFloatingText("-" + defender.Attack, attacker.transform);
		attacker.Move(attacker.MovementLeft());
	}
}

