using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
public class ScoreManagerUi : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputScore;
    [SerializeField] private TMP_InputField inputName;
    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text , int.Parse(inputScore.text));
        inputName.text = ""; // Input alanlarýný temizle
        inputScore.text = ""; // Input alanlarýný temizle
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
