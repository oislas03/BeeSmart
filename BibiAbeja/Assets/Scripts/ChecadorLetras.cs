
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/*
 * 
 * ESTE ES EL DEL PROYECTO A DONDE VA DIRIGIDO, NO LO OLVIDES
 * 
 * 
 */

public class ChecadorLetras : MonoBehaviour
{
    #region Singleton
    //un pequeño singleton por aqui...
    private static ChecadorLetras _instance;

    public static ChecadorLetras Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("ChecadorLetras");
                go.AddComponent<ChecadorLetras>();
            }

            return _instance;
        }
    }
    #endregion

    //Arreglo de puntos a utilizar. Aquí se agregan los puntos en la escena o ejercicio
    public int puntos;
    // public HandCursor hd;
    //Objeto del DrawLine para llamar sus metodos
    public DrawLine draw;

    string p;
    private float duracion;
    private float tiempoIntento;
    private bool updateTimer;
    private List<DrawLine> listaDL;
    private int indexDL;
    private bool usarOtroDL;

    int id;
    private int idPrimerPunto;
    private int idPuntoActual;
    private int idPuntoSiguiente;
    private int aciertos;
    private int ultimoPunto;
    private int idRepetido;
    private int i;
    private int aciertosNecesarios;
    private bool terminar;
    private bool primerPuntoNoTocado;
    public GameObject palabra;
    private GameObject[] letras;
    public DrawLine[] drawLines;
    private DrawLine drawActual;
    private string tipoLetra;
    private int idEspecialLetra;

    public GameObject btnsiguiente;
    public GameObject txtRetroDirecta;

    public string[] sonidosError;
    public string[] sonidosRe;
    private char[] letrasArreglo;

    public datosLetra datosL;


    //Parte del Singleton en unityCSharp
    void awake()
    {
        _instance = this;
    }

    //Inicialización de variables
    void Start()
    {
        #region desactivar para pruebas de palabras nuevas
        Time.timeScale = 1;

        p = EstadoJuego.estadoJuego.palabra;
        letrasArreglo = p.ToCharArray();
        UnityEngine.Debug.Log("Prefab/Palabras paso 2/palabra " + p);
        palabra = Resources.Load("Prefab/Palabras paso 2/palabra " + p) as GameObject;
        AudioClip sonido;
        sonido = Resources.Load<AudioClip>("Sonidos/Figuras/" + palabra);
        GetComponent<AudioSource>().PlayOneShot(sonido);

        Vector3 vectorCentro;

        GameObject go1 = GameObject.Find("Canvas").gameObject;
        GameObject go2 = GameObject.Find("BgImage").gameObject;
        vectorCentro = go2.transform.position;
        vectorCentro.z = -2;
        GameObject go = Instantiate(palabra, vectorCentro, Quaternion.identity, go1.transform);

        #endregion


        indexDL = -1;
        listaDL = new List<DrawLine>();
        usarOtroDL = false;

        //todo esto es por la primera letra
        //Conseguimos una lista de letras
        letras = GameObject.FindGameObjectsWithTag("letra");

        UnityEngine.Debug.Log("numero de letras en el juego: " + letras.Length);
        UnityEngine.Debug.Log("numero de letras en la palabra: " + letrasArreglo.Length);

        //acomodar las letras

        for (int d = 0; d < letras.Length; d++)
        {
            UnityEngine.Debug.Log(letras[d].gameObject.transform.name);
        }

        GameObject[] letrasAcomodadas = new GameObject[letras.Length];

        for (int m = 0; m < letrasArreglo.Length; m++)
        {
            UnityEngine.Debug.Log("m: " + m);
            for (int l = 0; l < letras.Length; l++)
            {
                UnityEngine.Debug.Log("l: " + l);
                //letra c <- son 7 caracteres en total
                if (letrasArreglo[m] == letras[l].gameObject.transform.name[6])
                {
                    //UnityEngine.Debug.Log("Hey este numero parece dar problemas: " + m + " o este: " + l);
                    letrasAcomodadas[m] = letras[l];
                    letras[l].gameObject.transform.name = "0000000000";
                    break;
                }

            }
        }

        letras = letrasAcomodadas;

        Array.Reverse(letras);

        UnityEngine.Debug.Log("DESPUES DEL ACOMODO Y REVERSA: ");
        for (int d = 0; d < letras.Length; d++)
        {
            UnityEngine.Debug.Log(letras[d].gameObject.transform.name);
        }

        //después de acomodar letras
        i = letras.Length - 1;
        UnityEngine.Debug.Log("i es: " + i);
        // Después, de la primera letra de la palabra sacamos su script
        // de datosL donde esta la lista de gameobject de puntos
        datosL = letras[i].GetComponent<datosLetra>();

        //solo se necesita la cantidad de puntos
        puntos = datosL.puntos.Length;

        drawLines = new DrawLine[letras.Length];

        //necesitamos varios drawline para poder hacer que esto funcione
        for (int f = 0; f < letras.Length; f++)
        {
            DrawLine clone;
            clone = Instantiate(draw);
            drawLines[f] = clone;
            drawLines[f].gameObject.name = drawLines[f].gameObject.name + " " + f;

            //UnityEngine.Debug.Log("nombre: " + drawLines[f].gameObject.transform.name);

        }

        //Al parecer, unity necesita un momento extra para poder activar esto, pues si lo activa muy rapido, se vuelve a desactivar.
        Invoke("activarPuntosPrueba", (float).2);
        tipoLetra = datosL.tipoLetra;
        idEspecialLetra = datosL.puntoIdEspecial;
        UnityEngine.Debug.Log(idEspecialLetra + "<- id especial");
        datosL.primerPunto(false);
        ultimoPunto = puntos;

        switch (tipoLetra)
        {
            case "cerrada":
                aciertosNecesarios = ultimoPunto + 1;
                break;
            case "unTrazo":
                aciertosNecesarios = ultimoPunto;
                break;
            case "dosTrazos":
                aciertosNecesarios = ultimoPunto;
                DrawLine clone;
                clone = Instantiate(draw);
                listaDL.Add(clone);
                indexDL++;
                break;
        }


        //todo esto es por la primera letra





        updateTimer = true;
        duracion = 0.0f;
        terminar = true;
        idPrimerPunto = 1;
        idPuntoActual = 1;
        idPuntoSiguiente = 1;
        aciertos = 0;
        idRepetido = 0;

        //Lo que hace que aparezcan y desaparezcan los cuadros y el mensaje inmediato

        GameObject.Find("btnSiguiente").SetActive(false);
        GameObject.Find("txtRetroDirecta").SetActive(false);

        //Suscribir este método (SeHizoContacto) al evento de Contacto en la clase Puntos.
        HandCursor.Contacto += SeHizoContacto;
    }

    void cargarLetras()
    {
        GameObject[] arregloLetras = new GameObject[letrasArreglo.Length];

        for (int i = 0; i < letrasArreglo.Length; i++)
        {
            arregloLetras[i] = (Resources.Load("Prefab/Letras paso 2/letra " + arregloLetras[i].ToString(), typeof(GameObject)) as GameObject); ;
        }
    }

    void activarPuntosPrueba()
    {
        datosL.primeraActivacion();
    }

    void Update()
    {

        duracion += Time.deltaTime * 1;
        tiempoIntento += Time.deltaTime * 1;

        if (tiempoIntento > 1000)
        {
            fallo();

        }

    }

    void siguienteLetra()
    {
        i--;
        if (i < 0)
        {
            EjercicioExitoso();
        }
        else
        {
            usarOtroDL = false;
            datosL = letras[i].GetComponent<datosLetra>();
            datosL.primeraActivacion();
            //datosL.activarPuntos();
            datosL.primerPunto(false);
            ultimoPunto = datosL.puntos.Length;
            tipoLetra = datosL.tipoLetra;
            idEspecialLetra = datosL.puntoIdEspecial;
            idPrimerPunto = 1;
            idPuntoActual = 0;
            idPuntoSiguiente = 1;
            idRepetido = 0;
            aciertos = 0;
            switch (tipoLetra)
            {
                case "cerrada":
                    aciertosNecesarios = ultimoPunto + 1;
                    break;
                case "unTrazo":
                    aciertosNecesarios = ultimoPunto;
                    break;
                case "dosTrazos":
                    aciertosNecesarios = ultimoPunto;
                    DrawLine clone;
                    clone = Instantiate(draw);
                    listaDL.Add(clone);
                    indexDL++;
                    break;
            }
            UnityEngine.Debug.Log("siguiente: " + idPuntoSiguiente + " actual: " + idPuntoActual);

        }
    }

    /*
     * Método que verifica que hacer cada vez que el mouse hace contacto con el punto
     */
    public void SeHizoContacto(String idString)
    {


        id = int.Parse(idString);
        UnityEngine.Debug.Log("estoy recibiendo del contacto: " + id);
        //el problema con la siguiente letra es que el ultimo id que tiene es 1
        //y no hay manera de cambiarlo, pues por como funciona el handcursor, envia
        //muchas veces y muy rapido los id's, y como la letra cerrada termina en 1
        //, segun el juego, ya esta activado ese punto y no hace nada. Tenemos que cambiar eso
        if (idRepetido == 1 && idPuntoSiguiente == 1)
        {
            idRepetido = 0;
        }

        if (idRepetido != id)
        {
            primerPuntoNoTocado = false;
            tiempoIntento = 0;

            if (id != idPuntoSiguiente)
            {
                fallo();
            }
            else
            {

                if (id == 1)
                {
                    datosL.primerPunto(true);
                }
                switch (tipoLetra)
                {
                    case "cerrada":
                        verificarContactoCerrada(id);
                        break;
                    case "unTrazo":
                        verificarContactoUnTrazo(id);
                        break;
                    case "dosTrazos":
                        verificarContactoDosTrazos(id);
                        break;
                }
            }
            if (aciertos == aciertosNecesarios)
            {
                //UnityEngine.Debug.Log("¿Como que ya termine?: tengo " + aciertos + " y necesito " + aciertosNecesarios);
                //UnityEngine.Debug.Log("i es al momento de hacer alto: " + i);
                letraCompletada();
                if (i >= 0)
                {
                    drawLines[i].Alto();
                }

            }

        }
        idRepetido = id;

    }

    public void nuevoDibujar(Vector3 mousePos)
    {
        if (i >= 0)
        {
            // UnityEngine.Debug.Log("Dibujando!" + i);
            if (usarOtroDL == false)
            {
                drawLines[i].Dibujar(mousePos);
                //UnityEngine.Debug.Log("usar otro dl es falso aqui en el nuevo dibujar");
            }
            else
            {
                listaDL[indexDL].Dibujar(mousePos);
                //UnityEngine.Debug.Log("usar otro dl es true aqui en el nuevo dibujar");
            }

        }

    }

    void fallo()
    {
        int index = UnityEngine.Random.Range(0, 9);

        AudioClip sonido;
        sonido = Resources.Load<AudioClip>("Sonidos/EnCasoDeError/" + this.sonidosError[index]);
        GetComponent<AudioSource>().PlayOneShot(sonido);
        txtRetroDirecta.SetActive(true);
        txtRetroDirecta.GetComponent<Text>().text = "UPPS";
        Reintentar();

        tiempoIntento = 0;


        GetComponent<AudioSource>().Play();
        Invoke("cargar", 1);
    }

    void cargar()
    {
        txtRetroDirecta.GetComponent<Text>().text = "Terminaste!";

        GameObject.Find("txtRetroDirecta").SetActive(false);
    }


    //aumenta los aciertos e indica cual sera el siguiente punto a conectar.
    void verificarContactoCerrada(int id)
    {
        //para este punto, el contacto ya fue verificado
        if (id == idPuntoSiguiente)
        {

            if (usarOtroDL == false)
            {

                drawLines[i].Seguir();
            }
            idPuntoSiguiente = id + 1;
            idPuntoActual = id;
            if (idPuntoSiguiente > ultimoPunto)
            {
                UnityEngine.Debug.Log("el id especial es, por ende el siguiente: " + idEspecialLetra);
                idPuntoSiguiente = idEspecialLetra;
                Material m = Resources.Load("Materials/punto") as Material;
                GameObject.Find("boton" + idPuntoSiguiente).GetComponent<MeshRenderer>().material = m;
            }

            aciertos++;
            retroPositiva();
            datosL.activarSiguientePunto();
            datosL.activarSiguienteFlecha();

            UnityEngine.Debug.Log("el siguiente es: " + idPuntoSiguiente);
        }

    }
    //aumenta los aciertos e indica cual sera el siguiente punto a conectar.
    void verificarContactoDosTrazos(int id)
    {

        //para este punto, el contacto ya fue verificado
        if (id == idPuntoSiguiente)
        {
            if (usarOtroDL == false)
            {
                drawLines[i].Seguir();
            }
            else
            {
                listaDL[indexDL].Seguir();
            }

            UnityEngine.Debug.Log("dejar de dibujar es: " + drawLines[i].getDejarDeDibujar());
            idPuntoSiguiente = id + 1;
            idPuntoActual = id;
            if (id == idEspecialLetra)
            {
                drawLines[i].Alto();

                usarOtroDL = true;
            }

            aciertos++;
            retroPositiva();
            datosL.activarSiguientePunto();
            datosL.activarSiguienteFlecha();

        }

    }

    void verificarContactoUnTrazo(int id)
    {
        //para este punto, el contacto ya fue verificado
        if (id == idPuntoSiguiente)
        {
            drawLines[i].Seguir();
            idPuntoSiguiente = id + 1;
            idPuntoActual = id;
            aciertos++;

            retroPositiva();
            datosL.activarSiguientePunto();
            datosL.activarSiguienteFlecha();

        }
    }


    public void retroPositiva()
    {

        Material m = Resources.Load("Materials/puntoActivo") as Material;
        GameObject.Find("boton" + idPuntoActual).GetComponent<MeshRenderer>().material = m;
        AudioClip sonido;
        sonido = Resources.Load<AudioClip>("Sonidos/EfectoPositivo2");
        GetComponent<AudioSource>().PlayOneShot(sonido);
    }



    /* Inicializa todas las variables como al principio
     */
    public void Reintentar()
    {
        if (i >= 0)
        {
            drawLines[i].AltoD();
        }
        if (usarOtroDL == true)
        {
            usarOtroDL = false;
            listaDL[indexDL].AltoD();
        }
        Material m = Resources.Load("Materials/punto") as Material;

        datosL.reintento();
        datosL.primerPunto(false);
        EstadoJuego.estadoJuego.registrarIntento(duracion, 0);
        //datosL.reintento();

        idRepetido = 0;
        idPrimerPunto = 1;
        idPuntoActual = 0;
        idPuntoSiguiente = 1;
        aciertos = 0;
        Invoke("ReintentarD", (float).2);
    }


    public void ReintentarD()
    {



        //Aquí debería guardar el intento
        duracion = 0;


        drawLines[i].Redo();
        listaDL[indexDL].Redo();
    }

    void letraCompletada()
    {
        //colorear letra y así
        datosL.desactivarTodo();
        siguienteLetra();
    }
    /*
     * Funcionalidad que pase cuando termino bien el ejercicio
     */
    void EjercicioExitoso()
    {
        if (terminar)
        {
            GameObject go = GameObject.Find("Canvas");


            updateTimer = false;
            AudioClip sonido;
            int index = UnityEngine.Random.Range(0, 9);

            sonido = Resources.Load<AudioClip>("Sonidos/RetroPositiva/" + this.sonidosRe[index]);
            GetComponent<AudioSource>().PlayOneShot(sonido);
            txtRetroDirecta.SetActive(true);


            EstadoJuego.estadoJuego.registrarIntento(duracion, 1);
            ////Por aquí debería guardar el intento

            //string palabra = EstadoJuego.estadoJuego.palabra;
            //GameObject.Find(palabra).GetComponent<SpriteRenderer>().sprite = Resources.Load("Textures/" + palabra + "R", typeof(Sprite)) as Sprite;

            //for (int i = 1; i <= this.puntos; i++)
            //{

            //    GameObject.Find("boton" + i).SetActive(false);
            //}
            GameObject.Find("btnReintentar").SetActive(false);
            GameObject.Find("btnSalir").SetActive(false);

            btnsiguiente.SetActive(true);
            txtRetroDirecta.SetActive(true);
            terminar = false;
            //draw.Redo();
            if (i >= 0)
            {
                drawLines[i].Alto();
            }

            duracion = 0;
            //GameObject.Find("handcursor").GetComponent<HandCursor>().primeraVez = true;


        }
    }

    void OnDestroy()
    {
        HandCursor.Contacto -= SeHizoContacto;
    }
}