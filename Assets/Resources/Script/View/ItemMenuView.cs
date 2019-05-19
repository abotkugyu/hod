using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuView : MonoBehaviour {
	
	[SerializeField]
	public GameObject item_menu;
	[SerializeField]
	public int item_width = 100;
	[SerializeField]
	public int item_height = 100;
	
	[SerializeField]
	public GameObject item_cell;
	
	public UserItemModel user_item_model = new UserItemModel();

	private void initialize()
	{	
	}
	
	public void show ()
	{
		item_menu.SetActive(true);
		refresh();
	}

	public void hide ()
	{
		item_menu.SetActive(false);
	}

	public bool is_visible()
	{
		return item_menu.active;
	}

	public void refresh()
	{		
		user_item_model.items.Add(new ItemModel(1,"魔法の石"));
		user_item_model.items.Add(new ItemModel(2,"夢のしずく"));

		for (int i = 0; i < user_item_model.items.Count; i++)
		{
			GameObject item_cell = Object.Instantiate(Resources.Load("Object/ItemWindow/ItemCell")) as GameObject;
			item_cell.transform.parent = item_menu.transform;
			item_cell.transform.localPosition = new Vector2(250 / -10 ,(Screen.height - 150) / 2 + item_height * i  );
		}
	}
}

