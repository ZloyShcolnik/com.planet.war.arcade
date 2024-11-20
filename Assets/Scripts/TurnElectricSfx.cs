using UnityEngine;

public class TurnElectricSfx : MonoBehaviour
{
    [SerializeField] AudioClip on;
    [SerializeField] AudioClip off;

    public static void Instant(bool isOn)
    {
        var instance = Instantiate(Resources.Load<TurnElectricSfx>("TurnElectricSfx"));
        var source = instance.GetComponent<AudioSource>();
        source.clip = isOn ? instance.on : instance.off;
        source.Play();

        Destroy(instance.gameObject, 2.5f);
    }
}
