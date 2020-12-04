using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Noé Animation/New Animation", fileName = "New Animation")]
public class NAnimation : ScriptableObject {
	public new string name;
	[AllowNesting]
	[MinValue(0)]
	public int framerate = 25;
	public bool lööp = true;
	[AllowNesting]
	[Tooltip("for infinite looping set to 0")]
	[ShowIf("lööp")]
	[MinValue(0)]
	public int loopCount = 1;
	[AllowNesting]
	[HideIf(EConditionOperator.And, "lööp", "infiniteLoop")]
	public string nextAnim;
	[HorizontalLine(1)]
	[ShowAssetPreview()]
	public Sprite[] frames;
	[HorizontalLine(1)]
	public AnimEvents events;
	[HideInInspector]
	public int index;

	private bool infiniteLoop => loopCount == 0;

	private Dictionary<int, NEvent> eventCache;

	public void CallEvents(int frame) {
		if (events.frameEvents.Length == 0)
			return;

		// if the cache hasnt been built, build it
		if (eventCache == null) {
			eventCache = new Dictionary<int, NEvent>();
			foreach (AnimEvent e in events.frameEvents) {
				if (eventCache.ContainsKey(e.frameIndex))
					eventCache[e.frameIndex].AddListener(e.call.Invoke);
				else
					eventCache.Add(e.frameIndex, e.call);
			}
		}

		if (eventCache.ContainsKey(frame))
			eventCache[frame].Invoke();
	}

	private void OnValidate() {
		for (int i = 0; i < events.frameEvents.Length; i++)
			if (events.frameEvents[i].frameIndex >= frames.Length)
				events.frameEvents[i].frameIndex = frames.Length - 1;
	}

	[System.Serializable]
	public struct AnimEvents {
		[AllowNesting]
		[Label("Call Each Loop")]
		public bool callStartEveryLoop;
		public NEvent OnStart;
		[AllowNesting]
		[Label("Call Each Loop")]
		public bool callFinishEveryLoop;
		public NEvent OnFinish;
		public AnimEvent[] frameEvents;
	}

	[System.Serializable]
	public struct AnimEvent {
		[AllowNesting]
		[MinValue(0)]
		[Tooltip("the frame this event will b called")]
		public int frameIndex;
		public NEvent call;
	}
}