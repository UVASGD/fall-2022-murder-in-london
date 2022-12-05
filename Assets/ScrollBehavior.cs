using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ScrollBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    float canvasBottom = 0f;
    float canvasTop = 0f;
    float canvasHeight = 0f;

    float height = 0f;

    float scrollingOffsetFromScreenEdge = 30;

    float scrollSpeed = 64f;
    private GameObject textContainer;
    void Start()
    {

        RectTransform rt = this.GetComponent<RectTransform>();
        height = rt.rect.height;

        RectTransform canvas = this.GetComponentInParent<RectTransform>();
        canvasHeight = canvas.rect.height;

        Transform t = this.GetComponentInParent<Transform>();
        canvasBottom = t.position.y - height / 2;
        canvasTop = t.position.y + height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //print("W");
            //print(canvasTop);
            //print(this.transform.position.y - height / 2 - scrollingOffsetFromScreenEdge);
            //if(this.transform.position.y - height/2 - scrollingOffsetFromScreenEdge < canvasTop)
                this.transform.position -= new Vector3(0, scrollSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //print("S");
            //print(canvasBottom);
            //print(this.transform.position.y + height / 2 + scrollingOffsetFromScreenEdge);
            //if (this.transform.position.y + height/2 + scrollingOffsetFromScreenEdge < canvasBottom)
                this.transform.position -= new Vector3(0, -scrollSpeed, 0);
        }

    }
}
