using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SliderControl : MonoBehaviour
{
    private ValueControlCenter valueControlCenter;
    private StartSliderTask startSliderTask;
    private AudioSource clickSound;

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

    private int endOfScale;

    private Vector3 lastMouseCoordinate = Vector3.zero;
    // public Slider slider;
    private bool selected = false;

    private void Awake() //intiate all variables which are set in different scripts
    {
        valueControlCenter = GameObject.Find("SliderControl").GetComponent<ValueControlCenter>();
        startSliderTask = GameObject.Find("SliderControl").GetComponent<StartSliderTask>();
        clickSound = GameObject.Find("ClickSound").GetComponent<AudioSource>();

        endOfScale = startSliderTask.endOfScale;
    }

    void Start()
    {
        valueSlider.maxValue = endOfScale;
        valueSlider.value = endOfScale / 2;
        NeueAufgabenstellung();
        fehlercounter = 0;
        aufgabenNr = 1;

        if(valueControlCenter.touchpadInput == false){
            ColorBlock colorVar = valueSlider.colors;
            colorVar.selectedColor = new Color(1f, 0.8509804f, 0.4f, 1);
            valueSlider.colors = colorVar;
            valueSlider.Select();
        }

        if (valueControlCenter.touchpadInput == true) // If the trackpad is used, the cursor will be reset to the middle of the screen each cursorResetTime - seconds
        {
            InvokeRepeating("CursorLock", valueControlCenter.cursorResetTime, valueControlCenter.cursorResetTime);  
        }
    }

    void handleTouchpadInput(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(Input.GetMouseButtonDown(0) && selected == false)
        {
            valueSlider.Select();
            selected = true;
        }  else if(Input.GetMouseButtonDown(0) && selected == true)
        {
            selected = false;
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            Comparision();
            clickSound.Play();
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
        maxAnzahlAufgabe.text = valueControlCenter.numberOfTasks.ToString();

        endOfScaleText.text = endOfScale.ToString();
        handleNumber.text = valueSlider.value.ToString();
        if(valueControlCenter.touchpadInput == true){
            CursorUnlock();
            handleTouchpadInput();
        }
    }

    public void Comparision()
    {
        if (valueSlider.value == aufgabenstellung){
            StartCoroutine(FeedbackCorrect());
            aufgabenNr++;
            if (aufgabenNr >= valueControlCenter.numberOfTasks){
                EndScreen();
            }
            NeueAufgabenstellung();
        } else {
            fehlercounter++;
            StartCoroutine(FeedbackWrong());
        }

        IEnumerator FeedbackCorrect(){
            panelCorrect.SetActive(true);
            yield return new WaitForSecondsRealtime(valueControlCenter.feedbackPanelTime);
            panelCorrect.SetActive(false);
        }

        IEnumerator FeedbackWrong(){
            panelWrong.SetActive(true);
            yield return new WaitForSecondsRealtime(valueControlCenter.feedbackPanelTime);
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

    private void CursorLock() //reset the Cursor by first locking it with this function and unlock it with the next on
    {
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void CursorUnlock()
    {
        if (Cursor.lockState == CursorLockMode.Locked) // If Cursor is Locked, unlock it to reset in the middle of the screen
        {
            Cursor.lockState = CursorLockMode.None;
            lastMouseCoordinate = Input.mousePosition; //prevent the false recognition of cursor reset as a swipe
        }
    }
}
