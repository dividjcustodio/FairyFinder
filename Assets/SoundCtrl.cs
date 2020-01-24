using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCtrl : MonoBehaviour {

	public GameObject m_FairyObj;
	public bool soundflag;


	// Use this for initialization
	void Start () {
		soundflag = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_FairyObj.activeSelf && !soundflag) {
			GetComponent<AudioSource> ().Play ();
			soundflag = true;
		}
	}
}
