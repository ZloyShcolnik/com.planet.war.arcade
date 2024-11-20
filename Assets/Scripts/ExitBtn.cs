using UnityEngine;
using UnityEngine.UI;

public class ExitBtn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SelectSfx.Instant();
            ExitPopup.Instant();
        });
    }
}
