using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Evidence", menuName = "Inventory/Evidence")]
public class Evidence : ScriptableObject
{

	new public string name = "New Evidence";    // Name of the item
	public string description = "Description goes here"; // Description of the item
	public Sprite icon = null;              // Item icon
	public bool isDefaultItem = false;      // Is the item default wear?
	public bool discovered = false;			//has the item been discovered so far?

	// Called when the item is pressed in the inventory
	public virtual void Use()
	{
		// Use the item
		// Something might happen

		Debug.Log("Using " + name);
	}

}