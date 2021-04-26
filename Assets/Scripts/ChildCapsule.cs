using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChildCapsule : MonoBehaviour
{
    [SerializeField]
    private GameObject capsule;
    public GameObject Capsule { get => capsule; set => capsule = value; }
    private Material material;
    public Material Material { get => material; set => material = value; }
    private float yAmplification;
    public float YAmplification { get => yAmplification; set => yAmplification = value; }
    private Vector3 movement;
    public Vector3 Movement { get => movement; set => movement = value; }
    private float movePerTime;
    public float MovePerTime { get => movePerTime; set => movePerTime = value; }
    private int generation = 0;
    public int Generation { get => generation; set => generation = value; }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        capsule.transform.position += movement * Time.deltaTime / movePerTime;
    }
}
