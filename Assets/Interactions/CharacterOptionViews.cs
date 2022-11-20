using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CharacterOptionViews : MonoBehaviour
{

    [SerializeField] public GameObject button;
    [SerializeField] public Sprite image;
    [SerializeField] public CanvasGroup mainCanvasGroup;
    [SerializeField] public List<string> optionChoices;
    private float fadeTime = 1f;

    List<GameObject> buttonList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

    }

    void CreateNewButton()
    {
        var choiceButton = Instantiate(button);

        GameObject buttonPanel = transform.GetChild(1).gameObject;
        choiceButton.gameObject.transform.SetParent(buttonPanel.transform, false);
        choiceButton.gameObject.transform.SetAsLastSibling();
        choiceButton.gameObject.SetActive(false);

        buttonList.Add(choiceButton);
    }

    public void Interact()
    {
        while (buttonList.Count < optionChoices.Count)
        {
            CreateNewButton();
        }

        //create new buttons for each option in list, add to button panel

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

        //set image
        GameObject characterImage = transform.GetChild(0).gameObject;

        characterImage.GetComponent<Image>().sprite = image;


        // Fade it all in
        StartCoroutine(Effects.FadeAlpha(mainCanvasGroup, 0, 1, fadeTime));

        print("supposed to have faded in");

    }

    public void SetValues(Sprite new_image, List<string> newOptionChoices)
    {
        image = new_image;
        optionChoices = newOptionChoices;
    }
    public void LeaveInteraction()
    {
        StartCoroutine(FadeOut(fadeTime, EndInteraction));

    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
    }

    IEnumerator FadeOut(float fadeOutTime, Action EndInteraction)
    {
        yield return StartCoroutine(Effects.FadeAlpha(mainCanvasGroup, 1, 0, fadeOutTime));
        EndInteraction();
        print("ending interaction");
    }
    void EndInteraction()
    {
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;
        image = null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
