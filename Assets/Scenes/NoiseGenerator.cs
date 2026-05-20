using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NoiseGenerator : MonoBehaviour
{
    [Range(0f, 1f)] public float noiseVolume = 0f;
    [Range(0f, 1f)] public float distortion = 0f;

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i++)
        {
            // Distorsion: clipea la señal
            data[i] = Mathf.Clamp(data[i] * (1f + distortion * 10f), -1f, 1f);
            // Ruido estatico encima
            data[i] += (Random.value * 2f - 1f) * noiseVolume;
            data[i] = Mathf.Clamp(data[i], -1f, 1f);
        }
    }
}