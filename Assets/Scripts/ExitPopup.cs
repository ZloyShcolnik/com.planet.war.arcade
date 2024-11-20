using UnityEngine;

public class ExitPopup : MonoBehaviour
{
    private static ExitPopup Instance;
    public static bool IsOpened;
    public static void Instant()
    {
        if(Instance)
        {
            return;
        }

        Instance = Instantiate(Resources.Load<ExitPopup>("exit-popup"));
    }

    public static void Destroy()
    {
        Destroy(Instance.gameObject);
    }

    private void OnEnable()
    {
        IsOpened = true;
    }

    private void OnDestroy()
    {
        IsOpened = false;
    }
}
