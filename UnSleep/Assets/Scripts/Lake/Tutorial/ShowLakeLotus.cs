using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLakeLotus : ShowLakeMovement
{
    public LotusManager lotusManager;
    public Lotus lotus;

    private Vector3 defaultLotusPos;
    private Quaternion defaultLotusRot;

    private Vector3 defaultLotusManagerPos;
    private Quaternion defaultLotusManagerRot;

    protected override void Start()
    {
        base.Start();
        defaultLotusPos = lotus.transform.localPosition;
        defaultLotusRot = lotus.transform.localRotation;
        defaultLotusManagerPos = lotusManager.transform.localPosition;
        defaultLotusManagerRot = lotusManager.transform.localRotation;
    }

    protected override void ResetData()
    {
        base.ResetData();
        lotus.transform.localPosition = defaultLotusPos;
        lotus.transform.localRotation = defaultLotusRot;
        lotus.ResetData();
        lotusManager.transform.localPosition = defaultLotusManagerPos;
        lotusManager.transform.localRotation = defaultLotusManagerRot;
        lotusManager.Stop();
    }
}
