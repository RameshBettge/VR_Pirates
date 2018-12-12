using UnityEngine;

public class Recoil : MonoBehaviour
{
    public bool isRecoiling = false;

    float maxRecoilx = -20f;
    float recoilSpeed = 10f;
    float recoilTimer = 0.2f;
    float recoilDuration = 0.2f;


    public void Update()
    {
        if (isRecoiling)
        {
            Recoiling();
        }
        else
        {
            RecoilingBack();
        }
    }

    public void Recoiling()
    {
        if (recoilTimer > 0)
        {
            Quaternion maxRecoil = Quaternion.Euler(maxRecoilx, 0, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoilTimer -= Time.deltaTime;
        }
        else if (recoilTimer <= 0)
        {
            isRecoiling = false;
            recoilTimer = recoilDuration;
        }
    }

    public void RecoilingBack()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed * 0.5f);
    }
}