using UnityEngine;

public class FootStepSfx : MonoBehaviour
{
    public static void Instant()
    {
        var instance = Instantiate(Resources.Load<GameObject>("footStepSfx"));
        Destroy(instance, 1.0f);
    }
}
