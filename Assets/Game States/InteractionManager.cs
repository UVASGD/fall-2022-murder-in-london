using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Cainos.PixelArtTopDown_Basic;

public class InteractionManager : MonoBehaviour
{

    #region Singleton
    private static InteractionManager _interactionManagerInstance;

    public static InteractionManager Instance { get { return _interactionManagerInstance; } }


    private void Awake()
    {
        if (_interactionManagerInstance != null && _interactionManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _interactionManagerInstance = this;
        }
    }

    #endregion 

    public enum InteractionState
    {
        playerMove,
        talkToCharacter,
        examineEvidence,
        viewInventory,
        presentEvidence,
    }

    private InteractionState currentState;
    private InteractionState nextState;
    private string characterName;
    private Sprite characterImage;
    private List<string> characterOptionChoices;
    private AudioSource inventoryOpen = new();
    private AudioSource inventoryClose = new();

    private bool hasUpdated = true;

    // saved information for the presenting evidence state
    private string lastYarnFileToNeedEvidence;
    private string lastEvidenceNeeded;

    // Start is called before the first frame update
    void Start()
    {
        currentState = InteractionState.playerMove;
        inventoryOpen = GameObject.Find("InventoryOpenSFX").GetComponent<AudioSource>();
        inventoryClose = GameObject.Find("InventoryCloseSFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //current state: playerMove
        if(currentState == InteractionState.playerMove)
        {
            //playerMove --> talkToCharacter
            if (nextState == InteractionState.talkToCharacter && !hasUpdated)
            {
                Debug.Log("changing state");
                CharacterOptionViews characterPanelController = GetCharacterViewController();
                characterPanelController.SetValues(characterImage, characterName, characterOptionChoices); // note from Jimmy: I had to change this line to get it to compile, took my best guess. If it's breaking check the name field in this
                hasUpdated = true;
                currentState = InteractionState.talkToCharacter;
                characterPanelController.Interact();

                
            }
            //playerMove --> view Inventory
            if(nextState == InteractionState.viewInventory && !hasUpdated){
                inventoryOpen.Play();
                InventoryManager.Instance.updateInventoryDisplay();
                currentState = InteractionState.viewInventory;
                hasUpdated = true;
            }
            // playerMove --> presentEvidence
            if(nextState == InteractionState.presentEvidence && !hasUpdated){
                currentState = InteractionState.presentEvidence;
                hasUpdated = true;
            }

            
        }
        //current state: talkToCharacter
        else if (currentState == InteractionState.talkToCharacter)
        {
            
            //talkToCharacter --> playerMove
            if (nextState == InteractionState.playerMove && !hasUpdated)
            {
                CharacterOptionViews characterPanelController = GetCharacterViewController();
                characterPanelController.LeaveInteraction(); //end interaction returns false when in dialogue mode

                currentState = InteractionState.playerMove;

                hasUpdated = true;
            }
            // talkToCharacter --> presentEvidence
            if(nextState == InteractionState.presentEvidence && !hasUpdated){
                currentState = InteractionState.presentEvidence;
                hasUpdated = true;
            }

        }

        //current state: examineEvidence
        else if(currentState == InteractionState.examineEvidence)
        {

        }
        // current state: view inventory
        else if(currentState == InteractionState.viewInventory){
            if(nextState != InteractionState.viewInventory && !hasUpdated){
                inventoryClose.Play();
                currentState = nextState;
                hasUpdated = true;
            }
        }
         else if(currentState == InteractionState.presentEvidence){
            if(nextState != InteractionState.presentEvidence && !hasUpdated){
                // leaving the present evidence file, need to call the right yarn file to handle this
                // if the evidence they selected is the same as the one the original file expected, then call the yarn with the name of the last one plus "Correct"
                // otherwise call the yarn file with the name of the last one plus "Incorrect"
                // For example: If the BobTalks yarn file runs and then asks to be presented a knife evidence, then if the player gets it right the BobTalksCorrect file will be called
                // If the player gets it wrong the BobTalksIncorrect yarn file will be called instead
                TopDownCharacterController player = FindObjectOfType<TopDownCharacterController>();
                if(player){
                    player.dialogueInput.enabled = true;
                }
                if(lastEvidenceNeeded == InventoryManager.Instance.getSelected()){
                    FindObjectOfType<DialogueRunner>().StartDialogue(lastYarnFileToNeedEvidence + "Correct");
                }else{
                    FindObjectOfType<DialogueRunner>().StartDialogue(lastYarnFileToNeedEvidence + "Incorrect");
                }
                currentState = nextState;
                hasUpdated = true;
            }
        }
        else
        {
            throw new NullReferenceException("InteractionManager: no game state set?");
        }
    }

    private CharacterOptionViews GetCharacterViewController()
    {
        CharacterOptionViews characterPanelController = GetComponentInChildren<CharacterOptionViews>();
        if (characterPanelController != null)
        {
            return characterPanelController;
        }
        else
        {
            throw new NullReferenceException("Error: The character panel attached to the character options view does not exist?");
        }
    }
    public InteractionState GetInteractionState()
    {
        return currentState;
    }
    public void SetToPlayerMovement()
    {
        nextState = InteractionState.playerMove;
        hasUpdated = false;
    }
    public void SetToCharacterInteraction(Sprite newCharacterImage, string newCharactername, List<string> newCharacterOptions)
    {
        nextState = InteractionState.talkToCharacter;
        characterImage = newCharacterImage;
        characterName = newCharactername;
        characterOptionChoices = newCharacterOptions;
        hasUpdated = false;
        print("setted character interaction");
    }
    public void SetToExamineEvidence()
    {
        nextState = InteractionState.examineEvidence;
        hasUpdated = false;
    }
    public void SetToViewInventory(){
        nextState = InteractionState.viewInventory;
        hasUpdated = false;
    }

        // function to require the player to present evidence
    // needs the name of the yarn file that's calling this (so it knows what to call later) and the name of the evidence the player must present to succeed
    public void SetToPresentEvidence(string lastYarnFileName, string desiredEvidenceName){
        lastYarnFileToNeedEvidence = lastYarnFileName;
        lastEvidenceNeeded = desiredEvidenceName;
        nextState = InteractionState.presentEvidence;
        hasUpdated = false;
    }
}
