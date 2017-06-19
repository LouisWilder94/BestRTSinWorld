using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FollowCursorPoint : NetworkBehaviour {

    private Rigidbody ourRigidbody;
    public float speed = 5;
    public float randomMovement = 5;
    public float randomTimer = 5;
    public float drag = 1f;
    public Camera playercam;


	// Use this for initialization
	void Start () {
        ourRigidbody = GetComponent<Rigidbody>();
        if (ourRigidbody == null)
            Debug.Log("Follow Cursor Script Has no Rigidbody attatched to it's object");

        ourRigidbody.drag = drag;
        //StartCoroutine(randomMovementApply());
	}

    public IEnumerator randomMovementApply()
    {
        while (true)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            ourRigidbody.AddRelativeForce(randomDirection * Random.Range(0, randomMovement));
            yield return new WaitForSeconds(Random.Range(0.1f, randomTimer));
        }
    }

	// Update is called once per frame
	void Update () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Vector3 dirToCursor = (hit.point + Vector3.up * 1) - transform.position;
            ourRigidbody.AddForce(dirToCursor * speed);
            //transform.position = hit.point + Vector3.up * 5;
        }

	}
}
