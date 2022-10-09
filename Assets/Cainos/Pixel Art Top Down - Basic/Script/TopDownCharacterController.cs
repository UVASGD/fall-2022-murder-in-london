using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        //because we are using the same button press for both starting and skipping dialogue they collide
        //so we are going to make it so that the input gets turned off
        private DialogueAdvanceInput dialogueInput;
        private Animator animator;
        public float interactionRadius = 0.75f;
        public int direction_facing = 0;
        public NPC current_interaction = null;


        private void Start()
        {
            animator = GetComponent<Animator>();
            dialogueInput = FindObjectOfType<DialogueAdvanceInput>();
            dialogueInput.enabled = false;
        }


        private void Update()
        {
            // Remove all player control when we're in dialogue
            if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
            {
                Vector2 dir_dialogue = Vector2.zero;
                dir_dialogue.Normalize();
                animator.SetBool("IsMoving", dir_dialogue.magnitude > 0);

                GetComponent<Rigidbody2D>().velocity = speed * dir_dialogue;
                return;
            }

            if (current_interaction != null)
            {
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    print("Z button pressed");
                    current_interaction.leaveInteraction();
                    current_interaction = null;
                }
            }
            // every time we LEAVE dialogue we have to make sure we disable the input again
            if (dialogueInput.enabled)
            {
                dialogueInput.enabled = false;
            }

            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
                direction_facing = 3;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
                direction_facing = 2;
            }

            if (Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
                direction_facing = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
                direction_facing = 0;
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = speed * dir;


            if (Input.GetKeyUp(KeyCode.X))
            {
                PerformInteraction();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                CheckForNearbyNPC();
            }
        }
        public void PerformInteraction()
        {
            RaycastHit2D hit;

            Vector3 raycast_direction = new Vector3(0, 0, 0);

            if (direction_facing == 0)
            {
                raycast_direction = new Vector3(0, -1, 0);
            }
            else if (direction_facing == 1)
            {
                raycast_direction = new Vector3(0, 1, 0);
            }
            else if (direction_facing == 2)
            {
                raycast_direction = new Vector3(1, 0, 0);
            }
            else if (direction_facing == 3)
            {
                raycast_direction = new Vector3(-1, 0, 0);
            }


            Debug.Log("trying to perform action: testing raycast");
            hit = Physics2D.Raycast(this.transform.position, raycast_direction);
            // If the ray hits
            if (hit.collider != null)
            {
                Debug.DrawRay(this.transform.position, raycast_direction * hit.distance, Color.black);
                Debug.Log("raycast hit");
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider);
                }
                NPC interaction = hit.collider.GetComponentInChildren<NPC>();
                if (interaction != null)
                {
                    current_interaction = interaction;
                    interaction.interact();
                }
                else
                {
                    print("interaction is null");
                }
            }
            else
            {
                Debug.DrawRay(this.transform.position, raycast_direction * 3f, Color.yellow);
                Debug.Log("raycast no hit");
            }
        }
        /// <summary>
        /// Find all DialogueParticipants
        /// </summary>
        /// <remarks>
        /// Filter them to those that have a Yarn start node and are in
        /// range; then start a conversation with the first one
        /// </remarks>
        public void CheckForNearbyNPC()
        {
            try
            {
                var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
                //Debug.Log(allParticipants.Count);
                var target = allParticipants.Find(delegate (NPC p)
                {
                    //Debug.Log(p.characterName);
                    //Debug.Log(string.IsNullOrEmpty(p.talkToNode) == false);
                    //Debug.Log((p.transform.position - this.transform.position)// is in range?
                    //.magnitude);
                    //Debug.Log((p.transform.position - this.transform.position).magnitude <= interactionRadius);
                    //Debug.Log(interactionRadius);

                    return string.IsNullOrEmpty(p.talkToNode) == false && //has a conversation node?
                    (p.transform.position - this.transform.position)// is in range?
                    .magnitude <= interactionRadius;
                });


                Vector3 raycast_direction = new Vector3(0, 0, 0);

                if (direction_facing == 0)
                {
                    raycast_direction = new Vector3(0, -1, 0);
                }
                else if (direction_facing == 1)
                {
                    raycast_direction = new Vector3(0, 1, 0);
                }
                else if (direction_facing == 2)
                {
                    raycast_direction = new Vector3(1, 0, 0);
                }
                else if (direction_facing == 3)
                {
                    raycast_direction = new Vector3(-1, 0, 0);
                }

                // Debug.Log(target);
                Vector3 direction_to_target = (target.transform.position - this.transform.position);
                //Debug.Log(direction_to_target);
                //Debug.Log(raycast_direction);
                //Debug.Log(Vector3.Angle(direction_to_target, raycast_direction));

                if (Vector3.Angle(direction_to_target, raycast_direction) > 49)
                {
                    return;
                }
                


                if (target != null)
                {
                    StartCoroutine(ObjectInteraction(target));
                }

                
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log(e);
                Debug.Log("No NPCs found nearby");
            }


        }
        IEnumerator ObjectInteraction(NPC target)
        {
            Debug.Log("entering coroutine");
            target.interact();

            yield return new WaitForSeconds(2);

            dialogueInput.enabled = true;
            FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);

            yield return new WaitForSeconds(0);
        }
    }
    

}
