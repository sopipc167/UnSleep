using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockBehavior : MonoBehaviour //블럭을 담당하는 스크립트 (맵에 있는 모든 물체가 블럭이라고 생각하면 되고, 블럭은 마우스를 가져다대면 모습을 바꾸고, 클릭으로 스왑할 수 있는 성질을 지니고 있다.)
{
    public Sprite Defalut, Selected; //기본스프라이트와 선택 했을 시에 나타나는 스프라이트
    public SpriteRenderer render; //블록 오브젝트이 렌더러
    public GameObject panel, Manager, swp; //게임매니저 (매니저를 통해 다른 블록들과 통신한다
    protected Game_Manager GM;
    protected bool select, isMagma; //블록의 상태를 체크하는 불 변수 각각 선택된 상태인가, 마그마 상태인가, 스왑 가능한가
    public bool swapable = false;
    public bool Select{ get { return select; } set { select = value; } }
    public bool IsMagma { get { return isMagma; } set { isMagma = value; } }
    Vector2 location; //블럭의 좌표, 그러니까 블럭사이의 상대적인 위치를 나타내는 변수
    protected bool inarea, exploding; //각각, 이 블럭이 폭탄의 폭발 범위를 표시해야 하는가를 나타내는 불 변수와, 이 블럭이 폭발 중 인지를 나타내는 불 변수
    public Sprite X; // 까먹음
    public Sprite[] Bom = new Sprite[6], flame = new Sprite[3]; //각각 폭발과 마그마상태의 애니메이션을 나타내기 위한 스프라이트들

    public Vector2 Location;
    public int blockNum;
    public bool Inarea { get { return inarea; } set { inarea = value; } }
    public bool Exploding { get { return exploding; } private set { } }

    protected virtual void Start()
    {
        render = gameObject.GetComponent<SpriteRenderer>();
        Manager = GameObject.Find("GameManagement");
        GM=Manager.GetComponent<Game_Manager>();
        Select = false;
        render.drawMode = SpriteDrawMode.Sliced; //이곳부터 이하 한줄의 스키립트를 통해서 스프라이트 이미지를 알맞은 크기로 게임오브젝트에 욱여넣어줌
        render.size -= new Vector2(0.45f/Defalut.rect.width/30f, 0.45f/Defalut.rect.height/30f);
        render.sprite = Defalut;
        exploding = false;
        if (isMagma) //마그마 상태일 경우 마그마 애니메이션 실행
            StartCoroutine(Mgm());
    }
    public void swapsprite()
    {
        swp.SetActive(!swp.activeSelf);
    }
    private void OnMouseEnter() //마우스가 들어 왔을 경우 1번에 한해
    {
        //SpriteChange(true);
    }
    private void OnMouseOver() //마우스가 계속 들어와 있는 경우
    {
        if (MemoManager.isMemoOn) return;
        if (GM.Raymode && !isMagma) //마우스 입력을 받는 상태이고 마그마 상태가 아니라면 
        {
            if (GM.getsnum() > 0)  //스왑할 수 있는 횟수가 남아있을 경우 버튼입력을 감지하고
                if (Input.GetMouseButtonDown(0) && swapable) //버튼입력이 감지되면 스왑 진행
                {
                    select = true;
                    Manager.GetComponent<Game_Manager>().Swap(gameObject);
                }
        }
    }
    private void OnMouseExit() //마우스가 나갈 경우
    {
        SpriteChange(false);
    }
    public void SpriteChange(bool a)
    {
        if (GM.Raymode && !isMagma)
        {
            if (a)
            {
                render.sprite = Selected;
                panel.SetActive(false);
            }
            else
            {
                render.sprite = Defalut;
                render.color = Color.white;
                panel.SetActive(false);
            }
        }
    }
    public void Selection() //이건 왜있지?
    {
        if (!exploding)
            panel.SetActive(true);
    }
    public IEnumerator Blast() //폭발 애니메이션
    {
        panel.SetActive(false);
        exploding = true;
        for (int i = 0; i < 6; i++)
        {
            render.sprite = Bom[i];
            yield return new WaitForSeconds(0.06f);
        }
        this.gameObject.SetActive(false);
        yield return null;
    }
    public IEnumerator Mgm() //마그마 애니메이션
    {
        int i = 0, k = 1;
        while (true)
        {
            if (i >= 2)
                k = -1;
            else if (i <= 0)
                k = 1;
            render.sprite = flame[i];
            i += k;
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void Setmagma() //마그마 상태로 변경 될 경우 설정을 담당
    {
        isMagma = true;
        this.gameObject.SetActive(true);
        Start();
        GM.GetMagmainfo((int)Location.x, (int)Location.y);
    }
}