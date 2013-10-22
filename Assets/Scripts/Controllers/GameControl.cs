using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GUIControl))]
[RequireComponent(typeof(CombatControl))]
[RequireComponent(typeof(MovementControl))]
[RequireComponent(typeof(MouseControl))]
[RequireComponent(typeof(GridControl))]
[RequireComponent(typeof(HandControl))]
[RequireComponent(typeof(NetworkControl))]
public class GameControl : MonoBehaviour {
	
	public static GameControl gameControl;
	
	private State state;
	public Player thisPlayer;
	public Player enemyPlayer;
	private Player currentPlayer;
	
	public static bool IsMulti = true;
	
	public List<Unit> units = new List<Unit>();	
	public GameObject unitPreFab;
	public bool AllowAction { get; set; }
	public GameObject CardPrefab;
	
	// Controllers
	public GridControl gridControl { get; set; }
	public GUIControl guiControl { get; set; }
	public CombatControl combatControl { get; set; }
	public MovementControl movementControl { get; set; }
	public MouseControl mouseControl { get; set; }
	public HandControl handControl {get; set; }
	public NetworkControl networkControl { get; set; }
	private AIController aiController;
	private EndTurn et;
	
	// Fix me
	private bool gameStarted = false;
	private bool gameIsSetUp = false;
	public bool myTurn = false;
	
	void Start () {
		gameControl = this;
		new MothershipCard();
		thisPlayer = new Player(Card.RandomDeck());
		thisPlayer.Ai = false;
		thisPlayer.Team = 1;
		thisPlayer.gameControl = this;
		enemyPlayer = new Player(Card.RandomDeck());
		enemyPlayer.Ai = !IsMulti;
		enemyPlayer.Team = 2;
		enemyPlayer.gameControl = this;
		if(!IsMulti) { aiController = new AIController(enemyPlayer, this); }
		initializeControllers();	
		state = State.START;
		AllowAction = false;
		et = GameObject.Find("EndTurn").GetComponent<EndTurn>();
		et.title = "Start Game";
		LoadingScreen.hide();
		if(!IsMulti) {
			SetUpGame();
		}
	}
		
	void DoGameLoop () {
		guiControl.UpdateGUI();
		switch(state) {
			case State.START: 
				state++;
				DoGameLoop();
				thisPlayer.DrawHand();				
				break;
			case State.PRETURN: 
				et.title = "End Turn";
				units.RemoveAll(u => u == null);
				foreach(Unit u in units) {
					u.ResetStats();
				}
				gameStarted = true;
				state++;
				DoGameLoop();
				
				break;
			case State.TURN:
				
				mouseControl.deselectHex();
				AllowAction = true;
				
				break;
			case State.POSTTURN: 
				state++;
				DoGameLoop();
				break;
			case State.NONETURN:
				state = State.PRETURN;
				DoGameLoop();
				break;
			case State.GAMEOVER:
				break;
			default: 
				break;
			}
	}
	
	public void EndTurnClicked () {
		if(gameIsSetUp) {
			switch(state) {
			case State.TURN:
				if(myTurn) {
					AllowAction = false;
					myTurn = false;
					if(!IsMulti) {
						aiController.DoMove();
					} else {
						networkControl.EndNetworkTurn();
					}
					state++;
					DoGameLoop();
				}
				break;
			case State.GAMEOVER:
				units.ForEach(u => u.Damage(int.MaxValue));
				state = State.START;
				DoGameLoop();
				break;
			case State.START:
				if(IsMulti) {
					networkControl.StartNetworkGame();
				} else {
					StartGame();
				}
				break;
			default:
				break;
			}
		}
	}
	
	public void StartGame() {
		if((!PhotonNetwork.isNonMasterClientInRoom && IsMulti) || !IsMulti) {
			myTurn = true;
		} 
		DoGameLoop();
	}
		
	private void initializeControllers() {
		gridControl = GetComponent<GridControl>();
		guiControl = GetComponent<GUIControl>();
		combatControl = GetComponent<CombatControl>();
		movementControl = GetComponent<MovementControl>();
		mouseControl = GetComponent<MouseControl>();
		handControl = GetComponent<HandControl>();	
		networkControl = GetComponent<NetworkControl>();
	}
	
	private void EndGame() {
		et.title = "End Game";
		AllowAction = false;	
		state = State.GAMEOVER;
	}
	
	public void SetUpGame() {
		Card baseCard = new MothershipCard();
		if(IsMulti) {
		networkControl.PlayNetworkCardOn(baseCard, gridControl.Map[16][4]);
		networkControl.PlayNetworkCardOn(baseCard, gridControl.Map[20][46]);
		} else {
			bool t = myTurn;
			myTurn = false;				
			PlayCardOnHex(new CruiserCard(), gridControl.Map[16][7],System.Guid.NewGuid().ToString());
			myTurn = t;
			PlayCardOnHex(baseCard, gridControl.Map[16][4], System.Guid.NewGuid().ToString());
			PlayCardOnHex(baseCard, gridControl.Map[20][46], System.Guid.NewGuid().ToString());
		}
		gameIsSetUp = true;
	}
		
	public void SetUpClientGame() {
		Camera.main.transform.localRotation = Quaternion.Euler(new Vector3(50, 180, 0));
		gameIsSetUp =  true;
	}
	
	public void EnemeyEndTurn() {
		myTurn = true;
		thisPlayer.DrawCard();
		thisPlayer.MaxMana++;
		thisPlayer.ManaSpend = 0;
		guiControl.ShowSplashText("Your turn!");
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
		unit.Team = myTurn ? 1 : 2;
		card.OnPlay(new StateObject(units, unit));
		if(myTurn && thisPlayer.Hand.Count != 0) {
			// Fix me
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
		// Fix me
		if(!gameStarted && gameIsSetUp && gridControl.Map[16][4].Unit != null && gridControl.Map[20][46].Unit != null) {
			Unit myBase = null;
			Unit enemyBase = null;
			if(!PhotonNetwork.isNonMasterClientInRoom) {
				 myBase = gridControl.Map[16][4].Unit;
				 enemyBase = gridControl.Map[20][46].Unit;
			} else {
				enemyBase = gridControl.Map[16][4].Unit;
				myBase = gridControl.Map[20][46].Unit;
			}
			if(thisPlayer.Base == null) {
				thisPlayer.Base = myBase;
				myBase.Hex.Adjacent(gridControl.Map).ForEach(h => h.Unit = myBase);
				myBase.Team = 1;
			} else if(enemyPlayer.Base == null) {
				enemyPlayer.Base = enemyBase;
				enemyBase.Hex.Adjacent(gridControl.Map).ForEach(h => h.Unit = enemyBase);
				enemyBase.Team = 2;
			}

		}
	}
	void OnGUI() {
		GUILayout.Label ("Mana left: " + thisPlayer.ManaLeft() + " / " + thisPlayer.MaxMana);
	}
	
	
}

public enum State { START, PRETURN, TURN, POSTTURN, NONETURN, GAMEOVER}
