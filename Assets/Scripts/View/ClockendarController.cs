using UnityEngine;
using UnityEngine.UI;

public class ClockendarController : MonoBehaviour
{
    [SerializeField] Text season;
    [SerializeField] Text date;
    [SerializeField] Text time;
    [SerializeField] Text AMPM;

    string hourString;
    string minString;

    // converts from 24 hour to 12
    public void UpdateClock(int hour, int minute)
    {
        if(hour >= 12)
        {
            AMPM.text = "PM";
            if(hour > 12)
            {
                hourString = (hour - 12).ToString();
            }
            else
                hourString = "12";
        }
        else
        {
            AMPM.text = "AM";
            hourString = hour.ToString();
        }

        if(minute >= 10)
            minString = minute.ToString();
        else
            minString = ("0" + minute.ToString());

        time.text = (hourString + ":" + minString);
    }
}
