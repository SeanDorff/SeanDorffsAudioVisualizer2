using UnityEngine;

public abstract class AAudioReceiver : MonoBehaviour
{
    internal float[] spectrum;
    internal void ReceiveSpectrum(float[] spectrum)
    {
        this.spectrum = spectrum;
    }
}
