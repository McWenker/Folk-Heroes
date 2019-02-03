using UnityEngine;
 
public class Selectable : MonoBehaviour
{
	[SerializeField] SpriteRenderer selectionSprite;
 
    internal bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            selectionSprite.gameObject.SetActive(value);
        }
    }
 
    private bool _isSelected;
 
    void OnEnable()
    {
        RTSSelection.selectables.Add(this);
    }
 
    void OnDisable()
    {
        RTSSelection.selectables.Remove(this);
    }
 
}
