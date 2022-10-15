using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private InteractionState currentState;
    private InteractionState nextState;
    private Sprite characterImage;
    private List<string> characterOptionChoices;

    private bool hasUpdated = true;

    // Start is called before the first frame update
    void Start()
    {
        currentState = InteractionState.playerMove;
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
                CharacterOptionViews characterPanelController = GetCharacterViewController();
                characterPanelController.SetValues(characterImage, characterOptionChoices);
                characterPanelController.Interact();

                hasUpdated = true;

                currentState = InteractionState.talkToCharacter;
            }

            
        }
        //current state: talkToCharacter
        else if (currentState == InteractionState.talkToCharacter)
        {
            
            //talkToCharacter --> playerMove
            if (nextState == InteractionState.playerMove && !hasUpdated)
            {
                CharacterOptionViews characterPanelController = GetCharacterViewController();
                characterPanelController.LeaveInteraction();
                hasUpdated = true;

                currentState = InteractionState.playerMove;
            }

        }

        //current state: examineEvidence
        else if(currentState == InteractionState.examineEvidence)
        {

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
    public void SetToCharacterInteraction(Sprite newCharacterImage, List<string> newCharacterOptions)
    {
        nextState = InteractionState.talkToCharacter;
        characterImage = newCharacterImage;
        characterOptionChoices = newCharacterOptions;
        hasUpdated = false;
    }
    public void SetToExamineEvidence()
    {
        nextState = InteractionState.examineEvidence;
        hasUpdated = false;
    }
}
