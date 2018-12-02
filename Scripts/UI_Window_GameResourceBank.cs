using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Window_GameResourceBank : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldField;
    [SerializeField] TextMeshProUGUI ironField;
    [SerializeField] TextMeshProUGUI manaField;
    [SerializeField] TextMeshProUGUI stoneField;

    private void Awake()
    {
        GameResourceBank.OnGoldAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResource.Gold); };
        GameResourceBank.OnIronAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResource.Iron); };
        GameResourceBank.OnManaAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResource.Mana); };
        GameResourceBank.OnStoneAmountChanged += delegate (object sender, EventArgs e){ UpdateResourceTextObject(GameResource.Stone); };
        UpdateResourceTextObject();
    }

    private void UpdateResourceTextObject()
    {
        goldField.SetText("GOLD: " + GameResourceBank.GetAmount(GameResource.Gold));
        ironField.SetText("IRON: " + GameResourceBank.GetAmount(GameResource.Iron));
        manaField.SetText("MANA: " + GameResourceBank.GetAmount(GameResource.Mana));
        stoneField.SetText("STONE: " + GameResourceBank.GetAmount(GameResource.Stone));
    }

    // better to give a resource type
    private void UpdateResourceTextObject(GameResource resource)
    {
        switch(resource)
        {
            case (GameResource.Gold):
                goldField.SetText("GOLD: " + GameResourceBank.GetAmount(GameResource.Gold));
                break;
            case (GameResource.Iron):
                ironField.SetText("IRON: " + GameResourceBank.GetAmount(GameResource.Iron));
                break;
            case (GameResource.Mana):
                manaField.SetText("MANA: " + GameResourceBank.GetAmount(GameResource.Mana));
                break;
            case (GameResource.Stone):
                stoneField.SetText("STONE: " + GameResourceBank.GetAmount(GameResource.Stone));
                break;
            default:
                UpdateResourceTextObject();
                break;
        }
    }
}
