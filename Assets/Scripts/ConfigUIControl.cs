using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigUIControl : MonoBehaviour {

	public Color keyTextColour;
	public Color keyBackgroundDefaultColour;
	public Color keyBackgroundBoundColour;
	public Color keyBackgroundBrokenColour;
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
	[Header("//KEYS")]
	public Image[] keyBGs;

	private Canvas canvas;
	private TextMeshProUGUI currentButtonText;
	private string currentKeyName;
	private HashSet<KeyCode> validKeysHash;
	private List<KeyCode> validKeys;
	private List<KeyCode> brokenKeys;

	private void Awake() {
		validKeys = new List<KeyCode>() {
			KeyCode.BackQuote,
				KeyCode.Alpha1,
				KeyCode.Alpha2,
				KeyCode.Alpha3,
				KeyCode.Alpha4,
				KeyCode.Alpha5,
				KeyCode.Alpha6,
				KeyCode.Alpha7,
				KeyCode.Alpha8,
				KeyCode.Alpha9,
				KeyCode.Alpha0,
				KeyCode.Minus,
				KeyCode.Equals,
				KeyCode.Backspace,
				KeyCode.Tab,
				KeyCode.Q,
				KeyCode.W,
				KeyCode.E,
				KeyCode.R,
				KeyCode.T,
				KeyCode.Y,
				KeyCode.U,
				KeyCode.I,
				KeyCode.O,
				KeyCode.P,
				KeyCode.LeftBracket,
				KeyCode.RightBracket,
				KeyCode.Backslash,
				KeyCode.A,
				KeyCode.S,
				KeyCode.D,
				KeyCode.F,
				KeyCode.G,
				KeyCode.H,
				KeyCode.J,
				KeyCode.K,
				KeyCode.L,
				KeyCode.Semicolon,
				KeyCode.Quote,
				KeyCode.Return,
				KeyCode.LeftShift,
				KeyCode.Z,
				KeyCode.X,
				KeyCode.C,
				KeyCode.V,
				KeyCode.B,
				KeyCode.N,
				KeyCode.M,
				KeyCode.Less,
				KeyCode.Greater,
				KeyCode.Slash,
				KeyCode.RightShift,
				KeyCode.Space,
				KeyCode.LeftArrow,
				KeyCode.UpArrow,
				KeyCode.DownArrow,
				KeyCode.RightArrow
		};
		validKeysHash = new HashSet<KeyCode>(validKeys);
		brokenKeys = new List<KeyCode>();
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

		UpdateKeyBGs();
	}

	public void ShowConfig() {
		canvas.enabled = true;
	}

	public void HideConfig() {
		canvas.enabled = false;
	}

	public void BreakKey(KeyCode key) {
		brokenKeys.Add(key);
		validKeysHash.Remove(key);
		UpdateKeyBGs();
	}

	public void RebindControl(string control) {
		StartCoroutine(Rebind(control));
	}

	public void UpdateKeyBGs() {
		for (int i = 0; i < keyBGs.Length; i++) {
			KeyCode key = validKeys[i];
			if (brokenKeys.Contains(key))
				keyBGs[i].color = keyBackgroundBrokenColour;
			else if (InputManager.IsKeyAlreadyBound(key))
				keyBGs[i].color = keyBackgroundBoundColour;
			else
				keyBGs[i].color = keyBackgroundDefaultColour;
		}

		foreach (KeyCode key in brokenKeys)
			keyBGs[validKeys.IndexOf(key)].color = keyBackgroundBrokenColour;

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

		currentButtonText.text = GetText(currentButtonText.gameObject.name, "<size=9><ESC TO CANCEL></size>");
		InputManager.RebindKey(control, UpdateText, false, false, validKeysHash);
		UpdateKeyBGs();
	}

	private void UpdateText() => UpdateText(currentButtonText, currentKeyName);

	private void UpdateText(TextMeshProUGUI text, string name) {
		text.text = GetText(text.gameObject.name, InputManager.GetKeyCodeNiceName(InputManager.GetBinding(name)).ToUpper());
	}

	private string GetText(string name, string input) {
		return $"{name} | {input.Colour(keyTextColour)}";
	}
}