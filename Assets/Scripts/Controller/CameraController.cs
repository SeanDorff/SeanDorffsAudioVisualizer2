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
    private Vector3 moveVector = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    public void OnMove(InputValue input)
    {
        Vector2 movement = input.Get<Vector2>();
        moveVector = new Vector3(movement.x, 0, movement.y);
    }
}
