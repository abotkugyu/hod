using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogPresenter : MonoBehaviour
{
	
    public ActionLogView actionLogView;
    private List<String> logs;
	
    public void Initialize()
    {
    }
	
    public void ShowItemMenu(bool isOpen)
    {
        if (isOpen)
        {
            Refresh();
            actionLogView.Show();
        }
        else
        {
            actionLogView.Hide();
        }
    }

    public void Refresh()
    {
        actionLogView.Refresh(logs);
    }
    
    public bool GetIsShow()
    {
        return actionLogView.IsVisible();
    }


}
