using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class ScratchTransition : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panel1;
    public GameObject panel2;
    public VideoPlayer videoPlayer1;
    public VideoPlayer videoPlayer2;

    [Header("UI Elementos")]
    public Button btnSiguiente;

    [Header("Scratch Settings")]
    public RawImage scratchOverlay;
    public float brushSize = 120f;
    public float revealThreshold = 0.5f;

    [Header("Imagen de Raspado")]
    public Texture2D imagenRaspado;

    [Header("Zona de raspado")]
    [Range(0f, 1f)] public float zonaX = 0.25f;
    [Range(0f, 1f)] public float zonaY = 0.25f;
    [Range(0f, 1f)] public float zonaAncho = 0.5f;
    [Range(0f, 1f)] public float zonaAlto = 0.5f;

    [Header("Cursor Visual")]
    public RectTransform cursorCircle;

    [Header("Sistema Imagenes")]
    public SistemaImagenesVideo sistemaImagenes;

    private Texture2D scratchMask;
    private Color[] maskPixels;
    private int totalPixels;
    private int revealedPixels = 0;
    private bool isScratching = false;
    private bool transitionDone = false;
    private bool raspaditaYaUsada = false;
    private int zonaPixX, zonaPixY, zonaPixW, zonaPixH;

    void Start()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        videoPlayer1.Play();
        scratchOverlay.gameObject.SetActive(false);

        // El botón se oculta siempre, ya no se usa para la transición
        if (btnSiguiente != null)
            btnSiguiente.gameObject.SetActive(false);

        if (cursorCircle != null)
            cursorCircle.gameObject.SetActive(false);
    }

    public void ActivarRaspadita()
    {
        if (raspaditaYaUsada) return;

        transitionDone = false;
        revealedPixels = 0;
        isScratching = true;

        scratchOverlay.gameObject.SetActive(true);
        if (cursorCircle != null) cursorCircle.gameObject.SetActive(true);
        InitScratchMask();
    }

    void InitScratchMask()
    {
        int w = Screen.width;
        int h = Screen.height;

        zonaPixX = (int)(zonaX * w);
        zonaPixY = (int)(zonaY * h);
        zonaPixW = (int)(zonaAncho * w);
        zonaPixH = (int)(zonaAlto * h);

        scratchMask = new Texture2D(w, h, TextureFormat.RGBA32, false);
        maskPixels = new Color[w * h];

        for (int i = 0; i < maskPixels.Length; i++)
            maskPixels[i] = Color.clear;

        totalPixels = 0;

        for (int x = zonaPixX; x < zonaPixX + zonaPixW; x++)
        {
            for (int y = zonaPixY; y < zonaPixY + zonaPixH; y++)
            {
                if (x >= 0 && x < w && y >= 0 && y < h)
                {
                    if (imagenRaspado != null)
                    {
                        float u = (float)(x - zonaPixX) / zonaPixW;
                        float v = (float)(y - zonaPixY) / zonaPixH;
                        Color colorImagen = imagenRaspado.GetPixelBilinear(u, v);
                        maskPixels[y * w + x] = new Color(
                            colorImagen.r,
                            colorImagen.g,
                            colorImagen.b,
                            colorImagen.a > 0 ? 1f : 0f
                        );
                    }
                    else
                    {
                        maskPixels[y * w + x] = Color.black;
                    }
                    totalPixels++;
                }
            }
        }

        scratchMask.SetPixels(maskPixels);
        scratchMask.Apply();
        scratchOverlay.texture = scratchMask;
        revealedPixels = 0;
    }

    void Update()
    {
        if (!isScratching || transitionDone) return;

        Vector2 inputPos = Vector2.zero;
        bool touching = false;

        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];
            if (touch.press.isPressed)
            {
                inputPos = touch.position.ReadValue();
                touching = true;
            }
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            inputPos = Mouse.current.position.ReadValue();
            touching = true;
        }

        Vector2 cursorPos = touching ? inputPos : (Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero);

        if (cursorCircle != null)
            cursorCircle.position = new Vector3(cursorPos.x, cursorPos.y, 0);

        if (touching)
            Scratch(inputPos);
    }

    void Scratch(Vector2 screenPos)
    {
        int texX = (int)(screenPos.x * scratchMask.width / Screen.width);
        int texY = (int)(screenPos.y * scratchMask.height / Screen.height);
        int radius = (int)(brushSize / 2);

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    int px = texX + x;
                    int py = texY + y;

                    if (px >= 0 && px < scratchMask.width && py >= 0 && py < scratchMask.height)
                    {
                        int idx = py * scratchMask.width + px;
                        if (maskPixels[idx].a > 0f)
                        {
                            maskPixels[idx] = Color.clear;
                            revealedPixels++;
                        }
                    }
                }
            }
        }

        scratchMask.SetPixels(maskPixels);
        scratchMask.Apply();

        float percent = (float)revealedPixels / totalPixels;
        if (percent >= revealThreshold)
            TransicionDirecta(); // ← antes llamaba a ShowNextButton()
    }

    // Ya no muestra botón, ejecuta la transición directo
    void TransicionDirecta()
    {
        transitionDone = true;
        isScratching = false;

        if (cursorCircle != null)
            cursorCircle.gameObject.SetActive(false);

        scratchOverlay.gameObject.SetActive(false);

        OnBotonPresionado(); // ← pasa directo al panel2
    }

    void OnBotonPresionado()
    {
        raspaditaYaUsada = true;
        isScratching = false;

        panel1.SetActive(false);
        scratchOverlay.gameObject.SetActive(false);

        if (btnSiguiente != null)
            btnSiguiente.gameObject.SetActive(false);

        panel2.SetActive(true);
        videoPlayer2.Play();

        if (sistemaImagenes != null)
            sistemaImagenes.ActivarCuadrosDespuesVideo2();
    }
}