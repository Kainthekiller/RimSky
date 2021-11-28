using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Control : MonoBehaviour
{
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;

    [SerializeField]
    private float jumpHeight = 1.0f;
    //[SerializeField]
    //private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    //private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraMainTransform;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void Start()
    {
        //controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {


        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        //move.y = 0;


        //// Changes the height position of the player..
        //if (jumpControl.action.triggered && groundedPlayer)
        //{
        //   playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        //}

        //playerVelocity.y += gravityValue * Time.deltaTime;


        if(movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
