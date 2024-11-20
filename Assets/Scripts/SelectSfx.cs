using UnityEngine;

public class SelectSfx : MonoBehaviour
{
    public static void Instant()
    {
        var go = Instantiate(Resources.Load<AudioSource>("selectSfx"));

        DontDestroyOnLoad(go.gameObject);
        Destroy(go.gameObject, 1.0f);
    }
}
