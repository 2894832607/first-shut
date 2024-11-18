using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundManager111;
using TMPro;
using Ammomanager111;

public class Weapon : MonoBehaviour
{   //shoot
    public Camera weaponCamera;
    public Camera worldCamera;
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    //burst
    public int bulletsperburst = 3;
    public int brustbulletleft;
    //spread
    public float spreadintensity;
    //bullet
    public GameObject bulletprefab;
    public Transform bulletSwpan;
    public float bulletspeed = 3f;
    public float bulletlifetime = 3f;

    public float reloadTime;
    public int magazineSize;
    public int bulletsLeft;
    public int bulletnum;

    public bool isReloading;

    public bool AutoReload;

    public GameObject muzzleeffect;

    private Animator animator;

    public enum Shootingmode    
    {
        Single,
        Burst,
        Auto
    }

    public Shootingmode currentshootingmode;

    public bool holdright = false;
    // 瞄准时的相机视野
    public float aimFOV = 30f;
    // 默认相机视野
    private float defaultWorldFOV;
    private float defaultWeaponFOV;

    public float suoxiaotime;

    private void Awake()
    {
        readyToShoot = true;
        brustbulletleft = bulletsperburst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
        defaultWorldFOV = worldCamera.fieldOfView;
        defaultWeaponFOV = weaponCamera.fieldOfView;
    }


    private void Reload()
    {   
        
        isReloading = true;
        animator.SetTrigger("RELOAD");
        Invoke("ReloadComplete", reloadTime);
    }


    private void ReloadComplete()
    {
       if(magazineSize<=bulletnum)
        {
            bulletnum -= (magazineSize - bulletsLeft);

            bulletsLeft = magazineSize;
        }else if(magazineSize>bulletnum)
        {
            if(bulletsLeft+bulletnum >= magazineSize)
            {

                bulletnum -= (magazineSize - bulletsLeft);
                bulletsLeft = magazineSize;
                        
            }
            else
            {
                bulletsLeft += bulletnum;
                bulletnum = 0;
            }


        }
       
        
        
        isReloading = false;
    }
    // Update is called once per frame

    //private void suoxiao()
    //{
    //    worldCamera.fieldOfView = aimFOV; // 切换到瞄准视野
    //    weaponCamera.fieldOfView = aimFOV; // 切换到瞄准视野
    //}

    //private void fangda()
    //{
    //    worldCamera.fieldOfView = defaultWorldFOV; // 恢复默认视野
    //    weaponCamera.fieldOfView = defaultWeaponFOV; // 恢复默认视野
    //}

    private void suoxiao()
    {
        StartCoroutine(ChangeFOV(worldCamera, worldCamera.fieldOfView, aimFOV, suoxiaotime));
        StartCoroutine(ChangeFOV(weaponCamera, weaponCamera.fieldOfView, aimFOV, suoxiaotime));
    }

    private void fangda()
    {
        StartCoroutine(ChangeFOV(worldCamera, worldCamera.fieldOfView, defaultWorldFOV, suoxiaotime));
        StartCoroutine(ChangeFOV(weaponCamera, weaponCamera.fieldOfView, defaultWeaponFOV, suoxiaotime));
    }

    private IEnumerator ChangeFOV(Camera camera, float fromFOV, float toFOV, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            camera.fieldOfView = Mathf.Lerp(fromFOV, toFOV, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camera.fieldOfView = toFOV;
    }
    void Update()
    {   

        

        if (currentshootingmode == Shootingmode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
            //if (isShooting)
            //{
            //    animator.SetTrigger("RECOIL");
            //}
            //else {
            //    animator.SetTrigger("UNRECOIL");
            //}
        }
        else if (currentshootingmode == Shootingmode.Single || currentshootingmode == Shootingmode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if(Input.GetKeyDown(KeyCode.R) && isReloading==false && bulletsLeft < magazineSize&& bulletnum> 0)
        {   
            Reload();
            SoundManager.Instance.reloadGun.Play();
        }

        if(AutoReload&&readyToShoot&&isShooting == false && bulletsLeft <= 0&&bulletnum>0)
        {
            Reload();
            SoundManager.Instance.reloadGun.Play();
        }

        if (isShooting && readyToShoot&& bulletsLeft>0)
        {

            brustbulletleft = bulletsperburst;

            Fireweapon();
        }

        //if (holdright == false&&Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    animator.SetTrigger("MIAOZHU");
        //    holdright = true;
        //    animator.ResetTrigger("SOUKAI");
        //    worldCamera.fieldOfView = aimFOV; // 切换到瞄准视野
        //    weaponCamera.fieldOfView = aimFOV; // 切换到瞄准视野
        //    //animator.ResetTrigger("RECOIL");
        //}
        //else if(holdright == true&&Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    animator.SetTrigger("SOUKAI");
        //    holdright = false;
        //    animator.ResetTrigger("MIAOZHU");
        //    worldCamera.fieldOfView = defaultWorldFOV; // 恢复默认视野
        //    weaponCamera.fieldOfView = defaultWeaponFOV; // 恢复默认

        //}

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("MIAOZHU");
            holdright = true;
            Invoke("suoxiao", suoxiaotime);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetTrigger("SOUKAI");
            holdright = false;
            Invoke("fangda", suoxiaotime);
        }


        //if (bulletsLeft == 0 )
        //{
        //    SoundManager.Instance.emptyGun.Play();
        //}
        //animator.ResetTrigger("RECOIL222");
        if (bulletsLeft == 0 && isShooting)
        {
            SoundManager.Instance.emptyGun.Play();
        }

        if (Ammomanager.Instance.ammodisplay1 != null)
        {
            Ammomanager.Instance.ammodisplay1.text = $"{bulletsLeft}";
            Ammomanager.Instance.ammodisplay2.text = $"{bulletnum}";
        }

     
    }

    private void Fireweapon()
    {   
        bulletsLeft--;

        animator.SetTrigger("RECOIL");
        animator.SetTrigger("RECOIL222");


        //if (holdright)
        //{


        //}

        SoundManager.Instance.soundGun.Play();

        muzzleeffect.GetComponent<ParticleSystem>().Play();

        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirection().normalized;

        GameObject bullet = Instantiate(bulletprefab, bulletSwpan.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletspeed * 100, ForceMode.Impulse);

        StartCoroutine(DestroyBullet(bullet, bulletlifetime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }
        if (currentshootingmode == Shootingmode.Burst && brustbulletleft > 1)
        {
            brustbulletleft--;

            Invoke("Fireweapon", shootingDelay);

        }
       

    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    public Vector3 CalculateDirection()
    {
        Ray ray = worldCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetpoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetpoint = hit.point;
        }
        else
        {
            targetpoint = ray.GetPoint(100);
        }
        Vector3 direction = targetpoint - bulletSwpan.position;

        float x = UnityEngine.Random.Range(-spreadintensity, spreadintensity);
        float y = UnityEngine.Random.Range(-spreadintensity, spreadintensity);  

        return direction + new Vector3(x, y, 0);
    }
    private IEnumerator DestroyBullet(GameObject bullet, float bulletlifetime)
    {
        yield return new WaitForSeconds(bulletlifetime);
        Destroy(bullet);
    }
}
