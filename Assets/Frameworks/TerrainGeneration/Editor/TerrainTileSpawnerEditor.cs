using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TerrainTileSpawner))]
public class TerrainTileSpawnerEditor : Editor {
	[DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy | GizmoType.Pickable)]
	private static void DrawGizmos(TerrainTileSpawner spawner, GizmoType type) {


		var parent = spawner.transform.parent;

		if (parent == null) {
			return;
		}


		var pos = spawner.transform.position;

		Gizmos.color = Color.white;


		foreach (Transform otherTile in parent) {
			if (otherTile.gameObject == spawner.gameObject) {
				continue;
			}

			if (CheckOverlap(pos, otherTile)) break;

			foreach (Transform sub in otherTile) {
				if (CheckOverlap(pos, sub)) break;
			}
		}

		Gizmos.matrix = spawner.transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		Gizmos.DrawSphere(Vector3.zero, 0.1f);
		Gizmos.matrix = Matrix4x4.identity;
		Gizmos.color = Color.white;
	}

	private static bool CheckOverlap(Vector3 pos, Transform otherTile) {
		Vector3 dif = pos - otherTile.position;
		var scale = otherTile.localScale;
		if (Mathf.Abs(dif.x) < scale.x / 2.0f + 0.5f &&
			Mathf.Abs(dif.y) < scale.y / 2.0f + 0.5f &&
			Mathf.Abs(dif.z) < scale.z / 2.0f + 0.5f) {
			var collider = otherTile.GetComponent<BoxCollider2D>();

			if (collider != null) {
				Gizmos.color = Color.green;
			} else {
				Gizmos.color = Color.red;
				return true;
			}
		}
		return false;
	}
}
