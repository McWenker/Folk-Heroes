using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
	[SerializeField] private AudioSource jukeBox;
	[SerializeField] private IntRange waitTime;
	[SerializeField] private AudioClip[] trackList;
	private int trackNumber;
	bool trackJustEnded;

	private int TrackNumber
	{
		get
		{
			return trackNumber;
		}
		set
		{
			if(value >= trackList.Length)
				trackNumber = 0;
			else
				trackNumber = value;
		}
	}

	private void PlayTrack(AudioClip trackToPlay)
    {
        jukeBox.clip = trackToPlay;
        jukeBox.Play();
		++TrackNumber;
    }

	void Awake()
	{	
		trackNumber = Random.Range(0, trackList.Length);
		PlayTrack(trackList[trackNumber]);
	}

	void Update()
	{
		if(!jukeBox.isPlaying && !trackJustEnded)
		{
			trackJustEnded = true;
			StartCoroutine(PauseThenPlay(waitTime.Random));
		}
	}

	private IEnumerator PauseThenPlay(int timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		trackJustEnded = false;
		PlayTrack(trackList[trackNumber]);
	}

}
