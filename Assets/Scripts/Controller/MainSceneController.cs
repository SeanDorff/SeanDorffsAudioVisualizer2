using Asset.WasAPI;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    public GameObject AudioListener;
    public GameObject CapsulePrefab;
    public GameObject CapsuleInstance;
    // Start is called before the first frame update
    void Start()
    {
        WasAPIInterface wasAPIInterface = AudioListener.GetComponent<WasAPIInterface>();
        int barCount = wasAPIInterface.SpectrumSize;
        for (int i = 0; i < barCount; i++)
        {
            GameObject instance=Instantiate(CapsulePrefab, Vector3.zero + (-(barCount / 2) + i) * (new Vector3(1, 0, 0)), Quaternion.identity);
            AudioReceiver audioReceiver = instance.GetComponent<AudioReceiver>();
            audioReceiver.spectrumPart = i;
            AudioListener.GetComponent<WasAPIInterface>().AddAudioReceiver(audioReceiver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
