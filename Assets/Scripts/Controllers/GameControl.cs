using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(GUIControl))]
[RequireComponent(typeof(CombatControl))]
[RequireComponent(typeof(MouseControl))]
[RequireComponent(typeof(GridControl))]
[RequireComponent(typeof(NetworkControl))]
public class GameControl : MonoBehaviour {
	
	public static GameControl gameControl;
	
	public State state;
	public Player thisPlayer;
	public Player enemyPlayer;	
	
	public List<Flag> flags = new List<Flag>();
	public List<Unit> units = new List<Unit>();	
	public List<AuraBuff> auraBuffs = new List<AuraBuff>();
	public GameObject unitPrefab;
	public GameObject CardPrefab;
	public GameObject DeckButtonPrefab;
	public GameObject flagPrefab;
	
	// Controllers
	public GridControl gridControl { get; set; }
	public GUIControl guiControl { get; set; }
	public CombatControl combatControl { get; set; }
	public MouseControl mouseControl { get; set; }
	public NetworkControl networkControl { get; set; }
	public AudioControl audioControl { get; set; }
	public CameraControl cameraControl { get; set; }
	AIControl aiController;
	
	// TODO Move to GUIController
	
	public static bool IsMulti = false;
	
	List<Card> deckFromInt(int i) {
		switch(i) {
		case 0: return Card.GoodDeck(); 
		case 1: return Card.NeutralDeck();
		case 2: return Card.EvilDeck();
		default: return Card.NeutralDeck();
		}
	}
	
	public void ChooseDeck(int i) {
		InitPlayers(deckFromInt (i));
		if(!IsMulti) {
			InitSinglePlayer();
		} else {
			guiControl.SetButton("Waiting for opponents");
		}
		HideOptions();
	}
	
	List<DeckOption> dos = new List<DeckOption>();
	
	void HideOptions() {
		networkControl.DeckChosen();
		dos.ForEach(Destroy);
	}
	
	void ShowDeckOptions() {
		DeckOption doZero = ((GameObject) Instantiate(DeckButtonPrefab)).GetComponent<DeckOption>();
		doZero.index = 0;
		doZero.h = -1000;
		doZero.title = "Good Deck";
		
		DeckOption doOne = ((GameObject) Instantiate(DeckButtonPrefab)).GetComponent<DeckOption>();
		doOne.index = 1;
		doOne.h = -800;
		doOne.title = "Neutral Deck";
		
		DeckOption doTwo = ((GameObject) Instantiate(DeckButtonPrefab)).GetComponent<DeckOption>();
		doTwo.index = 2;
		doTwo.h = -600;
		doTwo.title = "Evil Deck";
		
		dos.Add(doZero);
		dos.Add(doOne);
		dos.Add(doTwo);
	}
	
	void Start () {
		InitGame();
		InitControllers();	
		ShowDeckOptions();
		InitPlayers(deckFromInt(Random.Range(0, 2)));
		SetUpFlags();
		LoadingScreen.hide();
	}
	#region Init
	
	void InitGame() {
		new MothershipCard();
		state = State.PREGAME;
		Card.InitCards();
	}
	
	void InitPlayers(List<Card> deck) {
		thisPlayer = new Player(deck);
		thisPlayer.Ai = false;
		thisPlayer.Team = 1;
		thisPlayer.gameControl = this;
		if(IsMulti) {
			enemyPlayer = new Player(deck);
		} else {
			enemyPlayer = new Player(Card.AIDeck());
		}
		enemyPlayer.Ai = !IsMulti;
		enemyPlayer.Team = 2;
		enemyPlayer.gameControl = this;
		
	}
	
	void InitSinglePlayer() {
		guiControl.SetButton(Dictionary.startGame);
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
		networkControl = GetComponent<NetworkControl>();
		audioControl = GetComponent<AudioControl>();
		cameraControl = GetComponent<CameraControl>();
	}
	#endregion Init

	public int FlagCountValue(int i) {
		return Mathf.FloorToInt(Settings.FlagBaseValue*(Mathf.Pow(Settings.FlagMultValue,i-1)*i));
	}
	
