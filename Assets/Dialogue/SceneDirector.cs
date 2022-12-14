using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour
{

    #region Singleton
    private static SceneDirector _sceneDirector;

    public static SceneDirector Instance { get { return _sceneDirector; } }


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
            dialogueRunnerObject.AddCommandHandler<string, string>(
                "requireEvidence",
                PresentEvidence
            );
            dialogueRunnerObject.AddCommandHandler(
                "transitionScene",
                TransitionToNextScene
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
        List<string> requiredAchievements = new();
        requiredAchievements.Add("chest");
        if(ProgressManager.Instance){
            ProgressManager.Instance.SetMultipleExpectedProgressList(requiredAchievements);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if all achievements found, move onto next scen
        if(ProgressManager.Instance){
            if (ProgressManager.Instance.SceneComplete())
            {
                //when dialogue is done, do the scene change
                if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == false)
                {
                    //Debug.Log("changing scene");
                    List<string> nextExpectedAchievements = new();
                    nextExpectedAchievements.Add("chest");
                    ProgressManager.Instance.FinishScene(nextExpectedAchievements);
                }
                
            }
        }
    }


    private void ProgressScene(string achievement)
    {
        ProgressManager.Instance.ProgressScene(achievement);
    }
    private bool CheckIfPlayerCompleted(string nodeName)
    {
        return ProgressManager.Instance.CheckIfPlayerCompleted(nodeName);
    }
    private void PresentEvidence(string nameOfCurrentFile, string evidenceNeeded){
        InteractionManager.Instance.SetToPresentEvidence(nameOfCurrentFile, evidenceNeeded);
    }
    private void TransitionToNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
}
