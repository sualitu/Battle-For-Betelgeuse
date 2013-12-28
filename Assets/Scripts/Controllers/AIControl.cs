using UnityEngine;
using System.Collections;

public abstract class AIControl : MonoBehaviour
{	
	protected Player player;
	protected GameControl gameControl;
	protected AIState aistate = AIState.ENEMYTURN;

	const bool talking = true;
	
	protected void Say(string s) {
		if(talking)
			Debug.Log ("AI: " + s);
	}
	
	protected void Say(int i) {Say (i.ToString());}

	/// <summary>
	/// Ends the turn.
	/// </summary>
	public void EndTurn() {
		aistate = AIState.ENEMYTURN;
		gameControl.EnemeyEndTurn();
	}
	
	protected bool MyTurn() {
		return gameControl.state == State.ENEMYTURN;
	}

	/// <summary>
	/// Sets AI.
	/// </summary>
	/// <returns>
	/// The AIControl for the player.
	/// </returns>
	/// <param name='player'>
	/// Player.
	/// </param>
	/// <param name='gameControl'>
	/// Game control.
	/// </param>
	public AIControl SetAI(Player player, GameControl gameControl) {
		player.Ai = true;
		this.player = player;
		this.gameControl = gameControl;
		return this;
	}
}

public enum AIState { ENEMYTURN, BUILDINGHAND, ANALYZINGCARDOPTIONS, PLAYINGCARDS, ANALYZINGBOARD, MOVINGUNITS, DONE }
