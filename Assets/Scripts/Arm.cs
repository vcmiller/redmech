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
	}

    // Update is called once per frame
    public void UpdateAngles(Transform controller) {
        float x = -(Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(controller.localPosition.x * 1.5f * xScl - 1, -1, 1)) - 90);
        float y = -(Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp(controller.localPosition.z * 2 * xScl, -1, 1)) - 90);
        shoulder2.localEulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(shoulder2.localEulerAngles.z, x, jointSpeed * Time.deltaTime));
        upper.localEulerAngles = new Vector3(0, Mathf.MoveTowardsAngle(upper.localEulerAngles.y, y, jointSpeed * Time.deltaTime), 0);

        Quaternion q = Quaternion.Euler(90, 90, 0);
        AlignBone(elbow, controller, q, Vector3.right);
        
        AlignBone(lower, controller, q, Vector3.up);
    }

    private void AlignBone(Transform bone, Transform target, Quaternion offset, Vector3 alignDir) {
        bone.rotation = Quaternion.RotateTowards(bone.rotation, target.rotation * offset, 360 * Time.deltaTime);

        Vector3 alignSelf = bone.TransformVector(alignDir);
        Vector3 alignParent = bone.parent.TransformVector(alignDir);

        Vector3 cross = Vector3.Cross(alignSelf, alignParent).normalized;
        float angle = Vector3.Angle(alignSelf, alignParent);
        bone.Rotate(cross, angle, Space.World);
    }
}
