using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;
using System;

public class SceneDirector : MonoBehaviour
{

    #region Singleton
    private static SceneDirector _sceneDirector;

    public static SceneDirector Instance { get { return _sceneDirector; } }

    private bool loseGame = false;
    private string loseGameFile = "";
    private bool loseGameDialogueRan = false;
    private bool finishScene = true;
    private void Awake()
    {
        if (_sceneDirector != null && _sceneDirector != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _sceneDirector = this;
        }

        //get the current DialogueRunner object
        //there should only be one DialogueRunner object in the game
        DialogueRunner dialogueRunnerObject = FindObjectOfType<DialogueRunner>();

        //add the ProgressScene method to the DialogueRunner object as a command
        //whenever "achievement [x]" is called in yarn scripts, now that command will execute
        if (dialogueRunnerObject != null)
        {
            dialogueRunnerObject.AddCommandHandler<string>(
                "achievement",
                ProgressScene
                );
            dialogueRunnerObject.AddFunction(
                "checkAchievement",
                (string nodeName) => { return CheckIfPlayerCompleted(nodeName); } //lambda function
                );
            dialogueRunnerObject.AddFunction(
                "GetSceneNumber",
                () => { return GetSceneNumber(); } //lambda function
                );
            dialogueRunnerObject.AddFunction(
                "PlayerLost",
                () => { return PlayerLost(); } //lambda function
                );
            dialogueRunnerObject.AddCommandHandler<string, string>(
                "requireEvidence",
                PresentEvidence
            );
            dialogueRunnerObject.AddCommandHandler(
                "transitionScene",
                TransitionToNextScene
            );
            dialogueRunnerObject.AddCommandHandler(
                "showHealth",
                ShowHealth
            );
            dialogueRunnerObject.AddCommandHandler(
                "hideHealth",
                HideHealth
            );
            dialogueRunnerObject.AddCommandHandler<int>(
                "updateHealth",
                UpdateHealth
            );
            dialogueRunnerObject.AddCommandHandler<string>(
                "loseGame",
                LoseGame
            );

        }
        else
        {
            Debug.Log("Dialogue runner object null??");
        }

        Debug.Log("ProgressScene should be added as a command (?)");
    }
    #endregion 

    // Start is called before the first frame update
    void Start()
    {
        //set initial achievements
        loseGame = false;
        loseGameFile = "";
        loseGameDialogueRan = false;
        List<string> requiredAchievements = new();
        if(ProgressManager.Instance){
            ProgressManager.Instance.SetMultipleExpectedProgressList(requiredAchievements);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (loseGame == true)
        {
            if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == false)
            {
                if (loseGameDialogueRan == false)
                {
                    print("starting node: " + loseGameFile);
                    FindObjectOfType<DialogueRunner>().StartDialogue(loseGameFile);
                    loseGameDialogueRan = true;
                }
                else
                {
                    ProgressManager.Instance.LoseScene();

                }
            }
        }

        //if all achievements found, move onto next scene
        if (ProgressManager.Instance){
            if (ProgressManager.Instance.SceneComplete())
            {
                if(FindObjectOfType<DialogueRunner>().IsDialogueRunning == false){
                    if (finishScene)
                    {
                        int sceneType = ProgressManager.Instance.getInvestigationScene();
                        print(sceneType);
                        if(sceneType == 1)
                        {
                            FindObjectOfType<DialogueRunner>().StartDialogue("Investigation1Finish");
                        }
                        else
                        {
                            FindObjectOfType<DialogueRunner>().StartDialogue("Investigation2Finish");
                        }
                        finishScene = false;
                    }
                    else
                    {
                        //Debug.Log("changing scene");
                        HashSet<string> nextExpectedAchievements = new();
                        ProgressManager.Instance.FinishScene(nextExpectedAchievements);
                    }
                    
                }
            }
        }
    }


    private void ProgressScene(string achievement)
    {
        ProgressManager.Instance.ProgressScene(achievement);
    }
    private int GetSceneNumber()
    {
        return ProgressManager.Instance.getInvestigationScene();
    }
    private bool CheckIfPlayerCompleted(string nodeName)
    {
        return ProgressManager.Instance.CheckIfPlayerCompleted(nodeName);
    }
    
    private void PresentEvidence(string nameOfCurrentFile, string evidenceNeeded){
        InteractionManager.Instance.SetToPresentEvidence(nameOfCurrentFile, evidenceNeeded);
    }
    private void TransitionToNextScene(){
        ProgressManager.Instance.FinishScene(new HashSet<string>());
    }
    private void ShowHealth()
    {
        HealthController.Instance.ShowHealth();
    }
    private void HideHealth()
    {
        HealthController.Instance.HideHealth();
    }

    private bool PlayerLost()
    {
        return HealthController.Instance.getPlayerHealth() <= 0;
    }
    //Should be negative to subtract from current health
    private void UpdateHealth(int healthDelta)
    {
        int currentHealth = HealthController.Instance.getPlayerHealth();
        HealthController.Instance.UpdateHealth(currentHealth + healthDelta);
    }
    private void LoseGame(string filename)
    {
        print("Lost game, running yarn script?");
        loseGame = true;
        loseGameFile = filename;
    }

    
}
