using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KingOfTheHill : GameControl {

	void Start () {
		InitGame();
		InitControllers();	
		InitPlayers(Deck);
		SetUpFlags();
		LoadingScreen.hide();
	}
	#region Init
	
	void InitGame() {
		new MothershipCard();
		Card.InitiateCards();
	}
	
	void InitPlayers(List<Card> deck) {
		ThisPlayer = new Player(deck, this);
		ThisPlayer.Ai = false;
		ThisPlayer.Team = Team.ME;
		if(IsMulti) {
			// TODO Fix for multiplayer
			EnemyPlayer = new Player(deck, this);
		} else {
			EnemyPlayer = new Player(Card.ControlDeck(), this);
		}
		EnemyPlayer.Ai = !IsMulti;
		EnemyPlayer.Team = Team.ENEMY;		
	}
	
	void InitSinglePlayer() {
		GuiControl.SetMainButton(Dictionary.StartGame);
		switch (Difficulty) {
		case Difficulty.EASY:
			gameObject.AddComponent<EasyAI>();
			break;
		case Difficulty.HARD:
			gameObject.AddComponent<HardAI>();
			break;
		default:
			throw new System.ArgumentOutOfRangeException ();
		}
		aiController = GetComponent<AIControl>().SetAI(EnemyPlayer, this);
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
	
	public override int FlagCountValue(int i) {
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
	
	protected override void DoGameLoop () {
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
			ThisPlayer.DeselectCard();
			GuiControl.SetMainButton(Dictionary.EndTurn);
			Units.RemoveAll(u => u == null);
			foreach(Unit u in Units) {
				if(u.Team == ThisPlayer.Team) {
					u.ResetStats();
					u.OnNewTurn(new StateObject(Units, u.Hex, null, ThisPlayer, EnemyPlayer));
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
					u.OnNewTurn(new StateObject(Units, u.Hex, null, EnemyPlayer, ThisPlayer));
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
	
	bool TurnEnded = false;
	
	protected override void EndTurn() {
		TurnEnded = false;
		GuiControl.SetMainButton(Dictionary.EnemyTurnInProgress);
		if(IsMulti) {
			NetworkControl.EndNetworkTurn();
		}
		ThisPlayer.DeselectCard();
		State = State.ENEMYTURN;
		DoGameLoop();
	}
	
	public override void EndTurnClicked () {
		switch(State) {
		case State.MYTURN:
			if(MyTurn()) {
				GuiControl.SetMainButton(Dictionary.EndingTurn);
				TurnEnded = true;
			}
			break;
		case State.START:
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
			GuiControl.SetMainButton(Dictionary.StartGame);
			if(!IsMulti) {
				InitSinglePlayer();
				EndTurnClicked();
			}
			break;
		default:
			break;
		}
	}
	
	public override void StartGame() {
		if(PhotonNetwork.isNonMasterClientInRoom) {
			GuiControl.SetMainButton(Dictionary.EnemyTurnInProgress);
		} else {
			GuiControl.SetMainButton(Dictionary.EndTurn);
		}
		ThisPlayer.DrawHand();
		State = State.PRETURN;
		DoGameLoop();
	}
	
	protected override void EndGame() {
		State = State.GAMEOVER;
		GuiControl.SetMainButton(Dictionary.EndGame);
	}
	

	
	#region SetUp
	public override void SetUpMasterGame() {
		SetupBases();
		ThisPlayer.MaxMana++;
		State = State.START;
		CameraControl.SetPlayerCamera();
	}
	
	public override void SetUpClientGame() {
		SetupBases(false);
		Flags.ForEach(f => f.SwapOwner()); // This line is only needed when players start with flags.
		GuiControl.SetMainButton(Dictionary.WaitingForOpponent);
		State = State.START;
		CameraControl.SetPlayerCamera(false);
	}
	
	// TODO: Consider doing this a different way.
	void SetupBases(bool master = true) {
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
	
	void SetUpFlags() {
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
	
	public override void EnemyEndTurn() {
		
		GuiControl.SetMainButton(Dictionary.EndTurn);
		State = State.MYTURN;
		ThisPlayer.DrawCard();
		ThisPlayer.MaxMana++;
		ThisPlayer.ManaSpend = 0;
		AudioControl.PlayNewTurnSound();
		GuiControl.ShowSplashTexture(Assets.Instance.YourTurn);
		DoGameLoop();
	}
	
	public override Unit PlayCardOnHex(Card card, Hex hex, string id) {
		CardHistory.Add(card);
		GuiControl.AddCardToHistory(card);
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
			card.OnPlay(new StateObject(Units, hex, null, MyTurn() ? ThisPlayer : EnemyPlayer, MyTurn() ? EnemyPlayer : ThisPlayer));
			return unit;
		} else {
			if(MyTurn() && ThisPlayer.Hand.Count != 0) {
				// TODO Find a better way to sort this
				ThisPlayer.PlayCard();
			}
			card.OnPlay(new StateObject(Units, hex, null, MyTurn() ? ThisPlayer : EnemyPlayer, MyTurn() ? EnemyPlayer : ThisPlayer));
			return null;
		}
	}
	
	public override void EnemyCardPlayed(Card card) {
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
	
	IEnumerator LeaveGameDelayed() {
		yield return new WaitForSeconds(2);
		LeaveGame();
	}	
	
	public override void LeaveGame() {
		GameControl.gameControl.NetworkControl.QuitGame();
		Application.LoadLevel (0);
	}
}
