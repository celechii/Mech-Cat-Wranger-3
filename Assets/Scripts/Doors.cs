using System.Collections;
using UnityEngine;

public class Doors : MonoBehaviour {

	public float openTime;
	public AnimationCurve openCurve;
	public RangeFloat slideRange;
	public MechStatus mechStatus;
	public Transform leftDoor;
	public Transform rightDoor;

	private bool isOpen;
	private bool shouldBeOpen;
	private Coroutine openCoroutine;

	private void Start() {
		InputManager.AddListener("Toggle Doors", false, InputManager.InputEventType.OnKeyDown, ToggleDoors);
	}

	private void ToggleDoors() {
		if (!mechStatus.IsIntact(MechSystem.DoorControl))
			return;

		shouldBeOpen = !shouldBeOpen;
		if (openCoroutine == null)
			StartCoroutine(OpenDoors(shouldBeOpen));
	}

	private IEnumerator OpenDoors(bool open) {
		for (float elapsed = 0; elapsed < openTime; elapsed += Time.deltaTime) {
			while (WorldControl.IsPaused)
				yield return null;

			float percent = elapsed / openTime;
			if (!open)
				percent = 1 - percent;
			float value = slideRange.GetAt(openCurve.Evaluate(percent));
			leftDoor.localPosition = new Vector3(-value, leftDoor.localPosition.y, leftDoor.localPosition.z);
			rightDoor.localPosition = new Vector3(value, rightDoor.localPosition.y, rightDoor.localPosition.z);

			yield return null;
		}

		leftDoor.localPosition = new Vector3(open ? -slideRange.max : -slideRange.min, leftDoor.localPosition.y, leftDoor.localPosition.z);
		rightDoor.localPosition = new Vector3(open ? slideRange.max : slideRange.min, rightDoor.localPosition.y, rightDoor.localPosition.z);

		if (shouldBeOpen != open)
			StartCoroutine(OpenDoors(shouldBeOpen));
		else
			openCoroutine = null;
	}
}