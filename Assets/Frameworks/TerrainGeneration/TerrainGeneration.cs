using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct TerrainGenerationContext {
	public GameObject ExplicitChunck;

	public bool SpawnSaveZone;
}


public class TerrainGeneration : MonoBehaviour {
	//Grid weith predefined chunks
	//Divergence is allowed, but limited
	//To either short paths or branches that merge again
	public struct GeneratedColumn {
		public TerrainChunck Chunck1;
		public TerrainChunck Chunck2;
		public TerrainChunck Chunck3;

		public Level ExitLevel;

		public TerrainChunck GetChunckOnlevel(Level level) {

			switch (level) {
				case Level.Top:
					return Chunck1;
				case Level.Mid:
					return Chunck2;
				case Level.Bottom:
					return Chunck3;
			}

			Debug.LogError("Unknown level" + level.ToString());
			return null;
		}

		public TerrainChunck GetExit() {
			return GetChunckOnlevel(ExitLevel);
		}
	}

	//Multiple paths running above each other?
	private TerrainChunck[] m_chuncksTop;
	private TerrainChunck[] m_chuncksMid;
	private TerrainChunck[] m_chuncksBottom;

	private TerrainChunck[] m_chuncksShared;

	private Texture2D[] m_tileTextures;


	private TerrainTile[] m_tileResources;

	public const int ChunkWidth = 30;
	public const int ChunkHeight = 20;


	public enum Level {
		Top,
		Mid,
		Bottom
	}

	private void Awake() {
		m_chuncksTop = LoadChuncksInFolder("Top");
		m_chuncksMid = LoadChuncksInFolder("Mid");
		m_chuncksBottom = LoadChuncksInFolder("Bottom");

		m_chuncksShared = LoadChuncksInFolder("Shared");


		m_tileResources =
			Resources.LoadAll<GameObject>("TerrainTiles").Select(go => go.GetComponent<TerrainTile>()).ToArray();


		m_tileTextures = Resources.LoadAll<Texture2D>("TerrainTextures");

		if (m_chuncksTop.Length == 0) {
			Debug.LogError("Sheesh get to work and make some terrain chunks");
		}
	}

	private static TerrainChunck[] LoadChuncksInFolder(string path) {
		return Resources.LoadAll<GameObject>("TerrainChuncks/" + path).Select(go => go.GetComponent<TerrainChunck>()).ToArray();
	}

	public bool HasRightExit(TerrainChunck chunck) {
		if (chunck == null) {
			return false;
		}

		return chunck.HasRightExit;
	}


	public GeneratedColumn GenerateColumn(GeneratedColumn lastColumn, int column, int difficulty, TerrainGenerationContext context) {
		//How to pass context here?
		TerrainChunck chunck1 = null, chunck2 = null, chunck3 = null;
		var exitLevel = RandomLevel();


		if (context.ExplicitChunck == null) {
			while (!IsValidConfig(chunck1, chunck2, chunck3, lastColumn, exitLevel, context)) {
				chunck1 = PickChunck(difficulty, Level.Top);
				chunck2 = PickChunck(difficulty, Level.Mid);
				chunck3 = PickChunck(difficulty, Level.Bottom);
			}
		} else {
			chunck2 = context.ExplicitChunck.GetComponent<TerrainChunck>();
			exitLevel = Level.Mid;
		}

		var inst1 = InstantiateChunck(new IntVector3(column, 1), chunck1);
		var inst2 = InstantiateChunck(new IntVector3(column, 0), chunck2);
		var inst3 = InstantiateChunck(new IntVector3(column, -1), chunck3);

		return new GeneratedColumn { Chunck1 = inst1, Chunck2 = inst2, Chunck3 = inst3, ExitLevel = exitLevel };
	}

	private Level RandomLevel() {
		return (Level) Random.Range(0, 3);
	}

