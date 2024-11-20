using UnityEngine;

public class PhoneDialogueSfx : MonoBehaviour
{
    public static void Instant(AudioClip clip)
    {
        var instance = Instantiate(Resources.Load<AudioSource>("phoneDialogueSfx"));
        instance.clip = clip;
        instance.Play();

        Destroy(instance.gameObject, clip.length);
    }
}
