using UnityEngine;
using System.Collections;

public class KeyboardControl : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKey(KeyCode.Escape)) {
			Application.LoadLevel (1);
		}
	}
}

