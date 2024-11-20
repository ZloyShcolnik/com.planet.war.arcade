using System;
using UnityEngine;
using UnityEngine.UI;

public class LeverController : MonoBehaviour
{
    public static bool isActive;
    public static bool isProcessing;

    private GameObject canvas;

    private Animation anim;
    private AnimationClip clip;

    public static Action<bool> OnTurnElectrick { get; set; }

    private void Awake()
    {
        isActive = true;

        anim = GetComponent<Animation>();

        canvas = GetComponentInChildren<Canvas>().gameObject;
        var btn = canvas.GetComponentInChildren<Button>();
        canvas.SetActive(false);

        clip = anim.clip;
        anim[clip.name].time = 1.0f;
        anim[clip.name].speed = 1.0f;
        anim.Play();

        btn.onClick.AddListener(() =>
        {
            anim.Stop();
            isActive = !isActive;
            OnTurnElectrick?.Invoke(isActive);

            if (IsInvoking(nameof(TurnOff)))
            {
                CancelInvoke(nameof(TurnOff));
            }

            if (isActive)
            {
                Invoke(nameof(TurnOff), UnityEngine.Random.Range(45.0f, 135.0f));
            }

            TVController.Instance.Turn(isActive);
            CameraChanger.Instance.TurnElectric(isActive);
            BoyController.Inststance.EnableEye(isActive);
            TurnElectricSfx.Instant(isActive);

            canvas.SetActive(false);

            anim[clip.name].time = isActive ? 0 : 1;
            anim[clip.name].speed = isActive ? 1.0f : -1.0f;
            anim.Play();

            Invoke(nameof(EnableCanvas), 0.9f);
        });
    }

    private void Update()
    {
        isProcessing = anim.isPlaying;
    }

    private void Start()
    {
        BoyController.Inststance.EnableEye(isActive);
        Invoke(nameof(TurnOff), 45.0f);
    }

    private void TurnOff()
    {
        isActive = !isActive;
        OnTurnElectrick?.Invoke(isActive);

        TVController.Instance.Turn(isActive);

        CameraChanger.Instance.TurnElectric(isActive);
        BoyController.Inststance.EnableEye(isActive);
        TurnElectricSfx.Instant(isActive);

        anim[clip.name].time = isActive ? 0 : 1;
        anim[clip.name].speed = isActive ? 1.0f : -1.0f;
        anim.Play();
    }

    private void EnableCanvas()
    {
        canvas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player") || isProcessing)
        {
            return;
        }

        canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || isProcessing)
        {
            return;
        }

        canvas.SetActive(false);
    }
}
