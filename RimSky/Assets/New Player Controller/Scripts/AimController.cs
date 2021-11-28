using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private Canvas reticle;
    [SerializeField] private float normalSensitivity = 0.2f;
    [SerializeField] private float aimSensitivity = 0.25f;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfFireballProjectile;
    [SerializeField] private Transform spawnFireballPosition;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator _animator;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        if (starterAssetsInputs.aim && _animator.GetBool("playerDead") == false)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            reticle.gameObject.SetActive(true);
            thirdPersonController.RunSpeed = 5;

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if(starterAssetsInputs.move.x < 0)
            {
                _animator.SetBool("StrafeLeft", true);
                _animator.SetBool("StrafeRight", false);
                thirdPersonController.RunSpeed = 5;
            }
            else if(starterAssetsInputs.move.x > 0)
            {
                _animator.SetBool("StrafeRight", true);
                _animator.SetBool("StrafeLeft", false);
                thirdPersonController.RunSpeed = 5;
            }
            else if(starterAssetsInputs.move.x == 0)
            {
                _animator.SetBool("StrafeLeft", false);
                _animator.SetBool("StrafeRight", false);
            }
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            reticle.gameObject.SetActive(false);
            _animator.SetBool("StrafeLeft", false);
            _animator.SetBool("StrafeRight", false);
            thirdPersonController.RunSpeed = 8;
        }
    }
}
