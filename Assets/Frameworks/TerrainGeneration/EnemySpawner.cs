using UnityEngine;
using System.Collections;

public class EnemySpawner : TerrainSpawner {
	private static GameObject[] s_enemyPrefabs;

	private static GameObject[] EnemyPrefabs {
		get {
			if (s_enemyPrefabs == null) {
				s_enemyPrefabs = Resources.LoadAll<GameObject>("Enemies");
			}
			return s_enemyPrefabs;
		}
	}

	public override void OnSpawn(TerrainGeneration generator) {
		var randomEnemy = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)];
		SpawnPrefab(randomEnemy);
		DestroyImmediate(gameObject);
	}
}
