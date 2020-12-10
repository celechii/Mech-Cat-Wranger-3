using UnityEngine;

public class MechControl : MonoBehaviour {

	[Header("//MOVEMENT SHIT")]
	public float walkSpeed;
	public float crawlSpeed;
	public float moveAccel;
	public float turnSpeed;
	public float turnAccel;
	public float crouchAccel;
	[Header("//CAM BOB SHIT")]
	public RangeFloat camHeight;
	public float stepSize;
	public float stepForce;
	public float bobMaxHeight;
	public float comeToRestTime;
	[Header("//MECH CONFIG")]
	public ConfigUIControl configUI;

	private Transform camTrans;
	private Rigidbody rb;
	private MechStatus mechStatus;
	private Vector3 moveVelocity;
	private Vector3 moveVelRef;
	private float turnVelRef;
	private float crouchVelRef;
	private float strideDist;
	private Vector3 lastPos;
	private float bobHeight;
	private int moveInput;
	private float rotationVelocity;
	private float rotation;
	private int rotationInput;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		mechStatus = GetComponent<MechStatus>();
		camTrans = transform.GetChild(0);
		strideDist = 0;
	}

	private bool isCrawling => InputManager.GetKey("Crouch") && mechStatus.IsIntact(MechSystem.Crouch);

	private float moveSpeed => isCrawling ? crawlSpeed : walkSpeed;

	private void Update() {
		// move
		moveInput = (InputManager.GetKey("Walk Forwards") && mechStatus.IsIntact(MechSystem.MoveForwards) ? 1 : 0) + (InputManager.GetKey("Walk Backwards") && mechStatus.IsIntact(MechSystem.MoveBackwards) ? -1 : 0);
		Vector3 targetVel = transform.forward * moveInput * moveSpeed;
		moveVelocity = Vector3.SmoothDamp(moveVelocity, targetVel, ref moveVelRef, moveAccel);

		// turn
		rotationInput = (InputManager.GetKey("Turn Left") && mechStatus.IsIntact(MechSystem.TurnRight) ? -1 : 0) + (InputManager.GetKey("Turn Right") && mechStatus.IsIntact(MechSystem.TurnLeft) ? 1 : 0);
		transform.localRotation = Quaternion.Euler(0, rotation, 0);

		// crouch
		float crouchTarget = isCrawling ? camHeight.min : camHeight.max;
		camHeight.Value = Mathf.SmoothDamp(camHeight.Value, crouchTarget, ref crouchVelRef, crouchAccel);
		camTrans.localPosition = Vector3.up * (camHeight.Value + bobHeight);

		bobHeight = Mathf.Abs(Mathf.Sin(Mathf.PI * strideDist / stepSize)) * bobMaxHeight;
	}

	private void FixedUpdate() {
		rb.velocity = moveVelocity;

		rotationVelocity = Mathf.SmoothDamp(rotationVelocity, rotationInput * turnSpeed, ref turnVelRef, turnAccel, Mathf.Infinity, Time.fixedDeltaTime);
		rotation = FuckingAngles.AddToAngle(rotation, rotationVelocity);

		if (moveInput == 0) {
			if (strideDist > 0)
				strideDist += Time.fixedDeltaTime * comeToRestTime;
		} else {
			strideDist += Vector3.Distance(transform.localPosition, lastPos);
		}

		if (strideDist >= stepSize) {
			strideDist = 0;
			Step();
			lastPos = transform.localPosition;
		}
		lastPos = transform.localPosition;
	}

	private void Step() {
		ScreenShake.staticRef.Push(Vector2.down * stepForce * Mathf.Clamp01(rb.velocity.magnitude / walkSpeed));
	}
}