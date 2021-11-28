using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float RunSpeed = 8.0f;
		[Tooltip("Walk speed of the character in m/s")]
		public float WalkSpeed = 2.0f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		[Tooltip("Camera sensitivity")]
		public float Sensitivity = 1f;
		[Tooltip("Set max player health")]
		[SerializeField] int Maxhealth = Mathf.Clamp(100, 0, 100);
		[Tooltip("Set max player mana")]
		[SerializeField] int MaxMana = Mathf.Clamp(100, 0, 100);
		[Tooltip("Health Bar")]
		public HealthBar healthbar;
		[Tooltip("Mana Bar")]
		public ManaBar manaBar;
		[Tooltip("Crosshair")]
		public GameObject reticle;
		[Tooltip("Object that spawns fireball")]
		public GameObject _fireballSpawnPoint;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		private int CurrentPlayerHealth;
		public int CurrentPlayerMana;
		public bool isDead;
		private float trackTime;
		private int attackCounter = 0;
		private float maxDelayComboTime = 0.50f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDFireball;

		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		private bool _rotateOnMove = true;

		private const float _threshold = 0.01f;

		private bool _hasAnimator;

		Mouse mouse = Mouse.current; //Mouse Input 

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			// set health and mana
			CurrentPlayerHealth = Maxhealth;
			healthbar.SetMaxHealth(Maxhealth);
			CurrentPlayerMana = MaxMana;
			manaBar.SetMaxMana(MaxMana);
			transform.rotation = Quaternion.Euler(new Vector3(this.transform.rotation.x, -80, this.transform.rotation.z));
			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_fireballSpawnPoint = GameObject.Find("FireballSpawnPoint");

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);
			
			JumpAndGravity();
			GroundedCheck();
			Move();
			PlayerDead();
			Blocking();
			FireBall();
			LightAttack();
			StrongAttack();
			JumpAttack();
		}

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDFireball = Animator.StringToHash("fireballAttack");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				_cinemachineTargetYaw += _input.look.x * Time.deltaTime * Sensitivity;
				_cinemachineTargetPitch += _input.look.y * Time.deltaTime * Sensitivity;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? WalkSpeed : RunSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

				// rotate to face input direction relative to camera position
				if(_rotateOnMove)
                {
					transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
				}
			}


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			// move the player
			_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}

		public void SetSensitivity(float newSensitivity)
        {
			Sensitivity = newSensitivity;
        }

		public void SetRotateOnMove(bool newRotateOnMove)
        {
			_rotateOnMove = newRotateOnMove;
        }

		public void TakeDamage(int damage)
		{
			CurrentPlayerHealth = healthbar.GetHealth();
			//Trigger 
			_animator.SetTrigger("takeDamage");
			CurrentPlayerHealth -= damage;
			CurrentPlayerHealth = Mathf.Clamp(CurrentPlayerHealth, 0, 100);
			healthbar.SetHealth(CurrentPlayerHealth);
		}

		public void PlayerDead()
		{
			if (CurrentPlayerHealth == 0)
			{				
				_animator.SetBool("playerDead", true);
				_input.move.x = 0f;
				_input.move.y = 0f;
			}
		}

		public void Blocking()
		{
			if (_input.block && _animator.GetBool("playerDead") == false)
			{
				_input.move.x = 0f;
				_input.move.y = 0f;
				_animator.SetBool("isBlocking", true);
			}
			else
			{
				_animator.SetBool("isBlocking", false);
			}
		}

		public void LightAttack()
        {
			if (_animator.GetBool("Jump") == false && _animator.GetBool("Grounded") == true)
			{
				if (_input.attack)
				{
					timeTracker();
					_animator.SetBool("Attack1", true);
				}
				else if (_animator.GetBool("Attack2") == false && (_animator.GetBool("Attack3") == false))
				{
					_animator.SetBool("Attack1", false);
				}
				if (attackCounter == 2)
				{
					_animator.SetBool("Attack2", true);
				}
				if (attackCounter == 3)
				{
					_animator.SetBool("Attack3", true);
				}

				if (mouse.leftButton.wasPressedThisFrame)
				{
					attackCounter++;
				}
				if (Time.time - trackTime > maxDelayComboTime)
				{
					attackCounter = 0;
					_animator.SetBool("Attack2", false);
					_animator.SetBool("Attack3", false);
				}
				if (_animator.GetBool("Attack1"))
				{
					_input.move.x = 0f;
					_input.move.y = 0f;
				}
			}
		}

		public void StrongAttack()
		{
			if (_input.strongAttack)
			{
				_animator.SetBool("StrongAttack", true);
				_input.move.x = 0f;
				_input.move.y = 0f;
			}
			else
            {
				_animator.SetBool("StrongAttack", false);
            }
		}

		public void JumpAttack()
		{
			if (_input.attack && _animator.GetBool("Jump") == true && _animator.GetBool("Grounded") == false)
			{
				_animator.SetBool("JumpAttack", true);
			}
			else
			{
				_animator.SetBool("JumpAttack", false);
			}
		}

		public void FireBall()
        {
			if (_input.fireball && _animator.GetBool("playerDead") == false && _animator.GetBool("isBlocking") == false && _input.aim && CurrentPlayerMana != 0)
            {
                _animator.SetBool("fireballAttack", true);
				//_input.move.x = 0f;
				//_input.move.y = 0f;
			}
			else
            {
				_animator.SetBool("fireballAttack", false);
            }
        }

		public void Shoot()
        {
			if (CurrentPlayerMana != 0)
			{
				CurrentPlayerMana = manaBar.GetMana();
				//Debug.Log(CurrentPlayerMana);
				CurrentPlayerMana -= 20;
				CurrentPlayerMana = Mathf.Clamp(CurrentPlayerMana, 0, 100);
				manaBar.SetMana(CurrentPlayerMana);

				_fireballSpawnPoint.GetComponent<fire>().Shoot();
			}
        }

		private void timeTracker()
		{
			trackTime = Time.time;
		}
	}
}