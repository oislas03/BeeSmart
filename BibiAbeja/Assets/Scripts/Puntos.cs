using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Clase de los puntos. Cada punto debe de tener un ID que se le designará en
 * el inspector. Siempre el primer ID es el 1, y puede llevar a un número 
 * indefinido de puntos, mientras se le agregue a la clase observadora (checador)
 * en el inspector, solo basta con arrastrar todos los puntos necesarios.
 * 
 * Por el momento, solo puede ir en secuencia hacia delante (1,2,3,4,1) y no hacia atrás
 * (1,4,3,2,1)
 */
public class Puntos : MonoBehaviour {
    // Use this for initialization
    
    
    public int ID;
    private ParticleSystem PS;
    

    /* Esto es un delegate-event, básicamente es el mecanismo con el que se comunicará al
     * observador. Mientras el método del observador tenga las mismas especificaciones 
     * (Que regresa y cuantos parametros tiene, y se refiera al mismo evento), va a poder
     * haber comunicación 
     */
    //public delegate void JugadorEmpiezaDibujar(int id);
    //public static event JugadorEmpiezaDibujar Contacto;

  
    /* Método que viene consigo los GameObjects con collider. Registra el primer cuadro 
     * donde el mouse entra en contacto con el GameObject.
     * 
     */ 
    //void OnMouseEnter()
    //{
    //    // Revisa que haya un objeto suscrito al evento
    //    if (Contacto != null)
    //    {
    //        Debug.Log("toque un punto?");
    //        // llama al evento del observador
    //        Contacto(ID);
    //    }
    //}
    void Awake()
    {
        PS = gameObject.GetComponent("ParticleSystem") as ParticleSystem;
        //var em = PS.emission;
        //em.enabled = false;
    }

    void Start()
    {
    

    }

    public void PSEnableEmission()
    {

        var em = PS.emission;
        em.enabled = true;
    }

    public void PSDisableEmission()
    {
        var em = PS.emission;
        em.enabled = false;
    }
    

    //public void contacto()
    //{
    //    // Revisa que haya un objeto suscrito al evento
    //    if (Contacto != null)
    //    {
    //        Debug.Log("toque un punto?");
    //        // llama al evento del observador
    //        Contacto(ID);
    //    }
    //}

    public Puntos getInstance()
    {
        return this;
    }

}
