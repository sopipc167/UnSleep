using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool flag;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Vector3 curPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 7f));
            float randVal = Random.Range(1f, 10f);
            float randTime = Random.Range(5f, 8f);
            flag = true;
            transform.position = curPos;
            while (transform.position.x < curPos.x + randVal)
            {
                transform.position += new Vector3(randVal / randTime * Time.deltaTime, 0f, 0f);
                if (flag)
                {
                    spriteRenderer.color += new Color(0, 0, 0, 1 / (randTime / 2) * Time.deltaTime);
                }
                else
                {
                    spriteRenderer.color -= new Color(0, 0, 0, 1 / (randTime / 2) * Time.deltaTime);
                }
                if (spriteRenderer.color.a > 0.99f) flag = false;
                yield return null;
            }
        }
    }
}
