using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	//Timer that goes off every 30 seconds
	//Also beeps with set intervals to alert the timer is running out

	public static Timer Instance;

	public float TimeLeft { get { return 30.0f - m_timePassed; } }


	private float lastTime;
	private int beeps;
	private int musics;
	//private double[] intervals = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 };
	private double[] intervals = new double[8] {10, 20, 25, 27.5, 28.75, 29.25, 29.625, 29.8125};
	//{ 15, 22, 26, 27.25, 27.5, 27.75, 28, 28.125, 28.25, 28.375, 28.5, 28.625, 28.75, 28.875, 29, 29.125, 29.25, 29.375, 29.5, 29.625, 29.75, 29.875 };
	private double[] musicIntervals = {10, 23, 28, 29};

	private float m_timePassed;

	// Use this for initialization
	private void Start() {
		Instance = this;
		lastTime = Time.time;
	}

	// Update is called once per frame
	private void Update() {

		m_timePassed += Time.deltaTime;

		beep(m_timePassed);

		//Timer runs out, death events happen, timer reset
		if (m_timePassed >= 30) {
			Player.Instance.CheckDeath();
			//LevelController.instance.SafeScore();

			beeps = 0;
			musics = 0;
			lastTime = Time.time;
			AudioManager.instance.PlayMusic(Music.Tension1, false);
			AudioManager.instance.PlayMusic(Music.Tension2, false);
            m_timePassed = 0;
		}
	}

	private void beep(float timePassed) {
		for (int i = 0; i < intervals.Length; i++) {
			if (beeps <= i && timePassed >= intervals[i]) {
				//AudioManager.instance.PlaySound(Audio.Beep);
				beeps++;
			}
		}
		for (int i = 0; i < musicIntervals.Length; i++) {
            
			if (musics <= i && timePassed >= musicIntervals[i]) {
                if (i == 0)
                    AudioManager.instance.PlayMusic(i + 1);
                else if (i == 1)
                    AudioManager.instance.PlaySound(Audio.Heartbeat);
                else
                    AudioManager.instance.PlaySound(Audio.Flash);
                musics++;
			}
		}
	}
}
