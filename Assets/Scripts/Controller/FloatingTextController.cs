using UnityEngine;

public class FloatingTextController : MonoBehaviour {
    private static FloatingText popupText;
    private static GameObject canvas;

    private void Initialize()
    {
        canvas = GameObject.Find("World Canvas");
        if (!popupText)
            popupText = Resources.Load<FloatingText>("UI/PopupTextParent");
    }

    public void CreateFloatingText(Object sender, string damage)
    {
        Health senderHP = (Health)sender;
        FloatingText instance = Instantiate(popupText);
        instance.Position = new Vector3(senderHP.transform.position.x + Random.Range(-.05f, .05f), senderHP.transform.position.y + Random.Range(0.9f, 1.2f), senderHP.transform.position.z + 0.2f);

        instance.transform.SetParent(canvas.transform, false);
        instance.SetText(damage.ToString());
    }

    private void Awake()
    {
        Initialize();
        AnimationEventManager.OnFloatingText += CreateFloatingText;
    }
}