	void CalculatePoints() {
		int thisPlayerFlags = 0;
		int enemyPlayerFlags = 0;
		foreach(Flag flag in flags) {
			if(flag.OwnerTeam == thisPlayer.Team) thisPlayerFlags++;
			if(flag.OwnerTeam == enemyPlayer.Team) enemyPlayerFlags++;
		}
		thisPlayer.Points += FlagCountValue(thisPlayerFlags);
		enemyPlayer.Points += FlagCountValue(enemyPlayerFlags);
	}
	
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
				flags.ForEach(f => f.OnNewTurn(null));
				CalculatePoints();
				guiControl.SetButton(Dictionary.endTurn);
				units.RemoveAll(u => u == null);
				foreach(Unit u in units) {
					if(u.Team == thisPlayer.Team) {
						u.ResetStats();
						u.OnNewTurn(new StateObject(units, u.Hex, thisPlayer, enemyPlayer));
					}
				}
				break;
			case State.ENEMYTURN:
				flags.ForEach(f => f.OnNewTurn(null));
				CalculatePoints();
				units.RemoveAll(u => u == null);
				foreach(Unit u in units) {
					if(u.Team != thisPlayer.Team) {
						u.ResetStats();
						u.OnNewTurn(new StateObject(units, u.Hex, enemyPlayer, thisPlayer));
					}
				}
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
	
	public bool MyTurn() {
		return state == State.MYTURN;
	}
	
	public void EndTurnClicked () {
		switch(state) {
		case State.MYTURN:
			if(MyTurn()) {
				guiControl.SetButton(Dictionary.EnemyTurnInProgress);
				if(IsMulti) {
					networkControl.EndNetworkTurn();
				}
				thisPlayer.DeselectCard();
				mouseControl.DeselectHex();
				state = State.ENEMYTURN;
				DoGameLoop();
			}
			break;
		case State.START:
			HideOptions();
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
		case State.ENEMYTURN:
			if(!IsMulti) {
				aiController.EndTurn();
			}
			break;
		case State.PREGAME:
			HideOptions();
			guiControl.SetButton(Dictionary.startGame);
			if(!IsMulti) {
				InitSinglePlayer();
			} 
			break;
		default:
			break;
		}
	}
	
	public void StartGame() {
		if(PhotonNetwork.isNonMasterClientInRoom) {
			guiControl.SetButton(Dictionary.EnemyTurnInProgress);
		} else {
			guiControl.SetButton(Dictionary.endTurn);
		}
		thisPlayer.DrawHand();
		state = State.PRETURN;
		DoGameLoop();
	}
	
	private void EndGame() {
		state = State.GAMEOVER;
		guiControl.SetButton(Dictionary.endGame);
	}
	
	public static bool GameStarted() {
		return gameControl.state >= State.START;
	}
	
	#region SetUp
	public void SetUpMasterGame() {
		/*
		if(IsMulti) {
			networkControl.PlayNetworkCardOn(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)]);
			networkControl.PlayNetworkCardOn(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)]);
		} else {
			PlayCardOnHex(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)], System.Guid.NewGuid().ToString());
			PlayCardOnHex(baseCard, gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)], System.Guid.NewGuid().ToString());
		}*/
		SetupBases();
		thisPlayer.MaxMana++;
		state = State.START;
		cameraControl.SetPlayerCamera(true);
	}
		
	public void SetUpClientGame() {
		SetupBases(master : false);
		flags.ForEach(f => f.SwapOwner());
		guiControl.SetButton("Waiting for opponent");
		state = State.START;
		cameraControl.SetPlayerCamera(false);
	}

	public void SetupBases(bool master = true) {
		Hex p1Base = gridControl.Map[Mathf.FloorToInt(gridControl.Base1.x)][Mathf.FloorToInt(gridControl.Base1.y)];
		GameObject go = new GameObject();
		go.AddComponent<Base>();
		Base b = go.GetComponent<Base>();
		b.FromCard(null);
		b.Hex = p1Base;
		b.transform.position = p1Base.transform.position;
		Hex p2Base = gridControl.Map[Mathf.FloorToInt(gridControl.Base2.x)][Mathf.FloorToInt(gridControl.Base2.y)];
		GameObject go2 = new GameObject();
		go2.AddComponent<Base>();
		Base b2 = go2.GetComponent<Base>();
		b2.FromCard(null);
		b2.Hex = p2Base;
		b2.transform.position = p2Base.transform.position;
		thisPlayer.Base = master ? b : b2;
		enemyPlayer.Base = master ? b2 : b;
	}

