using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CogWheelInfoWatcher : MonoBehaviour
{

    public CogWheelInfoPanel cogWheelInfoPanel;

    void Update()
    {
        if (MemoManager.isMemoOn) return;

        if (Input.GetMouseButton(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 2000f);
            Debug.DrawRay(mousePosition, transform.forward * 2000f, Color.cyan);
            if (hit)
            {
                cogWheelInfoPanel.showInfo(hit.collider.gameObject.GetComponent<CogWheel>().getCogWheelInfo());
            }
            else
            {
                if (cogWheelInfoPanel.gameObject.activeSelf) cogWheelInfoPanel.hideInfo();
            }
        }
        else
        {
            if (cogWheelInfoPanel.gameObject.activeSelf) cogWheelInfoPanel.hideInfo();
        }

    }
}
