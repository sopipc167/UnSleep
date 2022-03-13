using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    public float speed = 2.0f; //이동속도
    public float TurnSpeed = 10.0f; //시점 회전 속도
    private Rigidbody rigid;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public bool Run(Vector3 targetPos) //targetPos 좌표로 이동
    {
        animator.SetBool("Running", true);
        //지금 위치랑 이동할 위치 사이 거리 
        float dis = Vector3.Distance(transform.position, targetPos);
        if (dis >= 0.01f) //어느정도 차이가 있으면 이동
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime); //스크립트가 달린 오브젝트가 이동

            return true; //이동했으면 true 
        }
        animator.SetBool("Running", false);
        return false; //이동 안했으면 false
    }

    public void Turn(Vector3 targetPos) //targetPos로 잠재우미 회전 *자연스럽게 수정하면 참 좋을듯
    {
        Vector3 dir = targetPos - transform.position; //방향벡터 = 클릭 좌표 - 지금 좌표 
        //Debug.Log(dir);
        Vector3 dirXZ = new Vector3(dir.x, 0f, dir.z); //위아래로 팔랑거리지는 않으니까 y좌표는 0

        Quaternion targetRot = Quaternion.LookRotation(dirXZ); //y가 0인 벡터3 dirXZ로 회전
        rigid.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, TurnSpeed * Time.deltaTime); //from이 tar을 TurnSpeed로 돌아보도록
    }
}
