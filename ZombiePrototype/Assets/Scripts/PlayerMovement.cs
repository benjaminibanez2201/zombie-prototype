using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movimiento")]
	[SerializeField] float walkSpeed = 5f;
	[SerializeField] float sprintSpeed = 8f;

	[Header("Salto/Gravedad")]
	[SerializeField] float jumpHeight = 1.5f;
	[SerializeField] float gravity = -9.81f;

	[Header("Cámara (FP)")]
	[SerializeField] Transform cameraTransform;
	[SerializeField] float mouseSensitivity = 150f; // grados/segundo
	[SerializeField] bool lockCursor = true;

	CharacterController _cc;
	float _verticalVelocity;
	float _cameraPitch;

	void Awake()
	{
		_cc = GetComponent<CharacterController>();
		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void Update()
	{
		Look();
		Move();
	}

	void Look()
	{
		if (cameraTransform == null) return;

		float mouseX = Input.GetAxis("Mouse X");
		float mouseY = Input.GetAxis("Mouse Y");

		// Yaw (rotación horizontal) en el cuerpo del jugador
		transform.Rotate(Vector3.up, mouseX * mouseSensitivity * Time.deltaTime);

		// Pitch (rotación vertical) solo en la cámara
		_cameraPitch -= mouseY * mouseSensitivity * Time.deltaTime;
		_cameraPitch = Mathf.Clamp(_cameraPitch, -89f, 89f);
		cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
	}

	void Move()
	{
		bool grounded = _cc.isGrounded;

		// Empuje suave hacia el suelo cuando está en suelo
		if (grounded && _verticalVelocity < 0f)
			_verticalVelocity = -2f;

		float h = Input.GetAxisRaw("Horizontal"); // A/D o flechas
		float v = Input.GetAxisRaw("Vertical");   // W/S o flechas

		Vector3 move = (transform.right * h + transform.forward * v);
		if (move.sqrMagnitude > 1f) move.Normalize();

		// Sprint con Shift (opcional)
		bool sprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		float speed = sprinting ? sprintSpeed : walkSpeed;

		// Salto con Space
		if (grounded && Input.GetButtonDown("Jump"))
		{
			// v0 = sqrt(2 * h * -g)
			_verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
		}

		// Gravedad acumulada
		_verticalVelocity += gravity * Time.deltaTime;

		Vector3 velocity = move * speed;
		velocity.y = _verticalVelocity;

		_cc.Move(velocity * Time.deltaTime);
	}

    
}

