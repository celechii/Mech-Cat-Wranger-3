using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUIControl : MonoBehaviour {

	public Color keyColour;
	[Header("//BUTTONS")]
	public TextMeshProUGUI walkForwards;
	public TextMeshProUGUI walkBackwards;
	public TextMeshProUGUI turnLeft;
	public TextMeshProUGUI turnRight;
	public TextMeshProUGUI crouch;
	public TextMeshProUGUI pspsps;
	public TextMeshProUGUI hold;
	public TextMeshProUGUI soothe;
	public TextMeshProUGUI toggleDoors;

	private Canvas canvas;
	private TextMeshProUGUI currentButtonText;
	private string currentKeyName;
	private HashSet<KeyCode> validKeys;

	private void Awake() {
		validKeys = new HashSet<KeyCode>() {
			KeyCode.BackQuote,
				KeyCode.Minus,
				KeyCode.Equals,
				KeyCode.Backspace,
				KeyCode.Tab,
				KeyCode.LeftBracket,
				KeyCode.RightBracket,
				KeyCode.Backslash,
				KeyCode.Semicolon,
				KeyCode.Quote,
				KeyCode.Return,
				KeyCode.LeftShift,
				KeyCode.Less,
				KeyCode.Greater,
				KeyCode.Slash,
				KeyCode.RightShift,
				KeyCode.Space
		};
		for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++)
			validKeys.Add((KeyCode)i);
		for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
			validKeys.Add((KeyCode)i);
	}

	private void Start() {
		InputManager.RemoveAllBindings();
		UpdateText(walkForwards, "Walk Forwards");
		UpdateText(walkBackwards, "Walk Backwards");
		UpdateText(turnLeft, "Turn Left");
		UpdateText(turnRight, "Turn Right");
		UpdateText(crouch, "Crouch");
		UpdateText(pspsps, "Pspsps");
		UpdateText(hold, "Hold");
		UpdateText(soothe, "Soothe");
		UpdateText(toggleDoors, "Toggle Doors");
	}

	public void ShowConfig() {
		canvas.enabled = true;
	}

	public void HideConfig() {
		canvas.enabled = false;
	}

	public void RebindControl(string control) {
		StartCoroutine(Rebind(control));
	}

	public IEnumerator Rebind(string control) {

		if (InputManager.isRebinding) {
			InputManager.CancelRebinding();
			yield return new WaitUntil(() => !InputManager.isRebinding);
		}

		if (control == "Walk Forwards")
			currentButtonText = walkForwards;
		else if (control == "Walk Backwards")
			currentButtonText = walkBackwards;
		else if (control == "Turn Left")
			currentButtonText = turnLeft;
		else if (control == "Turn Right")
			currentButtonText = turnRight;
		else if (control == "Crouch")
			currentButtonText = crouch;
		else if (control == "Pspsps")
			currentButtonText = pspsps;
		else if (control == "Hold")
			currentButtonText = hold;
		else if (control == "Soothe")
			currentButtonText = soothe;
		else if (control == "Toggle Doors")
			currentButtonText = toggleDoors;
		else
			print($"fuck, lookin for {control}");
		currentKeyName = control;

		currentButtonText.text = GetText(currentButtonText.gameObject.name, "...");
		InputManager.RebindKey(control, UpdateText, false, false, validKeys);
	}

	private void UpdateText() => UpdateText(currentButtonText, currentKeyName);

	private void UpdateText(TextMeshProUGUI text, string name) {
		text.text = GetText(text.gameObject.name, InputManager.GetKeyCodeNiceName(InputManager.GetBinding(name)).ToUpper());
	}

	private string GetText(string name, string input) {
		return $"{name} | {input.Colour(keyColour)}";
	}
}