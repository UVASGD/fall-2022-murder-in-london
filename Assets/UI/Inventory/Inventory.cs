using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Code adapted from Brackeys tutorial: https://github.com/Brackeys/RPG-Tutorial
 */

public class Inventory : MonoBehaviour
{

	#region Singleton

	public static Inventory instance;

	void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of Inventory found!");
			return;
		}

		instance = this;
	}

	#endregion

	// Callback which is triggered when
	// an item gets added/removed.
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public int space = 20;  // Amount of slots in inventory

	// Current list of items in inventory
	public List<Evidence> evidence_list = new List<Evidence>();

	// Add a new item. If there is enough room we
	// return true. Else we return false.
	public bool Add(Evidence e)
	{
		// Don't do anything if it's a default item
		if (!e.isDefaultItem)
		{
			// Check if out of space
			if (evidence_list.Count >= space)
			{
				Debug.Log("Not enough room.");
				return false;
			}

			evidence_list.Add(e);    // Add item to list

			// Trigger callback
			if (onItemChangedCallback != null)
				onItemChangedCallback.Invoke();
		}

		return true;
	}

	// Remove an item
	public void Remove(Evidence e)
	{
		evidence_list.Remove(e);     // Remove item from list

		// Trigger callback
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}

}