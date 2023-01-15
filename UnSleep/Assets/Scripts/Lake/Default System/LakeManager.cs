using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LakeManager : MonoBehaviour
{
    public bool moveWithMouseWheel;
    public Color changeColor;
    public Color nonInterativeColor;
    public SpriteMask nonInteractMask;
    public AudioClip rotationSound;
    public AudioClip doNotTouchSound;

    private int currentPhase;
    public GameObject[] PhaseGroups;


    private Camera mainCamera;
    private RotateLake[] lakes;
    private GameObject[] lakeObjs;
    private SpriteRenderer[] lakeSprites;
    private int nonInteractiveLakeIndex = -1;
    private int currentLake = -1;
    private bool isInteracting = false;
    private bool isStart = false;

    //드래그
    private Vector3 screenPos;
    private float angleOffset;
    private bool isDragging = false;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        // 현재 페이즈 찾기
        switch (Dialogue_Proceeder.instance.CurrentEpiID)
        {
            case 0: currentPhase = 1; break;
            case 8: currentPhase = 2; break;
            case 13: currentPhase = 3; break;
            case 19: 
                if (Dialogue_Proceeder.instance.CurrentDiaID == 8031)
                {
                    currentPhase = 4;
                    Dialogue_Proceeder.instance.AddCompleteCondition(50);
                }
                else
                {
                    currentPhase = 5;
                    Dialogue_Proceeder.instance.AddCompleteCondition(52);
                }
                break;
            default: currentPhase = 5; break;
        }

        //페이즈 설정
        for (int i = 0; i < PhaseGroups.Length; i++)
        {
            if (i != currentPhase - 1)
            {
                PhaseGroups[i].SetActive(false);
            }
            else
            {
                PhaseGroups[i].SetActive(true);
            }
        }

        //현재 퍼즐 세팅
        int size = PhaseGroups[currentPhase - 1].transform.childCount;
        lakeObjs = new GameObject[size];
        lakeSprites = new SpriteRenderer[size];
        lakes = new RotateLake[size];
        for (int i = 0; i < size; i++)
        {
            lakeObjs[i] = PhaseGroups[currentPhase - 1].transform.GetChild(i).gameObject;
            lakeSprites[i] = lakeObjs[i].GetComponent<SpriteRenderer>();
            lakes[i] = lakeObjs[i].GetComponent<RotateLake>();
            if (lakes[i] != null)
            {
                lakes[i].moveWithMouseWheel = moveWithMouseWheel;
            }
            else if (i != size - 1)
            {
                nonInteractiveLakeIndex = i;
                //lakeSprites[i].color = nonInterativeColor;
                nonInteractMask.sprite = lakeSprites[i].sprite;
            }
        }

        if (nonInteractiveLakeIndex == -1)
        {
            nonInteractMask.sprite = null;
        }
    } 

    private void Update()
    {
        //드래그일 때
        if (!moveWithMouseWheel && !isStart && currentLake != -1 && Time.timeScale != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentLake == -2)
                {
                    SoundManager.Instance.PlaySE(doNotTouchSound);
                    return;
                }
                isDragging = true;
                lakes[currentLake].isRotating = true;
                screenPos = mainCamera.WorldToScreenPoint(lakeObjs[currentLake].transform.position);
                Vector3 vec = Input.mousePosition - screenPos;
                angleOffset = (Mathf.Atan2(lakeObjs[currentLake].transform.right.y, lakeObjs[currentLake].transform.right.x) - Mathf.Atan2(vec.y, vec.x)) * Mathf.Rad2Deg;
                lakeSprites[currentLake].color = changeColor;
                SoundManager.Instance.PlaySE(rotationSound);
            }
            else if (currentLake < 0) return;
            else if (Input.GetMouseButton(0))
            {
                Vector3 vec = Input.mousePosition - screenPos;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                lakeObjs[currentLake].transform.localEulerAngles = new Vector3(0f, 0f, angle + angleOffset);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                isInteracting = false;
                lakes[currentLake].isRotating = false;
                lakeSprites[currentLake].color = Color.white;
                currentLake = -1;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isStart && !MemoManager.isMemoOn)
        {
            //마우스휠
            if (moveWithMouseWheel)
            {
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    //상호작용 가능한 호수를 처음 만났을 때
                    if (!isInteracting)
                    {
                        for (int i = 0; i < lakeObjs.Length; i++)
                        {
                            if (hit.transform.gameObject == lakeObjs[i] && lakes[i] != null)
                            {
                                isInteracting = true;
                                currentLake = i;
                                lakes[i].isRotating = true;
                                lakeSprites[i].color = changeColor;
                                break;
                            }
                        }
                    }

                    //상호작용 가능한 호수가 바뀌었을 때
                    else if (lakeObjs[currentLake] != hit.transform.gameObject)
                    {
                        lakes[currentLake].isRotating = false;
                        lakeSprites[currentLake].color = Color.white;

                        for (int i = 0; i < lakeObjs.Length; i++)
                        {
                            if (hit.transform.gameObject == lakeObjs[i] && lakes[i] != null)
                            {
                                currentLake = i;
                                lakes[i].isRotating = true;
                                lakeSprites[i].color = changeColor;
                                break;
                            }
                        }

                        //상호작용 불가능한 호수
                        if (lakeObjs[currentLake] != hit.transform.gameObject)
                        {
                            isInteracting = false;
                            lakes[currentLake].isRotating = false;
                            currentLake = -1;
                        }
                    }
                }
                //만약 아무것도 안 맞았다면 상호작용 해제
                else if (isInteracting)
                {
                    isInteracting = false;
                    if (currentLake != -1)
                    {
                        lakes[currentLake].isRotating = false;
                        lakeSprites[currentLake].color = Color.white;
                        currentLake = -1;
                    }
                }
            }

            //드래그
            else if (!isDragging)
            {
                if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    //상호작용 가능한 호수를 처음 만났을 때
                    if (!isInteracting)
                    {
                        for (int i = 0; i < lakeObjs.Length; i++)
                        {
                            if (hit.transform.gameObject == lakeObjs[i] && lakes[i] != null)
                            {
                                isInteracting = true;
                                currentLake = i;
                                break;
                            }
                        }
                    }

                    //상호작용 가능한 호수가 바뀌었을 때
                    else if (lakeObjs[currentLake] != hit.transform.gameObject)
                    {
                        for (int i = 0; i < lakeObjs.Length; i++)
                        {
                            if (hit.transform.gameObject == lakeObjs[i] && lakes[i] != null)
                            {
                                currentLake = i;
                                break;
                            }
                        }

                        //상호작용 불가능한 호수
                        if (lakeObjs[currentLake] != hit.transform.gameObject)
                        {
                            isInteracting = false;
                            currentLake = -2;
                        }
                    }
                }
                else if (!isDragging)
                {
                    isInteracting = false;
                    currentLake = -1;
                }
            }
        }
    }


    public void OnClickStart()
    {
        isStart = true;
        isInteracting = false;
        currentLake = -1;
        lakeSprites[nonInteractiveLakeIndex].color = Color.white;

        int size = lakeObjs.Length;
        for (int i = 0; i < size; i++)
        {
            if (lakes[i] != null)
            {
                lakes[i].isStart = true;
            }
        }
        //nonInteractMask.sprite = null;
    }

    //임시
    public void OnClickDrag()
    {
        moveWithMouseWheel = !moveWithMouseWheel;
        int size = PhaseGroups[currentPhase - 1].transform.childCount;
        for (int i = 0; i < size; i++)
        {
            if (lakes[i] != null)
            {
                lakes[i].moveWithMouseWheel = moveWithMouseWheel;
            }
        }
    }
}