using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class splashScreenEffect : MonoBehaviour {

	public ScriptableSplashscreen splashTimeTransition;

	void Start () {
		StartCoroutine (splashTimeTransition.startNextScene (GetComponentInChildren<Image>()));
	}

	public void EndTransitionFade(){
		splashTimeTransition.FinishEvent();
	}
}
