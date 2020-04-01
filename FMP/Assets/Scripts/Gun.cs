using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (AudioSource))]
public class Gun : MonoBehaviour
{
    public enum GunType
    {
        Semi,
        Burst,
        Auto
    }

    public float rateOfFire;

    private float secondsBetweenShots;
    private float nextPossibleShootTime;

    public GunType gunType;

    private LineRenderer tracer;
    public Transform bulletSpawn;
    public Transform shellEjectionPoint;

    public Rigidbody shell;


    void Start()
    {
        secondsBetweenShots = 60 / rateOfFire;
        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }
    }




    public void shoot()
    {

        if (CanShoot())
        {
            Ray ray = new Ray(bulletSpawn.position, bulletSpawn.forward);
            RaycastHit hit;

            float shotDistance = 20;
            if (Physics.Raycast(ray, out hit, shotDistance))
            {
                shotDistance = hit.distance;
            }

            nextPossibleShootTime = Time.time + secondsBetweenShots;

            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            if (tracer)
            {
                StartCoroutine("RenderTracer", ray.direction * shotDistance);
            }
        }

        Rigidbody newShell = Instantiate(shell, shellEjectionPoint.position, Quaternion.identity) as Rigidbody;
        newShell.AddForce(shellEjectionPoint.forward * Random.Range(150f, 200f) + bulletSpawn.forward * Random.Range(-10.0f, 10.0f));
    }

    public void shootContinuous()
    {
        if (gunType == GunType.Auto)
        {
            shoot();
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;

        if(Time.time < nextPossibleShootTime )
        {
            canShoot = false;
        }

        return canShoot;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, bulletSpawn.position);
        tracer.SetPosition(1, bulletSpawn.position + hitPoint);
        yield return new WaitForSeconds(0.2f);
        tracer.enabled = false;
    }
}
