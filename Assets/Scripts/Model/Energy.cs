using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] int maxEnergy;
	[SerializeField] BarStat bStat;
    private int energy;
    private SpriteRenderer spriteRenderer;
    private IUnit unit;
    private bool isInvuln;

	public int _Energy
	{
		get { return energy; }
	}

    private void Awake()
    {
        energy = maxEnergy;
        unit = GetComponent<IUnit>();
		if(bStat != null)
		{
			bStat.MaxVal = energy;
			bStat.CurrentVal = energy;
		}
    }

    public void ModifyEnergy(int modifyValue)
    {
        if(!isInvuln)
        {
            energy += modifyValue;
			if(bStat != null) bStat.CurrentVal = energy;
        }
    }
}
