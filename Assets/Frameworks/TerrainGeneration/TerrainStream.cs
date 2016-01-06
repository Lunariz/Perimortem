using UnityEngine;

[RequireComponent(typeof(TerrainGeneration))]
public class TerrainStream : MonoBehaviour {
	public GameObject FirstChunck;

	private Camera m_camera;
	private TerrainGeneration m_generator;
	private TerrainGeneration.GeneratedColumn m_lastColumn;
	private int m_numHorizontalChunks;

	private bool m_firstChunck = true;

	private const int c_saveZoneEvery = 2;
	private int m_needSaveZone = 0;


	private GameObject m_saveZoneTrigger;


	private void OnEnable() {
		m_camera = Camera.main;
		m_generator = GetComponent<TerrainGeneration>();
		m_saveZoneTrigger = Resources.Load<GameObject>("SaveZonePass");
	}

	void Update() {
		TerrainGenerationContext context = new TerrainGenerationContext();


		if (m_firstChunck) {
			m_firstChunck = false;
			m_numHorizontalChunks++;

			context.ExplicitChunck = FirstChunck;
			m_lastColumn = m_generator.GenerateColumn(m_lastColumn, 0, 0, context);
			context.ExplicitChunck = null;
			++m_needSaveZone;
		}

		var viewPoint = new Vector3(1.0f, 0.0f, -m_camera.transform.position.z);
		float maxDist = m_camera.ViewportToWorldPoint(viewPoint).x;
		int maxChunks = Mathf.CeilToInt(maxDist / 20.0f);

		for (; m_numHorizontalChunks < maxChunks; ++m_numHorizontalChunks) {
			if (m_needSaveZone % c_saveZoneEvery == 0) {
				context.SpawnSaveZone = true;
				m_generator.SpawnInChunck(new IntVector3(m_numHorizontalChunks, 0), m_saveZoneTrigger);
			}

			m_lastColumn = m_generator.GenerateColumn(m_lastColumn, m_numHorizontalChunks, 0, context);
			++m_needSaveZone;
		}
	}

}
