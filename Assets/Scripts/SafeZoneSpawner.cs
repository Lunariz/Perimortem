using UnityEngine;
using System.Collections;
using System;

public class SafeZoneSpawner : TerrainSpawner {

	private static GameObject s_saveZonePrefab;


	private static GameObject SaveZonePrefab {
		get {
			if (s_saveZonePrefab == null) {
				s_saveZonePrefab = Resources.Load<GameObject>("SaveZone");
			}

			return s_saveZonePrefab;
		}
	}
	
	public override void OnSpawn(TerrainGeneration generator) {
		SpawnPrefab(SaveZonePrefab);
		Destroy(gameObject);
	}
}
