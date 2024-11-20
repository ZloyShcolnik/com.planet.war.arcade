using UnityEngine;
using UnityEngine.UI;

public class CreepyBearController : MonoBehaviour
{
    private bool isCollided;

    private GameObject canvas;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        canvas = GetComponentInChildren<Canvas>().gameObject;
        var btn = canvas.GetComponentInChildren<Button>();
        canvas.SetActive(false);

        btn.onClick.AddListener(() =>
        {
            if (IsInvoking(nameof(EnableBear)))
            {
                CancelInvoke(nameof(EnableBear));
            }

            if (source.isPlaying)
            {
                source.Stop();
                return;
            }

            source.Play();
        });

        LeverController.OnTurnElectrick += LeverController_OnTurnElectrick;
    }

    private void OnDestroy()
    {
        LeverController.OnTurnElectrick -= LeverController_OnTurnElectrick;
    }

    private void LeverController_OnTurnElectrick(bool isOn)
    {
        if (IsInvoking(nameof(EnableBear)))
        {
            CancelInvoke(nameof(EnableBear));
        }

        if (!isOn && !source.isPlaying)
        {
            var delay = Random.Range(15.0f, 30.0f);
            Invoke(nameof(EnableBear), delay);
        }
    }

    private void EnableBear()
    {
        source.Play();
    }

    private void Update()
    {
        canvas.SetActive(isCollided);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        isCollided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        isCollided = false;
    }
}
