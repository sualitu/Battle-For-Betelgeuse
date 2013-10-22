using UnityEngine;
using System.Collections;

public class AIController
{
	Player player;
	GameControl gameControl;
	
	public AIController(Player player, GameControl gameControl) {
		this.player = player;
		this.gameControl = gameControl;
	}
	
	public void DoMove() {
		gameControl.EnemeyEndTurn();
	}
}

