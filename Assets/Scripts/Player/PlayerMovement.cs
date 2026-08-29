using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;

    [Header("Gravity")]
    public float gravity = -20f;

    [Header("Jump")]
    public float jumpHeight = 1.2f;

    [Header("Energy")]
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;
    public float energyDrain = 20f;
    public float energyRecovery = 15f;

    private CharacterController controller;

    private Vector3 velocity;

    private bool grounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        currentEnergy = maxEnergy;
    }

    void Update()
    {
        grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        bool sprinting =
            Input.GetKey(KeyCode.LeftShift) &&
            currentEnergy > 0 &&
            move.magnitude > 0;

        float speed = sprinting ? sprintSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        if (sprinting)
            currentEnergy -= energyDrain * Time.deltaTime;
        else
            currentEnergy += energyRecovery * Time.deltaTime;

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}