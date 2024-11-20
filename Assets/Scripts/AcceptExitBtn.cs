using UnityEngine;
using UnityEngine.UI;

public class AcceptExitBtn : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
