using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    bool isPickedUp = false;
    bool isSnapped = false;
    int environmentMask;

    // Start is called before the first frame update
    void Start()
    {
        environmentMask = LayerMask.GetMask("Teleport Environment");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp && !isSnapped) {
            transform.GetComponent<BoxCollider>().enabled = false;
            MoveObject();
        }
    }

    void MoveObject() {

        RaycastHit hit;
        // Only hit things w/ teleport environment mask
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f, environmentMask)) {
            return;
        }

        transform.position = new Vector3(hit.point.x, hit.point.y + 5f, hit.point.z);
        //transform.position.Set(hit.point.x, hit.point.y + 5f, hit.point.z);
    }

    private void OnTriggerStay(Collider other) {
        Debug.Log("Stay");
    }

    private void OnMouseDown() {
        Debug.Log("Collider clicked on");
        isPickedUp = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger Collidded");
        if (other.transform.tag.Equals("Snap Point")) {
            Debug.Log("Hit shpere");
            isSnapped = true;
            transform.position = other.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collidded");

    }


    private void OnCollisionExit(Collision collision) {
        Debug.Log("Exited Collider");
    }
}
