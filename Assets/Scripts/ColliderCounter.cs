using UnityEngine;
using System.Collections;

public class ColliderCounter : MonoBehaviour {

	public delegate void TriggerDelegate();
	public TriggerDelegate triggerEnter;
	public TriggerDelegate triggerExit;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Untagged")
            triggerEnter();
	}
	void OnTriggerExit2D(Collider2D other)
	{
        if (other.tag == "Untagged")
		    triggerExit();
	}
}
