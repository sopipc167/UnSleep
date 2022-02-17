using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Fog_of_war : MonoBehaviour //타일맵의 생성과 여러 동작들을 관장하는 스크립트
{
    public Tilemap tilemap,FogOfWar; //안개를 담당하는 타일맵과 일반 맵 타일맵
    public GameObject goCube,gameover; //타일 맵 위의 플레이어 캐릭터(잠재우미)와 게임 오버 UI
    public Mesh_Destroyer md; //안개 메쉬를 지워주는 스크립트
    Vector3 playerpos; //현재 플레이어의 위치
    bool ismoving; //현재 플레이어가 이동 중인지 판별하는 불 변수
    GUIStyle style; //임시 GUI표시를 위한 GUIStyle
    Rect rect; //이것도 위에거랑 비슷함
    int num; //걷힌 안개 타일의 개수
    private void Awake() //GUI세팅
    {
        int w = Screen.width, h = Screen.height;

        rect = new Rect(0, h / 5, w, h * 4 / 100);
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 5 / 100;
        style.normal.textColor = Color.red;
    }
    private void Start() 
    {
        playerpos = new Vector3(0, 0, -2);
        ismoving = false;
        num = 0;
        fogOFwar();
    }
    public bool getismoving() { return ismoving; }
    IEnumerator Move(Vector3 target, Vector3 src) //클릭한 타일의 위치로 잠재우미를 회전시키고, 이동시키는 함수
    {
        ismoving = true;
        target -= new Vector3(0, 0, 1);
        this.goCube.transform.rotation = target.x > src.x ? Quaternion.Euler(0, 0, 360-Vector3.Angle(Vector3.up, target - src)) : Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, target - src)); //오브젝트의 Vector3.uo방향이 클릭한 곳을 응시하도록 회전
        float d = Vector3.Distance(goCube.transform.position, target);
        if (d == 0) d = 1;
        float disOld = 100000;
        while (true) //클릭한 지점의 타일좌표에 도착할때 까지 지속
        {
            this.goCube.transform.Translate(Vector3.up * Time.deltaTime * d * 2.5f); //이동
            var distance = Vector3.Distance(goCube.transform.position, target);
            yield return null;
            if (disOld < distance)
                break;
            disOld = distance;
        }
        this.goCube.transform.rotation = new Quaternion(0, 0, 0, 0);
        this.goCube.transform.position = target;
        playerpos = target;
        reveal();
        ismoving = false;
    }
    private void OnMouseOver() //안개타일을 클릭할수 있도록 하는 함수
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue, 3.5f);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector3.zero);
        if (FogOfWar == hit.transform.GetComponent<Tilemap>())
        {
            int x, y;
            x = this.FogOfWar.WorldToCell(ray.origin).x; //월드 좌표를 셀좌표로 변환
            y = this.FogOfWar.WorldToCell(ray.origin).y;
            Vector3Int v3Int = new Vector3Int(x, y, 0);
            if (Input.GetMouseButtonDown(0) && ismoving == false)// 플레이어가 이미 움직이고 있는 동안에는 마우스가 안먹음
            {
                StartCoroutine(this.Move(this.tilemap.CellToWorld(v3Int), playerpos));
                md.DetectCall(); //메쉬 삭제 신호 전달
                num++;
                if (x == 1 && y == 1) //지뢰 타일 밟으면 게임오버됨
                    gameover.SetActive(true); 
            }
        }
    }
    private void reveal() //클릭한 안개타일을 없애주는 함수
    {
        Vector3Int ppos = this.tilemap.WorldToCell(playerpos);
        this.FogOfWar.SetTile(ppos, null);
    }
    private void fogOFwar() //플레이어가 위치한곳 주변 타일 6개를 없애주는 함수
    {
        Vector3Int ppos = this.tilemap.WorldToCell(playerpos);
        for (int ydel = -1; ydel <= 1; ydel++)
        {
            for (int xdel = -1; xdel <= 1; xdel++)
            {
                if (ppos.y % 2 == 0 && Mathf.Abs(ydel) != 0 && xdel == 1)
                {
                    continue;
                }
                else if (Mathf.Abs(ppos.y) % 2 == 1 && Mathf.Abs(ydel) != 0 && xdel == -1)
                {
                    continue;
                }
                this.FogOfWar.SetTile(ppos + new Vector3Int(xdel, ydel, 0), null);
            }
        }
    }
    private void OnGUI() //GUI 보여주는 함수, 걷힌 안개의 량을 막대 형태로 알려줌
    {
        string txt = "걷힌 안개 : " + num.ToString();
        if (num>0)
            GUI.Box(new Rect(100, 300, 100, num * 10)," ");
        GUI.Label(rect,txt,style);
    }
}