using UnityEngine;
using UnityEngine.UI;

public class WallPhoneController : MonoBehaviour
{
    private int id;
    private bool isReady;

    private bool isCollided;

    private GameObject canvas;
    private AudioSource source;

    [SerializeField] AudioClip[] audioClips;

    private void Awake()
    {
        id = 0;
        isReady = false;

        canvas = GetComponentInChildren<Canvas>().gameObject;
        var btn = canvas.GetComponentInChildren<Button>();

        source = GetComponent<AudioSource>();

        btn.onClick.AddListener(() =>
        {
            isReady = false;

            var clip = id < audioClips.Length ? audioClips[id] : audioClips[audioClips.Length - 1];
            var duration = clip.length;

            PhoneDialogueSfx.Instant(clip);
            source.Stop();

            id++;
            Invoke(nameof(TellMyPhome), duration + Random.Range(65.0f, 120.0f));
        });

        Invoke(nameof(TellMyPhome), Random.Range(75.0f, 120.0f));
    }

    private void Update()
    {
        canvas.SetActive(isCollided && isReady);
    }

    private void TellMyPhome()
    {
        source.Play();
        isReady = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !isReady)
        {
            return;
        }

        isCollided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !isReady)
        {
            return;
        }

        isCollided = false;
    }
}
