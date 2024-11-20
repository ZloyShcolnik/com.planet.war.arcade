using UnityEngine;

public class YmrController : MonoBehaviour
{
    private GameObject body;
    private Transform ymrPoints;

    private void Awake()
    {
        LeverController.OnTurnElectrick += LeverController_OnTurnElectrick;
        ymrPoints = GameObject.Find("ymrPoints").transform;

        body = transform.GetChild(0).gameObject;
        body.SetActive(false);
    }

    private void OnDestroy()
    {
        LeverController.OnTurnElectrick -= LeverController_OnTurnElectrick;
    }

    private void LeverController_OnTurnElectrick(bool isOn)
    {
        body.SetActive(!isOn);
        if(!isOn)
        {
            var randomPointId = Random.Range(0, ymrPoints.childCount);
            var randomPoint = ymrPoints.GetChild(randomPointId);

            transform.position = randomPoint.position;
            transform.right = -randomPoint.forward;
        }
    }
}
