using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GUIControl))]
[RequireComponent(typeof(CombatControl))]
[RequireComponent(typeof(MouseControl))]
[RequireComponent(typeof(GridControl))]
[RequireComponent(typeof(HandControl))]
[RequireComponent(typeof(NetworkControl))]
public class GameControl : MonoBehaviour {
	
	public static GameControl gameControl;
	
	public State state;
	public Player thisPlayer;
	public Player enemyPlayer;	
	
	public List<Unit> units = new List<Unit>();	
	public GameObject unitPreFab;
	public GameObject CardPrefab;
	
	// Controllers
	public GridControl gridControl { get; set; }
	public GUIControl guiControl { get; set; }
	public CombatControl combatControl { get; set; }
	public MouseControl mouseControl { get; set; }
	public HandControl handControl {get; set; }
	public NetworkControl networkControl { get; set; }
	public AudioControl audioControl { get; set; }
	AIControl aiController;
	
	// TODO Move to GUIController
	public EndTurn et;
	
	// TODO Consider Booleans
	public static bool IsMulti = false;
	
	void Start () {
		InitGame();
		InitPlayers();
		InitControllers();	
		if(!IsMulti) {
			InitSinglePlayer();
		}
		LoadingScreen.hide();
	}
	#region Init
	
	void InitGame() {
		new MothershipCard();
		state = State.PREGAME;
		et = GameObject.Find("EndTurn").GetComponent<EndTurn>();
		et.title = Dictionary.startGame;
	}
	
	void InitPlayers() {
		thisPlayer = new Player(Card.RandomDeck());
		thisPlayer.Ai = false;
		thisPlayer.Team = 1;
		thisPlayer.gameControl = this;
		enemyPlayer = new Player(Card.RandomDeck());
		enemyPlayer.Ai = !IsMulti;
		enemyPlayer.Team = 2;
		enemyPlayer.gameControl = this;
	}
	
	void InitSinglePlayer() {
		gameObject.AddComponent<AIControl>();
		aiController = GetComponent<AIControl>().SetAI(enemyPlayer, this);
		enemyPlayer.DrawHand();
		SetUpMasterGame();
	}
	
	void InitControllers() {
		gameControl = this;
		gridControl = GetComponent<GridControl>();
		guiControl = GetComponent<GUIControl>();
		combatControl = GetComponent<CombatControl>();
		mouseControl = GetComponent<MouseControl>();
		handControl = GetComponent<HandControl>();	
		networkControl = GetComponent<NetworkControl>();
		audioControl = GetComponent<AudioControl>();
	}
	#endregion Init
		
	void DoGameLoop () {
		guiControl.UpdateGUI();
		switch(state) {
			case State.PRETURN:
				if(!IsMulti || PhotonNetwork.isMasterClient) {
					state = State.MYTURN;
				} else {
					state = State.ENEMYTURN;
				}
				DoGameLoop();
				break;
			case State.MYTURN:
				et.title = Dictionary.endTurn;
				units.RemoveAll(u => u == null);
				foreach(Unit u in units) {
					u.ResetStats();
				}
				break;
			case State.ENEMYTURN:
				if(!IsMulti) {
					aiController.DoTurn();
					enemyPlayer.DrawCard();
					enemyPlayer.MaxMana++;
					enemyPlayer.ManaSpend = 0;
				}
				break;
			default: 
				break;
			}
	}
	
	public void EndTurnClicked () {
		if(state >= State.START) {
			switch(state) {
			case State.MYTURN:
				if(state == State.MYTURN) {
					et.title = Dictionary.EnemyTurnInProgress;
					if(IsMulti) {
						networkControl.EndNetworkTurn();
					}
					thisPlayer.DeselectCard();
					mouseControl.deselectHex();
					state = State.ENEMYTURN;
					DoGameLoop();
				}
				break;
			case State.START:
				if(IsMulti) {
					networkControl.StartNetworkGame();
				} else {
					StartGame();
				}
				break;
			case State.GAMEOVER:
				networkControl.QuitGame();
				Application.LoadLevel (0);
				break;
			default:
				break;
			}
		}
	}
	
	public void StartGame() {
		thisPlayer.DrawHand();
		state = State.PRETURN;
		DoGameLoop();
	}
	
	private void EndGame() {
		state = State.GAMEOVER;
		et.title = Dictionary.endGame;	
	}
	
	public static bool GameStarted() {
		return gameControl.state >= State.START;
	}
	
