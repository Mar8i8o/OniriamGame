using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeloFregonaControler : MonoBehaviour
{
    public Vector3 vector3pos;
    public Vector3 vector3rot;

    public Rigidbody rb;
    public bool primerPelo;
    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
       vector3pos = transform.localPosition;
       vector3rot = transform.localEulerAngles; 
       //if (!primerPelo) rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarPelos()
    {
        if (!primerPelo) 
        {
            rb.isKinematic = false;
            print("activarPelo");
        }
    }

    public void DesactivarPelos()
    {
        transform.localPosition = vector3pos;
        transform.localEulerAngles = vector3rot;
        if (!primerPelo) { rb.isKinematic = true; }
    }

}
