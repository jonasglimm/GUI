using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class AuswahlControl : MonoBehaviour
{
    // Creating gameobjects to run the task and count the amount of wrong actions
    // Gameobject need to be assigned in the inspector in Unity
    public GameObject zahlAufgabe; 
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    // Different overlay panels to give visuell feedback
    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endNachricht;
    public GameObject endPanel;

    // Private variables to use within the calculations
    private int aufgabenstellung;
    private int neueAufgabenstellung;
    private int fehlercounter;
    private int aufgabenNr;

    private Button[] buttonList;

    // Active time is the time in sec how long a feedback panel is shown
    public float activeTime = 0.5f;
    // Number of tasks 
    public int anzahlAufgaben = 5;
    // Different start set up if the task show be done using direct touch
    public bool directTouchInput = true;
    // If no direct touch is used, startButton is the first button to be highlighted
    public Button startButton;

    void Start()
    {
        aufgabenstellung = Random.Range(1, 7); //Which Button should be pressed?
        // Starting to count mistakes and tasks
        fehlercounter = 0;
        aufgabenNr = 1;

        // if no direct touch is used, the highlighted color is assigned to the selected status and the startButton is selected
        // if (directTouchInput == false) 
        // {
        //     buttonList = FindObjectsOfType<Button>();

        //     for (var i = 0; i < buttonList.Length; i++ )
        //     {
        //         ColorBlock colorVar = buttonList[i].colors;
        //         colorVar.selectedColor = new Color(1f, 0.8509804f, 0.4f, 1);
        //         buttonList[i].colors = colorVar;

        //     }

        //     startButton.Select();
        // }
    }

    //The correct text is assigned to the different textelements shown on the Canvas
    private void Update() 
    {
        zahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenstellung.ToString();
        anzahlFehler.GetComponent<TMPro.TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = anzahlAufgaben.ToString();
    }

    // Function to be assigned to each button (OnButtonClicked) - compares the name (number) of the button to the current task
    // Feedback is given and either the task counter or the mistake counter is increased
    public void Comparision(Button btn)
    {

        if (btn.name == aufgabenstellung.ToString())
        {
            StartCoroutine(FeedbackCorrect());

            aufgabenNr++;
            if(aufgabenNr >= anzahlAufgaben) // if task counter reaches the max number of task, the endscreem is called
            {
                EndScreen();
            }

            neueAufgabenstellung = Random.Range(1, 7);

            while (neueAufgabenstellung == aufgabenstellung)
            {
                neueAufgabenstellung = Random.Range(1, 7);
            }

            aufgabenstellung = neueAufgabenstellung;
        }

        else
        { 
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }

        IEnumerator FeedbackCorrect() // Correct Feedback for the time of activeTime
        {
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelCorrect.SetActive(false);
        }

        IEnumerator FeedbackWrong() // Wrong Feedback for the time of activeTime
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelWrong.SetActive(false);
        }

    }
    //Endscreen to show the endpanel
    public void EndScreen() {
        endPanel.SetActive(true);
        endNachricht.SetActive(true);
    }
}