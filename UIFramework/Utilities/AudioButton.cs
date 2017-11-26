using UnityEngine;
using System.Collections;

public class AudioButton : MonoBehaviour {

	public AudioSource audioSource;
	public AudioClip selectAudio, confirmAudio;
	
	private static AudioButton singleton;

	void Awake(){
		AudioListener.pause = false;
		singleton = this;
		audioSource.ignoreListenerPause=true;
	}

	public void Confirm(){
		audioSource.PlayOneShot(confirmAudio);
	}

	public void Select(){
		audioSource.PlayOneShot(selectAudio);
	}

	public static void sConfirm(){
		if(singleton)
			singleton.Confirm();
	}

	public static void sSelect(){
		if(singleton)
			singleton.Select();
	}
}
