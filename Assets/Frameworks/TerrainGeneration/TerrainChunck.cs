using UnityEngine;

public class TerrainChunck : MonoBehaviour {
	public TerrainSpawner[] Spawners { get { return GetComponentsInChildren<TerrainSpawner>(); } }
	public bool HasSaveZone { get { return GetComponentInChildren<SafeZoneSpawner>() != null; } }


	public int Difficulty;


	public bool HasUpExit;
	public bool HasRightExit;
	public bool HasDownExit;
	public bool HasLeftExit;
	
	private void OnDrawGizmos() {
		Gizmos.color = Color.gray * 0.7f;
		Gizmos.matrix = transform.localToWorldMatrix;


		float baseX = Mathf.RoundToInt(TerrainGeneration.ChunkWidth / 2.0f) + 0.5f;
		float baseY = Mathf.RoundToInt(TerrainGeneration.ChunkHeight / 2.0f) + 0.5f;

		for (int i = 0; i <= TerrainGeneration.ChunkWidth; ++i) {
			for (int j = 0; j <= TerrainGeneration.ChunkHeight; ++j) {
				Gizmos.DrawLine(new Vector3(-baseX + i, -baseY), new Vector3(-baseX + i, baseY));

				Gizmos.DrawLine(new Vector3(-baseX, -baseY + j), new Vector3(baseX, -baseY + j));

			}
		}

		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.color = Color.white;
	}
}
