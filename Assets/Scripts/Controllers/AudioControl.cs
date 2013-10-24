using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour
{

	public static void PlayAudioFile(string file) {
		var clip = (AudioClip) Resources.Load("Sounds/" + file);
		Camera.main.audio.clip = clip;
		Camera.main.audio.audio.Play();
	}
}

