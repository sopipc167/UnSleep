using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombBehavior : BlockBehavior //BlockBehavior를 상속받음으로서 블록으로서의 기능 + 폭발기능을 구현함
{
    int bombtype; //폭탄의 타입 0~3까지의 정수로 폭탄의 폭발범위및 색깔이 결정됨
    int[,] bombArr; //폭탄의 폭발범위를 담은 2차원 배열 각각의 폭탄의 폭발범위는 기획서 참고
    public int[,] BombArr { get { return bombArr; } } 
    public Sprite RB, RBS, BB, BBS, YB, YBS, WB, WBS; //각각 폭탄의 색깔에 맞는 스프라이트들
    bool isActive, inform; //각각 해당 폭탄이 존재(폭발 전)하는 지 알아보는 불 변수와, 커서가 폭탄 위에 위치하는지 알려주는 변수~☆ 찡긋
    
    public bool IsActive { get { return isActive; } set { isActive = value; } }
    public bool Inform { get { return inform; } set { inform = value; } }
    public int Bombtype //폭탄 타입을 게임매니저에서 받음과 동시에 색깔, 폭발범위를 그에 맞게 세팅한다.
    {
        get { return bombtype; }
        set
        {
            bombtype = value;
            if (bombtype == 0)
            {
                Defalut = RB;
                Selected = RBS;
                bombArr = new int[5, 5]{ 
                                   { 0,0,0,0,0 },
                                   { 0,0,0,0,0 },
                                   { 1,1,1,1,1 },
                                   { 0,0,0,0,0 },
                                   { 0,0,0,0,0 }};
            }
            else if (bombtype == 1)
            {
                Defalut = BB;
                Selected = BBS;
                bombArr = new int[5, 5]{ 
                                  { 0,0,0,0,0 },
                                  { 0,0,1,0,0 },
                                  { 0,1,1,1,0 },
                                  { 0,0,1,0,0 },
                                  { 0,0,0,0,0 }};
            }
            else if (bombtype == 2)
            {
                Defalut = YB;
                Selected = YBS;
                bombArr = new int[5, 5]{ 
                                  { 0,0,0,0,0 },
                                  { 1,0,0,0,1 },
                                  { 0,0,1,0,0 },
                                  { 1,0,0,0,1 },
                                  { 0,0,0,0,0 }};
            }
            else if (bombtype == 3)
            {
                Defalut = WB;
                Selected = WBS;
                bombArr = new int[5, 5]{ 
                                  { 1,0,0,0,1 },
                                  { 1,0,0,0,1 },
                                  { 1,0,1,0,1 },
                                  { 1,0,0,0,1 },
                                  { 1,0,0,0,1 } };
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        inform = false;
        isActive = true;
    }
    private void OnMouseEnter() //마우스 드가면
    {
        SpriteChange(true);
        if (GM.Raymode) //만약 마우스 입력을 받아야 되는 상태라면
            showArr(true); //폭발범위 보여줘
    }
    private void OnMouseOver() //마우스가 머무르면
    {
        if (GM.Raymode) //만약 마우스 입력을 받아야 되는 상태라면 마우스 버튼 클릭을 감지함
        {
            if (Input.GetMouseButtonDown(0) && Manager.GetComponent<Game_Manager>().getsnum() > 0) //클릭하면 스왑함
            {
                Select = true;
                showArr(false);
                Manager.GetComponent<Game_Manager>().Swap(gameObject);
            }
            if (Input.GetMouseButtonDown(1)) //오른쪽 클릭하면 폭발함
            {
                showArr(false);
                Manager.GetComponent<Game_Manager>().Boom((int)Location.x, (int)Location.y, gameObject, bombArr);
            }
            if (Input.GetMouseButtonDown(2)) //휠버튼 클릭은 폭발범위 회전
            {
                showArr(false);
                spin();
                showArr(true);
            }
        }
           
    }
    private void OnMouseExit() //마우스 나가면
    {
        SpriteChange(false);
        if (GM.Raymode) //만약 마우스 입력을 받아야 되는 상태라면 
        {
            render.sprite = Defalut;
            showArr(false);
        }
    }
    void spin() //폭발범위 회전함수
    {
        int tmp;
        for (int i = 0; i < 5; i++)
        {
            for (int j = i; j < 5; j++)
            {
                tmp = bombArr[j, i];
                bombArr[j, i] = bombArr[i, j];
                bombArr[i, j] = tmp;
            }
        }
    }
    public void showArr(bool val) //폭발범위를 보여주는 함수
    {
        inform = val;
        Manager.GetComponent<Game_Manager>().Redrawboard(Location.x, Location.y, bombArr, val);
    }
}
