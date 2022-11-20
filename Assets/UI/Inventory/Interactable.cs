using UnityEngine;

/*
 * Code adapted from Brackeys tutorial: https://github.com/Brackeys/RPG-Tutorial
 */

public class Interactable : MonoBehaviour
{
    public float interaction_radius = 3f; //how far the player can be for interacting with the item
    public Transform interactionTransform; //transform from where the interact happens

    bool is_focused_on = false; //if the current item is being focused on, not necessarily this will be needed
    Transform player; //player reference

    bool has_interacted = false; //if current item has already been interacted with

    void Start()
    {
        
    }

    //method meant to be overridden
    public virtual void Interact()
    {
        Debug.Log("interacting with: " + transform.name);
    }
    // Update is called once per frame
    void Update()
    {
        if (is_focused_on)
        {
            // If we are close enough
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= interaction_radius)
            {

                if (!has_interacted)
                {
                    //either configure in yarn command or do something here 
                }
                else
                {
                    // Interact with the object
                    Interact();
                    has_interacted = true;
                }
                
            }
        }

    }


    // Draw our radius in the editor
    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, interaction_radius);
    }

}
