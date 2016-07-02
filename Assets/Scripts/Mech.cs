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
    

    // Use this for initialization
    void Start() {
        Transform cameraRig = GetComponentInChildren<SteamVR_ControllerManager>().transform;
        controllerLeft = cameraRig.GetChild(1).GetComponent<SteamVR_TrackedObject>();
        controllerRight = cameraRig.GetChild(0).GetComponent<SteamVR_TrackedObject>();

        armLeft = cameraRig.parent.GetChild(1).GetComponent<Arm>();
        armRight = cameraRig.parent.GetChild(2).GetComponent<Arm>();

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

        Vector3 moveDir = Vector3.zero;
        if (moveLeft) {
            moveDir = controllerLeft.transform.localPosition - moveOrigin;
        } else if (moveRight) {
            moveDir = controllerRight.transform.localPosition - moveOrigin;
        }

        moveDir *= 3;

        transform.Translate(new Vector3(0, 0, Mathf.Clamp(-moveDir.z, -1, 1)) * Time.deltaTime * 15, Space.Self);
        transform.Rotate(0, Mathf.Clamp(-moveDir.x, -1, 1) * Time.deltaTime * 90, 0);


        /*
        Vector2 touchLeft = inputLeft.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
        Vector2 touchRight = inputRight.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

        if (inputLeft.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            lastLeft = touchLeft;
        }
        if (inputRight.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            lastRight = touchRight;
        }
        

        if (inputLeft.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
            transform.Translate(new Vector3(touchLeft.x, 0, touchLeft.y) * Time.deltaTime * 15, Space.Self);
        } else if (inputLeft.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
            transform.Translate(new Vector3(touchLeft.x, 0, touchLeft.y) * Time.deltaTime * 10, Space.Self);
        }

        if (inputRight.GetPress(SteamVR_Controller.ButtonMask.Touchpad)) {
            transform.Rotate(0, touchRight.x * Time.deltaTime * 90, 0);
        } else if (inputRight.GetTouch(SteamVR_Controller.ButtonMask.Touchpad)) {
            transform.Rotate(0, (touchRight.x - lastRight.x) * 30, 0);
        }


        lastLeft = touchLeft;
        lastRight = touchRight;*/

        if (!moveLeft) {
            armLeft.UpdateAngles(controllerLeft.transform);
        }
        if (!moveRight) {
            armRight.UpdateAngles(controllerRight.transform);
        }

    }
}
