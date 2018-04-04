using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ClockController : MonoBehaviour {

    [SerializeField] private Text clockText;

    DateTime time;

    public bool Stopped { get; set; }

	private void Awake()
	{
        time = new DateTime(1, 1, 1, 1, 20, 0);
        Stopped = false;
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
