using UnityEngine.UI;
using UnityEngine;

public class DeclineExitBtn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SelectSfx.Instant();
            ExitPopup.Destroy();
        });
    }
}
