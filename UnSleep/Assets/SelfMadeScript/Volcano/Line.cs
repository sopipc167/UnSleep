using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour //지금은 안쓰는 라인렌더러로 선 긋는 스크립트 (아마 쓸 일이 없을 거임)
{
    LineRenderer line;
    public Game_Manager Game_Manager;
    GameObject[,] map;
    int idx;
    // Start is called before the first frame update
    void Start()
    {
        idx = 1;
        line=GetComponent<LineRenderer>();
        line.positionCount = idx;
    }
    public void Setmap(GameObject [,] map)
    {
        foreach (GameObject i in map)
        {
            if (i.CompareTag("Magma"))
            {
                line.SetPosition(0, i.transform.position);
                break;
            }
        }
    }
    public void DrawLine(int x,int y)
    {
        idx++;
        line.positionCount = idx;
        line.SetPosition(idx-1, map[x, y].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
