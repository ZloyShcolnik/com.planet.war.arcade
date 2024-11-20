using UnityEngine;

public class HorrorSfx : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;

    public static void Instant()
    {
        var instance = Instantiate(Resources.Load<HorrorSfx>("HorrorSfx"));
        var source = instance.GetComponent<AudioSource>();

        var clip = instance.clips[Random.Range(0, instance.clips.Length)];
        source.clip = clip;
        source.pitch = Random.Range(1.0f, 1.6f);

        source.Play();
        Destroy(instance.gameObject, clip.length);
    }
}
