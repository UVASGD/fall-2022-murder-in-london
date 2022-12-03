using Cainos.PixelArtTopDown_Basic;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using static UnityEngine.GraphicsBuffer;

public class CharacterOptionViews : MonoBehaviour
{

    [SerializeField] public GameObject button;
    [SerializeField] public Sprite image;
    [SerializeField] public string characterName;
    [SerializeField] public CanvasGroup mainCanvasGroup;
    [SerializeField] public List<string> optionChoices;
    private float fadeTime = 1f;

    List<GameObject> buttonList = new List<GameObject>();

    private bool inDialogue = false;
    private bool inInteract = false;
    private bool buttonClicked = false;
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

        choiceButton.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(buttonList.IndexOf(choiceButton)));
    }

    public void Interact()
    {
        inInteract = true;
        Debug.Log("interacting");
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

    public void SetValues(Sprite newImage, string newName, List<string> newOptionChoices)
    {
        image = newImage;
        characterName = newName;
        optionChoices = newOptionChoices;
    }
    public void LeaveInteraction()
    {
        
        StartCoroutine(FadeOut(fadeTime, EndInteraction));
        inInteract = false;
    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
        buttonClicked = true;
        StartCoroutine(StartButtonDialogue(buttonNo, characterName));
    }

    IEnumerator StartButtonDialogue(int buttonNo, string name)
    {
        GameObject buttonPanel = transform.GetChild(1).gameObject;
        Debug.Log(buttonPanel);
        buttonPanel.SetActive(false);
        CanvasGroup buttonPanelCanvasGroup = buttonPanel.GetComponent<CanvasGroup>();
        Debug.Log(buttonPanelCanvasGroup);
        buttonPanelCanvasGroup.alpha = 0;
        buttonPanelCanvasGroup.blocksRaycasts = false;
        buttonPanelCanvasGroup.interactable = false;
        buttonPanelCanvasGroup.gameObject.SetActive(false);
        mainCanvasGroup.blocksRaycasts = false;
        
        //Debug.Log(name);

        try
        {
            FindObjectOfType<DialogueRunner>().StartDialogue(name + "_" + buttonNo.ToString());
            //FindObjectOfType<TopDownCharacterController>().dialogueInput.enabled = true;

        }

        catch (Exception e){
            Debug.Log(e);
        }

        yield return new WaitForSeconds(0);
    }
    IEnumerator FadeOut(float fadeOutTime, Action EndInteraction)
    {
        yield return StartCoroutine(Effects.FadeAlpha(mainCanvasGroup, 1, 0, fadeOutTime));
        EndInteraction();
        print("ending interaction");
    }
    void EndButtonDialogue(CanvasGroup buttonCanvasGroup, GameObject buttonPanel)
    {
        buttonClicked = false;
        buttonPanel.SetActive(true);
        buttonCanvasGroup.alpha = 1;    
        buttonCanvasGroup.blocksRaycasts = true;
        buttonCanvasGroup.interactable = true;
        buttonCanvasGroup.gameObject.SetActive(true);
        mainCanvasGroup.blocksRaycasts = true;
    }
    void EndInteraction()
    {
       
        mainCanvasGroup.alpha = 0;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = true;
        image = null; 
    }


    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
        {
            inDialogue = true;
        }
        else inDialogue = false;

        //mega scuffed :(
        if (!inDialogue && inInteract && buttonClicked)
        {
            GameObject buttonPanel = transform.GetChild(1).gameObject;
            CanvasGroup buttonPanelCanvasGroup = buttonPanel.GetComponent<CanvasGroup>();
            EndButtonDialogue(buttonPanelCanvasGroup, buttonPanel);
        }
    }
}
