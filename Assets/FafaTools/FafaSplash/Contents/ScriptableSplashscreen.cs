using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName="FafaTools/SplashScreenReglages")]
public class ScriptableSplashscreen : ScriptableObject {

	public string nextScene;
	public float splashScreenTime;

	public Sprite[] imgSplashs;

	private bool hasFinished=false;

	public IEnumerator startNextScene(Image slotImg){
		for(int i = 0; i<imgSplashs.Length;i++){
			slotImg.sprite = imgSplashs[i];
			Animator anim = slotImg.GetComponentInParent<Animator>();
			anim.Play("FadeSplashDebut");
			yield return new WaitForSeconds (splashScreenTime);
			anim.SetTrigger("EndFade");
			yield return new WaitUntil(delegate{return hasFinished;});
			hasFinished=false;
		}
		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}

	delegate bool getVar();

	public void FinishEvent(){
		hasFinished=true;
	}
}
