using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto : MonoBehaviour
{
    public FlipMode Mode;
    public bool AutoFlip;
    public Book_test ControledBook;
    public float PageFlipTime;
    public float TimeBetweenPages;
    public float AnimationFramesCount;
    public float DelayBeforeStarting;
    public bool isFlipping;
    public bool isAutoStart;

    void Start()
    {
        if (!ControledBook)
        {
            ControledBook = GetComponent<Book_test>();
        }
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
    }
    void PageFlipped()
    {
        Debug.Log("PageFlipped");
        isFlipping = false;
    }

    void Update()
    {
        if (AutoFlip && !isAutoStart)
        {
            isAutoStart = true;
            StartCoroutine(FlipToEnd());
        }
    }

    public IEnumerator FlipToCurrentPage(float DelayTime, float AnimationFrame, float BtweenTime, int currentPage)
    {
        yield return new WaitForSeconds(DelayTime);
        float frameTime = PageFlipTime / AnimationFrame;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2);
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFrame;

        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < currentPage)
                {
                    if (!isFlipping)
                        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(BtweenTime);
                }
                ControledBook.interactable = true;
                break;
                
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > currentPage)
                {
                    if (!isFlipping)
                        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(BtweenTime);
                }
                ControledBook.interactable = true;
                AutoFlip = false;
                break;
        }
    }

    IEnumerator FlipToEnd()
    {
        yield return new WaitForSeconds(DelayBeforeStarting);
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2);
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f; // 페이지 높이 1/2 * 0.9f
        float dx = (xl) * 2 / AnimationFramesCount;
        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount)
                {
                    if (!isFlipping)
                        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                break;
                ControledBook.interactable = true;
            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 0)
                {
                    if (!isFlipping)
                        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(TimeBetweenPages);
                }

                AutoFlip = false;
                break;
        }
    }

    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        isFlipping = true;
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();
    }
    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        isFlipping = true;
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();
    }
}
