using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;



public class moveVR : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float gravity = 9.81f;
    public LayerMask groundLayer; // Слой для проверки земли

    // Ссылка на SteamVR Input для движения (переменная movement типа Vector2)
    public SteamVR_Action_Vector2 movementAction;

    private Rigidbody rb;
    private bool isGrounded;

    public GameObject c;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        if (movementAction == null)
        {
            Debug.LogError("movementAction не привязано. Проверьте настройки SteamVR Input.");
            movementAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("Movement");
        }

    }

    void Update()
    {
        HandleMovement();
        ApplyGravity();


    }

    private void HandleMovement()
    {
        if (movementAction != null)
        {
            Vector2 inputAxis = movementAction.axis;

            Vector3 forward = c.transform.TransformDirection(Vector3.forward);
            Vector3 right = c.transform.TransformDirection(Vector3.right);

            float curSpeedX = inputAxis.y * walkSpeed;
            float curSpeedY = inputAxis.x * walkSpeed;

            Vector3 moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            Vector3 velocity = rb.velocity;

            velocity.x = moveDirection.x;
            velocity.z = moveDirection.z;

            rb.velocity = velocity;
        }
        else
        {
            Debug.LogWarning("movementAction не инициализировано");
        }
    }

    private void ApplyGravity()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        if (!isGrounded)
        {
            Vector3 velocity = rb.velocity;
            velocity.y -= gravity * Time.deltaTime;
            rb.velocity = velocity;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "stopFunic" && GetComponent<moveVR>().enabled == false)
        {
            transform.parent = null;
            transform.position = new Vector3(1240.622f, 631.868f, 1781.778f);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.GetComponent<moveVR>().enabled = true;
            PlayersList.isFunic = false;
        }

        if (other.gameObject.tag == "stopFunic1" && GetComponent<moveVR>().enabled == false)
        {
            transform.parent = null;
            transform.position = new Vector3(1261.622f, 4.83f, 260f);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.GetComponent<moveVR>().enabled = true;
            PlayersList.isFunic = false;
        }
    }

}


