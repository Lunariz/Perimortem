using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
    public static LevelController instance;

    public int difficulty = 0;
    public int score = 0;
    public int safeZonePassAmount = 0;


	// Use this for initialization
	void Start () {
	    instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Player.Instance.CheckDeath();
		}
	}



	/// <summary>
	/// Dim between 0% and 75%
	/// </summary>
	/// <returns></returns>
	public float dimPercent() {
		if (Timer.Instance.TimeLeft > 5)
			return 0f;
		return 1.0f - (Timer.Instance.TimeLeft * 20f / 100f);
	}

	//reseting level in case of death or manual reset
	void resetLevel()
    {

    }

    public void SafeScore()
    {
        AddScore(10 * (int) Mathf.Pow(2, safeZonePassAmount - 1));
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddDifficulty(int amount)
    {
        difficulty += amount;
    }
}
