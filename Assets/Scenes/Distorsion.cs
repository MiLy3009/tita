using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Distorsion : MonoBehaviour
{
    [Range(0f, 1f)] public float cantidad = 1f;

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] += Random.Range(-cantidad, cantidad) * 0.3f;
            data[i] = Mathf.Clamp(data[i] * (1f + cantidad * 5f), -1f, 1f);
        }
    }
}