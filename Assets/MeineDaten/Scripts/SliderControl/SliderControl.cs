using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SliderControl : MonoBehaviour
{
    public TextMeshProUGUI zahlAufgabe;
    public TextMeshProUGUI anzahlFehler;
    public TextMeshProUGUI nummerDerAufgabe;
    public TextMeshProUGUI maxAnzahlAufgabe;

    public GameObject panelCorrect;
    public GameObject panelWrong;
    public GameObject endPanel;

    public TextMeshProUGUI endOfScaleText;
    public TextMeshProUGUI handleNumber;
    public Slider valueSlider;

    private int aufgabenstellung;
    private int neueAufgabenstellung;
    private int fehlercounter;
    private int aufgabenNr;

    public float feedbackTime;
    public int anzahlAufgaben;
    public int endOfScale;

    public bool isTochpadEnabled;

    private Vector3 lastMouseCoordinate = Vector3.zero;
    // public Slider slider;
    private bool selected = false;

    void Start()
    {
        valueSlider.maxValue = endOfScale;
        valueSlider.value = endOfScale / 2;
        NeueAufgabenstellung();
        fehlercounter = 0;
        aufgabenNr = 1;

        if(isTochpadEnabled == false){
            ColorBlock colorVar = valueSlider.colors;
            colorVar.selectedColor = new Color(1f, 0.8509804f, 0.4f, 1);
            valueSlider.colors = colorVar;
            valueSlider.Select();
        }
    }

    void handleTouchpadInput(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(Input.GetMouseButtonDown(0)){
            valueSlider.Select();
            selected = true;
        }  else if(Input.GetMouseButtonUp(0)) {
            selected = false;
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            Comparision();
        }
        if(mouseDelta.x < 0 ){ // if difference less than zero, moved to left
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            if(selected == true){ // checking if the mouse button is pressed down. 
                valueSlider.value--;
            }
        } else if(mouseDelta.x > 0 ){ // if difference greater than zero, moved to right
            lastMouseCoordinate = Input.mousePosition;
            if(selected == true){
                valueSlider.value++;
            }
        } 
    }
    private void Update()
    {
        zahlAufgabe.text = aufgabenstellung.ToString();
        anzahlFehler.text = fehlercounter.ToString();
        nummerDerAufgabe.text = aufgabenNr.ToString();
        maxAnzahlAufgabe.text = anzahlAufgaben.ToString();

        endOfScaleText.text = endOfScale.ToString();
        handleNumber.text = valueSlider.value.ToString();
        if(isTochpadEnabled == true){
            handleTouchpadInput();
        }
    }

    public void Comparision()
    {
        if (valueSlider.value == aufgabenstellung){
            StartCoroutine(FeedbackCorrect());
            aufgabenNr++;
            if (aufgabenNr >= anzahlAufgaben){
                EndScreen();
            }
            NeueAufgabenstellung();
        } else {
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }

        IEnumerator FeedbackCorrect(){
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(feedbackTime);
            panelCorrect.SetActive(false);
        }

        IEnumerator FeedbackWrong(){
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(feedbackTime);
            panelWrong.SetActive(false);
        }
    }
    public void NeueAufgabenstellung(){
        neueAufgabenstellung = Random.Range(0, endOfScale + 1);
        while (neueAufgabenstellung == aufgabenstellung){
            neueAufgabenstellung = Random.Range(0, endOfScale +1);
        }
        aufgabenstellung = neueAufgabenstellung;
    }

    public void EndScreen(){
        endPanel.SetActive(true);
    }
}
