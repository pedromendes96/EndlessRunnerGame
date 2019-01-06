using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float minutes = 0f;
    private float seconds = 0f;
    private float milliseconds = 0f;
    private string minutesS = "";
    private string secondsS = "";
    private string millisecondsS = "";

    public Player.Player Player;

	public Text textLabel;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsPlaying)
        {
            if (milliseconds >= 100)
            {
                if (seconds >= 59)
                {
                    minutes++;
                    seconds = 0;
                }
                else if (seconds < 59)
                {
                    seconds++;
                }
                milliseconds = 0;
            }
            milliseconds += Time.deltaTime * 100;
            if (minutes < 10)
            {
                minutesS = "0" +minutes;
            }
            else
            {
                minutesS = "" +minutes;
            }

            if (seconds < 10)
            {
                secondsS = "0" +seconds;
            }
            else
            {
                secondsS = "" +seconds;
            }

            if ((int)milliseconds < 10)
            {
                millisecondsS = "0" +(int)milliseconds;
            }
            else
            {
                millisecondsS = "" +(int)milliseconds;
            }
            textLabel.text = string.Format("{0}:{1}:{2}", minutesS, secondsS, millisecondsS);   
        }
    }
}
