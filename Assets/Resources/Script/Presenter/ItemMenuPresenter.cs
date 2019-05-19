using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuPresenter : MonoBehaviour {
	
	public ItemMenuView itemMenuView;
	public List<ItemModel> itemsModels;
	
	public void Initialize(List<ItemModel> models)
	{
		itemsModels = models;
	}
	
	public void ShowItemMenu(bool is_show)
	{
		if (is_show)
		{
			Refresh();
			itemMenuView.show();
		}
		else
		{
			itemMenuView.hide();
		}
	}

	public void Refresh()
	{
		itemMenuView.Refresh(itemsModels);
	}
    
	public bool GetIsShowItemMenu()
	{
		return itemMenuView.is_visible();
	}

	void Update ()
	{
		if (GetIsShowItemMenu())
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
			}
		}
	}
}

