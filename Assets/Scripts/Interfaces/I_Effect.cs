using UnityEngine;

public interface I_Effect 
{
    void DoEffect(Vector3 effectLocation, LayerMask whatIsTarget, float chargePercent);

}
