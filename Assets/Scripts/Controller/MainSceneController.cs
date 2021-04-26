using Asset.WasAPI;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public GameObject AudioListener;
    public GameObject CapsulePrefab;
    public GameObject ChildCapsulePrefab;
    public Material SolidColor;
    public float childrenPerSecond = 2;
    float lastRealTimeSinceStartup = 0;

    private List<GameObject> primaryBarInstance = new List<GameObject>();
    private List<GameObject> childrenInstances = new List<GameObject>();
    private int barCount = 0;
    private WasAPIInterface wasAPIInterface;
    private float colorStep;
    // Start is called before the first frame update
    void Start()
    {
        wasAPIInterface = AudioListener.GetComponent<WasAPIInterface>();
        barCount = wasAPIInterface.SpectrumSize;
        colorStep = 1 / (float)barCount;
        for (int i = 0; i < barCount; i++)
        {
            GameObject instance = Instantiate(CapsulePrefab, Vector3.zero + (-(barCount / 2) + i) * (new Vector3(1, 0, 0)), Quaternion.identity);
            instance.name += $"_{i}";
            primaryBarInstance.Add(instance);
            AudioReceiver audioReceiver = instance.GetComponent<AudioReceiver>();
            audioReceiver.spectrumPart = i;
            SolidColor.color = Color.HSVToRGB(colorStep * i, 1, 1);
            audioReceiver.Material = SolidColor;
            AudioListener.GetComponent<WasAPIInterface>().AddAudioReceiver(audioReceiver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastRealTimeSinceStartup == 0)
            lastRealTimeSinceStartup = Time.realtimeSinceStartup;
        if (lastRealTimeSinceStartup > 1 / childrenPerSecond)
        {
            foreach (GameObject childInstance in childrenInstances)
            {
                ChildCapsule childCapsuleScript = childInstance.GetComponent<ChildCapsule>();
                childCapsuleScript.Generation++;
            }

            for (int i = childrenInstances.Count - 1; i >= 0; i--)
                if (childrenInstances[i].GetComponent<ChildCapsule>().Generation >= 150)
                {
                    Destroy(childrenInstances[i]);
                    childrenInstances.RemoveAt(i);
                }

            for (int i = 0; i < barCount; i++)
            {
                GameObject instance = Instantiate(ChildCapsulePrefab, Vector3.zero + (-(barCount / 2) + i) * (new Vector3(1, 0, 0)), Quaternion.identity);
                ChildCapsule childCapsuleScript = instance.GetComponent<ChildCapsule>();
                childCapsuleScript.Generation = 0;
                childCapsuleScript.Material = primaryBarInstance[i].GetComponent<AudioReceiver>().Material;
                childCapsuleScript.transform.localScale = primaryBarInstance[i].transform.localScale;
                childCapsuleScript.MovePerTime = 1;
                childCapsuleScript.Movement = new Vector3(0, 0, 1);
                childrenInstances.Add(instance);
            }
        }

        lastRealTimeSinceStartup = Time.realtimeSinceStartup;
    }
}
