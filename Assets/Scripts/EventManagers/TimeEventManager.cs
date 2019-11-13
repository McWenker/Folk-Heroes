using UnityEngine;

public class TimeEventManager
{
    // Start is called before the first frame update
    public delegate void DayEvent(Object sender);
    public static event DayEvent OnDayBegin;
    public static event DayEvent OnDayEnd;

    public static void DayBegin(Object sender)
    {
        if(OnDayBegin != null) OnDayBegin(sender);
    }

    public static void DayEnd(Object sender)
    {
        if(OnDayEnd != null) OnDayEnd(sender);
    }
}