	#region SetUp
	public void SetUpMasterGame() {
		
		Card baseCard = new MothershipCard();
		if(IsMulti) {
			networkControl.PlayNetworkCardOn(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)]);
			networkControl.PlayNetworkCardOn(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)]);
		} else {
			PlayCardOnHex(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)], System.Guid.NewGuid().ToString());
			PlayCardOnHex(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)], System.Guid.NewGuid().ToString());
		}
		thisPlayer.MaxMana++;
		state = State.START;
		// TODO Move to CameraControl
		Vector3 mainCameraPosition = new Vector3(40,30,-14);
		Vector3 mainCameraRotation = new Vector3(50,0,0);
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", mainCameraPosition,
			"delay", 0.5f,
			"easetype", "easeInQuad",
			"time", 5));
		iTween.RotateTo(Camera.main.gameObject, iTween.Hash("rotation", mainCameraRotation,
			"delay", 0.5f,
			"easetype", "easeInQuad",
			"time", 5));
	}
		
	public void SetUpClientGame() {
		state = State.START;
		// TODO Move to CameraControl
		Vector3 mainCameraPosition = new Vector3(40,30,104);
		Vector3 mainCameraRotation = new Vector3(50,180,0);
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash("position", mainCameraPosition,
			"delay", 0.5f,
			"easetype", "easeInQuad",
			"time", 5));
		iTween.RotateTo(Camera.main.gameObject, iTween.Hash("rotation", mainCameraRotation,
			"delay", 0.5f,
			"easetype", "easeInQuad",
			"time", 5));
	}
	#endregion SetUp
	
	public void EnemeyEndTurn() {
		et.title = Dictionary.endTurn;
		state = State.MYTURN;
		thisPlayer.DrawCard();
		thisPlayer.MaxMana++;
		thisPlayer.ManaSpend = 0;
		audioControl.PlayNewTurnSound();
		guiControl.ShowSplashText(Dictionary.yourTurn);
		DoGameLoop();
	}
	
	public void PlayCardOnHex(Card card, Hex hex, string id) {

		GameObject go = (GameObject) Instantiate(unitPreFab, Vector3.zero, Quaternion.identity);
		Unit unit = go.GetComponent<Unit>();
		unit.Id = id;
		unit.FromCard(card);
		unit.Hex = hex;
		unit.transform.position = hex.transform.position;
		hex.Unit = unit;
		units.Add(unit);
		unit.Team = state == State.MYTURN ? 1 : 2;
		card.OnPlay(new StateObject(units, unit));
		if(state == State.MYTURN && thisPlayer.Hand.Count != 0) {
			// TODO Find a better way to sort this
			thisPlayer.PlayCard();
		}
	}
	
	public void EnemyCardPlayed(Card card) {
		GUICard guiCard = ((GameObject) Object.Instantiate(CardPrefab)).GetComponent<GUICard>();
		guiCard.SetCard(card);
		guiCard.ForcePlaceCard(Screen.width, -300);
		guiCard.Played();		
	}
	
	void Update() {
		// TODO Do this properly
		if(state != State.PREGAME && gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)].Unit != null && gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)].Unit != null) {
			Unit myBase = null;
			Unit enemyBase = null;
			if(!PhotonNetwork.isNonMasterClientInRoom) {
				 myBase = gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)].Unit;
				 enemyBase = gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)].Unit;
			} else {
				enemyBase = gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)].Unit;
				myBase = gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)].Unit;
			}
			if(thisPlayer.Base == null) {
				thisPlayer.Base = myBase;
				myBase.Hex.Adjacent(gridControl.Map).ForEach(h => h.Unit = myBase);
				myBase.Team = 1;
			} 
			if(enemyPlayer.Base == null) {
				enemyPlayer.Base = enemyBase;
				enemyBase.Hex.Adjacent(gridControl.Map).ForEach(h => h.Unit = enemyBase);
				enemyBase.Team = 2;
			}
		}
		
		if(state > State.PREGAME && thisPlayer.Base == null) {
			guiControl.ShowSplashText("You lost!");
			EndGame();
		} else if(state > State.PREGAME && enemyPlayer.Base == null) {
			guiControl.ShowSplashText("You won!");
			EndGame();
		}
	}
	
	// TODO Do this properly
	void OnGUI() {
		GUILayout.Label ("Mana left: " + thisPlayer.ManaLeft() + " / " + thisPlayer.MaxMana);
		
	}
	
	
}

public enum State { PREGAME, START, PRETURN, MYTURN, ENEMYTURN, GAMEOVER}
