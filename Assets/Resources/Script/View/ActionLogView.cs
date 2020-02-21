using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ActionLogView : MonoBehaviour
{
	
    [SerializeField]
    public GameObject view;		
    public Text text;

    private ActionLogView()
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

    public void Refresh(List<String> logs)
    {
        lock(logs){
            text.text = string.Join("\n", logs.ToArray().Reverse());
        }
    }
}
