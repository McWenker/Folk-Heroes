using UnityEngine;

public class SunController : MonoBehaviour
{
    int startHour;
    int totalMinutes;

    [SerializeField] float startRotation;
    [SerializeField] float endRotation;
    [SerializeField] Light sun;
    [SerializeField] Gradient colorOverTime;
    [SerializeField] AnimationCurve sunArc;

    public void SetStart(int hour)
    {
        startHour = hour;
        totalMinutes = (24 - startHour) * 60;
    }

    public void UpdateFacing(int hour, int minute)
    {
        int currentMinutes = ((hour - startHour) * 60) + minute;

        Quaternion target = Quaternion.Euler(0, endRotation, 0);

        sun.transform.localRotation = Quaternion.Lerp(sun.transform.localRotation, target, sunArc.Evaluate((float)currentMinutes/totalMinutes));

        UpdateColor(currentMinutes);
        
    }

    private void UpdateColor(int min)
    {
        sun.color = colorOverTime.Evaluate((float)min/totalMinutes);
        sun.intensity = sunArc.Evaluate((float)min/totalMinutes) + 0.3f;
    }
}
