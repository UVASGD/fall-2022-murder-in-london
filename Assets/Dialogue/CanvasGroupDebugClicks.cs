using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yarn.Unity;

public class CanvasGroupDebugClicks : MonoBehaviour
{
    // Start is called before the first frame update
    GraphicRaycaster raycaster;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);
                string name = result.gameObject.name;
                if(name == "NameText" || name == "NamePanel" || name == "DialogText" || name == "ContinuePrompt")
                {
                    CustomLineView lv = GetComponentInChildren<CustomLineView>();
                    lv.UserRequestedViewAdvancement();
                }
                break;
            }

            
        }
    }
    private void Awake()
    {
        this.raycaster =  GetComponent<GraphicRaycaster>();
    }
}