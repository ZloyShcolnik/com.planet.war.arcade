using UnityEngine;
using UnityEngine.UI;

public class ChairController : MonoBehaviour
{
    private bool isActive;
    public static bool IsProcessing;

    private GameObject canvas;

    private void Awake()
    {
        isActive = false;
        IsProcessing = false;

        canvas = GetComponentInChildren<Canvas>().gameObject;
        var btn = canvas.GetComponentInChildren<Button>();
        canvas.SetActive(false);

        btn.onClick.AddListener(() =>
        {
            isActive = !isActive;
            canvas.SetActive(false);

            var pivot = transform.GetChild(1);
            var toSeePoint = transform.GetChild(2);
            BoyController.Inststance.StandToSit(isActive, pivot, toSeePoint);
            IsProcessing = true;

            Invoke(nameof(EnableCanvas), 2.4f);
        });
    }

    private void EnableCanvas()
    {
        canvas.SetActive(true);
        IsProcessing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || IsProcessing)
        {
            return;
        }

        canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || IsProcessing)
        {
            return;
        }

        canvas.SetActive(false);
    }
}
