using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MapTaskControl : MonoBehaviour
{ 

    public GameObject pinNorth;
    public GameObject pinSouth;
    public GameObject pinWest;
    public GameObject pinEast;

    public GameObject pointer;

    public GameObject nameAufgabe;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject endPanel;

    private bool taskNorth;
    private bool taskSouth;
    private bool taskEast;
    private bool taskWest;

    private string gesuchteSeite;

    private int aufgabenNr;
    private string gesuchteMarkierung;

    public float activeTime = 0.5f;
    public int anzahlAufgaben = 4;


    void Start()
    {
        pinNorth.SetActive(true);
        pinSouth.SetActive(false);
        pinWest.SetActive(false);
        pinEast.SetActive(false);

        taskNorth = true;
        taskSouth = taskEast = taskWest = false;
        aufgabenNr = 1;
        gesuchteMarkierung = "Markierung im Norden!";

    }

    void Update()
    {

        nameAufgabe.GetComponent<TextMeshProUGUI>().text = gesuchteMarkierung;
        nummerDerAufgabe.GetComponent<TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TextMeshProUGUI>().text = anzahlAufgaben.ToString();


 
    
        if (taskNorth == true && pinNorth.GetComponent<PinNorthCollider>().pinNorthEntered == true)
        {
            StartCoroutine(FeedbackCorrect());
            taskNorth = false;
            pinNorth.SetActive(false);
            taskEast = true;
            pinEast.SetActive(true);

            gesuchteMarkierung = "Markierung im Osten!";
            aufgabenNr++;
        }

        if (taskEast == true && pinEast.GetComponent<PinEastCollider>().pinEastEntered == true)
        {
            StartCoroutine(FeedbackCorrect());
            taskEast = false;
            pinEast.SetActive(false);
            taskWest = true;
            pinWest.SetActive(true);

            gesuchteMarkierung = "Markierung im Westen!";
            aufgabenNr++;

        }

        if (taskWest == true && pinWest.GetComponent<PinWestCollider>().pinWestEntered == true)
        {
            StartCoroutine(FeedbackCorrect());
            taskWest = false;
            pinWest.SetActive(false);
            taskSouth = true;
            pinSouth.SetActive(true);

            gesuchteMarkierung = "Markierung im Süden!";
            aufgabenNr++;
        }

        if (taskSouth == true && pinSouth.GetComponent<PinSouthCollider>().pinSouthEntered == true)
        {
           StartCoroutine(FeedbackCorrect());
            taskSouth = false;
            pinSouth.SetActive(false);

            EndScreen();
        }
    }


    IEnumerator FeedbackCorrect()
    {
        panelCorrect.SetActive(true);
        yield return new WaitForSecondsRealtime(activeTime);
        panelCorrect.SetActive(false);
    } 

    public void EndScreen()
    {
        pointer.SetActive(false);
        endPanel.SetActive(true);
    }
}
