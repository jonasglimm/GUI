using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollTaskControl : MonoBehaviour
{
    public ButtonListControl buttonListControl;
    private ScrollRectMovement scrollRectMovement;
    private ValueControlCenter valueControlCenter;

    public GameObject nameAufgabe;
    public GameObject anzahlFehler;
    public GameObject nummerDerAufgabe;
    public GameObject maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endNachricht;
    public GameObject endPanel;

    public AudioSource clickSound;

    private string gesuchterName;
    private string neuerName;

    private int fehlercounter;
    private int aufgabenNr;
    private int namesLength; 

    private float activeTime;
    private int anzahlAufgaben;

    private void Awake()
    {
        valueControlCenter = GameObject.Find("ScrollManager").GetComponent<ValueControlCenter>();
        scrollRectMovement = GameObject.Find("ButtonScrollList").GetComponent<ScrollRectMovement>();
    }

    void Start()
    {
        activeTime = valueControlCenter.feedbackPanelTime;
        anzahlAufgaben = valueControlCenter.numberOfTasks;
        namesLength = buttonListControl.names.Length;
        gesuchterName = buttonListControl.names[Random.Range(0, namesLength)];
        fehlercounter = 0;
        aufgabenNr = 1;
    }

    private void Update()
    {
        nameAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = gesuchterName;
        anzahlFehler.GetComponent<TMPro.TextMeshProUGUI>().text = fehlercounter.ToString();
        nummerDerAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = aufgabenNr.ToString();
        maxAnzahlAufgabe.GetComponent<TMPro.TextMeshProUGUI>().text = anzahlAufgaben.ToString();

        if (valueControlCenter.touchpadInput == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Comparision(scrollRectMovement.buttonText[0]);
                scrollRectMovement.selectedButton.Select();
                clickSound.Play();
            }
        }
    }

    public void Comparision(TextMeshProUGUI buttonText)
    {
        if (buttonText.text == gesuchterName)
        {
            aufgabenNr++;
            StartCoroutine(FeedbackCorrect());

            neuerName = buttonListControl.names[Random.Range(0, namesLength)];

            while (neuerName == gesuchterName)
            {
                neuerName = buttonListControl.names[Random.Range(0, namesLength)];
            }

            gesuchterName = neuerName;
        }

        else
        {
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }

        IEnumerator FeedbackCorrect()
        {
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelCorrect.SetActive(false);

            if (aufgabenNr >= anzahlAufgaben)
            {
                EndScreen();
            }
        }

        IEnumerator FeedbackWrong()
        {
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(activeTime);
            panelWrong.SetActive(false);
        }

    }
    public void EndScreen()
    {
        endPanel.SetActive(true);
        endNachricht.SetActive(true);
    }

}
