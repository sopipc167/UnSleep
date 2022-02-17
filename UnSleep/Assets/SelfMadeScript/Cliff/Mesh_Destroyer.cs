using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mesh_Destroyer : MonoBehaviour //메쉬를 지우는 스크립트, 삼각형, 사각형, 육각형 모양으로 지울 수 있고, 육각형의 경우 mawari 변수를 true로 설정하면 주변 6방향의 타일도 없어진다.
//시간이 없어서 일단 DelTri 에 대한 설명만 하겠다 나머지도 DelTri를 확장시킨 내용이니 보는데에 큰 어려움은 없을 것이라 믿는다.
{
    public bool Hexagonal;
    public bool mawari;
    public GameObject parent;
    public HexGrid Hex;
    // Start is called before the first frame update
    void Start()
    {
        Hexagonal = true;
        mawari = true;
        deleteTri(-1);
    }
    void delHex(Vector3 mpos) //육각 모양으로 지우기
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        mpos -= parent.transform.position;
        float tmp = mpos.z;
        mpos.z = mpos.y;
        mpos.y = tmp;
        
        float min = float.MaxValue;
        Vector3 minvec = new Vector3();
        for (int t = 0; t < Hex.cells.Length; t++)
        {
            if (Vector3.Distance(mpos, Hex.cells[t].transform.position) < min)
            {
                min = Vector3.Distance(mpos, Hex.cells[t].transform.position);
                minvec = Hex.cells[t].transform.position;
            }
        }

        Mesh m = transform.GetComponent<MeshFilter>().mesh;
        int[] j = new int[6];
        int k = 0;
        List<Vector3> vert = m.vertices.ToList();
        List<int> newTri = m.triangles.ToList();

        k = vert.IndexOf(minvec);
        for (int i = 0; i < 6; i++)
        {
            j[i] = newTri.IndexOf(k, i == 0 ? 0 : j[i]);
        }
        for (int i =0;i<6;i++)
        {
            if (j[i] % 3 == 0)
                newTri.RemoveRange(j[i], 3);
            else if (j[i] % 3 == 1)
                newTri.RemoveRange(j[i] - 1, 3);
            else if (j[i] % 3 == 2)
                newTri.RemoveRange(j[i] - 2, 3);
        }

        transform.GetComponent<MeshFilter>().mesh.triangles = newTri.ToArray();
        this.gameObject.AddComponent<MeshCollider>();
        this.gameObject.GetComponent<MeshCollider>().convex = true;
    }
    void delSquare(int id1, int id2) //사각형 모양으로 지우기
    {
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh m = transform.GetComponent<MeshFilter>().mesh;
        int[] oldTri = m.triangles;
        int[] newTri = new int[m.triangles.Length];

        int i = 0;
        int j = 0;
        Debug.Log(oldTri.Length);
        Debug.Log(newTri.Length);
        while (j < m.triangles.Length)
        {
            if (j != id1 * 3 && j!=id2*3)
            {
                newTri[i++] = oldTri[j++];
                newTri[i++] = oldTri[j++];
                newTri[i++] = oldTri[j++];
            }
            else
            {
                j += 3;
            }
        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTri;
        this.gameObject.AddComponent<MeshCollider>();
        this.gameObject.GetComponent<MeshCollider>().convex = true;
    }
    int FindTri(Vector3 v1, Vector3 v2,int notTriid) 
    {
        int[] tri = transform.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] vert = transform.GetComponent<MeshFilter>().mesh.vertices; 
        int j = 0;
        while (j< tri.Length) 
        {
            if (j/3 != notTriid)
            {
                if (vert[tri[j]] == v1 && (vert[tri[j + 1]] == v2 || vert[tri[j + 2]] == v2))
                    return j / 3;
                else if (vert[tri[j]] == v2 && (vert[tri[j + 1]] == v1 || vert[tri[j + 2]] == v1))
                    return j / 3;
                else if (vert[tri[j+1]] == v2 && (vert[tri[j]] == v1 || vert[tri[j + 2]] == v1))
                    return j / 3;
                else if (vert[tri[j+1]] == v1 && (vert[tri[j]] == v2 || vert[tri[j + 2]] == v2))
                    return j / 3;
            }
            j += 3;
        }
        return -1;
    }
    int findVert(Vector3 v) //메쉬의 정점찾기
    {
        Vector3[] vert = transform.GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i < vert.Length; i++)
        {
            if (vert[i] == v)
                return i;
        }
        return -1;
    }
    void deleteTri(int index) //그냥 삼각형 모양(기본 메쉬 구성요소)지우기
    {
        //간단히 보자면 메쉬는 정점들과 정점들을 잇는 삼각형으로 되어있는데, 이를 배열의 형태로 저장한다.
        //여기에서 우리는 지우고자 하는 삼각형의 정보만 지운 새로운 배열을 만들어서 메쉬에 그냥 덮어 씌울거다.
        Destroy(this.gameObject.GetComponent<MeshCollider>()); //메쉬 콜라이더 삭제
        Mesh m = transform.GetComponent<MeshFilter>().mesh; 
        int[] oldTri = m.triangles; //메쉬 삼각형 배열
        int[] newTri = new int[m.triangles.Length]; //새로 메쉬의 삼각형 배열

        int i = 0;
        int j = 0;
        //Debug.Log(oldTri.Length);
        //Debug.Log(newTri.Length);
        while (j < m.triangles.Length) //삼각 배열 끝까지
        {
            if (j != index*3) //만약 우리가 찾는 (지우려하는) 삼각형 배열이 맞다면
            {
                newTri[i++] = oldTri[j++];//세번 건너 뛰어서 새로운 삼각형 배열에 포함시키지 않음
                newTri[i++] = oldTri[j++];
                newTri[i++] = oldTri[j++];
            }
            else//새로운 삼각형 배열에 복사
            {
                j += 3;
            }
        }
        transform.GetComponent<MeshFilter>().mesh.triangles = newTri; //새로 만든 삼각형 배열 덮어쓰기
        this.gameObject.AddComponent<MeshCollider>(); 
        this.gameObject.GetComponent<MeshCollider>().convex = true; //사실 이건 뭔지 모르겠는데 안하면 콜라이더가 작동을 잘 안하더라
    }
    public void DetectCall()//메쉬 삭제 신호를 받아서 삭제를 진행해 주는 함수
    {
            Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Hexagonal && mawari)
            {
                delHex(mpos);
                delHex(mpos + new Vector3(HexMetrics.innerRadius * 2, 0, 0));
                delHex( mpos + new Vector3(-HexMetrics.innerRadius * 2, 0, 0));
                delHex( mpos + new Vector3(HexMetrics.innerRadius, HexMetrics.outerRadius * 1.5f, 0));
                delHex( mpos + new Vector3(HexMetrics.innerRadius, -HexMetrics.outerRadius * 1.5f, 0));
                delHex( mpos + new Vector3(-HexMetrics.innerRadius, HexMetrics.outerRadius * 1.5f, 0));
                delHex( mpos + new Vector3(-HexMetrics.innerRadius, -HexMetrics.outerRadius * 1.5f, 0));
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    if (!Hexagonal)
                        deleteTri(hit.triangleIndex);
                    else if (hit.triangleIndex != -1)
                        if (!mawari)
                            delHex(mpos);
                }
            }
    }
}
               //Delete Sqare의 경우 아래 코드를 추가해야 할 수 있음
               /*int[] Tris = transform.GetComponent<MeshFilter>().mesh.triangles;
               Vector3[] vert = transform.GetComponent<MeshFilter>().mesh.vertices;
               Vector3 p0 = vert[Tris[hitTri * 3 + 0]];
               Vector3 p1 = vert[Tris[hitTri * 3 + 1]];
               Vector3 p2 = vert[Tris[hitTri * 3 + 2]];

               float edge1 = Vector3.Distance(p0, p1);
               float edge2 = Vector3.Distance(p0, p2);
               float edge3 = Vector3.Distance(p2, p1);

               Vector3 shared1;
               Vector3 shared2;
               if(edge1>edge2 && edge1>edge3)
               {
                   shared1 = p0;
                   shared2 = p1;
               }
               else if(edge2>edge1&& edge2>edge3)
               {
                   shared1 = p0;
                   shared2 = p2;
               }
               else
               {
                   shared1 = p1;
                   shared2 = p2;
               }
               int v1 = findVert(shared1);
               int v2 = findVert(shared2);
               delSquare(hitTri, FindTri(vert[v1], vert[v2], hitTri));

                }*/
