using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLakeRotation : MonoBehaviour
{
    public Transform target1;
    public Image target2;
    public GameObject xImg1;
    public GameObject xImg2;
    public Color defaultColor;
    public Color changeColor;


    private readonly Quaternion[] destinations = new Quaternion[2];
    private readonly WaitForSeconds delay = new WaitForSeconds(0.5f);
    private readonly float timeDelay = 10f;

    void Awake()
    {
        destinations[0] = Quaternion.Euler(Vector3.zero);
        destinations[1] = Quaternion.Euler(new Vector3(0f, 0f, 360f / 7f));
    }

    private void OnEnable()
    {
        StartCoroutine(RotationCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        target2.color = defaultColor;
        target1.rotation = destinations[0];
        xImg1.SetActive(false);
        xImg2.SetActive(false);
    }

    // 함수로 간편화하고 싶지만...
    // 반복적인 코루틴 내 코루틴 함수가 메모리 사용량을 많이 증가시킬 것 같음
    private IEnumerator RotationCoroutine()
    {
        float timeCount;

        while (true)
        {
            // target 1
            timeCount = 0f;
            while (target1.rotation != destinations[1])
            {
                target1.rotation = Quaternion.Slerp(target1.rotation, destinations[1], timeCount);
                timeCount += Time.deltaTime / timeDelay;
                yield return null;
            }
            target1.rotation = destinations[1];
            yield return delay;

            timeCount = 0f;
            while (target1.rotation != destinations[0])
            {
                target1.rotation = Quaternion.Slerp(target1.rotation, destinations[0], timeCount);

                timeCount += Time.deltaTime / timeDelay;
                yield return null;
            }
            target1.rotation = destinations[0];
            yield return delay;

            // target 2 (can't rotate)
            xImg1.SetActive(true);
            xImg2.SetActive(true);

            target2.color = defaultColor;
            Color tmp = defaultColor - changeColor;
            tmp /= 0.7f;

            for (int i = 0; i < 2; ++i)
            {
                while (target2.color.b > changeColor.b)
                {
                    target2.color -= tmp * Time.deltaTime;
                    yield return null;
                }
                target2.color = changeColor;

                while (target2.color.b < defaultColor.b)
                {
                    target2.color += tmp * Time.deltaTime;
                    yield return null;
                }
                target2.color = defaultColor;
            }

            xImg1.SetActive(false);
            xImg2.SetActive(false);
        }
    }
}
