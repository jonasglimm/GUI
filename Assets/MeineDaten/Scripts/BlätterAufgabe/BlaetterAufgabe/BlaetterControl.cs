using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlaetterControl : MonoBehaviour
{
    private ButtonListBlaettern buttonListBlaettern;
    private ValueControlCenter valueControlCenter;
    private BlaetterRectMovement blaetterRectMovement;
    private AudioSource clickSound;

    // Gameobjects need to be assigned in the inspector in Unity
    public TextMeshProUGUI firstButtonText;

    // Creating gameobjects to run the task and count the amount of wrong actions
    public GameObject nameAufgabe;
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    // Different overlay panels to give visuell feedback
    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endPanel;

    // Private variables to use within the calculations
    private string gesuchteSeite;
    private string neueSeite;
    private int seitenzahl;

    private int fehlercounter;
    private int aufgabenNr;
    private int pagesLength;

    // Active time is the time in sec how long a feedback panel is shown
    private float activeTime;
    // Number of tasks 
    private int anzahlAufgaben;

    private void Awake()
    {
        valueControlCenter = GameObject.Find("BlaetterManager").GetComponent<ValueControlCenter>();
        clickSound = GameObject.Find("BlaetterManager").GetComponent<AudioSource>();
        blaetterRectMovement = GameObject.Find("SnapOnScroll").GetComponent<BlaetterRectMovement>();
        buttonListBlaettern = GameObject.Find("BlaetterManager").GetComponent<ButtonListBlaettern>();

        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
    }

    void Start()
    {
        pagesLength = buttonListBlaettern.pages.Length;
        SetGesuchteSeite();
        // Starting to count mistakes and tasks
        fehlercounter = 0;
        aufgabenNr = 1;

    }

    //The correct text is assigned to the different textelements shown on the Canvas
    private void Update()
    {
        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteSeite;
        anzahlFehler.GetComponent<TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = anzahlAufgaben.ToString();
        if (valueControlCenter.touchpadInput == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Comparision(blaetterRectMovement.buttonText[0]);
                blaetterRectMovement.selectedButton.Select();
                clickSound.Play();
            }
        }
    }

    // Function to be assigned to each button (OnButtonClicked) - compares the name (number) of the button to the current task
    // Feedback is given and either the task counter or the mistake counter is increased
    public void Comparision(TextMeshProUGUI buttonText)
    {
        if (buttonText.text == gesuchteSeite)
        {
            aufgabenNr++;
            StartCoroutine(FeedbackCorrect());
            SetGesuchteSeite();
           
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

            if (aufgabenNr >= anzahlAufgaben) // if task counter reaches the max number of task, the endscreem is called
            {
                EndScreen();
            }
        }

        IEnumerator FeedbackWrong() // Wrong Feedback for the time of activeTime
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelWrong.SetActive(false);
        }
    }

    // functions to select a random page as the new button to press
    public void SetGesuchteSeite()
    {
        gesuchteSeite = neueSeite;
        while (neueSeite == gesuchteSeite) // prevent that the same button (page) need to be selected twice im a row
        {
            seitenzahl = Random.Range(0, pagesLength); 

            if (seitenzahl == pagesLength) // the first Button is not included within the array and needs to be included like this
            {
                neueSeite = firstButtonText.text;
            }
            else
            {
                neueSeite = buttonListBlaettern.pages[seitenzahl];
            }
        }
        gesuchteSeite = neueSeite;
    }

    //activates the endpanel
    public void EndScreen()
    {
        endPanel.SetActive(true);

        if (valueControlCenter.touchpadInput == true)
        {
            blaetterRectMovement.CancelInvoke();
            ShowCursor();
        }
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
    }
}
