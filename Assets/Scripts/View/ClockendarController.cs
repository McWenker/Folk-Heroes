using UnityEngine;
using UnityEngine.UI;

public class ClockendarController : MonoBehaviour
{
    [SerializeField] Color[] seasonColors;
    [SerializeField] Color[] dayColors;
    [SerializeField] Text season;
    [SerializeField] Text dayOfTheWeek;
    [SerializeField] Text date;
    [SerializeField] Text time;
    [SerializeField] Text AMPM;

    string hourString;
    string minString;

    Image clockendarImage;
    [SerializeField] Image calendarImage;
    [SerializeField] Image clockImage;

    string[] seasons = new string[]{"Spring", "Summer", "Autumn", "Winter"};
    string[] days = new string[]{"Montir", "Denst", "Mittwik", "Doorsday", "Freeday", "Satir", "Sonnst"};

    void Awake()
    {
        clockendarImage = GetComponent<Image>();
    }

    public void SetDate(int date, int weekdayIndex, int seasonIndex)
    {
        clockendarImage.color = seasonColors[seasonIndex];
        calendarImage.color = dayColors[weekdayIndex];
        clockImage.color = dayColors[weekdayIndex];
        this.dayOfTheWeek.text = days[weekdayIndex];
        this.season.text = seasons[seasonIndex];
        this.date.text = date.ToString();
    }

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
