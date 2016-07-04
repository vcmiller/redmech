using UnityEngine;
using System.Collections;

public class Arm : MonoBehaviour {
    private Transform shoulder1;
    private Transform shoulder2;
    private Transform upper;
    private Transform elbow;
    private Transform lower;
    private Transform wrist;

    private Weapon[] weapons;
    private int curWep = 0;

    private float swipeOrigin;
    private bool swiping;
    public float swipeLength = 0.25f;

    public float xScl = 1;
    public float jointSpeed = 360;

	// Use this for initialization
	void Start () {
        shoulder1 = transform;
        shoulder2 = shoulder1.GetChild(0);
        upper = shoulder2.GetChild(0);
        elbow = upper.GetChild(0);
        lower = elbow.GetChild(0);
        wrist = lower.GetChild(0);

        weapons = GetComponentsInChildren<Weapon>();
	}

    public void UpdateWeapons(Transform controller, SteamVR_Controller.Device input) {
        if (weapons == null || weapons.Length == 0) {
            return;
        }

        if (input.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            swipeOrigin = input.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x;
            swiping = true;
        }

        if (input.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad)) {
            swiping = false;
        }

        if (swiping) {
            float x = input.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0).x;
            if (x - swipeOrigin > swipeLength) {
                curWep++;
                swipeOrigin += swipeLength;
            }
        }

        curWep %= weapons.Length;
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].SetActive(curWep == i);
            weapons[i].SetFiring(curWep == i && input.GetPress(SteamVR_Controller.ButtonMask.Trigger));
        }
    }

    private void UpdateAudioSource(Transform bone, float angle) {
        AudioSource src = bone.GetComponent<AudioSource>();
        if (src != null) {
            src.enabled = angle > .2f;
            src.volume = angle * 0.5f;
        }
    }

    // Update is called once per frame
    public void UpdateAngles(Transform controller) {
        float x = -(Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(controller.localPosition.x * 1.5f * xScl - 1, -1, 1)) - 90);
        float y = -(Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(controller.localPosition.z * 2 * xScl, -1, 1)) - 90);

        float max = jointSpeed * Time.deltaTime;
        float a = shoulder2.localEulerAngles.z;
        float b = upper.localEulerAngles.y;

        shoulder2.localEulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(shoulder2.localEulerAngles.z, x, max));
        upper.localEulerAngles = new Vector3(0, Mathf.MoveTowardsAngle(upper.localEulerAngles.y, y, max), 0);

        UpdateAudioSource(shoulder2, Mathf.Abs(a - shoulder2.localEulerAngles.z) / max);
        UpdateAudioSource(upper, Mathf.Abs(b - upper.localEulerAngles.y) / max);

        Quaternion q = Quaternion.Euler(90, 90, 0);
        AlignBone(elbow, controller, q, Vector3.right);
        
        AlignBone(lower, controller, q, Vector3.up);
    }

    private void AlignBone(Transform bone, Transform target, Quaternion offset, Vector3 alignDir) {
        Quaternion initial = bone.rotation;
        float max = jointSpeed * Time.deltaTime;
        bone.rotation = Quaternion.RotateTowards(bone.rotation, target.rotation * offset, jointSpeed * Time.deltaTime);

        Vector3 alignSelf = bone.TransformVector(alignDir);
        Vector3 alignParent = bone.parent.TransformVector(alignDir);

        Vector3 cross = Vector3.Cross(alignSelf, alignParent).normalized;
        float angle = Vector3.Angle(alignSelf, alignParent);
        bone.Rotate(cross, angle, Space.World);
        
        float a = Quaternion.Angle(initial, bone.rotation);
        UpdateAudioSource(bone, a / max);
    }
}
