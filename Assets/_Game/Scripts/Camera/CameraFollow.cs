using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // cache transform player
    private Transform tf;

    public Transform TF
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    [SerializeField] private float timeAnimateCam = 1f;
    [SerializeField] private float timeAnimateCamInShopDress = 0.2f;

    private Transform cameraMainTF;
    private int standardRange = 10;
    private Vector3 standarOffsetVector3 = new Vector3(0, 25, -25);
    private Vector3 standarPosVector3 = new Vector3(0, 2, -8);
    private Vector3 standarRotationVector3 = new Vector3(11, 0, 0);

    private Vector3 offsetPosCam;
    private Vector3 desRotateCam = new Vector3(40, 0, 0);

    private void Awake()
    {
        cameraMainTF = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsState(GameState.GamePlay))
        {
            cameraMainTF.position = TF.position + offsetPosCam;
        }
    }

    public void ChangeOffSetBaseScale()
    {
        Vector3 oriOffset = offsetPosCam;
        offsetPosCam = oriOffset + new Vector3(0, 1f, -1f);
    }
    public void ChangeOffSetBaseRangeWeapon(int rangeWeapon)
    {
        Vector3 oriOffset = standarOffsetVector3;
        if (rangeWeapon == standardRange)
        {
            offsetPosCam = oriOffset;
            return;
        }
        if (rangeWeapon > standardRange) // thay thu rangeWeapon = 11, do moi lan chi scale 0.05 nen phai nhan 2
        {
            offsetPosCam = oriOffset + new Vector3(0, ((rangeWeapon - standardRange) * 2) + 1, (-(rangeWeapon - standardRange) * 2) - 1);
        }
        else
        {
            offsetPosCam = oriOffset + new Vector3(0, ((rangeWeapon - standardRange) * 2) - 1, (-(rangeWeapon - standardRange) * 2) + 1);
        }

    }

    public void AnimateCamera()
    {
        StartCoroutine(AnimateCamPos());
        StartCoroutine(AnimateCamRotate());
    }

    public IEnumerator AnimateCamPos()
    {
        Vector3 startPos = standarPosVector3;
        Vector3 endPos = offsetPosCam;

        float time = 0;
        while (time < timeAnimateCam)
        {
            offsetPosCam = Vector3.Lerp(startPos, endPos, time / timeAnimateCam);
            time += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator AnimateCamRotate()
    {
        Quaternion startRotate = Quaternion.Euler(standarRotationVector3);
        Quaternion endRotate = Quaternion.Euler(desRotateCam);
        float time = 0;
        while (time < timeAnimateCam)
        {
            cameraMainTF.rotation = Quaternion.Slerp(startRotate, endRotate, time / timeAnimateCam);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator AnimatCamPullDown()
    {
        Vector3 startPos = cameraMainTF.position;
        Vector3 endPos = cameraMainTF.position + Vector3.down;

        float time = 0;
        while (time < timeAnimateCamInShopDress)
        {
            cameraMainTF.position = Vector3.Lerp(startPos, endPos, time / timeAnimateCamInShopDress);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void StartAnimatCamPullUp()
    {
        StartCoroutine(AnimatCamPullUp());
    }

    public IEnumerator AnimatCamPullUp()
    {
        Vector3 startPos = cameraMainTF.position;
        Vector3 endPos = cameraMainTF.position + Vector3.up;

        float time = 0;
        while (time < timeAnimateCamInShopDress)
        {
            cameraMainTF.position = Vector3.Lerp(startPos, endPos, time / timeAnimateCamInShopDress);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void SetCamBachToInit()
    {
        cameraMainTF.position = standarPosVector3;
        cameraMainTF.rotation = Quaternion.Euler(standarRotationVector3);
    }
}

