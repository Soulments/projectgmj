using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SDPT_ItemSpin : MonoBehaviour
{
    public GameObject trackingObject = null;
    public Vector3 minSpeed;
    public Vector3 maxSpeed;
    private Vector3 speed;
    public Vector3 minSpinSpeed;
    public Vector3 maxSpinSpeed;
    private Vector3 spinSpeed;

    private void Start() 
    {
        if (trackingObject == null) Destroy(this);
        speed = new Vector3(UnityEngine.Random.Range(minSpeed.x, maxSpeed.x), UnityEngine.Random.Range(minSpeed.y, maxSpeed.y), UnityEngine.Random.Range(minSpeed.z, maxSpeed.z));
        spinSpeed = new Vector3(UnityEngine.Random.Range(minSpinSpeed.x, maxSpinSpeed.x), UnityEngine.Random.Range(minSpinSpeed.y, maxSpinSpeed.y), UnityEngine.Random.Range(minSpinSpeed.z, maxSpinSpeed.z));
    }

    private void Update() 
    {
        trackingObject.transform.position += speed*Time.deltaTime;
        speed += Physics.gravity*Time.deltaTime;

        trackingObject.transform.Rotate(spinSpeed.x*Time.deltaTime, spinSpeed.y*Time.deltaTime ,spinSpeed.z*Time.deltaTime , Space.Self);
    }

    private void OnCollisionEnter(Collision other) 
    {
        Vector3 eular = trackingObject.transform.eulerAngles;
        trackingObject.transform.rotation = Quaternion.Euler(0, eular.y, 0);
        Destroy(this.gameObject);
    }
}
