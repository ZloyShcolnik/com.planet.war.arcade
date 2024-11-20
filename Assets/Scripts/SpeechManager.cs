using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance;
    [SerializeField] AudioClip[] clips;

    private void Awake()
    {
        Instance = this;
    }

    public void InstantSpeech(int id)
    {
        var instance = new GameObject("speech", typeof(AudioSource));
        instance.transform.position = Vector3.zero;
        var source = instance.GetComponent<AudioSource>();
        source.volume = 0.5f;
        source.playOnAwake = false;
        source.clip = clips[id];
        source.Play();

        Destroy(instance, 5.0f);
    }
}
