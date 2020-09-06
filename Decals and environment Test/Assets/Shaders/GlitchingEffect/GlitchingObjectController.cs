using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchingObjectController : MonoBehaviour
{
    [Header("Glitching Material Handling")]
    public bool canGlitch = true;
    [SerializeField, Range(0, 1)]
    private float maxIntensity = 0.4f;
    [SerializeField]
    private float maxTimeBetweenGlitches = 3.0f;
    private bool isGlitching = false;

    [SerializeField] private GameObject defaultObject;
    [SerializeField] private GameObject glitchingObject;
    private Material glitchingMaterial;

    [Header("Texture Handling - Noise Texture")]
    [SerializeField] private Vector2Int noiseTextureSize;
    [SerializeField] private float noiseStep = 1.0f;
    private Texture2D _noiseTexture;

    [Header("Texture Handling - Trash Texture")]
    [SerializeField] private Vector2Int trashTextureSize;
    private Texture2D _TrashTexture;

    void Start()
    {
        // Instancing Materials
        glitchingMaterial = glitchingObject.GetComponent<MeshRenderer>().material;

        // Setting Up Noise Texture
        _noiseTexture = new Texture2D(noiseTextureSize.x, noiseTextureSize.y, TextureFormat.ARGB32, false);
        _noiseTexture.hideFlags = HideFlags.DontSave;
        _noiseTexture.wrapMode = TextureWrapMode.Repeat;
        _noiseTexture.filterMode = FilterMode.Point;

        // Setting Up Trash Texture
        _TrashTexture = new Texture2D(trashTextureSize.x, trashTextureSize.y, TextureFormat.ARGB32, false);
        _TrashTexture.hideFlags = HideFlags.DontSave;
        _TrashTexture.wrapMode = TextureWrapMode.Repeat;
        _TrashTexture.filterMode = FilterMode.Point;

        glitchingMaterial.SetTexture("_NoiseTex", _noiseTexture);
        glitchingMaterial.SetTexture("_TrashTex", _TrashTexture);

        // Starting Screen Animnation
        StartCoroutine(GlitchAnim(1.0f, 0.0f));
    }

    void Update()
    {
        if (isGlitching && canGlitch)
        {
            defaultObject.SetActive(false);
            glitchingObject.SetActive(true);

            float f = Random.value;
            if (f > Mathf.Lerp(0.9f, 0.5f, maxIntensity))
            {
                UpdateNoiseTexture();
            }
            UpdateTrashTexture();
        }
        else
        {
            defaultObject.SetActive(true);
            glitchingObject.SetActive(false);
        }
    }

    /*
    ====================================================================================================
    Screen Glitching
    ====================================================================================================
    */
    private IEnumerator GlitchAnim(float animTime, float intensity)
    {
        UpdateIntensity(intensity);


        if (intensity > 0.0f)
        {
            isGlitching = true;
        }
        else
        {
            isGlitching = false;
        }

        yield return new WaitForSeconds(animTime);

        if (intensity == 0.0f && canGlitch)
        {
            StartCoroutine(GlitchAnim(Random.Range(0.0f, 3.0f), Random.Range(0.0f, maxIntensity)));
        }
        else
        {
            StartCoroutine(GlitchAnim(Random.Range(0.0f, maxTimeBetweenGlitches), 0.0f));
        }
    }


    /*
    ====================================================================================================
    Glitching
    ====================================================================================================
    */
    public void UpdateIntensity(float newIntensity)
    {
        if (glitchingMaterial != null)
        {
            glitchingMaterial.SetFloat("_Intensity", newIntensity);
        }
    }

    private Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, 1.0f);
    }

    void UpdateNoiseTexture()
    {
        Vector2 offset = new Vector2
        {
            x = Random.Range(0.0f, 255.0f),
            y = Random.Range(0.0f, 255.0f)
        };

        for (var y = 0; y < _noiseTexture.height; y++)
        {
            for (var x = 0; x < _noiseTexture.width; x++)
            {
                float value = Mathf.PerlinNoise(((float)x * noiseStep) + offset.x, ((float)y * noiseStep) + offset.y);
                _noiseTexture.SetPixel(x, y, new Color(value, value, value, value));
            }
        }

        _noiseTexture.Apply();
    }

    void UpdateTrashTexture()
    {
        var color = RandomColor();

        for (var y = 0; y < _TrashTexture.height; y++)
        {
            for (var x = 0; x < _TrashTexture.width; x++)
            {
                if (Random.value > 0.89f) color = RandomColor();
                _TrashTexture.SetPixel(x, y, color);
            }
        }

        _TrashTexture.Apply();
    }
}