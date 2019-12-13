using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuPresenter : MonoBehaviour {
	
	public ItemMenuView itemMenuView;
	public List<ItemModel> itemsModels;
	private int selectedId = 1;
	
	public void Initialize(List<ItemModel> models)
	{
		itemsModels = models;
	}
	
	public void ShowItemMenu(bool isOpen)
	{
		if (isOpen)
		{
			selectedId = 1;
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
				if (itemsModels.Count <= selectedId)
				{
					selectedId = 1;
				}
				else
				{
					selectedId++;
				}
				itemMenuView.SetCursor(selectedId);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (selectedId == 1)
				{
					selectedId = itemsModels.Count;
				}
				else
				{
					selectedId--;
				}
				itemMenuView.SetCursor(selectedId);
			}
		}
	}
}