	private bool IsValidConfig(TerrainChunck chunck1, TerrainChunck chunck2, TerrainChunck chunck3, GeneratedColumn lastColumn, Level newLevel, TerrainGenerationContext context) {
		if (chunck1 == null || chunck2 == null || chunck3 == null) {
			return false;
		}


		GeneratedColumn column = new GeneratedColumn {Chunck1 = chunck1, Chunck2 = chunck2, Chunck3 = chunck3, ExitLevel = newLevel};

		var curLevel = lastColumn.ExitLevel;
		bool thisChunckReachable = lastColumn.GetExit().HasRightExit &&
																		column.GetChunckOnlevel(curLevel).HasLeftExit;
		if (!thisChunckReachable) {
			return false;
		}
		

		var exitChunck = column.GetChunckOnlevel(newLevel);


		if (exitChunck == null) {
			Debug.LogError("Whut");
		}

		if (!exitChunck.HasRightExit) {
			return false;
		}



		if (!RandomBranch(chunck1, exitChunck)) {
			return false;
		}
		if (!RandomBranch(chunck2, exitChunck)) {
			return false;
		}
		if (!RandomBranch(chunck3, exitChunck)) {
			return false;
		}




		bool midToUp = chunck1.HasDownExit && chunck2.HasUpExit;
		bool midToDown = chunck2.HasDownExit && chunck3.HasUpExit;

		int paths = (midToUp ? 1 : 0) + (midToDown ? 1 : 0);
		int pathsRequired = Mathf.Abs((int)curLevel - (int) newLevel);

		if (paths < pathsRequired) {
			return false;
		}

		if (pathsRequired == 1) {
			switch (curLevel) {
				case Level.Top:
					if (!midToUp) {
						return false;
					}
					break;

				case Level.Mid:
					if (newLevel == Level.Top && !midToUp || newLevel == Level.Bottom && !midToDown) {
						return false;
					}
					break;
				case Level.Bottom:
					if (!midToDown) {
						return false;
					}
					break;
			}
		}


		bool anySaveZone = chunck1.HasSaveZone || chunck2.HasSaveZone || chunck3.HasSaveZone;
		
		if (context.SpawnSaveZone) {
			if (!anySaveZone) {
				return false;
			}
		}else if(anySaveZone) {
			return false;
		}

		return true;
	}

	private static bool RandomBranch(TerrainChunck chunck1, TerrainChunck exitChunck) {
		if (chunck1 != exitChunck && exitChunck.HasRightExit && Random.value < 0.2f) {
			return false;
		}

		return true;
	}

	private TerrainChunck PickChunck(int difficulty, Level level) {
		//TODO: Implement some nice logic here
		while (true) {
			TerrainChunck chunck = null;

			if (Random.value < 0.3f) {
				chunck = RandomChunck(m_chuncksShared);
			} else {
				switch (level) {
					case Level.Top:
						chunck = RandomChunck(m_chuncksTop);
						break;

					case Level.Mid:
						chunck = RandomChunck(m_chuncksMid);
						break;

					case Level.Bottom:
						chunck = RandomChunck(m_chuncksBottom);
						break;
				}
			}

			if (chunck.Difficulty > difficulty) {
				continue;
			}

			return chunck;
		}
	}

	private TerrainChunck RandomChunck(TerrainChunck[] chuncks) {
		int index = Random.Range(0, chuncks.Length);
		var chunck = chuncks[index];
		return chunck;
	}

	public void SpawnInChunck(IntVector3 rowPos, GameObject spawn) {
		Vector3 worldPos = RowToWorld(rowPos);

		GameObject dupGo = Instantiate(spawn, worldPos, Quaternion.identity) as GameObject;
		dupGo.transform.parent = transform;
	}

	private TerrainChunck InstantiateChunck(IntVector3 rowPos, TerrainChunck chunck) {
		if (chunck == null) {
			return null;
		}

		Vector3 worldPos = RowToWorld(rowPos);

		GameObject dupGo = Instantiate(chunck.gameObject, worldPos, Quaternion.identity) as GameObject;
		if (dupGo == null) {
			Debug.LogError("Something went horribly wrong in the terrain geenration");
			return null;
		}
		dupGo.transform.parent = transform;

		var dupChunck = dupGo.GetComponent<TerrainChunck>();
		ProcesSpawnersForChunk(dupChunck);

		return dupChunck;
	}

	private static Vector3 RowToWorld(IntVector3 rowPos) {
		Vector3 worldPos;
		worldPos = new Vector3(rowPos.X * (ChunkWidth) + ChunkWidth / 2.0f, rowPos.Y * (ChunkHeight + 1) + ChunkHeight / 2.0f,
			0.0f);
		return worldPos;
	}

	private void ProcesSpawnersForChunk(TerrainChunck dupChunck) {
		var spawners = dupChunck.Spawners;

		foreach (var spawn in spawners) {
			spawn.OnSpawn(this);
		}
	}

	public TerrainTile GetRandomTile() {
		return m_tileResources[Random.Range(0, m_tileResources.Length)];
	}

	public Texture2D GetRandomTexture() {
		return m_tileTextures[Random.Range(0, m_tileTextures.Length)];
	}


}
