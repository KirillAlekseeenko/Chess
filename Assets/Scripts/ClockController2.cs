using System;
using UnityEngine;
using UnityEngine.UI;

public class ClockController2 : MonoBehaviour 
{
    [SerializeField] private Text clockText;

    DateTime time;

    public bool Stopped { get; set; }

    private void Awake()
    {
        time = new DateTime(1, 1, 1, 1, 20, 0);
        Stopped = true;
    }

    private void Update()
    {
        if(!Stopped)
        {
            time = time.Subtract(new TimeSpan(0, 0, 0, 0, (int)(Time.deltaTime * 1000)));
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        clockText.text = time.Minute + " : " + time.Second;
    }
}