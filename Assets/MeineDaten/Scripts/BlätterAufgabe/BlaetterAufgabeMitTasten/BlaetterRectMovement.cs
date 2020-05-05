using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic; 
// this class moves the current button into view when the task is done using buttons
// needs tio be assigned to the SnapOnScroll-Gameobject (the gameobject with an scroll rect)
public class BlaetterRectMovement : MonoBehaviour
// not using Start() because array of Buttons needs to be intiated before this SetUp() calls.
//Gets called from ButtonListBlaetternMitTasten - Script
{
    public ScrollRect scrollRect;
    public int startButton = 0;
    public float lerpTime = 0.1f;

    private Button[] buttons;
    private int numberOfButtons;
    private int index;
    private Vector2 position = new Vector2(0.5f, 0.5f);
    private Vector2 positionSetTo = new Vector2(0.5f, 0.5f);
    public Button selectedButton;


    //-------------- PageSelection
    public Sprite unselectedPage;
    public Sprite selectedPage;
    public Transform pageSelectionIcons;

    public bool isTrackpadEnabled;
    private bool _showPageSelection;
    private int _previousPageSelectionIndex;
    // container with Image components - one Image for each page
    private List<Image> _pageSelectionImages;
    private Vector3 lastMouseCoordinate = Vector3.zero;
    private bool swipeInProgress = false;
    public TextMeshProUGUI[] buttonText;
    public float swipeMovementX;

    public float totalSwipeTime;

    public void SetUp()
    {
        buttons = GetComponentsInChildren<Button>();
        numberOfButtons = buttons.Length;
        index = startButton;
        selectedButton = buttons[index];
        buttons[index].Select();

        position.x = ((float)index / (buttons.Length - 1));
        InitPageSelection();
        SetPageSelection(startButton);

    }
    IEnumerator moveLeft(){
        yield return new WaitForSeconds(totalSwipeTime);
        if(index > 0){
            index--; 
            selectedButton = buttons[index];   
        } 
    }

    IEnumerator moveRight(){
        yield return new WaitForSeconds(totalSwipeTime);
        if(index < numberOfButtons-1){
            index++; 
            selectedButton = buttons[index];         
        }
    }
     private void handleSwipe(){
        Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
        if(mouseDelta.x > swipeMovementX){ // if difference less than zero, moved to left
            if(swipeInProgress == false ){ // checking if the swipe gesture that had been started is still in progress or not.         
                swipeInProgress = true;  
                StartCoroutine(moveLeft());         
            }
        } else if(mouseDelta.x < -swipeMovementX){ // if difference greater than zero, moved to right      
            if(swipeInProgress == false ){
                swipeInProgress = true;   
                StartCoroutine(moveRight());  
            }
        } else if(mouseDelta.x == 0) {
            swipeInProgress = false;
        }
        lastMouseCoordinate = Input.mousePosition;
    }

    void Update(){
        if(isTrackpadEnabled == true){
            handleSwipe();
        } else {
            swipeInProgress = false;
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            index = System.Array.IndexOf(buttons, selectedButton);
            Debug.Log(index);
            
        }
        buttonText = selectedButton.GetComponentsInChildren<TextMeshProUGUI>();
        buttons[index].Select();
        position.x = ((float)index / (buttons.Length - 1));  
        positionSetTo.x = Mathf.Lerp(scrollRect.normalizedPosition.x, position.x, Time.deltaTime / lerpTime);
        scrollRect.normalizedPosition = positionSetTo;
        SetPageSelection(index);
       
    }

   
    
    private void InitPageSelection()
    {
        // page selection - only if defined sprites for selection icons
        _showPageSelection = unselectedPage != null && selectedPage != null;
        if (_showPageSelection)
        {
            // also container with selection images must be defined and must have exatly the same amount of items as pages container
            if (pageSelectionIcons == null || pageSelectionIcons.childCount != buttons.Length)
            {
                Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                _showPageSelection = false;
            }
            else
            {
                _previousPageSelectionIndex = -1;
                _pageSelectionImages = new List<Image>();

                // cache all Image components into list
                for (int i = 0; i < pageSelectionIcons.childCount; i++)
                {
                    Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                    if (image == null)
                    {
                        Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
                    }
                    _pageSelectionImages.Add(image);
                }
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPageSelection(int aPageIndex)
    {
        // nothing to change
        if (_previousPageSelectionIndex == aPageIndex)
        {
            return;
        }

        // unselect old
        if (_previousPageSelectionIndex >= 0)
        {
            _pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
            _pageSelectionImages[_previousPageSelectionIndex].SetNativeSize();
        }

        // select new
        _pageSelectionImages[aPageIndex].sprite = selectedPage;
        _pageSelectionImages[aPageIndex].SetNativeSize();

        _previousPageSelectionIndex = aPageIndex;
    }
}