using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NamePanelControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TextMeshProUGUI text = this.gameObject.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>();
        GameObject namePanel = this.gameObject.transform.GetChild(2).gameObject;
        GameObject nameText = namePanel.transform.GetChild(0).gameObject;
        //print(namePanel);
        TextMeshProUGUI text = nameText.GetComponent<TextMeshProUGUI>();
        string s = "";
        if (text != null)
        {
            s = text.text;
        }
        // Debug.Log(s);
        if (s != null)
        {
            namePanel.gameObject.SetActive(true);

        }
        else
        {
            namePanel.gameObject.SetActive(false);
            // this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
