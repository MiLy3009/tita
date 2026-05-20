using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Radio : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [Header("Audio")]
    public AudioSource audioSource;
    public Distorsion distorsion;

    [Header("LEDs")]
    public Image ledRojo;
    public Image ledVerde;

    [Header("Waveform")]
    public LineRenderer lineRenderer;

    private float _tuning = 0f;
    private Vector2 _lastMouse;
    private float _angulo = -135f;
    private float[] _spectrum = new float[64];
    private float _noise = 0f;

    void Start()
    {
        lineRenderer.positionCount = 64;
        AplicarTuning();
    }

    void Update()
    {
        audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);
        _noise = Mathf.Lerp(_noise, 1f - _tuning, Time.deltaTime * 5f);

        for (int i = 0; i < 64; i++)
        {
            float x = -3f + i * 0.1f;
            float cleanY = Mathf.Sqrt(_spectrum[i]) * 3f;
            float noiseY = Random.Range(-1f, 1f);
            float y = Mathf.Lerp(cleanY, noiseY, _noise);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        Color c = Color.Lerp(new Color(1f, 0.4f, 0f), new Color(0.2f, 1f, 0f), _tuning);
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    }

    public void OnBeginDrag(PointerEventData e) => _lastMouse = e.position;

    public void OnDrag(PointerEventData e)
    {
        float delta = (e.position.y - _lastMouse.y) * 0.5f;
        _angulo = Mathf.Clamp(_angulo + delta, -135f, 135f);
        _lastMouse = e.position;
        transform.rotation = Quaternion.Euler(0, 0, -_angulo);
        _tuning = Mathf.InverseLerp(-135f, 135f, _angulo);
        AplicarTuning();
    }

    void AplicarTuning()
    {
        audioSource.volume = _tuning;
        audioSource.pitch = Mathf.Lerp(0.7f, 1f, _tuning);
        distorsion.cantidad = 1f - _tuning;

        bool buena = _tuning > 0.7f;
        ledRojo.color = buena ? new Color(0.3f, 0, 0) : Color.red;
        ledVerde.color = buena ? Color.green : new Color(0, 0.3f, 0);
    }
}