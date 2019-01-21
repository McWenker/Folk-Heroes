using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Window_GameResourceBank : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldField;
    [SerializeField] TextMeshProUGUI ironField;
    [SerializeField] TextMeshProUGUI manaField;
    [SerializeField] TextMeshProUGUI bloodField;

    private void Awake()
    {
        GameResourceBank.OnGoldAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResourceType.Gold); };
        GameResourceBank.OnIronAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResourceType.Iron); };
        GameResourceBank.OnManaAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResourceType.Mana); };
        GameResourceBank.OnBloodAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResourceType.Blood); };
        UpdateResourceTextObject();
    }

    private void UpdateResourceTextObject()
    {
        goldField.SetText("GOLD: " + GameResourceBank.GetAmount(GameResourceType.Gold));
        ironField.SetText("IRON: " + GameResourceBank.GetAmount(GameResourceType.Iron));
        manaField.SetText("MANA: " + GameResourceBank.GetAmount(GameResourceType.Mana));
        bloodField.SetText("BLOOD: " + GameResourceBank.GetAmount(GameResourceType.Blood));
    }

    // better to give a resource type
    private void UpdateResourceTextObject(GameResourceType resource)
    {
        switch(resource)
        {
            case (GameResourceType.Gold):
                goldField.SetText("GOLD: " + GameResourceBank.GetAmount(GameResourceType.Gold));
                break;
            case (GameResourceType.Iron):
                ironField.SetText("IRON: " + GameResourceBank.GetAmount(GameResourceType.Iron));
                break;
            case (GameResourceType.Mana):
                manaField.SetText("MANA: " + GameResourceBank.GetAmount(GameResourceType.Mana));
                break;
            case (GameResourceType.Blood):
                bloodField.SetText("BLOOD: " + GameResourceBank.GetAmount(GameResourceType.Blood));
                break;
            default:
                UpdateResourceTextObject();
                break;
        }
    }
}
