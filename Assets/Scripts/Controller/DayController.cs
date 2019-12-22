using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayController : MonoBehaviour
{
    [SerializeField] int startDate = 5;
    [SerializeField] int daysInSeason = 30;
    [SerializeField] int startHour = 6;
    [SerializeField] float howLongIsGameMinuteInSeconds = 1f;
    [SerializeField] ClockendarController clock;
    [SerializeField] SunController sun;
    int seasonIndex;
    int weekdayIndex;
    int date;
    int hour;
    int minute;
    bool runClock;

    public int SeasonIndex
    {
        get { return seasonIndex; }
        private set
        {
            seasonIndex = value;
            if(seasonIndex > 3)
            {
                seasonIndex = 0;
            }
        }
    }

    public int WeekdayIndex
    {
        get { return weekdayIndex; }
        private set
        {
            weekdayIndex = value;
            if(weekdayIndex > 6)
            {
                weekdayIndex = 0;
            }
        }
    }

    public int Date
    {
        get { return date; }
        private set
        {
            date = value;
            if(date > daysInSeason)
            {
                ++SeasonIndex;
                date = 1;
            }
        }
    }

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
    void PassDay(Object sender)
    {
        ++Date;
        ++WeekdayIndex;
        hour = startHour;
        minute = 0;
        clock.SetDate(date, weekdayIndex, seasonIndex);
        clock.UpdateClock(hour, minute);
        sun.UpdateFacing(hour, minute);
    }

    private void Awake()
    {
        GameplayEventManager.OnSceneChange += SceneChange;
        SceneManager.sceneLoaded += AssignValues;
        TimeEventManager.OnDayEnd += PassDay;
        date = startDate;
        hour = startHour;
        minute = 0;
    }

    private void SceneChange(Object sender)
    {
        runClock = false;
    }

    private void AssignValues(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainMenu" && scene.name != "InitScene")
        {
            clock = FindObjectOfType<ClockendarController>();
            clock.SetDate(date, weekdayIndex, seasonIndex);
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
