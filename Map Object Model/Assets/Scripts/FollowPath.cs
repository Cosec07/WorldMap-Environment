using System.Collections;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private Transform[] routes;
    private int routeToGo;
    private float tParam;
    private Vector3 objectPosition;
    private float speedModifier;
    private bool coroutineAllowed;
    private bool increaseSpeed;
    private int frame;

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.01f;
        coroutineAllowed = true;
        increaseSpeed = true;
        frame = 0;
    }

    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector3 p0 = routes[routeNum].GetChild(0).localPosition;
        Vector3 p1 = routes[routeNum].GetChild(1).localPosition;
        Vector3 p2 = routes[routeNum].GetChild(2).localPosition;
        Vector3 p3 = routes[routeNum].GetChild(3).localPosition;

        while (tParam <= 1)
        {
            frame++;
            if (frame >= 1)
            {
                frame = 0;
                if (increaseSpeed)
                {
                    speedModifier += 0.01f;
                }
                else
                {
                    speedModifier -= 0.01f;
                }
                increaseSpeed = !increaseSpeed;
            }

            tParam += Time.deltaTime * speedModifier;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            transform.localPosition = new Vector3(objectPosition.x, 0.35f, objectPosition.z);

            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
    }
}
