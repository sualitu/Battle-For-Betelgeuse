using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {
	
	public static GameControl gameControl;
	
	public State State { get; set; } // TODO: Consider changing the main gui button
	public Player ThisPlayer { get; set; }
	public Player EnemyPlayer { get; set; }

	// Collections
	public List<Flag> Flags = new List<Flag>();
	public List<Unit> Units = new List<Unit>();	
	public List<AuraBuff> AuraBuffs = new List<AuraBuff>();

	// Unity Stuff
	public GameObject UnitPrefab;
	public GameObject CardPrefab;
	public GameObject DeckButtonPrefab;
	public GameObject FlagPrefab;

	// State
	public static bool IsMulti = false;
	public static bool Cheating = true;
	
	// Controllers
	public GridControl GridControl { get; set; }
	public GUIControl GuiControl { get; set; }
	public CombatControl CombatControl { get; set; }
	public MouseControl MouseControl { get; set; }
	public NetworkControl NetworkControl { get; set; }
	public AudioControl AudioControl { get; set; }
	public CameraControl CameraControl { get; set; }
	public KeyboardControl KeyboardControl { get; set; }
	AIControl aiController;

	#region Deck Picking 
	
	// The deck chosen should be defined before loading of this scene. This is temporary. 	
	List<Card> deckFromInt(int i) {
		switch(i) {
		case 0: return Card.GoodDeck(); 
		case 1: return Card.NeutralDeck();
		case 2: return Card.EvilDeck();
		default: return Card.NeutralDeck();
		}
	}

	public bool NoMovesInProgress() {
		return iTween.tweens.Count < 1;
	}
	
	public void ChooseDeck(int i) {
		InitPlayers(deckFromInt (i));
		if(!IsMulti) {
			InitSinglePlayer();
		} else {
			GuiControl.SetMainButton("Waiting for opponents");
		}
		HideOptions();
	}
	
	List<DeckOption> dos = new List<DeckOption>();
	
	void HideOptions() {
		NetworkControl.DeckChosen();
		dos.ForEach(Destroy);
	}
	
	void ShowDeckOptions() {
		DeckOption doZero = ((GameObject) Instantiate(DeckButtonPrefab)).GetComponent<DeckOption>();
		doZero.index = 0;
		doZero.h = -1000;
		doZero.title = "Good Deck";
		
		DeckOption doTwo = ((GameObject) Instantiate(DeckButtonPrefab)).GetComponent<DeckOption>();
		doTwo.index = 2;
		doTwo.h = -600;
		doTwo.title = "Evil Deck";
		
		dos.Add(doZero);
		dos.Add(doTwo);
	}
	#endregion 
	
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
		State = State.PREGAME;
		Card.InitCards();
	}
	
	void InitPlayers(List<Card> deck) {
		ThisPlayer = new Player(deck, this);
		ThisPlayer.Ai = false;
		ThisPlayer.Team = Team.ME;
		if(IsMulti) {
			EnemyPlayer = new Player(deck, this);
		} else {
			EnemyPlayer = new Player(Card.AIDeck(), this);
		}
		EnemyPlayer.Ai = !IsMulti;
		EnemyPlayer.Team = Team.ENEMY;		
	}
	
	void InitSinglePlayer() {
		GuiControl.SetMainButton(Dictionary.StartGame);
		gameObject.AddComponent<HardAI>();
		aiController = GetComponent<HardAI>().SetAI(EnemyPlayer, this);
		EnemyPlayer.DrawHand();
		SetUpMasterGame();
	}
	
	void InitControllers() {
		gameControl = this;
		KeyboardControl = gameObject.AddComponent<KeyboardControl>();
		GridControl = GetComponent<GridControl>();
		GuiControl = gameObject.AddComponent<GUIControl>();
		CombatControl = gameObject.AddComponent<CombatControl>();
		MouseControl = gameObject.AddComponent<MouseControl>();
		NetworkControl = gameObject.AddComponent<NetworkControl>();
		AudioControl = gameObject.AddComponent<AudioControl>();
		CameraControl = gameObject.AddComponent<CameraControl>();
	}
	#endregion Init

	public int FlagCountValue(int i) {
		return Mathf.FloorToInt(Settings.FlagBaseValue*(Mathf.Pow(Settings.FlagMultValue,i-1)*i));
	}
	
	void CalculatePoints() {
		int thisPlayerFlags = 0;
		int enemyPlayerFlags = 0;
		foreach(Flag flag in Flags) {
			if(flag.OwnerTeam == ThisPlayer.Team) thisPlayerFlags++;
			if(flag.OwnerTeam == EnemyPlayer.Team) enemyPlayerFlags++;
		}
		ThisPlayer.Points += FlagCountValue(thisPlayerFlags);
		EnemyPlayer.Points += FlagCountValue(enemyPlayerFlags);
	}
	
	void DoGameLoop () {
		GuiControl.UpdateGUI();
		switch(State) {
			case State.PRETURN:
				if(!IsMulti || PhotonNetwork.isMasterClient) {
					State = State.MYTURN;
				} else {
					State = State.ENEMYTURN;
				}	
				DoGameLoop();
				break;
			case State.MYTURN:
				Flags.ForEach(f => f.OnNewTurn(null));
				CalculatePoints();
				GuiControl.SetMainButton(Dictionary.EndTurn);
				Units.RemoveAll(u => u == null);
				foreach(Unit u in Units) {
					if(u.Team == ThisPlayer.Team) {
						u.ResetStats();
						u.OnNewTurn(new StateObject(Units, u.Hex, ThisPlayer, EnemyPlayer));
					}
				}
				break;
			case State.ENEMYTURN:
				Flags.ForEach(f => f.OnNewTurn(null));
				CalculatePoints();
				Units.RemoveAll(u => u == null);
				foreach(Unit u in Units) {
					if(u.Team != ThisPlayer.Team) {
						u.ResetStats();
						u.OnNewTurn(new StateObject(Units, u.Hex, EnemyPlayer, ThisPlayer));
					}
				}
				if(!IsMulti) {
					EnemyPlayer.DrawCard();
					EnemyPlayer.MaxMana++;
					EnemyPlayer.ManaSpend = 0;
				}
				break;
			default: 
				break;
			}
	}
	
	public bool MyTurn() {
		return State == State.MYTURN;
	}

	bool TurnEnded = false;

	void EndTurn() {
		TurnEnded = false;
		GuiControl.SetMainButton(Dictionary.EnemyTurnInProgress);
		if(IsMulti) {
			NetworkControl.EndNetworkTurn();
		}
		ThisPlayer.DeselectCard();
		State = State.ENEMYTURN;
		DoGameLoop();
	}
	
	public void EndTurnClicked () {
		switch(State) {
		case State.MYTURN:
			if(MyTurn()) {
				GuiControl.SetMainButton(Dictionary.EndingTurn);
				TurnEnded = true;
			}
			break;
		case State.START:
			HideOptions();
			if(IsMulti) {
				NetworkControl.StartNetworkGame();
			} else {
				StartGame();
			}
			break;
		case State.GAMEOVER:
			NetworkControl.QuitGame();
			Application.LoadLevel (0);
			break;
		case State.ENEMYTURN:
			if(!IsMulti && Cheating) {
				aiController.EndTurn();
			}
			break;
		case State.PREGAME:
			HideOptions();
			GuiControl.SetMainButton(Dictionary.StartGame);
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
			GuiControl.SetMainButton(Dictionary.EnemyTurnInProgress);
		} else {
			GuiControl.SetMainButton(Dictionary.EndTurn);
		}
		ThisPlayer.DrawHand();
		State = State.PRETURN;
		DoGameLoop();
	}
	
	private void EndGame() {
		State = State.GAMEOVER;
		GuiControl.SetMainButton(Dictionary.EndGame);
	}
	
	public static bool GameStarted() {
		return gameControl.State >= State.START;
	}
	
	#region SetUp
	public void SetUpMasterGame() {
		SetupBases();
		ThisPlayer.MaxMana++;
		State = State.START;
		CameraControl.SetPlayerCamera();
	}
		
	public void SetUpClientGame() {
		SetupBases(false);
		Flags.ForEach(f => f.SwapOwner()); // This line is only needed when players start with flags.
		GuiControl.SetMainButton(Dictionary.WaitingForOpponent);
		State = State.START;
		CameraControl.SetPlayerCamera(false);
	}

	// TODO: Consider doing this a different way.
	public void SetupBases(bool master = true) {
		Hex p1Base = GridControl.Map[Mathf.FloorToInt(GridControl.Base1.x)][Mathf.FloorToInt(GridControl.Base1.y)];
		GameObject go = new GameObject();
		go.AddComponent<Base>();
		Base b = go.GetComponent<Base>();
		b.FromCard(null);
		b.Hex = p1Base;
		b.transform.position = p1Base.transform.position;
		Hex p2Base = GridControl.Map[Mathf.FloorToInt(GridControl.Base2.x)][Mathf.FloorToInt(GridControl.Base2.y)];
		GameObject go2 = new GameObject();
		go2.AddComponent<Base>();
		Base b2 = go2.GetComponent<Base>();
		b2.FromCard(null);
		b2.Hex = p2Base;
		b2.transform.position = p2Base.transform.position;
		ThisPlayer.Base = master ? b : b2;
		EnemyPlayer.Base = master ? b2 : b;
	}

	public void SetUpFlags() {
		int i = 1;
		foreach(Vector2 v in GridControl.flags.Keys) {
			Hex hex = GridControl.Map[Mathf.FloorToInt(v.x)][Mathf.FloorToInt(v.y)];
			GameObject go = (GameObject) Instantiate(FlagPrefab, Vector3.zero, Quaternion.identity);
			Flag flag = go.GetComponent<Flag>();
			flag.prefabString = "Buildings/Flag" + i;
			flag.Id = System.Guid.NewGuid().ToString();
			flag.FromCard(null);
			flag.Hex = hex;
			hex.Unit = flag;
			Flags.Add(flag);
			flag.transform.position = hex.transform.position;
			i++;
		}
	}
	#endregion SetUp
	
	public void EnemeyEndTurn() {

		GuiControl.SetMainButton(Dictionary.EndTurn);
		State = State.MYTURN;
		ThisPlayer.DrawCard();
		ThisPlayer.MaxMana++;
		ThisPlayer.ManaSpend = 0;
		AudioControl.PlayNewTurnSound();
		GuiControl.ShowSplashText(Dictionary.YourTurn);
		DoGameLoop();
	}
	
	public Unit PlayCardOnHex(Card card, Hex hex, string id) {
		// TODO Clean up this method to better handle multiple card types.
		if(typeof(EntityCard).IsAssignableFrom(card.GetType())) {
			EntityCard eCard = (EntityCard) card;
			GameObject go = (GameObject) Instantiate(UnitPrefab, Vector3.zero, Quaternion.identity);
			Unit unit = go.GetComponent<Unit>();
			unit.Id = id;
			unit.FromCard(eCard);
			unit.Hex = hex;
			unit.transform.position = hex.transform.position;
			hex.Unit = unit;
			Units.Add(unit);
			unit.Team = MyTurn() ? Team.ME : Team.ENEMY;
			if(MyTurn() && ThisPlayer.Hand.Count != 0) {
				// TODO Find a better way to sort this
				ThisPlayer.PlayCard();
			}
			card.OnPlay(new StateObject(Units, hex, MyTurn() ? ThisPlayer : EnemyPlayer, MyTurn() ? EnemyPlayer : ThisPlayer));
			return unit;
		} else {
			if(MyTurn() && ThisPlayer.Hand.Count != 0) {
				// TODO Find a better way to sort this
				ThisPlayer.PlayCard();
			}
			card.OnPlay(new StateObject(Units, hex, MyTurn() ? ThisPlayer : EnemyPlayer, MyTurn() ? EnemyPlayer : ThisPlayer));
			return null;
		}
	}
	
	public void EnemyCardPlayed(Card card) {
		GUICard guiCard = ((GameObject) Object.Instantiate(CardPrefab)).GetComponent<GUICard>();
		guiCard.SetInfo(card, EnemyPlayer);
		guiCard.ForcePlaceCard(Screen.width, -300);
		guiCard.HandCard = false;
		guiCard.Played();		
	}

	int i = 0;
	void Update() {
		if(TurnEnded && NoMovesInProgress()) { 
			if( i < 10) i++;
			else {
				i = 0;
				EndTurn(); 
			}
		}

		if(State > State.PREGAME && (EnemyPlayer.Points >= Settings.VictoryRequirement)) {
			GuiControl.ShowSplashText("You lost!");
			EndGame();
		} else if(State > State.PREGAME && (ThisPlayer.Points >= Settings.VictoryRequirement)) {
			GuiControl.ShowSplashText("You won!");
			EndGame();
		}
	}
	
	
}

public enum State { PREGAME, START, PRETURN, MYTURN, ENEMYTURN, GAMEOVER}
