using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Distorsion : MonoBehaviour
{
    private System.Random _rand = new System.Random();

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (float)(_rand.NextDouble() * 2.0 - 1.0);
        }
    }
}