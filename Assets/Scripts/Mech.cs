using UnityEngine;
using System.Collections;

public class Mech : MonoBehaviour {
    private SteamVR_TrackedObject controllerLeft;
    private SteamVR_TrackedObject controllerRight;
    private SteamVR_Controller.Device inputLeft { get { return SteamVR_Controller.Input((int)controllerLeft.index); } }
    private SteamVR_Controller.Device inputRight { get { return SteamVR_Controller.Input((int)controllerRight.index); } }

    private Vector2 lastLeft;
    private Vector2 lastRight;

    private Arm armLeft;
    private Arm armRight;

    private bool moveLeft;
    private bool moveRight;
    private Vector3 moveOrigin;

    private AudioSource walkSound;
    private CooldownTimer footstepTimer;
    private Rigidbody rigidbody;

    public float maxFuel = 100;
    private float fuel;

    private Transform fuelGauge;

    // Use this for initialization
    void Start() {
        Transform cameraRig = GetComponentInChildren<SteamVR_ControllerManager>().transform;
        controllerLeft = cameraRig.GetChild(1).GetComponent<SteamVR_TrackedObject>();
        controllerRight = cameraRig.GetChild(0).GetComponent<SteamVR_TrackedObject>();

        armLeft = cameraRig.parent.GetChild(1).GetComponent<Arm>();
        armRight = cameraRig.parent.GetChild(2).GetComponent<Arm>();
        walkSound = GetComponent<AudioSource>();
        footstepTimer = new CooldownTimer(0.7f);
        rigidbody = GetComponent<Rigidbody>();

        fuel = maxFuel;
        fuelGauge = cameraRig.parent.FindChild("fuel");
    }

    // Update is called once per frame
    void Update() {
        if (inputLeft.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !moveRight) {
            moveLeft = true;
            moveOrigin = controllerLeft.transform.localPosition;
        } else if (inputLeft.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            moveLeft = false;
        }

        if (inputRight.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !moveLeft) {
            moveRight = true;
            moveOrigin = controllerRight.transform.localPosition;
        } else if (inputRight.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            moveRight = false;
        }
        
        if (!moveLeft) {
            armLeft.UpdateAngles(controllerLeft.transform);
            armLeft.UpdateWeapons(controllerLeft.transform, inputLeft);
        }
        if (!moveRight) {
            armRight.UpdateAngles(controllerRight.transform);
            armRight.UpdateWeapons(controllerRight.transform, inputRight);
        }
    }

    void FixedUpdate() {
        Vector3 moveDir = Vector3.zero;
        if (moveLeft) {
            moveDir = controllerLeft.transform.localPosition - moveOrigin;
        } else if (moveRight) {
            moveDir = controllerRight.transform.localPosition - moveOrigin;
        }

        moveDir *= 3;

        if (moveDir.sqrMagnitude > 0 && footstepTimer.Use) {
            walkSound.volume = moveDir.magnitude * 0.5f;
            walkSound.Play();
        }

        rigidbody.MovePosition(transform.TransformPoint(new Vector3(0, 0, Mathf.Clamp(-moveDir.z, -1, 1)) * Time.fixedDeltaTime * 15));
        rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(0, Mathf.Clamp(-moveDir.x, -1, 1) * Time.fixedDeltaTime * 90, 0));
    }

    public void Damage(float damage) {
        fuel -= damage;
        fuelGauge.localScale = new Vector3(fuel / maxFuel, 1, 1);
    }
}
