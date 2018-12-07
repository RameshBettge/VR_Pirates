using UnityEngine;

public class Recoil : MonoBehaviour
{
    float maxRecoilx = -20f;
    float recoilSpeed = 5f;
    float recoil = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            recoil += 0.1f;
        }
        Recoiling();
    }

    void Recoiling()
    {
        if (recoil > 0)
        {
            Quaternion maxRecoil = Quaternion.Euler(maxRecoilx, 0, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0f;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed * 0.5f);
        }
    }
}