using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GameControl))]
public class NetworkControl : Photon.MonoBehaviour {
	
	GameControl gameControl;
	PhotonView NetworkPhoton;
	
	// Use this for initialization
	void Start () {
		if(GameControl.IsMulti) {
			PhotonNetwork.ConnectUsingSettings(Dictionary.version);
			gameControl = GetComponent<GameControl>();
			NetworkPhoton = GetComponent<PhotonView>();
		}
	}
	
	
	void OnJoinedLobby() {
		PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed() {
		PhotonNetwork.CreateRoom(null);
	}
	
	void OnJoinedRoom() {
		if(PhotonNetwork.isNonMasterClientInRoom) {
			NetworkPhoton.RPC ("Joined", PhotonTargets.MasterClient, null);
			gameControl.SetUpClientGame();
		}
	}
	
	void OnCreatedRoom() {	
		gameControl.guiControl.ShowSplashText("Waiting for an opponent...");
	}
	
	[RPC]
	void Joined() {
		gameControl.guiControl.ShowSplashText("Opponent found!");
		gameControl.SetUpGame();
		PhotonNetwork.room.open = false;
	}

	
	public void MoveNetworkUnit(Unit unit, Hex hex) {
		System.Object[] args = new System.Object[3];
		args[0] = unit.Id.ToString();
		args[1] = Mathf.FloorToInt(hex.GridPosition.x);
		args[2] = Mathf.FloorToInt(hex.GridPosition.y);
		NetworkPhoton.RPC("ReceiveNetworkUnitMove", PhotonTargets.All, args);	
	}
	
	[RPC]
	public void ReceiveNetworkUnitMove(string id, int x, int y) {
		Unit unit = gameControl.units.Find(u => u.Id.ToString() == id);
		Hex hex = gameControl.gridControl.Map[x][y];
		unit.PrepareMove(hex);
	}
	
	public void PlayNetworkCardOn(Card card, Hex hex) {
		System.Object[] args = new System.Object[4];
		args[0] = card.id;
		args[1] = Mathf.FloorToInt(hex.GridPosition.x);
		args[2] = Mathf.FloorToInt(hex.GridPosition.y);
		args[3] = System.Guid.NewGuid().ToString();
		NetworkPhoton.RPC("ReceiveNetworkCard", PhotonTargets.All, args);
		NetworkPhoton.RPC("ReceiveOpponentPlayedCard", PhotonTargets.Others, args);
	}
	
	[RPC]
	public void ReceiveNetworkCard(int id, int x, int y, string guid) {
		Card card = (Card) Card.cardTable[id];
		Hex hex = gameControl.gridControl.Map[x][y];
		gameControl.PlayCardOnHex(card, hex, guid);
	}
	
	[RPC]
	public void ReceiveOpponentPlayedCard(int id, int x, int y, string guid) {
		Card card = (Card) Card.cardTable[id];
		gameControl.EnemyCardPlayed(card);
	}
	
	public void StartNetworkGame() {
		if(PhotonNetwork.isMasterClient) {
			NetworkPhoton.RPC("ReceiveStartGame", PhotonTargets.All, null);
		}
	}
	
	[RPC]
	public void ReceiveStartGame() {
		gameControl.StartGame();
	}
	
	public void EndNetworkTurn() {
		NetworkPhoton.RPC("ReceiverEndNetworkTurn", PhotonTargets.Others, null);
	}
	
	[RPC]
	public void ReceiverEndNetworkTurn() {
		gameControl.EnemeyEndTurn();
	}
}
