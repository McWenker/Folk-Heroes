using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] protected int minValue;
    [SerializeField] protected int maxValue;
    protected int currentValue;

    protected bool canChange = true;
    protected bool canRegenerate;
    protected bool isRegenerating;
    protected float regenRate;

    public int Value
    {
        get { return currentValue; }
    }

    protected virtual void Increase(int valueToIncrease)
    {
        if(canChange)
            currentValue = (int)Mathf.Clamp(currentValue + valueToIncrease, minValue, maxValue);
    }

    protected virtual void Decrease(int valueToDecrease)
    {
        if(canChange)
            currentValue = (int)Mathf.Clamp(currentValue - valueToDecrease, minValue, maxValue);
    }    

    protected virtual void Regenerate()
    {
        if(canChange)
            Increase((int)(Time.fixedDeltaTime * regenRate));
    }

    protected virtual void FixedUpdate()
    {
        if(canRegenerate && isRegenerating)
        {
            Regenerate();
        }
    }
    public virtual void Increase(Transform sender, int valueToIncrease)
    {
        if(sender == transform)
            Increase(valueToIncrease);
    }

    public virtual void Decrease(Transform sender, int valueToDecrease)
    {
        if(sender == transform)
            Decrease(valueToDecrease);
    }

    public virtual void Set(Transform sender, int valueToSet)
    {
        if(sender == transform && canChange)
            currentValue = (int)Mathf.Clamp(valueToSet, 0, maxValue);
    }

    public virtual void LockOrUnlock(Transform sender, bool toLock)
    {
        if(sender == transform)
            canChange = !toLock;
    }
}
