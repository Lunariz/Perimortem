using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour {
    public static Player Instance;

    public bool IsSafe = false;
    public bool knockbacked = false;
    private Rigidbody2D body;
    private Movement movement;

	private float footstepTimer = 0;

	// Use this for initialization
	void Start () {
        body = gameObject.GetComponent<Rigidbody2D>();
        movement = gameObject.GetComponent<Movement>();
        Instance = this;
	}

    /// <summary>
    /// Kills the player if he is not in a safe zone
    /// </summary>
    /// <returns>If the player is killed</returns>
    public bool CheckDeath()
    {
        if (!IsSafe)
            SceneManager.LoadScene("MainMenu"); //reset level
        return !IsSafe;
    }

    public void KnockBack(Enemy e)
    {
        movement.horSpeed = e.knockbackDirection.x;
        movement.verSpeed = e.knockbackDirection.y;
        knockbacked = true;
        AudioManager.instance.PlaySound(Audio.Bump);
    }

	/// <summary>
	/// General collision used for enemies and safe zones, NOT platforming
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "TriggerZone") //If collider is SafeZone, otherwise null -> false
		{
			IsSafe = true;
		}
		if (other.tag == "EnemyKnockback") {
			KnockBack(other.GetComponent<Enemy>());
		}
		if (other.tag == "SafeZonePass") {
			LevelController.instance.safeZonePassAmount++;
		}
	}

	/// <summary>
    /// General collision used for enemies and safe zones, NOT platforming
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "TriggerZone") //If collider is SafeZone, otherwise null -> false
        {
            IsSafe = false;
        }
    }

}
