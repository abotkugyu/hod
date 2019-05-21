using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemMenuView : MonoBehaviour {
	
	[SerializeField]
	public GameObject itemWindow;
	
	private int item_cell_width = 100;
	private int item_cell_height = -20;
	
	[SerializeField]
	public List<GameObject> itemCellViewList;

	private void initialize()
	{	
	}
	
	public void show ()
	{
		itemWindow.SetActive(true);
	}

	public void hide ()
	{
		itemWindow.SetActive(false);
	}

	public bool is_visible()
	{
		return itemWindow.active;
	}

	public void Refresh(List<ItemModel> itemModels)
	{
		itemCellViewList.ForEach(Destroy);
		itemCellViewList.Clear();
		for (int i = 0; i < itemModels.Count; i++)
		{
			GameObject itemCell = Instantiate (Resources.Load ("Object/ItemWindow/ItemCell"), itemWindow.transform, true) as GameObject;
			itemCell.transform.localPosition = new Vector2(250 / -10 ,(Screen.height - 150) / 2 + item_cell_height * i  );
			itemCellViewList.Add(itemCell);
			
			ItemCellView itemCellView = itemCell.GetComponent<ItemCellView>();
			itemCellView.Name.text = itemModels[i].name;
		}
		
	}
}

