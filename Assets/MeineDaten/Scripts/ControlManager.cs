using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ControlManager : MonoBehaviour
{
    [Header("Task Values")]
    public string[] tasks;

    [Header("GUI Elements")]
    public TextMeshProUGUI taskTextField;
    public TextMeshProUGUI errorCountTextField;
    public TextMeshProUGUI currentTaskTextField;
    public GameObject errorScreen;
    public GameObject successScreen;
    public GameObject completionScreen;
    
    [Header("Task overview")]
    public int totalTasks;
    public float activeTime;
    [Header("Input modality")]
    public bool touchscreenInput;
    public bool touchpadInput;
    public bool iDriveInput;
    public bool gestureInput;
    [Header("Start requirements")]
    public GameObject modalityWarning;
    public  float cursorResetTime;
    [Header("Sounds")]
    public AudioSource clickSound;
    
    private int taskNumber;
    
    private int errors;
    private bool[] modalities = new bool[4];
    
// Use this for initialization
    void Start () {
        CheckModalities();
        errors = 0;
        setRandomName(currentTaskTextField.text);
        taskNumber = 1;
        taskTextField.text = taskNumber.ToString() + "/"+totalTasks.ToString();
        errorCountTextField.text = errors.ToString();
    }

    public void CheckModalities() // Check if exactly one modality is set to active
    {
        modalities[0] = touchscreenInput;
        modalities[1] = touchpadInput;
        modalities[2] = iDriveInput;
        modalities[3] = gestureInput;

        int counter = 0;
        for (int i = 0; i < modalities.Length; i++)
        {
            if (modalities[i] == true)
            {
                counter++;
            }
        }

        if (counter != 1)
        {
            modalityWarning.SetActive(true);
        }
    }

    void setRandomName(string oldText){
        while(currentTaskTextField.text == oldText){
            int randIndex =  Random.Range(1, tasks.Length);
            currentTaskTextField.text = tasks[randIndex-1];
        }
    }
    void updateValues(){
        //show success screen
        StartCoroutine(showSuccessFeedback());
        setRandomName(currentTaskTextField.text);
        taskNumber++;
        taskTextField.text = taskNumber.ToString() + "/"+totalTasks;
    }

     IEnumerator showSuccessFeedback(){
        successScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(activeTime);
        successScreen.SetActive(false);
    }

    IEnumerator showErrorFeedback(){
        errorScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(activeTime);
        errorScreen.SetActive(false);
    }

     public void checkOutput(string inputText){
        clickSound.Play();
        string verificationString = currentTaskTextField.text;
        verificationString = verificationString.ToUpper();
        if(verificationString == inputText){
            updateValues();
            if(taskNumber > totalTasks){
                completionScreen.SetActive(true);
            }
        } else {
            // show error screen
            StartCoroutine(showErrorFeedback());
            errors++;
            errorCountTextField.text = errors.ToString();
        }
     }
}
