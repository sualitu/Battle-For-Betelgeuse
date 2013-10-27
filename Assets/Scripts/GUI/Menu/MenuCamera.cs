using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 pos1 = new Vector3(-182,33,-16);
		Vector3 mainCameraPosition = new Vector3(0,-2,-10);
		Vector3[] Path = new Vector3[2];
		Path[0] = pos1;
		Path[1] = mainCameraPosition;
		Vector3 mainCameraRotation = new Vector3(0,235,0);
		iTween.MoveTo(Camera.main.gameObject, iTween.Hash("path", Path,
			"delay", 0f,
			"time", 10,
			"easetype", "easeInOutQuad"));
		iTween.RotateTo(Camera.main.gameObject, iTween.Hash("rotation", mainCameraRotation,
			"delay", 5,
			"time", 5,
			"easetype", "easeInOutQuad"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
