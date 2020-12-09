using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CatBrain : MonoBehaviour {

	public RangeFloat waitTime;

	private NavMeshAgent agent;

	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
	}

	private IEnumerator Start() {
		while (true) {
			waitTime.SetValueToRandomFloat();
			Vector3 pos = CatHiveBrain.GetPositionOutside() + Vector3.up;
			agent.destination = pos;
			yield return new WaitUntil(() => agent.remainingDistance < .1f);
			yield return new WaitForSeconds(waitTime);
		}
	}
}