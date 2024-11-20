using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TVController : MonoBehaviour
{
    public static TVController Instance;

    private GameObject canvas;
    private GameObject noise;

    private bool isActive;
    private bool isCollision;
    public static bool isProcessing;

    private void Awake()
    {
        Instance = this;

        isActive = false;
        noise = transform.GetChild(0).gameObject;
        noise.SetActive(isActive);

        canvas = GetComponentInChildren<Canvas>().gameObject;
        var btn = canvas.GetComponentInChildren<Button>();
        canvas.SetActive(false);

        btn.onClick.AddListener(() =>
        {
            isActive = !isActive;
            noise.SetActive(isActive);

            canvas.SetActive(false);
            Invoke(nameof(EnableCanvas), 0.1f);
        });
    }

    private void Update()
    {
        canvas.SetActive(isCollision);
    }

    private void EnableCanvas()
    {
        canvas.SetActive(true);
    }

    public void Turn(bool isOn)
    {
        isActive = isOn;
        noise.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isProcessing || !LeverController.isActive)
        {
            return;
        }

        isCollision = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || isProcessing || !LeverController.isActive)
        {
            return;
        }

        isCollision = false;
    }
}
