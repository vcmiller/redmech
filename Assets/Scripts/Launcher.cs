using UnityEngine;
using System.Collections;

public class Launcher : Weapon {
    public GameObject projectile;
    public float shotVel;
    public Transform mag;
    public float fireRate = 2.0f;
    public AudioClip sound;

    private CooldownTimer shoot;
    private float angle;
    private float angleAct;

    void Start() {
        shoot = new CooldownTimer(1 / fireRate);
    }

    void Update() {
        if (firing && shoot.Use) {
            GameObject proj = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
            proj.GetComponent<Rigidbody>().velocity = transform.forward * shotVel;
            AudioSource.PlayClipAtPoint(sound, transform.position);
            angle += 60;
        }

        angleAct = Mathf.MoveTowards(angleAct, angle, 360 * Time.deltaTime);

        mag.localEulerAngles = new Vector3(angleAct % 360, 0, 0);
    }
}
