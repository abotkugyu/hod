﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemMenuView : MonoBehaviour {
	
	[SerializeField]
	public GameObject view;
		
	public GameObject cursor;
	private int itemCellWidth = 100;
	private int itemCellHeight = -20;
	
	[SerializeField]
	public List<GameObject> itemCellViewList;

	private void Initialize()
	{	
	}
	
	public void Show ()
	{
		view.SetActive(true);
	}

	public void Hide ()
	{
		view.SetActive(false);
	}

	public bool IsVisible()
	{
		return view.active;
	}

	public void Refresh(List<ItemModel> itemModels)
	{
		Destroy(cursor);
		itemCellViewList.ForEach(Destroy);
		itemCellViewList.Clear();
		GameObject res1 = Resources.Load("Object/Window/_ItemMenu/ItemCell") as GameObject;
		for (int i = 0; i < itemModels.Count; i++)
		{
			GameObject itemCell = Instantiate (res1, view.transform, true);
			itemCell.transform.localPosition = GetItemCellPosition(i);
			itemCellViewList.Add(itemCell);
			
			ItemCellView itemCellView = itemCell.GetComponent<ItemCellView>();
			itemCellView.Name.text = itemModels[i].name;
		}
		
		// cursor
		GameObject res2 = Resources.Load("Object/Window/_ItemMenu/Cursor") as GameObject;		
		cursor = Instantiate (res2, view.transform, true);
		cursor.transform.localPosition = GetCursorPosition(0);	
	}

	public void SetCursor(int seq)
	{
		// 計算上-1しておく
		cursor.transform.localPosition = GetCursorPosition(seq-1);
	}
	
	private Vector2 GetCursorPosition(int seq)
	{	
		return new Vector2(50 ,(Screen.height - 150) / 2 + itemCellHeight * seq  );
	}
	private Vector2 GetItemCellPosition(int seq)
	{	
		return new Vector2(250 / -10 ,(Screen.height - 150) / 2 + itemCellHeight * seq  );
	}
}

