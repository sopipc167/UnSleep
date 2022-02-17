using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Creator : MonoBehaviour //처음에 메쉬를 만들어주는 함수 (실험용) 지금은 아마 안 쓸 것이다. 만약 다른 방법으로 메쉬를 만들어야 할 때에 참고하길 바란다.
{
    void Start()
    {
        Vector3[] vertics = new Vector3[100];
        int k = 0;
        for (int i = 2; i >= -2; i--)
        {
            for (int j = -2; j <= 2; j++)
            {
                vertics[k] = new Vector3(j, i, 0f);
                k++;
                Debug.Log(k);
            }
        }
        k = 0;
        int[] triangles = new int[96];
        for (int i = 0; i < 20; i++)
        {
            if ((i-4)%5 != 0)
            {
                //상단
                triangles[k] = i;
                k++;
                triangles[k] = i+1;
                k++;
                triangles[k] = i+6;
                k++;
                //하단
                
                triangles[k] = i + 6;
                k++;
                triangles[k] = i + 5;
                k++;
                triangles[k] = i;
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
