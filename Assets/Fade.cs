using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	public Texture2D texture;
	static Fade Instance { get; set; }
	
	void Start() {
		SetInstance();
		gameObject.AddComponent<GUITexture>();
		Fade.FadeIn();
	}
	
	void SetInstance() {
		if(Instance == null) {
			Instance = this;
		} else {
			Debug.LogError("Creation of a second fade instance was attempted. It will be destroyed.");
			Destroy (this);
		}
	}
	// Use this for initialization

	public static void FadeIn() {
		Instance.guiTexture.texture = Instance.texture;
		Instance.guiTexture.color = Color.black;
		Instance.guiTexture.pixelInset = new Rect(0, 0, 1920, 1080);
		iTween.ColorTo(Instance.gameObject, iTween.Hash("a", 0f, "time", 2f));
	}

	public static void FadeOut() {
		Instance.guiTexture.texture = Instance.texture;
		Instance.guiTexture.color = new Color(0, 0, 0, 0);
		Instance.guiTexture.pixelInset = new Rect(0, 0, 1920, 1080);
		iTween.ColorTo(Instance.gameObject, iTween.Hash("a", 1f, "time", 2f));
	}
	
	void OnGUI() {
		GUI.depth = 0;
	}
}
