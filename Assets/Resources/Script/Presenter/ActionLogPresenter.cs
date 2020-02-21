using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogPresenter : MonoBehaviour
{
	
    public ActionLogView actionLogView;

    private void Start()
    {        
        if (GetIsShow())
        {
            Refresh();
        }
    }

    public void ShowView(bool isOpen)
    {
        if (isOpen)
        {
            actionLogView.Show();
        }
        else
        {
            actionLogView.Hide();
        }
    }
    public void Refresh()
    {
        actionLogView.Refresh(ActionLogModel.Instance.logs);
    }
    
    public bool GetIsShow()
    {
        return actionLogView.IsVisible();
    }
}
