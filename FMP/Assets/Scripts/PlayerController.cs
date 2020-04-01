using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour
{
    //Player settings 
    private float rotationSpeed = 800;
    private float walkSpeed = 5;
    private float runSpeed = 8;

    //System
    private Quaternion targetRotation;

    //Components
    private CharacterController controller;
    private Camera cam;
    public Gun gun;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //controlMouse();
        controlController();
      if (Input.GetButtonDown("Shoot"))
        {
            gun.shoot();
        }
      else if (Input.GetButton("Shoot"))
        {
            gun.shootContinuous();
        }

    }


    void controlMouse()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
        targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0 , transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && (Mathf.Abs(input.z) == 1) ? 0.7f : 1);
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);
    }

    void controlController()
    {
        Vector3 inputLeft = new Vector3(Input.GetAxisRaw("HorizontalLeft"), 0, Input.GetAxisRaw("VerticalLeft"));
        Vector3 inputRight = new Vector3(Input.GetAxisRaw("HorizontalRight"), 0, Input.GetAxisRaw("VerticalRight"));

        if (inputRight != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(inputRight);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        if (inputRight == Vector3.zero && inputLeft != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(inputLeft);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }

        Vector3 motion = inputLeft;
        motion *= (Mathf.Abs(inputLeft.x) == 1 && (Mathf.Abs(inputLeft.z) == 1) ? .7f : 1);
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        controller.Move(motion * Time.deltaTime);
    }
}
