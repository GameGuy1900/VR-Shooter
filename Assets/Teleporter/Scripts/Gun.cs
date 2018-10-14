using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float shootSpeed = 100f;
    public float impactForce = 30f;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GameObject fpsCam;

    private bool isReloading = false;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 4f;

    public GameObject reloadText;
    public Text ammoText;

    public Animation animScoped;
    public Animation animIdle;

    //public Camera mainCamera;
    //public Camera aimCamera;

    //private bool firstClick;

    void Start()
    {
        OVRGazePointer.instance.RequestShow();

        //firstClick = false;

        //mainCamera.tag = "MainCamera";

        currentAmmo = maxAmmo;

        ammoText.text = "Ammo: " + currentAmmo;
        
        reloadText.SetActive(false);
    }

    void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.GetDown(OVRInput.Button.Back))
        //{ // The back button is pressed.
        //    if (!firstClick)
        //    { // The first click is detected, so toggle aim mode.
        //        firstClick = true;
        //        aimCamera.gameObject.SetActive(true);
        //        mainCamera.tag = "Untagged";
        //        aimCamera.tag = "MainCamera";
        //    }
        //    else
        //    { // The second click detected, so toggle back.
        //        firstClick = false;
        //        aimCamera.gameObject.SetActive(false);
        //        mainCamera.tag = "MainCamera";
        //        aimCamera.tag = "Untagged";
        //    }
        //}
        
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Shoot();
        }

        if (OVRInput.GetDown(OVRInput.Button.Back))
        {
            Aim();
        }

        if (OVRInput.GetUp(OVRInput.Button.Back))
        {
            Idle();
        }

        ammoText.text = "Ammo: " + currentAmmo;
    }

    IEnumerator Reload ()
    {
        isReloading = true;

        reloadText.SetActive(true);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;

        reloadText.SetActive(false);
    }

    void Aim()
    {
        animScoped = GetComponent<Animation>();
        foreach (AnimationState state in animScoped)
        {
            state.speed = 0.5F;
        }
    }

    void Idle()
    {
        animIdle = GetComponent<Animation>();
        foreach (AnimationState state in animIdle)
        {
            state.speed = 0.5F;
        }
    }

    void Shoot()
    {
        currentAmmo--;

        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shootSpeed;

        // Destroy the bullet after 2 seconds
        //Destroy(bullet, 2.0f);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
                Destroy(bullet, 1.0f);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}