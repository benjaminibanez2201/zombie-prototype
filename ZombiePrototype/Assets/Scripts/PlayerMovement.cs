using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movimiento")]
	[SerializeField] float walkSpeed = 5f;
	[SerializeField] float sprintSpeed = 8f;
	[SerializeField] float inputDeadzone = 0.2f;

	[Header("Salto/Gravedad")]
	[SerializeField] float jumpHeight = 1.5f;
	[SerializeField] float gravity = -20f;

	[Header("Cámara (FP)")]
	[SerializeField] Transform cameraTransform; // Cámara en primera persona
	[SerializeField] float mouseSensitivity = 150f; // grados/segundo
	[SerializeField] bool lockCursor = true; // Bloquear cursor al iniciar

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
		// Si no hay cámara asignada, salir
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
		bool grounded = _cc.isGrounded; //¿está en el suelo?

		// Empuje suave hacia el suelo cuando está en suelo
		if (grounded && _verticalVelocity < 0f)
			_verticalVelocity = -2f;

		float h = Input.GetAxisRaw("Horizontal"); // A/D o flechas
		float v = Input.GetAxisRaw("Vertical");   // W/S o flechas

		// Deadzone para evitar drift por mandos/ruido
		if (Mathf.Abs(h) < inputDeadzone) h = 0f;
		if (Mathf.Abs(v) < inputDeadzone) v = 0f;

		Vector3 move = (transform.right * h + transform.forward * v);
		if (move.sqrMagnitude > 1f) move.Normalize(); // si se mueve en diagonal, normalizar

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

