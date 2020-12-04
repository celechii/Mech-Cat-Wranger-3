using System.Collections.Generic;
using UnityEngine;

public class MechStatus : MonoBehaviour {

	public int reactorCore;
	public int moveForwards;
	public int moveBackwards;
	public int turnLeft;
	public int turnRight;
	public int crouch;
	public int pspsps;
	public int hold;
	public int doorControl;

	private MechSystem currentSystem;
	private Dictionary<MechSystem, int> systems;

	private void Awake() {
		systems = new Dictionary<MechSystem, int>();
		systems.Add(MechSystem.ReactorCore, reactorCore);
		systems.Add(MechSystem.MoveForwards, moveForwards);
		systems.Add(MechSystem.MoveBackwards, moveBackwards);
		systems.Add(MechSystem.TurnLeft, turnLeft);
		systems.Add(MechSystem.TurnRight, turnRight);
		systems.Add(MechSystem.Crouch, crouch);
		systems.Add(MechSystem.Pspsps, pspsps);
		systems.Add(MechSystem.Hold, hold);
		systems.Add(MechSystem.DoorControl, doorControl);
	}

	public bool IsIntact(MechSystem system) => systems[system] > 0;

	public void SwitchSystem() {
		bool canSwitch = false;
		foreach (KeyValuePair<MechSystem, int> kp in systems) {
			if (kp.Value > 0) {
				canSwitch = true;
				break;
			}
		}
		if (!canSwitch)
			return;

		do {
			currentSystem = (MechSystem)Random.Range(0, System.Enum.GetNames(typeof(MechSystem)).Length);
		} while (systems[currentSystem] != 0);
	}

	public void Damage() {
		systems[currentSystem]--;
		if (systems[currentSystem] == 0) {
			print(currentSystem.MakeEnumReadable() + " is broken!! D:");
		}
	}

}

public enum MechSystem {
	ReactorCore,
	MoveForwards,
	MoveBackwards,
	TurnLeft,
	TurnRight,
	Crouch,
	Pspsps,
	Hold,
	DoorControl
}