﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class ButtonListControl : MonoBehaviour
{
    public GameObject buttonTemplate;

    public string[] names;



    private void Start()
    {
        for (int i = 0; i < names.Length; i++)
        {

            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonListButton>().SetText(names[i]);

            button.transform.SetParent(buttonTemplate.transform.parent, false);

        }



    }
  
}
