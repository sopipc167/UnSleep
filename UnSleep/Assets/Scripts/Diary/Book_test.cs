using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum FilpMode
{
    RightToLeft,
    LeftToRight
}
[ExecuteInEditMode]
public class Book_test : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField]
    RectTransform BookPanel;
    public Sprite background;
    public Image[] bookPages;
    //public Image[] bookPages2;
    public bool interactable = true;
    public bool enableShadowEffect = true;
    public int currentPage = 0;
    public AudioClip pageFlipSE;

    public int TotalPageCount
    {
        get { return bookPages.Length; }
    }
    public Vector3 EndBottomLeft
    {
        get { return ebl; }
    }
    public Vector3 EndBottomRight
    {
        get { return ebr; }
    }
    public float Height
    {
        get
        {
            return BookPanel.rect.height;
        }
    }

    // Image1~4는 6개의 고정된 오브젝트의 이미지를 중복해서 가져와서 사용
    // 인스펙터에는 여기다가만 넣어두고, 얘를 리스트에 넣어서 사용
    public Image INTRO;
    public Image IMG1;
    public Image IMG2;
    public Image IMG3;
    public Image IMG4;
    public Image END;



    public Image ClippingPlane; // 넘기는 페이지 뒷 페이지
    public Image NextPageClip; // 다음 페이지
    public Image Shadow; // 오른쪽에서 왼쪽으로 넘어갈 때 그림자
    public Image ShadowLTR; // 왼쪽에서 오른쪽으로 넘어갈 때 그림자
    public Image Left;  // 넘기는 페이지 앞 이미지
    public Image LeftNext; // 넘기는 페이지 전 페이지
    public Image Right; // 넘기는 페이지 뒤 이미지
    public Image RightNext; // 넘기는 페이지 다음 페이지
    public Image RightSpot;
    public Image LeftSpot;
    public Image stack_R;
    public Image stack_L;
    public Image Mask_R;
    public Image Mask_L;
    public Sprite LeftPage;
    public Sprite RightPage;
    public RectTransform Page;
    public UnityEvent OnFlip;
    float radius1, radius2; // 페이지 가로 길이, 페이지 대각선

    Vector3 sb;
    Vector3 st;
    Vector3 c;
    Vector3 ebr;
    Vector3 ebl;
    Vector3 f;
    bool pageDragging = false;
    public bool drag;
    FlipMode mode;

    public Auto auto;
    public Cover cover;

    public Pages pages;
    public int[] epiID;
    public int epiIndex;


    void Start()
    {
        if (!canvas) canvas = GetComponentInParent<Canvas>(); //캔버스
        //if (!auto) auto = GetComponent<Auto>();
        if (!canvas) Debug.LogError("Book should be a  child to canvas");

        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        bookPages = updateBookPages();
        UpdateImages();
        CalcCurllCriticalPoints(); // 필요한 좌표 대입

        float pageWidth = BookPanel.rect.width / 2.0f; // 페이지 넓이
        float pageHeight = BookPanel.rect.height; // 페이지 높이
        NextPageClip.rectTransform.sizeDelta = new Vector2(pageHeight, pageHeight + pageHeight * 2);
        ClippingPlane.rectTransform.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);

        float hyp = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight); // 페이지 대각선 길이
        float shadowPageHeight = pageWidth / 2 + hyp; // 그림자 길이

        if(currentPage == 0)
        {
            Mask_L.gameObject.SetActive(false);
        }

        Shadow.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        Shadow.rectTransform.pivot = new Vector2(1, (pageWidth / 2) / shadowPageHeight); // 넘길 때 페이지에 잘 적용될 수 있도록 pivot 변경

        ShadowLTR.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        ShadowLTR.rectTransform.pivot = new Vector2(0, (pageWidth / 2) / shadowPageHeight);

        epiIndex = -1;

        SoundManager.Instance.PlayBGM("EsthersWaltz");
    }

    void UpdateImages()
    {
        // currentPage는 RightNext 기준
        if(currentPage > 0 && currentPage <= bookPages.Length)
        {
            LeftNext.sprite = LeftPage;
            if(currentPage >= 4)
            {
                bookPages[currentPage - 3].gameObject.SetActive(false);
                bookPages[currentPage - 3].transform.SetParent(Page, true);
            }
            //bookPages[currentPage - 1].transform.localScale = new Vector3(1, 1, 1);
            bookPages[currentPage - 1].transform.SetParent(LeftNext.transform, true);
            bookPages[currentPage - 1].gameObject.SetActive(true);
            bookPages[currentPage - 1].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage - 1].rectTransform.pivot = new Vector2(0, 0);
            bookPages[currentPage - 1].transform.position = LeftNext.transform.position;
        }
        else if(currentPage == 0)
        {
            LeftNext.sprite = background;
        }

        if(currentPage >= 0 && currentPage < bookPages.Length)
        {
            RightNext.sprite = RightPage;
            if(mode == FlipMode.RightToLeft)
            {
                if(currentPage > 0)
                {
                    bookPages[currentPage - 2].gameObject.SetActive(false);
                    bookPages[currentPage - 2].transform.SetParent(Page, true);
                }else if(currentPage == 0)
                {
                    bookPages[currentPage + 2].gameObject.SetActive(false);
                    bookPages[currentPage + 2].transform.SetParent(Page, true);
                }
            }
            else
            {
                if(currentPage < bookPages.Length - 2)
                {
                    bookPages[currentPage + 2].gameObject.SetActive(false);
                    bookPages[currentPage + 2].transform.SetParent(Page, true);
                }
            }

            //bookPages[currentPage].transform.localScale = new Vector3(1, 1, 1);
            bookPages[currentPage].transform.SetParent(RightNext.transform, true);
            bookPages[currentPage].gameObject.SetActive(true);
            bookPages[currentPage].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage].rectTransform.pivot = new Vector2(0, 0);
            bookPages[currentPage].transform.position = RightNext.transform.position;
        }
        else if(currentPage == bookPages.Length)
        {
            RightNext.sprite = background;
            bookPages[currentPage - 2].gameObject.SetActive(false);
            bookPages[currentPage - 2].transform.SetParent(Page, true);
        }
    }

    private void CalcCurllCriticalPoints()
    {
        sb = new Vector3(0, -BookPanel.rect.height / 2);
        ebr = new Vector3(BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        ebl = new Vector3(-BookPanel.rect.width / 2, -BookPanel.rect.height / 2);
        st = new Vector3(0, BookPanel.rect.height / 2);
        radius1 = Vector2.Distance(sb, ebr);
        float pageWidth = BookPanel.rect.width / 2.0f;
        float pageHeight = BookPanel.rect.height;
        radius2 = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
    }

    public Vector3 transformPoint(Vector3 mouseScreenPos) // WorldPos -> LocalPos
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 mouseWorldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, canvas.planeDistance));
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseWorldPos);

            return localPos;
        }
        else if (canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 globalEBR = transform.TransformPoint(ebr);
            Vector3 globalEBL = transform.TransformPoint(ebl);
            Vector3 globalSt = transform.TransformPoint(st);
            Plane p = new Plane(globalEBR, globalEBL, globalSt);
            float distance;
            p.Raycast(ray, out distance);
            Vector2 localPos = BookPanel.InverseTransformPoint(ray.GetPoint(distance));
            return localPos;
        }
        else
        {
            Vector2 localPos = BookPanel.InverseTransformPoint(mouseScreenPos); // 월드 위치 벡터를 로컬로 바꾼다.
            return localPos;
        }
    }

    void Update()
    {
        if(pageDragging && interactable)
        {
            UpdateBook();
        }
    }

    public void UpdateBook()
    {
        f = Vector3.Lerp(f, transformPoint(Input.mousePosition), Time.deltaTime * 10);
        if (mode == FlipMode.RightToLeft)
            UpdateBookRTLToPoint(f);
        else
            UpdateBookLTRToPoint(f);
    }
    public void UpdateBookLTRToPoint(Vector3 followLocation)
    {
        mode = FlipMode.LeftToRight;
        f = followLocation;
        ShadowLTR.transform.SetParent(ClippingPlane.transform, true);
        ShadowLTR.transform.localPosition = new Vector3(0, 0, 0);
        ShadowLTR.transform.localEulerAngles = new Vector3(0, 0, 0);
        Left.transform.SetParent(ClippingPlane.transform, true);

        Right.transform.SetParent(BookPanel.transform, true);
        Right.transform.localEulerAngles = Vector3.zero;
        LeftNext.transform.SetParent(BookPanel.transform, true);

        c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(c, ebl, out t1);
        //0 < T0_T1_Angle < 180
        clipAngle = (clipAngle + 180) % 180;

        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
        ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

        Left.transform.position = BookPanel.TransformPoint(c);
        float C_T1_dy = t1.y - c.y;
        float C_T1_dx = t1.x - c.x;
        float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
        Left.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - 90 - clipAngle);

        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
        NextPageClip.transform.position = BookPanel.TransformPoint(t1);
        LeftNext.transform.SetParent(NextPageClip.transform, true);
        Right.transform.SetParent(ClippingPlane.transform, true);
        Right.transform.SetAsFirstSibling();

        ShadowLTR.rectTransform.SetParent(Left.rectTransform, true);
    }
    public void UpdateBookRTLToPoint(Vector3 followLocation)
    {
        //Debug.Log(currentPage);
        mode = FlipMode.RightToLeft;
        f = followLocation;
        Shadow.transform.SetParent(ClippingPlane.transform, true);
        Shadow.transform.localPosition = Vector3.zero;
        Shadow.transform.localEulerAngles = Vector3.zero;
        Right.transform.SetParent(ClippingPlane.transform, true);

        Left.transform.SetParent(BookPanel.transform, true);
        Left.transform.localEulerAngles = Vector3.zero;
        RightNext.transform.SetParent(BookPanel.transform, true);

        c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(c, ebr, out t1);
        if (clipAngle > -90) clipAngle += 180;

        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);
        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

        //page position and angle
        Right.transform.position = BookPanel.TransformPoint(c);
        float C_T1_dy = t1.y - c.y;
        float C_T1_dx = t1.x - c.x;
        float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
        Right.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - (clipAngle + 90));

        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        NextPageClip.transform.position = BookPanel.TransformPoint(t1);
        RightNext.transform.SetParent(NextPageClip.transform, true);
        Left.transform.SetParent(ClippingPlane.transform, true);
        Left.transform.SetAsFirstSibling();

        Shadow.rectTransform.SetParent(Right.rectTransform, true);
    }
    private Vector3 Calc_C_Position(Vector3 followLocation)
    {
        Vector3 c;
        f = followLocation;
        float F_SB_dy = f.y - sb.y;
        float F_SB_dx = f.x - sb.x;
        float F_SB_Angle = Mathf.Atan2(F_SB_dy, F_SB_dx);
        Vector3 r1 = new Vector3(radius1 * Mathf.Cos(F_SB_Angle), radius1 * Mathf.Sin(F_SB_Angle), 0) + sb;

        float F_SB_distance = Vector2.Distance(f, sb);
        if (F_SB_distance < radius1)
            c = f;
        else
            c = r1;
        float F_ST_dy = c.y - st.y;
        float F_ST_dx = c.x - st.x;
        float F_ST_Angle = Mathf.Atan2(F_ST_dy, F_ST_dx);
        Vector3 r2 = new Vector3(radius2 * Mathf.Cos(F_ST_Angle), radius2 * Mathf.Sin(F_ST_Angle), 0) + st;

        float C_ST_distance = Vector2.Distance(c, st);
        if (C_ST_distance > radius2)
            c = r2;
        return c;
    }
    private float CalcClipAngle(Vector3 c, Vector3 bookCorner, out Vector3 t1)
    {
        Vector3 t0 = (c + bookCorner) / 2;
        float T0_CORNER_dy = bookCorner.y - t0.y;
        float T0_CORNER_dx = bookCorner.x - t0.x;
        float T0_CORNER_Angle = Mathf.Atan2(T0_CORNER_dy, T0_CORNER_dx);
        float T0_T1_Angle = 90 - T0_CORNER_Angle;

        float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
        T1_X = normalizeT1X(T1_X, bookCorner, sb);
        t1 = new Vector3(T1_X, sb.y, 0);

        float T0_T1_dy = t1.y - t0.y;
        float T0_T1_dx = t1.x - t0.x;
        T0_T1_Angle = Mathf.Atan2(T0_T1_dy, T0_T1_dx) * Mathf.Rad2Deg;
        return T0_T1_Angle;
    }
    private float normalizeT1X(float t1, Vector3 corner, Vector3 sb)
    {
        if (t1 > sb.x && sb.x > corner.x) // 접힘점이 페이지를 넘어가지 못하도록 방지
            return sb.x;
        if (t1 < sb.x && sb.x < corner.x)
            return sb.x;
        return t1;
    }

    public void DragRightPageToPoint(Vector3 point)
    {
        //Debug.Log("오른쪽 넘김");
        if (currentPage >= bookPages.Length) return;
        if (currentPage > 0) pages.isChange = false;
        if (auto.isFlipping) interactable = false;
        pageDragging = true;
        drag = true;
        mode = FlipMode.RightToLeft;
        f = point;

        if (currentPage == (bookPages.Length - 2))
        {
            Mask_R.gameObject.SetActive(false);
            RightNext.sprite = background;
        }

        stack_R.transform.localPosition += new Vector3(-2.5f, 2.5f, 0);

        NextPageClip.rectTransform.pivot = new Vector2(0, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

        Left.gameObject.SetActive(true);
        Left.rectTransform.pivot = new Vector2(0, 0);
        Left.transform.position = RightNext.transform.position;
        Left.transform.eulerAngles = new Vector3(0, 0, 0);
        if(currentPage < bookPages.Length)
        {
            if (currentPage != 0)
            {
                bookPages[currentPage - 2].gameObject.SetActive(false);
                bookPages[currentPage - 2].transform.SetParent(Page, true);
            }
            bookPages[currentPage].transform.localScale = new Vector3(1, 1, 1);
            bookPages[currentPage].gameObject.SetActive(true);
            bookPages[currentPage].transform.SetParent(Left.transform, true);
            bookPages[currentPage].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage].rectTransform.pivot = new Vector2(0, 0);
            bookPages[currentPage].transform.position = Left.transform.position;
        }
        Left.transform.SetAsFirstSibling();


        Right.gameObject.SetActive(true);
        Right.transform.position = RightNext.transform.position;
        Right.transform.eulerAngles = new Vector3(0, 0, 0);
        if(currentPage < bookPages.Length - 1)
        {
            bookPages[currentPage + 1].gameObject.SetActive(true);
            bookPages[currentPage + 1].transform.SetParent(Right.transform, true);
            bookPages[currentPage + 1].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage + 1].rectTransform.pivot = new Vector2(0, 0);
            bookPages[currentPage + 1].transform.position = Right.transform.position;
        }

        if(currentPage < bookPages.Length - 2)
        {
            bookPages[currentPage + 2].gameObject.SetActive(true);
            bookPages[currentPage + 2].transform.SetParent(RightNext.transform, true);
            bookPages[currentPage + 2].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage + 2].transform.position = RightNext.transform.position;
        }
        
        LeftNext.transform.SetAsFirstSibling();
        Mask_R.transform.SetAsFirstSibling();
        Mask_L.transform.SetAsFirstSibling();
        if (enableShadowEffect) Shadow.gameObject.SetActive(true);
        UpdateBookRTLToPoint(f);
        SoundManager.Instance.PlaySE(pageFlipSE);
        Debug.Log("DragRightPageToPoint_END");
    }
    public void OnMouseDragRightPage()
    {
        if (interactable)
            DragRightPageToPoint(transformPoint(Input.mousePosition));

    }
    public void DragLeftPageToPoint(Vector3 point)
    {
        //Debug.Log("왼쪽 넘김");
        if (currentPage <= 0) return;
        if (currentPage > 2) pages.isBack = false;
        if (auto.isFlipping) interactable = false;
        pageDragging = true;
        drag = true;
        mode = FlipMode.LeftToRight;
        f = point;

        if (currentPage == 2)
        {
            Mask_L.gameObject.SetActive(false);
            LeftNext.sprite = background;
        }

        stack_L.transform.localPosition += new Vector3(2.5f, 2.5f, 0);

        NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(0, 0.35f);

        Right.gameObject.SetActive(true);
        Right.transform.position = LeftNext.transform.position;
        if(currentPage <= bookPages.Length)
        {
            if(currentPage < bookPages.Length)
            {
                bookPages[currentPage + 1].gameObject.SetActive(false);
                bookPages[currentPage + 1].transform.SetParent(Page, true);
            }
            //bookPages[currentPage - 1].transform.localScale = new Vector3(1, 1, 1);
            bookPages[currentPage - 1].gameObject.SetActive(true);
            bookPages[currentPage - 1].transform.SetParent(Right.transform, true);
            bookPages[currentPage - 1].transform.localEulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage - 1].rectTransform.pivot = new Vector2(0, 0);
            bookPages[currentPage - 1].transform.position = Right.transform.position;
        }

        Right.transform.eulerAngles = new Vector3(0, 0, 0);
        Right.transform.SetAsFirstSibling();

        Left.gameObject.SetActive(true);
        Left.rectTransform.pivot = new Vector2(1, 0);
        Left.transform.position = LeftNext.transform.position;
        Left.transform.eulerAngles = new Vector3(0, 0, 0);
        if(currentPage >= 2)
        {
            bookPages[currentPage - 2].transform.localScale = new Vector3(1, 1, 1);
            bookPages[currentPage - 2].gameObject.SetActive(true);
            bookPages[currentPage - 2].transform.SetParent(Left.transform, true);
            bookPages[currentPage - 2].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage - 2].rectTransform.pivot = new Vector2(1, 0);
            bookPages[currentPage - 2].transform.position = Left.transform.position;
        }

        if(currentPage >= 3)
        {
            bookPages[currentPage - 3].gameObject.SetActive(true);
            bookPages[currentPage - 3].transform.SetParent(LeftNext.transform, true);
            bookPages[currentPage - 3].transform.eulerAngles = new Vector3(0, 0, 0);
            bookPages[currentPage - 3].transform.position = LeftNext.transform.position;
        }

        RightNext.transform.SetAsFirstSibling();
        Mask_L.transform.SetAsFirstSibling();
        Mask_R.transform.SetAsFirstSibling();
        if (enableShadowEffect) ShadowLTR.gameObject.SetActive(true);
        UpdateBookLTRToPoint(f);
        SoundManager.Instance.PlaySE(pageFlipSE);
        Debug.Log("DragLeftPageToPoint_END");
    }
    public void OnMouseDragLeftPage()
    {
        if (interactable)
            DragLeftPageToPoint(transformPoint(Input.mousePosition));

    }
    public void OnMouseRelease()
    {
        if (interactable)
            ReleasePage();
    }
    public void ReleasePage()
    {
        if (pageDragging)
        {
            pageDragging = false;
            float distanceToLeft = Vector2.Distance(c, ebl);
            float distanceToRight = Vector2.Distance(c, ebr);
            if (distanceToRight < distanceToLeft && mode == FlipMode.RightToLeft && !auto.AutoFlip)
                TweenBack();
            else if (distanceToRight > distanceToLeft && mode == FlipMode.LeftToRight && !auto.AutoFlip)
                TweenBack();
            else  //distanceToRight > distanceToLeft && mode == FlipMode.RightToLeft || distanceToRight < distanceToLeft && mode == FlipMode.RightToLeft
                TweenForward();
        }
    }
    Coroutine currentCoroutine;

    public void TweenForward()
    {
        if (mode == FlipMode.RightToLeft)
            currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f, () => { Flip(); })); // 람다식 (입력-매개-변수) => {실행-문장}
        else
            currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f, () => { Flip(); }));
    }
    void Flip()
    {
        if (mode == FlipMode.RightToLeft)
            currentPage += 2;
        else
            currentPage -= 2;
        LeftNext.transform.SetParent(BookPanel.transform, true);
        Left.transform.SetParent(BookPanel.transform, true);
        LeftNext.transform.SetParent(BookPanel.transform, true);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        Right.transform.SetParent(BookPanel.transform, true);
        RightNext.transform.SetParent(BookPanel.transform, true);
        Debug.Log("FLIP ->" + currentPage);
        
        UpdateImages();
        Shadow.gameObject.SetActive(false);
        ShadowLTR.gameObject.SetActive(false);
        if (OnFlip != null)
            OnFlip.Invoke();

        if (mode == FlipMode.LeftToRight)
        {
            Mask_R.gameObject.SetActive(true);
            stack_R.transform.localPosition += new Vector3(2.5f, -2.5f, 0);
            epiIndex--;
            //Mask_R.transform.SetAsFirstSibling();
        }
        else
        {
            Mask_L.gameObject.SetActive(true);
            stack_L.transform.localPosition -= new Vector3(2.5f, 2.5f, 0);
            epiIndex++;
            //Mask_L.transform.SetAsFirstSibling();
        }

        if (currentPage > 0)
        {
            //pages.epi = epiID[epiIndex];
            //Debug.Log("epi: " + epiID[epiIndex]);
        }
        Debug.Log("Flip_END");
        
    }
    public void TweenBack()
    {
        if (mode == FlipMode.RightToLeft)
        {
            currentCoroutine = StartCoroutine(TweenTo(ebr, 0.15f,
                () => // 람다식
                {
                    Debug.Log("TWEENBACK_r ->" + currentPage);
                    UpdateImages();
                    if(currentPage <= bookPages.Length - 3)
                    {
                        bookPages[currentPage + 2].gameObject.SetActive(false);
                        bookPages[currentPage + 2].transform.SetParent(Page, true);
                    }
                    else
                        Mask_R.gameObject.SetActive(true);
                    RightNext.transform.SetParent(BookPanel.transform);
                    Right.transform.SetParent(BookPanel.transform);

                    stack_R.transform.localPosition += new Vector3(2.5f, -2.5f, 0);

                    Left.gameObject.SetActive(false);
                    Right.gameObject.SetActive(false);
                    pageDragging = false;

                    pages.FlipCheck(true);
                    Debug.Log("TWEENBACK_r_END");
                }
                ));
        }
        else
        {
            currentCoroutine = StartCoroutine(TweenTo(ebl, 0.15f,
                () =>
                {
                    Debug.Log("TWEENBACK_l ->" + currentPage);
                    UpdateImages();

                    if(currentPage == 0)
                        Mask_L.gameObject.SetActive(true);

                    LeftNext.transform.SetParent(BookPanel.transform, true); //아마 오류의 원인
                    Left.transform.SetParent(BookPanel.transform, true);

                    stack_L.transform.localPosition -= new Vector3(2.5f, 2.5f, 0);

                    Left.gameObject.SetActive(false);
                    Right.gameObject.SetActive(false);
                    pageDragging = false;

                    pages.FlipCheck(false);
                    Debug.Log("TWEENBACK_l_END");
                }
                ));
        }
    }
    public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish)
    {
        int steps = (int)(duration / 0.025f);
        Vector3 displacement = (to - f) / steps;
        for (int i = 0; i < steps - 1; i++)
        {
            if (mode == FlipMode.RightToLeft)
                UpdateBookRTLToPoint(f + displacement);
            else
                UpdateBookLTRToPoint(f + displacement);

            yield return new WaitForSeconds(0.025f);
        }
        if (onFinish != null)
            onFinish();

        if (mode == FlipMode.LeftToRight)
        {
            //stack_R.transform.localPosition += new Vector3(0.5f, -0.5f, 0);
            Mask_R.transform.SetAsFirstSibling();
        }
        else
        {
            //stack_L.transform.localPosition -= new Vector3(0.5f, 0.5f, 0);
            Mask_L.transform.SetAsFirstSibling();
        }

        if(currentPage == 0 && auto.AutoFlip)
            cover.isEnd = true;

        Debug.Log("TweenTo_END");
        drag = false;
        auto.isFlipping = false;
        pages.transform.SetAsFirstSibling();
        RightSpot.transform.SetAsLastSibling();
        LeftSpot.transform.SetAsLastSibling();
    }


    public Image[] updateBookPages() 
    {
        //bookPage 페이지 구성 규칙: INTRO + IMAGE1234 * n//2 + IMAGE12 * n%2 + END
        //어차피 에피소드 갱신은 씬 생성 이전 혹은 동시에 이루어지므로 
        //생성 시 매번 BookPage 배열의 페이지 구성을 update한 채로 생성
        //리스트로 만든 다음에 Array로 변환하여 리턴 -> 원 코드를 해치지 않는 선에서 구현 가능

        int lastclear = SaveDataManager.Instance.Progress + 1; //표시할 페이지 수
        List<Image> pageList = new List<Image>();

        pageList.Add(INTRO);
        
        for (int i = 0; i < lastclear/2; i++)
        {
            pageList.Add(IMG1);
            pageList.Add(IMG2);
            pageList.Add(IMG3);
            pageList.Add(IMG4);
        }

        if (lastclear%2 == 1)
        {
            pageList.Add(IMG1);
            pageList.Add(IMG2);
        }

        pageList.Add(END);

        return pageList.ToArray();
    }

}
