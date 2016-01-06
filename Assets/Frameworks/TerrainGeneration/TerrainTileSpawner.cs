using UnityEngine;


public class TerrainTileSpawner : TerrainSpawner {
	public bool HasEmission = true;

	public override void OnSpawn(TerrainGeneration generator) {
		var tile = generator.GetRandomTile();
		var emissionTex = generator.GetRandomTexture();
		var newTile = Instantiate(tile.gameObject, transform.position, Quaternion.identity) as GameObject;
		newTile.transform.localScale = transform.localScale;
		newTile.transform.parent = transform.parent;
		

		var scaleX = Mathf.Abs(transform.localScale.x);
		var scaleY = Mathf.Abs(transform.localScale.y);
		var scaleZ = Mathf.Abs(transform.localScale.z);

		var tileComp = newTile.GetComponent<TerrainTile>();

		if (HasEmission && scaleX < 1.5f && scaleY < 1.5f && scaleZ < 1.5f) {
			tileComp.EmissionMap = emissionTex;

		} else {
			tileComp.EmissionMap = Texture2D.blackTexture;
		}

		DestroyImmediate(gameObject);
	}
}
