using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Min(0.1f)]
    private float moveSpeed = 5.0f;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    [SerializeField]
    [Min(0.1f)]
    private float lookSpeed = 15.0f;
    public float LookSpeed { get => lookSpeed; set => lookSpeed = value; }

    private Vector3 moveVector = Vector3.zero;
    private Vector3 lookVector = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(lookVector * lookSpeed * Time.deltaTime);
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    public void OnMove(InputValue input)
    {
        Vector2 movement = input.Get<Vector2>();
        moveVector = movement.x * transform.TransformDirection(Vector3.right) + movement.y * transform.TransformDirection(Vector3.forward);
    }

    public void OnLook(InputValue input)
    {
        Vector2 look = input.Get<Vector2>();
        lookVector = new Vector3(look.y, -look.x, 0);
    }
}
