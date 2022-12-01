using UnityEngine;

/*
 * Code adapted from Brackeys tutorial: https://github.com/Brackeys/RPG-Tutorial
 */

public class InvestigateEvidence : Interactable
{

	public Evidence evidence;   // Item to put in the inventory on pickup

	// When the player interacts with the item
	
	// Pick up the item
	void Examine()
	{
		Debug.Log("checking " + evidence.name);
		Inventory.instance.Add(evidence);
	}

}
