using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayController : MonoBehaviour
{
    [SerializeField] int startHour = 6;
    [SerializeField] float howLongIsGameMinuteInSeconds = 1f;
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
        SceneManager.sceneLoaded += AssignValues;
        hour = startHour;
        minute = 0;
    }

    private void AssignValues(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainMenu" && scene.name != "InitScene")
        {
            clock = FindObjectOfType<ClockendarController>();
            sun = FindObjectOfType<SunController>();
            sun.SetStart(startHour);
            runClock = true;
            clock.UpdateClock(hour, minute);
            sun.UpdateFacing(hour, minute);
            StartCoroutine(RunClock());
        }
        
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
