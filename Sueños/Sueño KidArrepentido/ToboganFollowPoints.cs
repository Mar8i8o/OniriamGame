using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToboganFollowPoints : MonoBehaviour
{
    public bool activo;
    public float distance;
    public Transform[] points;
    public ToboganController toboganController;
    public int indice;
    public float speed;
    public float timeSpeed;
    void Start()
    {
        timeSpeed = 1;
    }

    void Update()
    {
        if(activo)
        {

            distance = Vector3.Distance(transform.position, points[indice].position);

            timeSpeed += Time.deltaTime * 0.5f;

            if(distance < 0.1f) 
            { 
                if(indice < points.Length - 1) { indice++; print("sumar indice" + indice); }
                else
                {
                    toboganController.DesmontarTobogan();
                    timeSpeed = 1;
                    activo = false;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, points[indice].position, speed * timeSpeed * Time.deltaTime);


        }

}


    public void IniciarRecorrido()
    {
        activo = true;
    }


}
