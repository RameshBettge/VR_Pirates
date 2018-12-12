using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public bool cannonSelected = false;

    public Transform weaponHolder;

    public GameObject fist1;
    public GameObject fist2;
    public GameObject sniper;

    public Throwing throwingScript;
    public CameraSwitching switchScript;
    public Cannon cannonScript;
    public CannonRotation rotationScript;

    void Start()
    {
        throwingScript.enabled = true;
        cannonScript.enabled = false;
        rotationScript.enabled = false;
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            if (throwingScript.enabled == false)
            {
                throwingScript.enabled = true;
            }
            switchScript.ThirdPerson();
            if (cannonScript.enabled == true)
            {
                cannonScript.enabled = false;
            }
            cannonScript.DeactivateCannon();
            if (rotationScript.enabled == true)
            {
                rotationScript.enabled = false;
            }
            cannonSelected = false;
            fist1.SetActive(false);
            fist2.SetActive(false);
            sniper.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
            if (throwingScript.enabled == true)
            {
                throwingScript.enabled = false;
            }
            switchScript.FirstPerson();
            if (cannonScript.enabled == true)
            {
                cannonScript.enabled = false;
            }
            cannonScript.DeactivateCannon();
            if (rotationScript.enabled == true)
            {
                rotationScript.enabled = false;
            }
            cannonSelected = false;
            fist1.SetActive(false);
            fist2.SetActive(false);
            sniper.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
            if (throwingScript.enabled == true)
            {
                throwingScript.enabled = false;
            }
            switchScript.ThirdPerson();
            if (cannonScript.enabled == false)
            {
                cannonScript.enabled = true;
            }
            cannonScript.ActivateCannon();
            if (rotationScript.enabled == false)
            {
                rotationScript.enabled = true;
            }
            cannonSelected = true;
            fist1.SetActive(true);
            fist2.SetActive(true);
            sniper.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}