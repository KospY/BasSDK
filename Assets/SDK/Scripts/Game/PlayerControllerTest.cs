using UnityEngine;
using UnityEngine.XR;

public class PlayerControllerTest : MonoBehaviour
{
    CharacterController characterController;
    public Transform head;
    public float speed = 2.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    void Awake()
    {
#if ProjectCore
        Destroy(this.gameObject);
#else
        characterController = GetComponent<CharacterController>();
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
#endif
    }

#if !ProjectCore
    void FixedUpdate()
    {
        characterController.center = new Vector3(this.transform.InverseTransformPoint(head.position).x, 0, this.transform.InverseTransformPoint(head.position).z);
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(head.TransformDirection(moveDirection) * Time.deltaTime);
        this.transform.RotateAround(head.position, Vector3.up, Input.GetAxis("Turn"));
    }
#endif
}