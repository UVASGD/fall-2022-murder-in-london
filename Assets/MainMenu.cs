using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private AudioSource playSource = new();
    private AudioSource creditsSource = new();
    private int selectedButtonIndex = 0;
    private Button selectedButton;
    private List<Button> optionButtons = new();
    public void Start()
    {
        playSource = GameObject.Find("PlaySound").GetComponent<AudioSource>();
        creditsSource = GameObject.Find("CreditsSound").GetComponent<AudioSource>();
        optionButtons.Add(GameObject.Find("Play").GetComponent<Button>());
        optionButtons.Add(GameObject.Find("Credits").GetComponent<Button>());
        optionButtons.Add(GameObject.Find("Exit").GetComponent<Button>());

        setNewSelectedButton(selectedButtonIndex);

    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            if(selectedButtonIndex > 0)
            {
                setNewSelectedButton(selectedButtonIndex - 1);
            }
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if(selectedButtonIndex < 3)
            {
                setNewSelectedButton(selectedButtonIndex + 1);
            }
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            selectedButton.onClick.Invoke();
        }
    }

    public void setNewSelectedButton(int newSelectedIndex)
    {
        optionButtons[newSelectedIndex].Select();

        selectedButton = optionButtons[newSelectedIndex];
        selectedButtonIndex = newSelectedIndex;
    }
    public void PlayGame()
    {
        
        StartCoroutine(PlayGameOnClick());
    }

    public void RollCredits()
    {

        StartCoroutine(CreditsOnClick());

    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    IEnumerator PlayGameOnClick()
    {
        
        playSource.Play();
        //print(audio.clip.length);
        yield return new WaitForSeconds(playSource.clip.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    IEnumerator CreditsOnClick()
    {
        creditsSource.Play();
        //print(audio.clip.length);
        yield return new WaitForSeconds(creditsSource.clip.length);
        SceneManager.LoadScene("Credits");
    }
}
