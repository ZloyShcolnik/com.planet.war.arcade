using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class CameraChanger : MonoBehaviour
{
    public static CameraChanger Instance;

    private int camIdValue;
    private GameObject latestCam;

    private ColorAdjustments colorAdjustmentsValue;
    [SerializeField] Material canvasNoiseMaterial;
    private Bloom bloomValue;
    private Volume volume;

    private float postExplosureTarget;
    private float bloomIntencityTarget;

    private AudioSource source;
    private CanvasGroup canvasGroup;

    public GameObject grid;
    [SerializeField] GameObject startCam;
    [SerializeField] Text roomName;
    [SerializeField] Button showGridBtn;
    [SerializeField] GameObject cams2;
    [SerializeField] GameObject redDot;

    [SerializeField] string[] roomNames;
    [SerializeField] GameObject[] cameras;
    [SerializeField] Transform[] cameras2;
    [SerializeField] Button[] buttons;

    private void Awake()
    {
        latestCam = startCam;
        camIdValue = 2;
        roomName.text = roomNames[camIdValue];

        volume = FindObjectOfType<Volume>();
        volume.profile.TryGet(out colorAdjustmentsValue);
        volume.profile.TryGet(out bloomValue);

        canvasGroup = FindObjectOfType<CanvasGroup>();
        cams2.SetActive(false);

        foreach (var cam in cameras)
        {
            cam.SetActive(cam == startCam);
        }

        source = GetComponent<AudioSource>();
        showGridBtn.onClick.AddListener(() =>
        {
            grid.SetActive(true);
            cams2.SetActive(true);

            redDot.SetActive(false);
            showGridBtn.gameObject.SetActive(false);
        });

        postExplosureTarget = 1.3f;

        bloomIntencityTarget = 0.0f;
        bloomValue.intensity.value = bloomIntencityTarget;

        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras2[i].SetPositionAndRotation(cameras[i].transform.position, cameras[i].transform.rotation);
        }

        var index = 0;
        foreach(var button in buttons)
        {
            button.transform.GetChild(0).GetComponent<Text>().text = roomNames[index];
            index++;

            button.onClick.AddListener(() =>
            {
                cams2.SetActive(false);
                ChangeCamera(button.transform.GetSiblingIndex());
            });
        }
    }

    private void Update()
    {
        var currentPostExplosure = colorAdjustmentsValue.postExposure.value;
        colorAdjustmentsValue.postExposure.value = Mathf.MoveTowards(currentPostExplosure, postExplosureTarget, 15.0f * Time.deltaTime);

        var currentBloomIntencity = bloomValue.intensity.value;
        bloomValue.intensity.value = Mathf.MoveTowards(currentBloomIntencity, bloomIntencityTarget, Time.deltaTime);
    }

    private void SetContrastTo25()
    {
        var cam2 = cameras2[camIdValue].GetComponent<Camera>();
        var roomNameTmp = cam2.cullingMask < 0 ? roomNames[camIdValue] : "NO DATA";
        roomName.text = roomNameTmp;

        colorAdjustmentsValue.contrast.value = 6.0f;
        canvasNoiseMaterial.color = new Color(255, 255, 255, 0.3f);

        canvasGroup.alpha = 1.0f;

        source.Stop();
    }

    public void ChangeCamera(int camId)
    {
        camIdValue = camId;
        redDot.SetActive(true);

        latestCam.SetActive(false);
        latestCam = cameras[camId];
        latestCam.SetActive(true);

        colorAdjustmentsValue.contrast.value = -100.0f;
        canvasNoiseMaterial.color = new Color(255, 255, 255, 1);

        grid.SetActive(false);
        showGridBtn.gameObject.SetActive(true);
        
        canvasGroup.alpha = 0.0f;

        var cam2 = cameras2[camId].GetComponent<Camera>();
        Camera.main.cullingMask = cam2.cullingMask;

        source.Play();
        Invoke(nameof(SetContrastTo25), Random.Range(0.2f, 0.6f));
    }

    public void TurnElectric(bool isOn)
    {
        postExplosureTarget = isOn ? 1.3f : -2.5f;
        bloomIntencityTarget = isOn ? 0.0f : 0.35f;

        if(!isOn)
        {
            Camera camera;

            var excludeNumber = 5;
            var numbers = new List<int>();

            for (int i = 0; i < 12; i++)
            {
                if (i != excludeNumber)
                {
                    numbers.Add(i);
                }
            }

            List<int> randomNumbers = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                int index = Random.Range(0, numbers.Count);
                randomNumbers.Add(numbers[index]);
                numbers.RemoveAt(index);
            }

            foreach(var index in randomNumbers)
            {
                camera = cameras2[index].GetComponent<Camera>();
                camera.cullingMask = 0;

                var camText = buttons[index].transform.GetChild(0).GetComponent<Text>();
                camText.text = "NO DATA";
            }

            camera = cameras2[camIdValue].GetComponent<Camera>();
            Camera.main.cullingMask = camera.cullingMask;
            roomName.text = camera.cullingMask < 0 ? roomNames[camIdValue] : "NO DATA";
        }
        else
        {
            foreach(var camera in cams2.GetComponentsInChildren<Camera>())
            {
                camera.cullingMask = -1;
            }

            var index = 0;
            foreach (var button in buttons)
            {
                button.transform.GetChild(0).GetComponent<Text>().text = roomNames[index];
                index++;
            }

            Camera.main.cullingMask = -1;
            roomName.text = roomNames[camIdValue];
        }
    }
}
