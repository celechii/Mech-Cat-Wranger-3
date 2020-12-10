using System.Collections.Generic;
using UnityEngine;

public class MechStatus : MonoBehaviour {

	public int reactorCore = 50;
	public int moveForwards = 15;
	public int moveBackwards = 15;
	public int turnLeft = 15;
	public int turnRight = 15;
	public int crouch = 15;
	public int pspsps = 15;
	public int hold = 15;
	public int soothe = 15;
	public int doorControl = 15;

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
		systems.Add(MechSystem.Soothe, soothe);
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
	Soothe,
	DoorControl
}