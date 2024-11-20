using UnityEngine;
using UnityEngine.EventSystems;

public class ShowGridEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool isProcessing;

    private void Awake()
    {
        isProcessing = false;
    }

    private void OnDisable()
    {
        isProcessing = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isProcessing = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isProcessing = false;
    }
}
