using System.Collections;
using UnityEngine;

public class DayController : MonoBehaviour
{
    [SerializeField] int startHour;
    [SerializeField] float howLongIsGameMinuteInSeconds;
    [SerializeField] ClockendarController clock;
    [SerializeField] SunController sun;

    int hour;
    int minute;

    bool runClock;

    public int StartHour
    {
        get { return startHour; }
    }

    //kept in 24 hour time
    public int Hour
    {
        get { return hour; }
        private set
        {
            hour = value;
            if(hour >= 24)
            {
                hour = 1;
                PassDay();
            }
        }
    }

    public int Minute
    {
        get { return minute; }
        private set
        {
            minute = value;
            if(minute >= 60)
            {
                ++Hour;
                minute = 0;
            }
        }
    }
    public void PassDay()
    {
        TimeEventManager.DayEnd(this);
    }

    private void Awake()
    {
        hour = startHour;
        sun.SetStart(startHour);
        minute = 0;
        runClock = true;
        clock.UpdateClock(hour, minute);
        sun.UpdateFacing(hour, minute);
        StartCoroutine(RunClock());
    }

    private IEnumerator RunClock()
    {
        while(runClock)
        {
            yield return new WaitForSeconds(howLongIsGameMinuteInSeconds);
            ++Minute;
            clock.UpdateClock(hour, minute);
            sun.UpdateFacing(hour, minute);
        }
    }
}
