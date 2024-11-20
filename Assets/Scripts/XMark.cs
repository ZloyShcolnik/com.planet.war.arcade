using UnityEngine;

public class XMark : MonoBehaviour
{
    public static Vector3 Instant(Vector3 point)
    {
        var last = FindObjectOfType<XMark>();
        if(last != null)
        {
            Destroy(last.gameObject);
        }

        var instance = Instantiate(Resources.Load<GameObject>("xMark"));
        instance.transform.position = point + Vector3.up * 0.01f;

        return instance.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }

        Destroy(gameObject);
    }
}
