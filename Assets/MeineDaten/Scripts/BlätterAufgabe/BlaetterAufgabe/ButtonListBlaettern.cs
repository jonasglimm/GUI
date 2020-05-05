using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class ButtonListBlaettern : MonoBehaviour
{
    // first button to set a template (needs to be assigned)
    public GameObject firstButton;
    public string firstButtonText; // because the first Button is used, the text of the first button can be changed

    // Array to set the amount of pages (+ first Button) and name each created Button
    public string[] pages;


    private void Start()
    {
       
        // Intiating a button for each element in the pages array
        for (int i = 0; i < pages.Length; i++)
        {
            GameObject button = Instantiate(firstButton) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonListButtonBlaettern>().SetText(pages[i]);

            button.transform.SetParent(firstButton.transform.parent, false);

        }
        // Assigning the changable text of the first button
        firstButton.GetComponent<ButtonListButtonBlaettern>().SetText(firstButtonText);

    }

}
