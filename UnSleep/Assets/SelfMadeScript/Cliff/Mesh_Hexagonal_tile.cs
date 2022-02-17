using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Hexagonal_tile : MonoBehaviour //육각형 메쉬를 생성하는 실험을 했던 스크립트(아마 지금은 안쓸거)
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vertics = new Vector3[100];
        int k = 0;
        for (int i = 3; i >= -3; i--)
        {
            for (int j = -3; j <= 3; j++)
            {
                if(i % 3 == 0)
                {
                    vertics[k] = new Vector3(Mathf.Sqrt(3) * (2 * j + 1), i + (int)((i + 1) / 3), 0f);
                    k++;
                }
                else
                {
                    vertics[k] = new Vector3(Mathf.Sqrt(3) * (2 * j), i + (int)((i + 1) / 3), 0f);
                    k++;
                }
            }
        }
        k = 0;
        int[] triangles = new int[96];
        for (int i = 0; i < 20; i++)
        {
            if ((i - 4) % 5 != 0)
            {
                //좌단
                triangles[k] = i;
                k++;
                triangles[k] = i + 1;
                k++;
                triangles[k] = i + 6;
                k++;

                //좌중단
                triangles[k] = i + 6;
                k++;
                triangles[k] = i + 5;
                k++;
                triangles[k] = i;
                k++;

                //우중단
                triangles[k] = i;
                k++;
                triangles[k] = i + 1;
                k++;
                triangles[k] = i + 6;
                k++;

                //우단
                triangles[k] = i;
                k++;
                triangles[k] = i + 1;
                k++;
                triangles[k] = i + 6;
                k++;
            }
        }
        Mesh mes = new Mesh();
        mes.vertices = vertics;
        mes.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mes;

        Material mat = new Material(Shader.Find("Standard"));
        GetComponent<MeshRenderer>().material = mat;
        this.gameObject.AddComponent<MeshCollider>();
    }
}
