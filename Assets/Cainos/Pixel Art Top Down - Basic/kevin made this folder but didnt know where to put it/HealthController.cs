using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private static HealthController _instance;

    public static HealthController Instance { get { return _instance; } }

    private int playerHealth = 4; // random number corresponding to number of hearts images

    public CanvasGroup allhearts;

    //figure out canvasgroup and fix this owifhiow
    
    [SerializeField] private Image[] hearts;

    private void Awake()
    {
        if (_instance != null && _instance != this  )
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        allhearts = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        playerHealth = 5;
        UpdateHealth(playerHealth);
        HideHealth();
    }

    public void UpdateHealth(int newHealth)
    {
        for (int currentHealth = newHealth; currentHealth < hearts.Length; currentHealth++)
        {
            hearts[currentHealth].color = Color.black;
        }
        
        playerHealth = newHealth;
    }

    public void ShowHealth()  
    {  
        allhearts.alpha = 1;
    }  

    public void HideHealth()  
    {  
        allhearts.alpha = 0;
    }
    public int getPlayerHealth()
    {
        return playerHealth;
    }
}