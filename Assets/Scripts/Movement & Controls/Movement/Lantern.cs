using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Lantern : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light lanternLight;
    [SerializeField] private AudioSource audioSource;

    [Header("UI")]
    [SerializeField] private TMP_Text oilLevelText;
    [SerializeField] private TMP_Text flaskCountText;
    [SerializeField] private Image oilBarFill;
    [SerializeField] private Color normalBarColor = new Color(1f, 0.6f, 0.1f);
    [SerializeField] private Color lowBarColor = Color.red;

    [Header("Oil")]
    [SerializeField] private float maxOil = 100f;
    [SerializeField] private float startingOil = 100f;
    [SerializeField] private float drainPerSecond = 1f;
    [SerializeField] private float oilPerFlask = 50f;
    [SerializeField] private int flasks = 0;

    [Header("Low Oil Dimming")]
    [SerializeField, Range(0f, 1f)] private float lowOilFraction = 0.2f;
    [SerializeField, Range(0f, 1f)] private float minDimMultiplier = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioClip lightOnClip;
    [SerializeField] private AudioClip lightOffClip;
    [SerializeField] private AudioClip refuleClip;

    private float oil;
    private float baseIntensity = -1f;
    private bool isLit;

    private int lastShownPercent = -1;
    private int lastShownFlasks = -1;

    private void Awake()
    {
        if (lanternLight == null)
            lanternLight = GetComponentInChildren<Light>();

        if (lanternLight == null)
        {
            Debug.LogError("Lantern: no Light assigned or found", this);
            enabled = false;
        }
    }

    private void Start()
    {
        oil = Mathf.Clamp(startingOil, 0f, maxOil);
        baseIntensity = lanternLight.intensity;
        SetLit(false);
        UpdateUI();
    }

    private void Update()
    {
        HandleInput();
        DrainOil();
        UpdateDimming();
        UpdateUI();
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.fKey.wasPressedThisFrame)
            ToggleLight();

        if (keyboard.rKey.wasPressedThisFrame)
            Refuel();
    }

    private void ToggleLight()
    {
        if (isLit)
            SetLit(false);
        else if (oil > 0f)
            SetLit(true);
    }

    private void SetLit(bool lit)
    {
        isLit = lit;
        lanternLight.enabled = lit;
        PlaySound(lit ? lightOnClip : lightOffClip);
    }

    private void DrainOil()
    {
        if (!isLit)
            return;

        oil -= drainPerSecond * Time.deltaTime;

        if (oil <= 0f)
        {
            oil = 0f;
            SetLit(false);
        }
    }

    private void UpdateDimming()
    {
        if (!isLit)
            return;

        float dim = Mathf.Clamp01((oil / maxOil) / lowOilFraction);
        lanternLight.intensity = baseIntensity * Mathf.Lerp(minDimMultiplier, 1f, dim);
    }

    private void Refuel()
    {
        if (flasks <= 0 || oil >= maxOil)
            return;

        flasks--;
        oil = Mathf.Min(maxOil, oil + oilPerFlask);
        PlaySound(refuleClip);
    }

    public void AddFlasks(int amount)
    {
        flasks += amount; 
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    private void UpdateUI()
    {
        float fraction = oil / maxOil;

        if (oilBarFill != null)
        {
            oilBarFill.fillAmount = fraction;
            oilBarFill.color = fraction <= lowOilFraction ? lowBarColor : normalBarColor;
        }

        int percent = Mathf.CeilToInt(oil / maxOil * 100f);

        if (oilLevelText != null && percent != lastShownPercent)
        {
            lastShownPercent = percent;
            oilLevelText.text = percent + "%";
        }

        if (flaskCountText != null && flasks != lastShownFlasks)
        {
            lastShownFlasks = flasks;
            flaskCountText.text = flasks.ToString();
        }
    }
}
