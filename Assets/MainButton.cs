using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainButton : MonoBehaviour {

	public void StartGame()
	{
        SceneManager.LoadScene(1);
	}
}
