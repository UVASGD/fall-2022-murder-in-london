using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private AudioSource menuSource = new();
    private Button back;
    public void Start()
    {
        menuSource = GameObject.Find("MenuSound").GetComponent<AudioSource>();
        back = GameObject.Find("Back").GetComponent<Button>();
        back.Select();
    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            back.onClick.Invoke();
        }
    }
    public void Back()
    {
        StartCoroutine(MenuOnClick());
    }
    IEnumerator MenuOnClick()
    {
        menuSource.Play();
        yield return new WaitForSeconds(menuSource.clip.length);
        SceneManager.LoadScene("StartMenu");
    }
}
