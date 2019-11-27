using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEMO_PassDay : MonoBehaviour
{
    public void PassDay()
    {
        TimeEventManager.DayEnd(FindObjectOfType<DayController>());

    }
}
