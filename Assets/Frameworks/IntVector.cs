using UnityEngine;

public struct IntVector3 {
	public int X;
	public int Y;
	public int Z;


	public static IntVector3 operator +(IntVector3 a, IntVector3 b) {
		a.X += b.X;
		a.Y += b.Y;
		a.Z += b.Z;

		return a;
	}

	public static IntVector3 operator -(IntVector3 a, IntVector3 b) {
		a.X -= b.X;
		a.Y -= b.Y;
		a.Z -= b.Z;

		return a;
	}

	public static implicit operator Vector3(IntVector3 a) {
		return new Vector3(a.X, a.Y, a.Z);
	}

	public static implicit operator IntVector3(Vector3  a) { 
		return new IntVector3((int)a.x, (int)a.y, (int)a.z);
	}

	public IntVector3(int x, int y, int z) {
		X = x;
		Y = y;
		Z = z;
	}

	public IntVector3(int x, int y) {
		X = x;
		Y = y;
		Z = 0;
	}
}
