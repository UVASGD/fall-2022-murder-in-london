using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using Yarn.Unity;

public class ProgressManager : MonoBehaviour
{
    #region Singleton
    private static ProgressManager _progressManager;

    public static ProgressManager Instance { get { return _progressManager; } }


    private void Awake()
    {
        if (_progressManager != null && _progressManager != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _progressManager = this;
        }
    }
    #endregion 

    //track progress of player for entire game
    //this should keep track of progress in terms of scenes completed
    public HashSet<string> gameProgressList = new();

    //track progress of player for scene
    //this should contain instances of "evidence discovered" or "interaction found"
    public HashSet<string> sceneProgressList = new(); 

    private HashSet<string> expectedSceneProgressList = new(); //expected progress by the end of the scene

    public CanvasGroup curtainCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        List<string> expectedAchievements = new();
        List<string> currentAchievements = new();
        int current_scene = getInvestigationScene();
        if(current_scene == 1)
        {
            Debug.Log("entering current scene");
            expectedAchievements = GameProgressManager.Scene2Requirements;
            
        }
        else if (current_scene == 2)
        {
            currentAchievements = GameProgressManager.Scene2Requirements;
            expectedAchievements = GameProgressManager.Scene4Requirements;
        }
        else if (current_scene == 3){
            currentAchievements = GameProgressManager.Scene4Requirements;
        }
        else{
            currentAchievements = GameProgressManager.Scene2Requirements;
        }
        foreach (string s in expectedAchievements)
        {
            expectedSceneProgressList.Add(s);
        }
        foreach(string s in currentAchievements)
        {
            sceneProgressList.Add(s);
        }
        foreach (string s in expectedSceneProgressList)
        {
            Debug.Log(s);
        }
        //expectedSceneProgressList.Add("Key");
        //expectedSceneProgressList.Add("Handgun");
    }

    //transition to new scene, need a new set of requirements to meet
    private void SceneTransition(List<string> newExpectedSceneProgressList)
    {
        SetMultipleExpectedProgressList(newExpectedSceneProgressList);
        //empty out current progress
        sceneProgressList.Clear(); 
    }

    public int getInvestigationScene()
    {
        string current_scene = SceneManager.GetActiveScene().name;
        Debug.Log(current_scene);
        if (current_scene == "Investigation_Phase_1")
        {
            return 1;
        }
        else if (current_scene == "Investigation_Phase_2")
        {
            return 2;
        }
        else if (current_scene == "FinalInvestigation"){
            return 3;
        }
        else{
            return 4;
        }
    }
    //set new expected progress list
    public void SetMultipleExpectedProgressList(List<string> newExpectedProgress)
    {
        //set expected requirements to new requirements
        for (int i = 0; i < newExpectedProgress.Count; i++)
        {
            expectedSceneProgressList.Add(newExpectedProgress[i]);
        }
    }
    public void SetSingleExpectedProgressList(List<string> newExpectedProgress)
    {
        //set expected requirements to new requirements
        for (int i = 0; i < newExpectedProgress.Count; i++)
        {
            expectedSceneProgressList.Add(newExpectedProgress[i]);
        }
    }

    //player made progress; track where they are in the scene
    public void ProgressScene(string achievement)
    {
        //is this an expected achievement the player should get?
        if (expectedSceneProgressList.Contains(achievement))
        {
            if (!sceneProgressList.Contains(achievement)){
                sceneProgressList.Add(achievement);
            }
            if (!gameProgressList.Contains(achievement)){
                gameProgressList.Add(achievement);
            }
        }
        else
        {
            throw new NullReferenceException(achievement + " is NOT an achievement");
        }

        foreach (string s in sceneProgressList)
        {
            print(s);
        }
    }

    //same as ProgressScene, but with a list
    //mostly a dev command
    public void MultipleProgressScene(List<string> achievements)
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            //is this an expected achievement the player should get?
            if (expectedSceneProgressList.Contains(achievements[i]))
            {
                sceneProgressList.Add(achievements[i]);
            }
            else
            {
                throw new NullReferenceException(achievements[i] + " is NOT an achievement");
            }
        }
        
    }

    //go back progresses that the player has found
    //mostly a dev command
    public void BacktrackProgressScene(List<string> achievements)
    {
        for(int i = 0; i < achievements.Count; i++)
        {
            if (sceneProgressList.Contains(achievements[i]))
            {
                sceneProgressList.Remove(achievements[i]);
            }
            //if not in sceneProgressList set, do nothing
        }
    }

    //check to see if player has already finished the achievement
    public bool CheckIfPlayerCompleted(string achievement)
    {
        return sceneProgressList.Contains(achievement);
    }

    public void FinishScene(HashSet<string> newExpectedSceneList)
    {
        //transition to next scene once done
        InventoryManager.Instance.updateInventoryDisplay();
        expectedSceneProgressList = newExpectedSceneList;
        StartCoroutine(FadeOutFadeIn());
        
        //move to next scene?
        //SceneTransition(newExpectedSceneList);
    }
    public bool SceneComplete()
    {
        return expectedSceneProgressList.Count == sceneProgressList.Count && expectedSceneProgressList.Count != 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //coroutine that fades outs then fades in
    //used for testing the progress manager
    IEnumerator FadeOutFadeIn()
    {
        Debug.Log("entering: ProgressManager.FadeOutFadeIn()");
        yield return StartCoroutine(Effects.FadeAlpha(curtainCanvasGroup, 0, 1, 0.5f));

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }
}
