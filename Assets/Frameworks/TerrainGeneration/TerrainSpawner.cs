using UnityEngine;

abstract public class TerrainSpawner : MonoBehaviour {
	abstract public void OnSpawn(TerrainGeneration generator);

	protected void SpawnPrefab(GameObject randomEnemy) {
		var inst = Instantiate(randomEnemy, transform.position, Quaternion.identity) as GameObject;
		inst.transform.localScale = transform.localScale;
		inst.transform.parent = transform.parent;
	}
}
