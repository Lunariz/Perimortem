using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Background : MonoBehaviour {

	GameObject cam;
	float y;


	void Start () 
	{
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		CenterYOnCam ();
	}
	

	void Update ()  {
		if( cam != null) transform.position = new Vector3 (cam.transform.position.x, y, 25.0f);
	}

	public void CenterYOnCam()
	{
		if (cam != null) {
			y = cam.transform.position.y;
		}
	}
}
