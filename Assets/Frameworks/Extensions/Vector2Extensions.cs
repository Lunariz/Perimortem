using UnityEngine;
using System.Collections;

public static class Vector2Extensions {

	public static void Rotate(this Vector2 vec, float angle) {

		float angleRad = angle * Mathf.Deg2Rad;

		float cs = Mathf.Cos(angleRad);
		float ss = Mathf.Sin(angleRad);


		float xx = vec.x * cs - vec.y * ss;
		float yy = vec.x * ss + vec.y * cs;
		vec.x = xx;
		vec.y = yy;
	}
}
