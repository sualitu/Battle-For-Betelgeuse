using UnityEngine;
using System.Collections;

public class Assets : MonoBehaviour
{
	public static Assets Instance { get; set; }

	void Start() {
		SetInstance();
	}

	void SetInstance() {
		if(Instance == null) {
			Instance = this;
		} else {
			Debug.LogError("Creation of a second assets instance was attempted. It will be destroyed.");
			Destroy (this);
		}
	}

	// Models
	public GameObject Flag;

	// Animations
	public GameObject GreenFlagRing;
	public GameObject GreyFlagRing;
	public GameObject RedFlagRing;

	// Grid
	public Texture2D BaseHex;
	public Texture2D MoveHex;
	public Texture2D AttackHex;
	public Texture2D OwnedFlag;
	public Texture2D EnemyFlag;
	public Texture2D NeutralFlag;

	// Mouse
	public GameObject Selector;
	public GameObject MouseOver;
	public GameObject MouseOverCircle;

	// Sounds
	public AudioClip NewTurnSound;
	public AudioClip ErrorSound;
	public AudioClip EndTurnSound;

	// GUI
	public GUIText SmallTextSplashPrefab;
	public GUIText TextSplashPrefab;
	public GUITexture SelUnitBox;
	public GUIText SelUnitLabels;
	public GUIText SelUnitValues;
	public GUIText SelUnitName;
	public GUIText PlayerStats;
	public GUIText EnemyStats;
	public GUISkin Skin;
	public GameObject CardPrefab;
}