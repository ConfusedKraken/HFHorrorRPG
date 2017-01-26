﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Serialize this to be preserved through saves
//TODO debug ArrayList not accepting generic types
using UnityEngine.UI;


public class Inventory : MonoBehaviour {
	public ArrayList playerInventory;
	private const int MAX_INV_SIZE = 9;
	public int size = 0;
	public Canvas inventoryUI;
	public Transform[] invSlots;
	private int slotIndex = 0;
	// Use this for initialization
	void Start () {
		inventoryUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
		playerInventory = new ArrayList();
		invSlots = new Transform[MAX_INV_SIZE];
		InitializeGUI (inventoryUI);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.I)){
			inventoryUI.enabled = !inventoryUI.enabled;
		}
	}

	/**
	 * Method to add an item found in game to the inventory 
	 * Inventory must have atleast one free space
	 * @param Item - found item to be added
	 * @returns true if item was added to inventory
	 */
	private bool addItem(Item foundItem){
		if (size < MAX_INV_SIZE) {
			Contains (foundItem);
			InitializeGUI (inventoryUI);
			return true;
		} else {
			Debug.Log ("Inventory is full");
		}
		return false;

	}

	/**
	 * Remove method mainly used when disposing of an item thats is not usefull to the player
	 * @params index - the index of the inventory where the specific item is located
	 */ 
	private void removeItem(int index){
		if (size > 0) {
			playerInventory.RemoveAt(index);
			size--;
		} else {
			Debug.Log ("There is nothing in your inventory to remove");
		}
	}

	/**
	 * Remove method used for crafting and removing duplicates of an item.
	 */
	private Item removeDuplicates(int index){
		Item removingItem = (Item) playerInventory [index];
		if (removingItem.numOfItem > 1) { // remove duplicates
			removingItem.setNumOf (removingItem.numOfItem - 1);
			return removingItem;
		} else { // no more duplicates, remove from inventory
			removeItem (index);
			return removingItem;
		}
	}

	private bool Contains(Item invItem){
		if (size > 0) {
			for (int i = 0; i < size; i++) {
				Item currentItem = (Item)playerInventory [i];
				if (currentItem.itemName.Equals (invItem.itemName)) {
					currentItem.numOfItem++;
					playerInventory [i] = currentItem;
					return true;
				}
			}
		}
		playerInventory.Add (invItem);
		size++;
		return false;
	}

	/**
	 * Method to set the initial Inventory GUI, sets all buttons to display "No Item"
	 * 
	 * Finds all children of the canvas by tag"InvSlot" and goes into their children and
	 * looks for a text component to manipulate
	 * TODO - implement adding and removing items
	 * @params GUI canvas housing buttons
	 */ 
	private void InitializeGUI(Canvas canvas){
		if (size == 0) {
			for (int i = 0; i < canvas.transform.childCount; i++) {
				Transform child = canvas.transform.GetChild (i);
				if (child.tag == "InvSlot") {
					invSlots [slotIndex] = child;
					slotIndex++;
					Transform childsChild = child.transform.GetChild (0);
					Text bText = childsChild.GetComponent<Text> ();
					bText.text = "No Item";

				}
			}
		} else {
			updateInv (size - 1);
		}
		//disable GUI visual
		inventoryUI.enabled = false;
	}

	private void updateInv(int index){
		
		Transform slot = invSlots [index];
		Text bText = slot.GetComponent<Text> ();
		Item invItem = (Item) playerInventory [index];
		bText.text = invItem.itemName;
	}
}