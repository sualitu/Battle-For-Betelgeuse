using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameControl : MonoBehaviour {
	
	public static GameControl gameControl;


	public State State { get; set; } // TODO: Consider changing the main gui button
	public Player ThisPlayer { get; set; }
	public Player EnemyPlayer { get; set; }

	// Collections
	public List<Card> CardHistory = new List<Card>();
	public List<Flag> Flags = new List<Flag>();
	public List<Unit> Units = new List<Unit>();	
	public List<AuraBuff> AuraBuffs = new List<AuraBuff>();

	// Unity Stuff
	public GameObject UnitPrefab;
	public GameObject CardPrefab;
	public GameObject DeckButtonPrefab;
	public GameObject FlagPrefab;

	// Globals
	public static bool IsMulti = false;
	public static bool Cheating = false;
	public static Difficulty Difficulty;
	public static List<Card> Deck;
	
	// Controllers
	public GridControl GridControl { get; set; }
	public GUIControl GuiControl { get; set; }
	public CombatControl CombatControl { get; set; }
	public MouseControl MouseControl { get; set; }
	public NetworkControl NetworkControl { get; set; }
	public AudioControl AudioControl { get; set; }
	public CameraControl CameraControl { get; set; }
	public KeyboardControl KeyboardControl { get; set; }
	protected AIControl aiController;

	protected abstract void DoGameLoop();
	protected abstract void EndGame();
	protected abstract void EndTurn();

	public abstract void EndTurnClicked ();
	public abstract void EnemyEndTurn();
	public abstract void EnemyCardPlayed(Card card);
	public abstract void LeaveGame();
	public abstract void SetUpClientGame();
	public abstract void SetUpMasterGame();
	public abstract void StartGame();
	public abstract Unit PlayCardOnHex(Card card, Hex hex, string id);

	// Change
	public abstract int FlagCountValue(int i);

	public static bool GameStarted() {
		return gameControl.State >= State.START;
	}
	
	public bool MyTurn() {
		return State == State.MYTURN;
	}
	
	public bool NoMovesInProgress() {
		return iTween.tweens.Count < 1;
	}
}

public enum State { PREGAME, START, PRETURN, MYTURN, ENEMYTURN, GAMEOVER }
public enum Difficulty { EASY, HARD }
