using UnityEngine;

public class StartBtn : MonoBehaviour
{
    [SerializeField] GameObject gameSetup;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectSfx.Instant();
            Destroy(transform.parent.gameObject);

            gameSetup.SetActive(true);
        }
    }
}
