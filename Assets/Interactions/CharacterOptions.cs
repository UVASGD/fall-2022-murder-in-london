using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CharacterOptions : NPC
{

    [SerializeField] public GameObject button;
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] public List<string> optionChoices;
    private float fadeTime = 1f;

    List<GameObject> buttonList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

    }

    void CreateNewButton()
    {
        var choiceButton = Instantiate(button);

        choiceButton.gameObject.transform.SetParent(transform, false);
        choiceButton.gameObject.transform.SetAsLastSibling();
        choiceButton.gameObject.SetActive(false);

        buttonList.Add(choiceButton);
    }

    public override void interact()
    {
        while (buttonList.Count < optionChoices.Count)
        {
            CreateNewButton();
        }

        //foreach (Transform child in transform)
        //{
        //    Debug.Log(child.name);
        //}

        for (int i = 0; i < optionChoices.Count; i++)
        {
            var button = buttonList[i];
            Debug.Log(button);
            try
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = optionChoices[i];
                Debug.Log(button.GetComponentInChildren<TextMeshProUGUI>().text);
                button.gameObject.SetActive(true);

            }
            catch (NullReferenceException e)
            {
                Debug.Log("NULL REFERENCE EXCEPTION CAUGHT");
                Debug.Log(e);
            }

        }


        // Fade it all in
        StartCoroutine(Effects.FadeAlpha(canvasGroup, 0, 1, fadeTime));

        print("supposed to have faded in");

    }

    public override void leaveInteraction()
    {
        StartCoroutine(FadeOut(fadeTime, EndInteraction));

    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
    }

    IEnumerator FadeOut(float fadeOutTime, Action EndInteraction)
    {
        yield return StartCoroutine(Effects.FadeAlpha(canvasGroup, 1, 0, fadeOutTime));
        EndInteraction();
        print("ending interaction");
    }
    void EndInteraction()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
