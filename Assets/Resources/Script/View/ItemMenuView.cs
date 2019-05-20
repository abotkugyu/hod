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
	public List<ItemCellView> itemCellViewList;

	private void initialize()
	{	
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

	public void Refresh(List<ItemModel> itemModels)
	{		
		for (int i = 0; i < itemModels.Count; i++)
		{
			GameObject itemCell = Instantiate ((GameObject)Resources.Load ("Object/ItemWindow/ItemCell"));
			itemCell.transform.parent = item_menu.transform;
			itemCell.transform.localPosition = new Vector2(250 / -10 ,(Screen.height - 150) / 2 + item_height * i  );
			
			ItemCellView itemCellView = itemCell.AddComponent<ItemCellView>();
			itemCellViewList.Add(itemCellView);
			itemCellView.Name.text = itemModels[i].name;
		}
		
	}
}

