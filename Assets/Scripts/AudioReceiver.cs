using UnityEngine;

public class AudioReceiver : AAudioReceiver
{
    [SerializeField]
    private GameObject cube;
    internal GameObject Cube { get => cube; set => cube = value; }
    [SerializeField]
    private Material material;
    internal Material Material { get => material; set => SetMaterial(value); }
    internal int spectrumPart = 0;
    [SerializeField]
    private float yAmplification = 5;
    internal float YAmplification { get => yAmplification; set => yAmplification = value; }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localScale = cube.transform.localScale;
        if (spectrum != null)
            localScale.y = spectrum[spectrumPart] * yAmplification;

        cube.transform.localScale = localScale;
        Color.RGBToHSV(material.color, out float H, out float S, out float V);
        H += Time.deltaTime / 36;
        material.color = Color.HSVToRGB(H, S, V);
    }

    private void SetMaterial(Material material)
    {
        this.material = material;
        cube.GetComponent<Renderer>().material = Instantiate<Material>(this.material);
    }
}