	public void SetUpFlags() {
		int i = 1;
		foreach(Vector2 v in gridControl.flags.Keys) {
			Hex hex = gridControl.Map[Mathf.FloorToInt(v.x)][Mathf.FloorToInt(v.y)];
			GameObject go = (GameObject) Instantiate(flagPrefab, Vector3.zero, Quaternion.identity);
			Flag flag = go.GetComponent<Flag>();
			flag.prefabString = "Buildings/Flag" + i;
			flag.Id = System.Guid.NewGuid().ToString();
			flag.FromCard(null);
			flag.Hex = hex;
			hex.Unit = flag;
			flag.OwnerTeam = gridControl.flags[v];
			flags.Add(flag);
			flag.transform.position = hex.transform.position;
			i++;
		}
	}
	#endregion SetUp
	
	public void EnemeyEndTurn() {
		guiControl.SetButton(Dictionary.endTurn);
		state = State.MYTURN;
		thisPlayer.DrawCard();
		thisPlayer.MaxMana++;
		thisPlayer.ManaSpend = 0;
		audioControl.PlayNewTurnSound();
		guiControl.ShowSplashText(Dictionary.yourTurn);
		DoGameLoop();
	}
	
	public Unit PlayCardOnHex(Card card, Hex hex, string id) {
		// TODO Clean up this method to better handle multiple card types.
		if(typeof(EntityCard).IsAssignableFrom(card.GetType())) {
			EntityCard eCard = (EntityCard) card;
			GameObject go = (GameObject) Instantiate(unitPrefab, Vector3.zero, Quaternion.identity);
			Unit unit = go.GetComponent<Unit>();
			unit.Id = id;
			unit.FromCard(eCard);
			unit.Hex = hex;
			unit.transform.position = hex.transform.position;
			hex.Unit = unit;
			units.Add(unit);
			unit.Team = MyTurn() ? 1 : 2;
			if(MyTurn() && thisPlayer.Hand.Count != 0) {
				// TODO Find a better way to sort this
				thisPlayer.PlayCard();
			}
			card.OnPlay(new StateObject(units, hex, MyTurn() ? thisPlayer : enemyPlayer, MyTurn() ? enemyPlayer : thisPlayer));
			return unit;
		} else {
			if(MyTurn() && thisPlayer.Hand.Count != 0) {
				// TODO Find a better way to sort this
				thisPlayer.PlayCard();
			}
			card.OnPlay(new StateObject(units, hex, MyTurn() ? thisPlayer : enemyPlayer, MyTurn() ? enemyPlayer : thisPlayer));
			return null;
		}
	}
	
	public void EnemyCardPlayed(Card card) {
		GUICard guiCard = ((GameObject) Object.Instantiate(CardPrefab)).GetComponent<GUICard>();
		guiCard.SetInfo(card, enemyPlayer);
		guiCard.ForcePlaceCard(Screen.width, -300);
		guiCard.HandCard = false;
		guiCard.Played();		
	}
	
	void Update() {
		// TODO Do this properly
		/*
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
		}*/
		
		if(state > State.PREGAME && (enemyPlayer.Points >= 1000)) {
			guiControl.ShowSplashText("You lost!");
			EndGame();
		} else if(state > State.PREGAME && (thisPlayer.Points >= 1000)) {
			guiControl.ShowSplashText("You won!");
			EndGame();
		}
	}
	
	// TODO Do this properly
	void OnGUI() {
		GUI.skin = guiControl.skin;
		GUILayout.Label ("Mana status: " + thisPlayer.ManaLeft() + " / " + thisPlayer.MaxMana + "\nEnemy Stats:" + "\nCards: " + enemyPlayer.Hand.Count + "\nMana: " + enemyPlayer.ManaLeft() + " / " + enemyPlayer.MaxMana + "\nYour points: " + thisPlayer.Points + "/1000\nEnemy points: " + enemyPlayer.Points + "/1000");
		
	}
	
	
}

public enum State { PREGAME, START, PRETURN, MYTURN, ENEMYTURN, GAMEOVER}
