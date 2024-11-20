using UnityEngine;

public class ExitController : MonoBehaviour
{
    private static ExitController Instance;
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPopup.Instant();
        }
    }
}
