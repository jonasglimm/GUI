using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ScrollRectMovement : MonoBehaviour
// not using Start() because array of Buttons needs to be intiated before this SetUp() calls.
//Gets called from ButtonListControlMitTasten - Script
{
    public ScrollRect scrollRect;
    public int startButton = 0;
    public float lerpTime = 0.1f;

    private Button[] buttons;
    private int numberOfButtons;
    private int index;
    private float verticalPosition;
    public Button selectedButton;
    public TextMeshProUGUI[] buttonText;
    public bool isTrackpadEnabled;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    private float twoFingerScrollMovement = 0.08f;
    private float oneFingerScrollMovement = 2f;
    public void SetUp()
    {
        buttons = GetComponentsInChildren<Button>();
        numberOfButtons = buttons.Length;
        index = startButton;
        buttons[index].Select();
        verticalPosition = 1f - ((float)index / (buttons.Length - 1));

    }

    void Update()
    {
        if(isTrackpadEnabled == true){
            handleTwoFingerScroll();
            handleOneFingerScroll();
            selectedButton = buttons[index];
            selectedButton.Select();
            buttonText = selectedButton.GetComponentsInChildren<TextMeshProUGUI>();
        } else{
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            index = System.Array.IndexOf(buttons, selectedButton);
        }
        verticalPosition = 1f - ((float)index / (buttons.Length - 1));
        scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, verticalPosition, Time.deltaTime / lerpTime);
    }
   

    void handleOneFingerScroll(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(mouseDelta.y > oneFingerScrollMovement){ // if difference less than zero, moved to left
            lastMouseCoordinate = Input.mousePosition; // reseting the last mouse coordinate to the new location
            if(index < numberOfButtons-1){
                index++;
            }
        } else if(mouseDelta.y < -oneFingerScrollMovement ){ // if difference greater than zero, moved to right
            lastMouseCoordinate = Input.mousePosition;
            if(index > 0){
                index--;
            }
        } 
    }
    void handleTwoFingerScroll(){
        if(Input.mouseScrollDelta.y < -twoFingerScrollMovement){
            if(index < numberOfButtons-1){
                index++;
            }
        } else if(Input.mouseScrollDelta.y > twoFingerScrollMovement){
            if(index > 0){
                index--;
            }
        }
    }
}
