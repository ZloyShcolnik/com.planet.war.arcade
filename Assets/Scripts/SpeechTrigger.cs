using UnityEngine;

public class SpeechTrigger : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] bool isDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        SpeechManager.Instance.InstantSpeech(id);
        if(isDestroy)
        {
            Destroy(gameObject);
        }
    }
}
