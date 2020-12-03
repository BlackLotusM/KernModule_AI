using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public abstract class Command
    {
        public abstract void Execute();
    }

    public class JumpFunction : Command
    {
        public override void Execute()
        {
            Jump();
        }
    }

    public class TelekinesisFunction : Command
    {
        public override void Execute()
        {
            Telekinesis();
        }
    }


    public static void Telekinesis()
    {

    }

    public static void Jump()
    {

    }

    public static void DoMove()
    {
        Command keySpace = new JumpFunction();
        Command keyX = new TelekinesisFunction();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            keySpace.Execute();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            keyX.Execute();
        }
    }


    public CharacterController characterController;
    public float speed = 3;
    public float mouseSensitivity = 2f;
    private float gravity = 9.87f;
    private float verticalSpeed = 0;

    void Update()
    {
        Move();
        Rotate();
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
    }

    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if (characterController.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;
        Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);

        Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
    }
}