using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HoleManController : MonoBehaviour
{
    private bool isFading;
    private bool isInvisible;

    private GameObject body;
    private Material material;

    private Animator anim;
    private NavMeshAgent agent;

    private readonly int speedId = Animator.StringToHash("speed");
    private int currentWaypointIndex = 0;
    private bool skipWait;

    private float nextTime;
    [SerializeField] float stepRate;

    private GameObject[] waypoints;

    private void Awake()
    {
        body = transform.GetChild(0).gameObject;
        body.SetActive(false);

        material = body.GetComponent<SkinnedMeshRenderer>().material;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("holeManPoint");
        yield return new WaitForSeconds(Random.Range(10, 15));
        body.SetActive(true);

        StartCoroutine(Fade(false));

        if (waypoints.Length > 0)
        {
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    private void Update()
    {
        anim.SetFloat(speedId, agent.velocity.magnitude);

        var destination = agent.steeringTarget - transform.position;
        if (destination != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(destination), 5.0f * Time.deltaTime);
        }

        if (agent.velocity.magnitude > 0 && Time.time > nextTime && isInvisible)
        {
            nextTime = Time.time + stepRate;
            FootStepSfx.Instant();
        }
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].transform.position);
            yield return new WaitUntil(() => agent.hasPath);
            agent.stoppingDistance = 0;

            if (!isInvisible)
            {
                if (!isFading)
                {
                    StartCoroutine(Fade(false));
                }
            }

            while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
            {
                yield return null;
            }

            float waitTime = Random.Range(10.0f, 15.0f);
            float elapsedTime = 0f;

            while (elapsedTime < waitTime)
            {
                if (skipWait)
                {
                    if(!isFading)
                    {
                        StartCoroutine(Fade(true));
                    }

                    skipWait = false;
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private IEnumerator Fade(bool isOn)
    {
        isFading = true;

        var value = isOn ? 1.0f : -1.0f;
        var target = value * -1;

        while (value != target)
        {
            value = Mathf.MoveTowards(value, target, 1.5f * Time.deltaTime);
            material.SetFloat("_DissolveAmount", value);
            yield return null;
        }

        isInvisible = !isOn;
        isFading = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !isInvisible)
        {
            return;
        }

        agent.stoppingDistance = Vector3.Distance(transform.position, other.transform.position);
        skipWait = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !isInvisible)
        {
            return;
        }

        skipWait = false;
    }
}
