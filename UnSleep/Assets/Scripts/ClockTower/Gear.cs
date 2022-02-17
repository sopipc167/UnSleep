using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Gear : MonoBehaviour
{
    public GameObject center;
    public GameObject edge;
    public GameObject Origin_Parent;
    public GameObject DoublingGear;

    public GameObject[] Gears;

    public bool Operating;
    public bool Static_Gear;
    public bool Start_Gear; //시작 기어 여부
    public bool in_Main_Panel;
    public bool Doubling = false;
    public bool Doub_Parent = false;
    public bool Doub_Child = false;
    public bool Undoing = false;
    //public bool OK = false;


    public int Gear_Order; //Start_Gear에서부터 센 순서 : 0이면 멈춤, 1부터 시작기어
    public int Gear_From ; //0이면 멈춤, 1부터 시작 기어, doubling으로 연결되어 오면 1씩 증가

    public bool RotateDirection = true; //회전방향 맞으면 true, 틀리면 false

    public int teeth_num; //이빨 개수
    public float radius;  //반지름 : 중심~끝점까지의 거리 
    public int rotation; //1 : 반시계방향 , -1: 시계방향
    public float rotate_speed=0f; //회전 속도

    public Collider2D[] col_gears;
    public Collider2D[] col_gears_d;

    public Image Gear_image;
    public Image mask_image;

    public Sprite[] masks;
    ClkSound clkSound;

    //public GameObject ok_gear;

    private void Start()
    {
        radius = Vector2.Distance(center.transform.position, edge.transform.position); //시작하면 직경 측정
        Gear_image = transform.GetChild(0).GetComponent<Image>();//이미지 가져옴
        if (Start_Gear) //시작기어면 100의 속도로 회전
        {
            Gear_Order = 1;
            Gear_From = 1;
        }
        Origin_Parent = transform.parent.gameObject;
        Gears = GameObject.FindGameObjectsWithTag("Gear");
        clkSound = GameObject.Find("EtcManager").GetComponent<ClkSound>();
    }

    private void Update()
    {
        if (Start_Gear) //시작기어는 항상 작동중
        {
            Operating = true;
            Gear_Order = 1;
            Gear_From = 1;

        }


        if (Operating) //작동중이면 회전
        {
            transform.Rotate(new Vector3(0f, 0f, rotation * rotate_speed) * Time.deltaTime);

            if (Doubling && Doub_Parent) 
            {
                if (DoublingGear.GetComponent<Gear>().Doubling && !DoublingGear.GetComponent<Gear>().Operating)
                    GearDoubling(this.gameObject, DoublingGear); //더블링 업데이트: 나는 정상작동 쟤는 Doubling = true고 나머지는 false
                else if (DoublingGear.GetComponent<Gear>().Doubling && DoublingGear.GetComponent<Gear>().Operating) //무한동력 방지
                    Isolation_Doubling(this.gameObject, DoublingGear);


            }

        }
        else
        {
            Gear_Order = 0; //작동x일때는 항상 0
            Gear_From = 0;
            rotate_speed = 0f;
        }



        if (Doubling && Doub_Parent)
            CheckDoublingDistance(this.gameObject, DoublingGear);

        if (in_Main_Panel && Operating &&!Doubling) //메인 패널에 있는 기어들만 인접 기어 체크함
            Check_Adjust_Gear();

        

        if (!Static_Gear) //동적 기어들 마스크 관리
        {
            if (this.GetComponent<Gear_Drag>().Draging && Operating) //드래그 중에는 멈춘다
            {
                False_Operating(this.gameObject);
            }
 

            if (Gear_From == 0)
                mask_image.sprite = masks[0];
            else if (Gear_From == 1)
                mask_image.sprite = masks[1];
            else if (Gear_From == 2)
                mask_image.sprite = masks[2];
            else if (Gear_From == 3)
                mask_image.sprite = masks[3];

            /*
            if (this.GetComponent<Gear_Drag>().Draging && OK)
            {
                ok_gear.SetActive(true);
            }
            else
            {
                ok_gear.SetActive(false);
            }
            */
        }

        

    }

    public void Check_Adjust_Gear() //인접기어 체크
    {

        //<--------------col_gears 확보------------------------->
        col_gears = Physics2D.OverlapCircleAll(center.transform.position, radius + 0f); //주변에 있는 콜라이더 인식
        List<Collider2D> tmp = new List<Collider2D>(col_gears); //위에 저거 리스트로 임시 저장
        for (int i= 0; i <col_gears.Length; i++) //인식된 콜라이더 개수만큼 반복
        {
            if (col_gears[i].gameObject == this.gameObject) //자기 자신은 뺌
            {
                tmp.RemoveAt(i);
            }
         }
        
        col_gears = tmp.ToArray(); //tmp를 배열로 변환시켜 다시 col_gears로


        if (!Start_Gear && col_gears.Length == 0) //시작기어도 아닌데 col_gears가 비었다? 
        {
            False_Operating(this.gameObject);
            Debug.Log("파바바박");

        }

        //<--------------col_gears 순회------------------------->


        for (int i = 0; i < col_gears.Length; i++) //유효한 기어들을 점검
          {
            int CorrectRotation = col_gears[i].GetComponent<Gear>().rotation;

            if (!col_gears[i].GetComponent<Gear>().in_Main_Panel) //메인 패널에 없으면 뛰어넘어요
                continue;

            if (!col_gears[i].GetComponent<Gear>().Static_Gear && col_gears[i].GetComponent<Gear_Drag>().Draging) //드래깅 중에도 뛰어넘어요 
                continue;


            GameObject col_center = col_gears[i].gameObject.GetComponent<Gear>().center; //센터
            float col_radius = col_gears[i].gameObject.GetComponent<Gear>().radius; //직경


               if (Vector3.Distance(center.transform.position, col_center.transform.position) <= (radius + col_radius) * 0.15f)
               {
                    //중심끼리 가까워졌을 때
                    if (!Doubling && !col_gears[i].gameObject.GetComponent<Gear>().Doubling) //둘 다 더블링X 면 더블링
                        GearDoubling(this.gameObject, col_gears[i].gameObject);
                }
               else if (Vector3.Distance(center.transform.position, col_center.transform.position) > (radius + col_radius) * 0.90f && Vector3.Distance(center.transform.position, col_center.transform.position) <= (radius + col_radius) * 1.05f)
               { //적정 거리 내에 있으면 

                    if (!col_gears[i].gameObject.GetComponent<Gear>().Operating) //실행x면 동력 전달
                    {
                        Power_Transmit(this.gameObject, col_gears[i].gameObject);
                        //col_gears[i].gameObject.GetComponent<Gear>().OK = true;

                    }
                    else //실행되고 있다면 방향 체크
                    {
                            if (rotation == col_gears[i].GetComponent<Gear>().rotation)
                            {
                             WrongRotation(col_gears[i].gameObject);

                            }
                     }
                }
                else if (Vector3.Distance(center.transform.position, col_center.transform.position) > (radius + col_radius) * 1.05f) //멀어지면
                {
                    if (col_gears[i].gameObject.GetComponent<Gear>().Gear_From > 0 && col_gears[i].gameObject.GetComponent<Gear>().Gear_From != Gear_From) //다른 from이면 무시
                        continue;


                    if (!col_gears[i].gameObject.GetComponent<Gear>().Start_Gear && Gear_From == col_gears[i].gameObject.GetComponent<Gear>().Gear_From) //from이 달라도 무시하자
                    { //시작기어가 아니면
                       // col_gears[i].gameObject.GetComponent<Gear>().OK = false;
                        False_Operating(col_gears[i].gameObject);
                        Debug.Log("호다다다닥");
                    //여기서 제 3의 톱니에게 검출되어서 멀다고 느껴서 멈추게 됨

                }
            }



        }
    }





    public void WrongRotation(GameObject gear)
    {
        //gear.GetComponent<Gear>().Gear_image.color = new Color(1f, 0f, 0f);
        Debug.Log("잘못된방향");
        //팅! 하는 소리로 튕겨 나오도록 -> 파지지직도 연출의 일종으로 써먹기
        clkSound.PlaySound(2);


        gear.GetComponent<Gear_Drag>().GoToStartPos();

        //gear.GetComponent<Gear>().Gear_image.color = new Color(1f, 1f, 1f);


    }



    public void CheckDoublingDistance(GameObject A, GameObject B)
    {
        Gear GearA = A.GetComponent<Gear>();
        Gear GearB = B.GetComponent<Gear>();

       // Debug.Log(Vector3.Distance(GearA.center.transform.position, GearB.center.transform.position));

        if (Vector3.Distance(GearA.center.transform.position, GearB.center.transform.position) > (GearA.radius + GearB.radius)*0.5f) //더블링 중인 두 기어가 멀어지면
        {
            //Debug.Log("더블링 멀어짐");
            UnDoGearDoubling(A, B);
        }

    }

    public void GearDoubling(GameObject A, GameObject B) //A에 B를 꽂아요~
    {
        //Debug.Log("Doubling"+A.name+"에"+B.name);

        Gear GearA = A.GetComponent<Gear>();
        Gear GearB = B.GetComponent<Gear>();

        if (GearA.Static_Gear)
           return;

        if (GearB.Static_Gear)
           return;

        if (GearA.Undoing || GearB.Undoing)
            return;



        GearA.Doubling = true;
        GearB.Doubling = true;
        GearB.Operating = true;

        GearA.DoublingGear = B;
        GearB.DoublingGear = A;

        GearA.Doub_Parent = true;
        GearB.Doub_Child = true;

        GearB.rotate_speed = GearA.rotate_speed;
        GearB.rotation = GearA.rotation; 
        GearB.Gear_Order = GearA.Gear_Order;

       

        GearB.Gear_From = GearA.Gear_From + 1;

        if (GearA.teeth_num < GearB.teeth_num)
            A.transform.parent.transform.SetAsLastSibling();
        else
            B.transform.parent.transform.SetAsLastSibling();


    }

    public void UnDoGearDoubling(GameObject A, GameObject B) //A에서 B를 빼요~
    {
        //Debug.Log("Doubling해제" + A.name + "에서" + B.name);


        Gear GearA = A.GetComponent<Gear>();
        Gear GearB = B.GetComponent<Gear>();

        GearA.Undoing = true;
        GearB.Undoing = true;


        GearA.Doubling = false;
        GearB.Doubling = false;
        GearB.Operating = false;

        GearA.Doub_Parent = false;
        GearB.Doub_Child = false;


        GearB.rotate_speed = 0f;
        GearB.rotation = 0;
        GearB.Gear_Order = 0;
        GearB.Gear_From = 0;

        Check_Order(GearB.Gear_Order);

        GearA.Undoing = false;
        GearB.Undoing = false;

    }

    public void Isolation_Doubling(GameObject A, GameObject B) //doubling 무한 동력 여부 확인 후 멈추도록
    {
        Gear GearA = A.GetComponent<Gear>();
        Gear GearB = B.GetComponent<Gear>();
        float radius_d;
        if (GearA.teeth_num > GearB.teeth_num)
            radius_d = GearA.radius;
        else
            radius_d = GearB.radius;


        col_gears_d = Physics2D.OverlapCircleAll(center.transform.position, radius_d + 20f); //주변에 있는 콜라이더 인식
        List<Collider2D> tmp = new List<Collider2D>(col_gears_d); //위에 저거 리스트로 임시 저장
        for (int i = 0; i < col_gears_d.Length; i++) //인식된 콜라이더 개수만큼 반복
        {
            if (col_gears_d[i].gameObject == this.gameObject) //자기 자신은 뺌
            {
                tmp.RemoveAt(i);
            }
        }
        col_gears_d = tmp.ToArray(); //tmp를 배열로 변환시켜 다시 col_gears로

        if (col_gears_d.Length == 1)
        {
            if (col_gears_d[0].gameObject == B)
            {
                False_Operating(A);
                False_Operating(B);
                Debug.Log("설마 여기겠어?");

                UnDoGearDoubling(A, B);
            }
        }

        for (int i = 0; i < col_gears_d.Length; i++) 
        {

            if (!col_gears_d[i].GetComponent<Gear>().in_Main_Panel) //메인 패널에 없으면 뛰어넘어요
                continue;

            if (!col_gears_d[i].GetComponent<Gear>().Static_Gear && col_gears_d[i].GetComponent<Gear_Drag>().Draging) //드래깅 중에도 뛰어넘어요 
                continue;

            if (col_gears_d[i].GetComponent<Gear>().Operating)
                continue;

            GameObject col_center_d = col_gears_d[i].gameObject.GetComponent<Gear>().center; //센터
            float col_radius_d = col_gears_d[i].gameObject.GetComponent<Gear>().radius; //직경

            if (GearA.teeth_num > GearB.teeth_num)
            {
                if (Vector3.Distance(GearA.center.transform.position, col_center_d.transform.position) <= (radius_d + col_radius_d)*1.05f)
                {
                    
                    
                    if (Vector3.Distance(GearA.center.transform.position, col_center_d.transform.position) > (radius_d + col_radius_d) * 0.90f)
                        Power_Transmit(GearA.gameObject, col_gears_d[i].gameObject);

                    if (Vector3.Distance(GearB.center.transform.position, col_center_d.transform.position) > (GearB.radius + col_radius_d) * 0.90f 
                        && Vector3.Distance(GearB.center.transform.position, col_center_d.transform.position) <= (GearB.radius + col_radius_d) * 1.05f)
                    {
                            Power_Transmit(GearB.gameObject, col_gears_d[i].gameObject);
                    }
                }
            }
            else
            {
                if (Vector3.Distance(GearB.center.transform.position, col_center_d.transform.position) <= (radius_d + col_radius_d) * 1.05f)
                {
                    if (Vector3.Distance(GearB.center.transform.position, col_center_d.transform.position) > (radius_d + col_radius_d) * 0.90f)
                        Power_Transmit(GearB.gameObject, col_gears_d[i].gameObject);

                    if (Vector3.Distance(GearA.center.transform.position, col_center_d.transform.position) > (GearA.radius + col_radius_d) * 0.90f
                        && Vector3.Distance(GearA.center.transform.position, col_center_d.transform.position) <= (GearA.radius + col_radius_d) * 1.05f)
                    {
                            Power_Transmit(GearA.gameObject, col_gears_d[i].gameObject);
                    }
                }

            }


        }



 
    }


    public void Power_Transmit(GameObject A, GameObject B) //A에서 B로 동력 전달
    {

        if (A.GetComponent<Gear>().Operating)
        {
            //Debug.Log("Power_Trasmit" + A.name + "에서" + B.name);

            B.GetComponent<Gear>().rotation = -1 * A.GetComponent<Gear>().rotation;
            B.GetComponent<Gear>().rotate_speed = A.GetComponent<Gear>().rotate_speed * ((float)A.GetComponent<Gear>().teeth_num / (float)B.GetComponent<Gear>().teeth_num);
            B.GetComponent<Gear>().Gear_Order = A.GetComponent<Gear>().Gear_Order + 1;
            B.GetComponent<Gear>().Gear_From = A.GetComponent<Gear>().Gear_From;
            B.GetComponent<Gear>().Operating = true;
        }
    }

    public void False_Operating(GameObject G)
    {
        G.GetComponent<Gear>().Operating = false;
        G.GetComponent<Gear>().rotation = 0;
        Check_Order(G.GetComponent<Gear>().Gear_Order);
        G.GetComponent<Gear>().Gear_Order = 0;
        G.GetComponent<Gear>().Gear_From = 0;
    }

    public void Check_Order(int init_order)  //init_order  = 멈추는 기어의 Gear_Order
    {
        List<Gear> tmpList = new List<Gear>();
        for (int i=0; i < Gears.Length; i++)
        {
            if (Gears[i].GetComponent<Gear>().Gear_Order > init_order && Gears[i].GetComponent<Gear>().Operating) //init보다 뒤쪽의 작동되는 기어들은 모두 해제
            {
                if (Gears[i].GetComponent<Gear>().Start_Gear)
                    continue;

                tmpList.Add(Gears[i].GetComponent<Gear>());

            }

        }

        Gear[] tmpArray = tmpList.OrderByDescending(x => x.Gear_Order).ToArray<Gear>();



        for (int i=0;i<tmpArray.Length;i++)
        {
            tmpArray[i].GetComponent<Gear>().Operating = false;
            tmpArray[i].GetComponent<Gear>().rotation = 0;
            tmpArray[i].GetComponent<Gear>().Gear_Order = 0;
        }
    }

}



