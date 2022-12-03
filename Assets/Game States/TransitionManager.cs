using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Transitions;

namespace Transitions
{

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
        public RectTransform objectionImage;
        public bool playingTransition { get; private set; } = false;
        public IEnumerator PlayTransition(string transition)
        {
            playingTransition = true;
            if (transition == "fadeOutIn")
            {
                yield return StartCoroutine(FadeOutFadeIn());
            }
            else if (transition == "objectionShake")
			{
                yield return StartCoroutine(ObjectionShake());
			}
            playingTransition = false;

        }


        IEnumerator FadeOutFadeIn()
        {
            Debug.Log("entering: ProgressManager.FadeOutFadeIn()");
            yield return StartCoroutine(Effects.FadeAlpha(curtainCanvasGroup, 0, 1, 2));

            yield return new WaitForSeconds(2);

            yield return StartCoroutine(Effects.FadeAlpha(curtainCanvasGroup, 1, 0, 2));
        }

        IEnumerator ObjectionShake()
        {
            Debug.Log("entering: ProgressManager.ObjectionShake()");
            float shakeDuration = 3f;
            float shakeMagnitude = 100f;
            float shakeRate = 0.001f;
            Vector3 initialPosition = objectionImage.localPosition;
            objectionImage.gameObject.SetActive(true);
            while (shakeDuration > 0)
            {
                objectionImage.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
                yield return new WaitForSeconds(shakeRate);
                shakeDuration -= shakeRate;
            }
            transform.localPosition = initialPosition;
            objectionImage.gameObject.SetActive(false);

            yield return null;
        }

    }
}
