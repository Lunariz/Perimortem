using UnityEngine;
using System.Collections;

public class SetDim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Shader.SetGlobalFloat("_Dim", 1.0f);


	}

}
