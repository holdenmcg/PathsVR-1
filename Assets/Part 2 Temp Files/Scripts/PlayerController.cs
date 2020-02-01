using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Range(1f, 1000f)]
    public float maxRaycastDistance = 100f;
    [Range(1, 20)]
    public int curveSegmentCount = 5;
    [SerializeField]
    Transform teleportIndicator;

    LineRenderer lineRenderer;
    Camera cam;
    Vector3 teleportPos;
    Touch touch;
    int environmentMask;
    Animator teleportAnimator;
    bool isTeleporting;

    // Start is called before the first frame update
    void Start() {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.positionCount = curveSegmentCount;
        isTeleporting = false;
        environmentMask = LayerMask.GetMask("Teleport Environment");
        teleportPos = Vector3.negativeInfinity;
        cam = Camera.main;
        teleportAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (isTeleporting) {
            return;
        }
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
        }
        if (Input.GetMouseButton(0) || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
            CheckForNode();
            ShowTeleportLocation();
        } else {
            teleportIndicator.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0)) {
            TeleportToPoint();
        }
    }

    void CheckForNode() { // TODO change it back to one raycast, not raycastall
        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit)) {
            return;
        }
        if (hit.transform.gameObject.tag.Equals("Feature Node")) {
            //GameObject UIGameObject = hit.transform.parent.GetComponent<Billboard>().ui;
            //GameController.OpenMobileGUI(UIGameObject);
        }
    }

    void ShowTeleportLocation() {
        teleportIndicator.gameObject.SetActive(true);

        RaycastHit hit;
        // Only hit things w/ teleport environment mask
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, maxRaycastDistance, environmentMask)) {
            teleportPos = Vector3.negativeInfinity;
            Debug.Log("nope");
            teleportIndicator.gameObject.SetActive(false);
            return;
        }
        MeshCollider meshCollider = (MeshCollider)hit.collider;


        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];


        teleportPos = hit.point;

        teleportIndicator.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);

        Quaternion rotateTarget = Quaternion.LookRotation(Vector3.Cross(teleportIndicator.right, hit.normal));
        teleportIndicator.rotation = Quaternion.Lerp(teleportIndicator.rotation, rotateTarget, 0.2f); // TODO adjust lerp value

        // TODO this is only for mobile where users will not be able to change their y rotation (side by side) when teleporting. In VR, users will have that ability
        teleportIndicator.rotation = Quaternion.Euler(teleportIndicator.eulerAngles.x, cam.transform.eulerAngles.y, 0f); // Changing the y rotation to face the same direction as the camera

        DrawCurve();
    }

    // Draws a quadratic bezier curve from the player to the teleport position
    void DrawCurve() {
        Vector3 startPoint = transform.position; // TODO this has to be at the hand position
        Vector3 controlPoint = Vector3.Lerp(transform.position, teleportPos, 0.5f);
        controlPoint.y += 5f; // Arbitrary height on the curve
        lineRenderer.SetPosition(0, startPoint + new Vector3(0, -1f, 0));
        for (int i = 1; i < curveSegmentCount; i++) {
            float t = (float)i / curveSegmentCount;
            Vector3 q0 = Vector3.Lerp(startPoint, controlPoint, t);
            Vector3 q1 = Vector3.Lerp(controlPoint, teleportPos, t);
            Vector3 q2 = Vector3.Lerp(q0, q1, t);
            lineRenderer.SetPosition(i, q2);
        }
        lineRenderer.SetPosition(curveSegmentCount - 1, teleportPos);
    }

    // Teleports the camera's GameObject to the defined teleport destination
    void TeleportToPoint() {
        if (teleportPos.Equals(Vector3.negativeInfinity)) {
            return;
        }
        StartCoroutine(TeleportTransition());
    }

    IEnumerator TeleportTransition() {
        teleportAnimator.SetTrigger("isTeleportingTrigger");
        isTeleporting = true;
        yield return new WaitForSeconds(0.3f);
        isTeleporting = false;
        transform.position = teleportPos;
        transform.Translate(0, 10f, 0); // TODO temp adjustment for player height
    }
    

}
