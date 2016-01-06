using UnityEngine;
using System.Collections;

public class TerrainTile : MonoBehaviour {
	public Texture2D EmissionMap;
	private MaterialPropertyBlock m_propBlock;


	private void OnEnable() {
		m_propBlock = new MaterialPropertyBlock();
	}

	private void Update() {
		m_propBlock.SetTexture("_EmissionMap", EmissionMap);

		if (EmissionColorAnimation.Instance != null) {
			m_propBlock.SetColor("_EmissionColor", EmissionColorAnimation.Instance.CurrentEmissionColor);
		} else {
			m_propBlock.SetColor("_EmissionColor", Color.blue);
		}

		GetComponent<MeshRenderer>().SetPropertyBlock(m_propBlock);

	}
}
