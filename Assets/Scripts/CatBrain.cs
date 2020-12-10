using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CatBrain : MonoBehaviour {

	public RangeFloat waitTime;
	public float waitForMechTime;
	public AnimationCurve attackProbability;
	public RangeFloat attackFrequency;
	public RangeFloat sootheDuration;
	public bool inBase;

	private CatState state = CatState.Nyoom;
	private Transform mechTrans;
	private MechStatus mechStatus;
	private NavMeshAgent agent;
	private bool switchState;
	private float timeHeld;
	private bool isBeingHeld;

	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
		mechTrans = GameObject.FindWithTag("Player").transform;
		mechStatus = mechTrans.GetComponent<MechStatus>();
	}

	private IEnumerator Start() {
		while (true) {

			if (state == CatState.Nyoom) {
				waitTime.SetValueToRandomFloat();
				Vector3 pos = CatHiveBrain.GetPositionOutside() + Vector3.up;
				agent.destination = pos;
				yield return StartCoroutine(InteruptableWait(() => agent.remainingDistance <= agent.stoppingDistance, CatState.WalkToMech));
				yield return StartCoroutine(InteruptableWait(waitTime, CatState.WalkToMech));

			} else if (state == CatState.WalkToMech) {
				agent.destination = mechTrans.localPosition + Vector3.up;
				while (agent.remainingDistance > agent.stoppingDistance)
					yield return null;
				yield return StartCoroutine(InteruptableWait(waitForMechTime, CatState.PickedUp));

				if (state != CatState.PickedUp)
					state = CatState.Nyoom;

			} else if (state == CatState.PickedUp) {
				timeHeld += Time.deltaTime;
				if (UnityEngine.Random.value <= attackProbability.Evaluate(timeHeld))
					state = CatState.Attwwackking;
				yield return null;

			} else if (state == CatState.Attwwackking) {
				Attack();
				attackFrequency.SetValueToRandomFloat();
				yield return StartCoroutine(InteruptableWait(attackFrequency, CatState.Soothed | CatState.Nyoom));

			} else if (state == CatState.Soothed) {
				sootheDuration.SetValueToRandomFloat();
				yield return StartCoroutine(InteruptableWait(sootheDuration, CatState.Nyoom));
				if (state == CatState.Soothed)
					state = CatState.PickedUp;
			}

			yield return null;
		}
	}

	private void Attack() {
		mechStatus.Damage();
	}

	public void Soothe() {
		if (state == CatState.Attwwackking)
			state = CatState.Soothed;
	}

	public void PickUp() {
		isBeingHeld = true;
		state = CatState.PickedUp;
		mechStatus.SwitchSystem();
		timeHeld = 0;
	}

	public void LetDown() {
		if (isBeingHeld) {
			state = CatState.Nyoom;
			isBeingHeld = false;
		}
	}

	private IEnumerator InteruptableWait(Func<bool> predicate, CatState interuptState) {
		while (!predicate() && !interuptState.HasFlag(state))
			yield return null;
	}

	private IEnumerator InteruptableWait(float duration, CatState interuptState) {
		for (float elapsed = 0; elapsed < duration && !interuptState.HasFlag(state); elapsed += Time.deltaTime)
			yield return null;
	}

	[System.Flags]
	public enum CatState {
		Nyoom = 1 << 0,
		WalkToMech = 1 << 1,
		PickedUp = 1 << 2,
		Attwwackking = 1 << 3,
		Soothed = 1 << 4
	}
}