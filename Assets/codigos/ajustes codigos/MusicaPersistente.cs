using UnityEngine;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instancia;

    void Awake()
    {
        if (instancia != null)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);
    }
}