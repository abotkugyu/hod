using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour {
	
	public GameObject item_menu;
	public UserItemModel user_item_model;

	private void initialize()
	{
		
	}
	
	void Start()
	{
		item_menu = GameObject.Find("ItemMenu");
	}
	public void show ()
	{
		item_menu.SetActive(true);
	}

	public void hide ()
	{
		item_menu.SetActive(false);
	}

	public bool is_visible()
	{
		return item_menu.active;
	}
	
}
