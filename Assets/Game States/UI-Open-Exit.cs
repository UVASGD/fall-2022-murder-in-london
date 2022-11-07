using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject UIPanel;
    void MaximizeUIPanel()
    {

    }

    void MinimizeUIPanel()
    {
            
    }

    void PauseWhenOpen()
    {
            UIPanel.SetActive(true);
            Time.timeScale = 0f;
    }

    void ResumeWhenClose()
    {
            UIPanel.SetActive(false);
            Time.timeScale = 1f;
    }

}
