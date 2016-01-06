using UnityEngine;
using System.Collections;

public class ShowTimer : MonoBehaviour {
	private void Update() {
		GetComponent<TextMesh>().text = Mathf.RoundToInt(Timer.Instance.TimeLeft).ToString();
	}
}
