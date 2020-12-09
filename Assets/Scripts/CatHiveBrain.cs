using UnityEngine;
using UnityEngine.AI;

public class CatHiveBrain : MonoBehaviour {

	private static CatHiveBrain control;

	public Collider outside;
	public Collider inside;

	private void Awake() {
		control = this;
	}

	public static void GetRandomPosition() {

	}

	public static Vector3 GetPositionOutside() {
		return Utils.Lerp(control.outside.bounds.min, control.outside.bounds.max, new Vector3(Random.value, Random.value, Random.value));
	}

	public static void GetPositionInside() {

	}
}