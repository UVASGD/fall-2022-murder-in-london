using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Evidence", menuName = "Inventory/Evidence")]
public class Evidence : ScriptableObject
{

	new public string name = "New Evidence";    // Name of the item
	public Sprite icon = null;              // Item icon
	public bool isDefaultItem = false;      // Is the item default wear?

	// Called when the item is pressed in the inventory
	public virtual void Use()
	{
		// Use the item
		// Something might happen

		Debug.Log("Using " + name);
	}

	public void RemoveFromInventory()
	{
		Inventory.instance.Remove(this);
	}

}