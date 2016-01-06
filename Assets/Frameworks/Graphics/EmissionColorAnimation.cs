using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EmissionColorAnimation : MonoBehaviour {

	public static EmissionColorAnimation Instance;

	public Gradient EmissionOverTimeGradient;
	public float Duration;
	public float EmissionScale;

	private Color m_emissionColor;

	public Color CurrentEmissionColor { get { return m_emissionColor; } }

	public Color TopColor;
	public Color MidColor;
	public Color BottomColor;

	public Color BaseColor;

	public Color AmbientColor;

	public float dim = 0.0f;


	private void Awake() {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		Shader.SetGlobalColor("_BckTopColor", TopColor);
		Shader.SetGlobalColor("_BckMidColor", MidColor);
		Shader.SetGlobalColor("_BckBottomColor", BottomColor);
		Shader.SetGlobalColor("_BckBaseColor", BaseColor);

		//Get from level controller

		if (LevelController.instance != null) {
			dim = LevelController.instance.dimPercent();
		}


		Shader.SetGlobalFloat("_Dim", 1.0f - dim);
		RenderSettings.ambientLight = Mathf.Lerp(0.1f, 0.0f, dim) * AmbientColor;

		m_emissionColor = EmissionOverTimeGradient.Evaluate(Mathf.Repeat(Time.time / Duration, 1.0f)) * EmissionScale *
				(1.0f - dim);
	}
}
