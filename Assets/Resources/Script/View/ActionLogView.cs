using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ActionLogView : MonoBehaviour
{
    [SerializeField]
    public GameObject view;		
    public Text text;


    public void Initialize()
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
        return text.IsActive();
    }

    public void Refresh(List<String> logs)
    {
        text.text = string.Join("\n", logs.ToArray().Reverse());  
    }
}
