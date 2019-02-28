using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BarStat : MonoBehaviour
{
	[SerializeField]
	BarAnim bar;

	[SerializeField]
	float maxVal;

	[SerializeField]
	float currentVal;

	public BarAnim Bar
	{
		set
		{
			this.bar = value;
		}
	}

	public float CurrentVal
	{
		get
		{
			return currentVal;
		}

		set
		{
			this.currentVal = value;
			bar.Value = currentVal;
		}
	}

	public float MaxVal
	{
		get
		{
			return maxVal;
		}

		set
		{
			this.maxVal = value;
			bar.MaxValue = maxVal;
		}
	}
}
