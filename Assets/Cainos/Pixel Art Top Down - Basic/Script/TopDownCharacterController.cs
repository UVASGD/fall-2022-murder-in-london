using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;

        // because we are using the same button press for both starting and skipping dialogue they collide
        // so we are going to make it so that the input gets turned off
        public DialogueAdvanceInput dialogueInput;      //note from Jimmy: I made this public so that I could enable it remotely, very hacky but crunch night is tomorrow lol
        private Animator animator;
        private DialogueRunner dialogueRunner;
        public float interactionRadius = 0.75f;
        public int direction_facing = 0;
        public double time = 0;

        private void Start()
        {
            animator = GetComponent<Animator>();
            dialogueInput = FindObjectOfType<DialogueAdvanceInput>();
            dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler<int>("textSpeed", changeTextSpeed);
            dialogueInput.enabled = false;
        }
        
        private void changeTextSpeed(int speed)
        {
            FindObjectOfType<CustomLineView>().typewriterEffectSpeed = speed;
        }


        private void Update()
        {
            // Check for change in dialog speed and change depending on current speed. 
            if (Input.GetKeyUp(KeyCode.K))
            {
                    
                    var cur = FindObjectOfType<CustomLineView>().typewriterEffectSpeed;
                    if (cur == 50)
                        FindObjectOfType<CustomLineView>().typewriterEffectSpeed = 80;
                    else if (cur == 80)
                        FindObjectOfType<CustomLineView>().typewriterEffectSpeed = 110;
                    else
                        FindObjectOfType<CustomLineView>().typewriterEffectSpeed = 50;
                
            }
            // Remove all player control when we're in dialogue
            if (FindObjectOfType<DialogueRunner>().IsDialogueRunning == true || InteractionManager.Instance.GetInteractionState()==InteractionManager.InteractionState.presentEvidence)
            {
                Vector2 dir_dialogue = Vector2.zero;
                dir_dialogue.Normalize();
                animator.SetBool("IsMoving", dir_dialogue.magnitude > 0);

                GetComponent<Rigidbody2D>().velocity = speed * dir_dialogue;

                if(!FindObjectOfType<DialogueRunner>().IsDialogueRunning == true)
                {
                    if (Input.GetKeyUp(KeyCode.Z))
                    {
                        Debug.Log("setting to playermovement");
                        InteractionManager.Instance.SetToPlayerMovement();
                    }
                }
                return;
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
            if(Input.GetKeyUp(KeyCode.E) && InteractionManager.Instance.GetInteractionState() == InteractionManager.InteractionState.playerMove){
                InteractionManager.Instance.SetToViewInventory();
            }
        }
        public void PerformInteraction()
        {

            var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
            //Debug.Log(allParticipants.Count);
            var target = allParticipants.Find(delegate (NPC p)
            {
                Debug.Log(p.characterName);
                Debug.Log(string.IsNullOrEmpty(p.talkToNode) == false);
                Debug.Log((p.transform.position - this.transform.position)// is in range?
                .magnitude);
                Debug.Log((p.transform.position - this.transform.position).magnitude <= interactionRadius);
                Debug.Log(interactionRadius);
                return string.IsNullOrEmpty(p.talkToNode) == false && //has a conversation node?
                (p.transform.position - this.transform.position)// is in range?
                .magnitude <= interactionRadius;
            });

            if (target == null)
            {
                return;
            }


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
            Debug.Log(direction_to_target);
            Debug.Log(raycast_direction);
            Debug.Log(Vector3.Angle(direction_to_target, raycast_direction));

            if (Vector3.Angle(direction_to_target, raycast_direction) > 49)
            {
                return;
            }
            if (target != null)
            {
                CharacterOptionFields targetFields = target.GetComponent<CharacterOptionFields>();

                //if target has no character option fields, then that means that the target is a piece of evidence
                if (targetFields == null)
                {

                }
                else
                {
                    Debug.Log("setting to character Interaction");
                    Debug.Log(InteractionManager.Instance.GetInteractionState());
                    InteractionManager.Instance.SetToCharacterInteraction(targetFields.character_image, targetFields.talkToNode ,targetFields.character_options);
                }
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
                Debug.Log(direction_to_target);
                Debug.Log(raycast_direction);
                Debug.Log(Vector3.Angle(direction_to_target, raycast_direction));

                Debug.Log(target == null);
                if (Vector3.Angle(direction_to_target, raycast_direction) > 49)
                {
                    Debug.Log("angle too small...?");
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
            dialogueInput.enabled = true;
            FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
            yield return new WaitForSeconds(0);
        }
    }
    

}
