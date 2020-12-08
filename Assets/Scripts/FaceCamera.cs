using UnityEngine;

public class FaceCamera : MonoBehaviour {

	public bool matchX = true;
	public bool matchY = true;
	public bool matchZ = true;

	private Transform camTrans;
	private SpriteRenderer spriteRenderer;

	private void Awake() {
		camTrans = Camera.main.transform;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		if (spriteRenderer.isVisible)
			Turn();
	}

	private void Turn() {

		Vector3 camEuler = camTrans.eulerAngles;
		Vector3 selfEuler = transform.eulerAngles;
		transform.localRotation = Quaternion.Euler(
			matchX? camEuler.x : selfEuler.x,
			matchY? camEuler.y : selfEuler.y,
			matchZ? camEuler.z : selfEuler.z
		);
	}
}