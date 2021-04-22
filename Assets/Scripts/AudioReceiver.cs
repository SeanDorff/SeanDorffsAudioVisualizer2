
using UnityEngine;

public class AudioReceiver : AAudioReceiver
{
    [SerializeField]
    private GameObject cube;
    internal GameObject Cube { get => cube; set => cube = value; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localScale = cube.transform.localScale;
        if (spectrum != null)
            localScale.y = spectrum[0];
        cube.transform.localScale = localScale;
    }
}
