using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Transitions;

namespace Transitions{
    public enum TransitionType
    {
        fadeOutIn,
    }

    public class TransitionManager : MonoBehaviour
    {
     #region Singleton
    private static TransitionManager _transitionManager;

    public static TransitionManager Instance { get { return _transitionManager; } }


    private void Awake()
    {
        if (_transitionManager != null && _transitionManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _transitionManager = this;
        }
    }

    #endregion 
    //Holds animation commands that can be called in yarn
    public CanvasGroup curtainCanvasGroup;

    public void PlayTransition(string transition){
        if (transition  == "fadeOutIn") {
            StartCoroutine(FadeOutFadeIn());
        }
    } 
    

    IEnumerator FadeOutFadeIn()
    {
        Debug.Log("entering: ProgressManager.FadeOutFadeIn()");
        yield return StartCoroutine(Effects.FadeAlpha(curtainCanvasGroup, 0, 1, 2));

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(Effects.FadeAlpha(curtainCanvasGroup, 1, 0, 2));
    }
}

}
