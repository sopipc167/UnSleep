using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public int[] triggerDiaIds;
    public int nextId;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Player")
        {
            foreach (int id in triggerDiaIds)
            {
                if (Dialogue_Proceeder.instance.AlreadyDone(id))
                {
                    Dialogue_Proceeder.instance.UpdateCurrentDiaID(nextId); //Proceeder 업데이트.
                    SceneChanger.Instance.ChangeScene(SceneType.Dialogue, false);
                }
            }
        }
    }
}
