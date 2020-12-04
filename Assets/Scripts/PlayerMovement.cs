using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Header("//MOVEMENT SHIT")]
	[SerializeField] private float lookSpeed;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float sprintSpeed;
	[SerializeField] private float accel;
	[SerializeField] private float airAccel;
	[SerializeField] private float jumpForce;
	[SerializeField] private bool isSprinting;
	[SerializeField] private float collisionRayOvershoot = .1f;
	[Header("//CAM MOVEMENT SHIT")]
	[SerializeField] private float headBobSpeed;
	[SerializeField] private Vector2 headBobAmount;
	[SerializeField] private LayerMask terrainMask;
	[SerializeField] private float strafeTiltAngle;
	[SerializeField] private float strafeTiltAccel;
	[SerializeField] private float defaultFOV;
	[SerializeField] private float sprintFOV;
	[SerializeField] private float fovAccel;
	[SerializeField] private Transform cameraTrans;
	[Header("//SOUND SHIT")]
	[SerializeField] private float footstepOffset;

	private Rigidbody rb;
	private CapsuleCollider capCol;
	private Camera mainCam;
	private Vector3 velRef;
	private float headBobTime;
	private Vector3 initHeadPos;
	[HideInInspector]
	public Vector3 inputDirection;
	private float strafeTiltVelRef;
	private float fovVelRef;
	private bool isGrounded;
	private bool queueJump;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		capCol = GetComponent<CapsuleCollider>();
		mainCam = cameraTrans.GetComponent<Camera>();
		initHeadPos = cameraTrans.localPosition;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update() {

		inputDirection.x = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
		inputDirection.z = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0);

		if (Input.GetKeyDown(KeyCode.Space))
			queueJump = true;

		Look();
		Move();
	}

	private void FixedUpdate() {
		isGrounded = Physics.Raycast(capCol.bounds.center, Vector3.down, capCol.bounds.extents.y + collisionRayOvershoot, terrainMask);

		if (queueJump) {
			Jump();
			queueJump = false;
		}
	}

	public void Look() {
		Vector2 mouseMovement = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

		float angle = Vector3.SignedAngle(transform.forward, cameraTrans.forward, transform.right);
		float nextRotation = -mouseMovement.y * lookSpeed;

		transform.Rotate(0, mouseMovement.x * lookSpeed, 0);
		cameraTrans.eulerAngles = new Vector3(Mathf.Clamp(angle + nextRotation, -90, 90), cameraTrans.eulerAngles.y, cameraTrans.eulerAngles.z);
	}

	public void Move() {

		isSprinting = Input.GetKey(KeyCode.LeftShift) && new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 2f; //this is now definied publically so i can use it for sound stuff :^)

		TiltHead(inputDirection.normalized.x);

		inputDirection = transform.TransformVector(inputDirection);
		Vector3 targetSpeed = inputDirection.normalized * (isSprinting? sprintSpeed : moveSpeed);
		targetSpeed.y = rb.velocity.y;
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetSpeed, ref velRef, isGrounded?accel : airAccel);

		mainCam.fieldOfView = Mathf.SmoothDamp(mainCam.fieldOfView, isSprinting?sprintFOV : defaultFOV, ref fovVelRef, fovAccel);

		HeadBob(new Vector2(rb.velocity.x, rb.velocity.z).magnitude);

	}

	public void Jump() {
		if (isGrounded)
			rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
	}

	private void HeadBob(float amount) {

		if (isGrounded) {
			float prevBobTime = GetBobTime(.25f);
			headBobTime += amount;
			cameraTrans.localPosition = new Vector3(GetBobVal(.5f) * headBobAmount.x, initHeadPos.y + (GetBobVal() * headBobAmount.y), 0);

			// if (Mathf.Sign(Mathf.Tan(prevBobTime + footstepOffset)) != Mathf.Sign(Mathf.Tan(GetBobTime(.25f) + footstepOffset)))
			// 	play footstep sound

		} else headBobTime = 0;
	}

	private float GetBobVal() => GetBobVal(1);
	private float GetBobVal(float speedMul) => Mathf.Sin(GetBobTime(speedMul));
	private float GetBobTime() => GetBobTime(1);
	private float GetBobTime(float speedMul) => headBobTime * headBobSpeed * speedMul;

	private void TiltHead(float xInput) {
		float zAngle = Mathf.SmoothDampAngle(cameraTrans.eulerAngles.z, strafeTiltAngle * -xInput, ref strafeTiltVelRef, strafeTiltAccel);

		cameraTrans.localRotation = Quaternion.Euler(cameraTrans.localRotation.eulerAngles.x, 0, zAngle);
	}

}