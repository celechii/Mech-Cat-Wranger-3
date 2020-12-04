using System.Collections;
using UnityEngine;

public class WorldControl : MonoBehaviour {

	private static WorldControl control;

	private static bool isPaused;
	public static bool IsPaused {
		get => isPaused;
		set {
			if (value)
				Pause();
			else
				Resume();
		}
	}

	private void Awake() {
		control = this;
	}

	private void Start() {
		SaveSystem.Load();
	}

	private void OnApplicationQuit() {
		SaveSystem.Save();
	}

	public static void Pause() {
		isPaused = true;
		Time.timeScale = 0;
	}

	public static void Resume() {
		isPaused = false;
		Time.timeScale = 1;
	}

	public static IEnumerator PauseableWait(float duration) {
		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {
			while (isPaused)
				yield return null;
			yield return null;
		}
	}
}