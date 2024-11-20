using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BoyController : MonoBehaviour
{
    public static BoyController Inststance;

    private Vector3 agentDestination;
    private NavMeshAgent agent;
    private Animator anim;

    private bool isSitting;

    private readonly int walkId = Animator.StringToHash("speed");
    private readonly int sitId = Animator.StringToHash("sit");

    private float nextTime;
    [SerializeField] Button chairInteract;
    [SerializeField] float stepRate;

    private void Awake()
    {
        Inststance = this;

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat(walkId, agent.velocity.magnitude);

        if (Camera.main.cullingMask == 0)
        {
            return;
        }

        if (ChairController.IsProcessing || ShowGridEventHandler.isProcessing)
        {
            return;
        }

        if (CameraChanger.Instance.grid.activeSelf || LeverController.isProcessing)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(isSitting)
            {
                chairInteract.onClick.Invoke();
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider && hit.collider.CompareTag("ground"))
                {
                    agentDestination = XMark.Instant(hit.point);
                    if(agent.enabled)
                    {
                        agent.destination = agentDestination;
                    }
                }
            }
        }

        var destination = agent.steeringTarget - transform.position;
        if (destination != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destination), 2.0f * Time.deltaTime);
        }

        if(agent.velocity.magnitude > 0 && Time.time > nextTime)
        {
            nextTime = Time.time + stepRate;
            FootStepSfx.Instant();
        }
    }

    public void EnableEye(bool isOn)
    {
        var light = transform.GetComponentInChildren<Light>();
        light.enabled = !isOn;
    }

    public void StandToSit(bool isSitting, Transform pivot, Transform toSeePoint)
    {
        anim.SetFloat(walkId, 0);
        agent.enabled = false;

        this.isSitting = isSitting;
        transform.position = pivot.position;

        var direction = (toSeePoint.position - transform.position).normalized;
        transform.forward = direction;

        anim.SetBool(sitId, isSitting);

        if (!isSitting)
        {
            Invoke(nameof(EnableAgent), 2.4f);
        }
    }

    private void EnableAgent()
    {
        agent.enabled = true;
        agent.destination = agentDestination;
    }
}
