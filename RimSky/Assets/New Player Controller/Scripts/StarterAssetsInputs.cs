using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool fireball;
		public bool block;
		public bool attack;
		public bool strongAttack;

		[Header("Movement Settings")]
		public bool analogMovement;

		private Animator _animator;
		private bool isActive;

        private void Awake()
        {
			_animator = GetComponent<Animator>();
        }

#if !UNITY_IOS || !UNITY_ANDROID
        [Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}

		public void OnFireball(InputValue value)
		{
			FireballInput(value.isPressed);
		}

		public void OnBlock(InputValue value)
		{
			BlockInput(value.isPressed);
		}

		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}

		public void OnStrongAttack(InputValue value)
		{
			StrongAttackInput(value.isPressed);
		}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			if (Conditions(isActive))
			{
				move = newMoveDirection;
			}
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (Conditions(isActive))
            {
				jump = newJumpState;
			}
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
			if (_animator.GetBool("playerDead") == false)
			{
				aim = newAimState;
			}
		}

		public void FireballInput(bool newFireballState)
		{
			fireball = newFireballState;
		}

		public void BlockInput(bool newBlockState)
		{
			block = newBlockState;
		}

		public void AttackInput(bool newAttackState)
        {
			attack = newAttackState;
        }

		public void StrongAttackInput(bool newStrongAttackState)
		{
			strongAttack = newStrongAttackState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
#endif

		private bool Conditions(bool isActive)
        {
			if (_animator.GetBool("playerDead") == false && _animator.GetBool("isBlocking") == false && _animator.GetBool("fireballAttack") == false && _animator.GetBool("Attack1") == false && _animator.GetBool("StrongAttack") == false)
            {
				isActive = true;
            }
				return isActive;
        }
	}
	
}