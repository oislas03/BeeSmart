using System;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using UnityEngine.SceneManagement;


/**
 * Clase checador. Se encarga de la logica detras de cada ejercicio. Revisa las condiciones de fallo, victoria, reinicio y lógica
 * en el orden de los puntos. Recibe el número del punto que colisiona el handCursor.
 * 
 * puntos: Arreglo de puntos. Se especifica en el inspector de unity
 * hd: objeto HandCursor, para tener aquí adentro el objeto y tenerlo parte del prefab, todavía no se si es necesario
 * draw: objeto LineDraw, maneja el gameObject que dibuja y permite borrar y reiniciar la linea que esta siendo dibujada por el handcursor, pues usan el mismo gameObject 
 * btnSiguiente: Para agregar el btnSiguiente, que aparecera cuando el ejercicio sea completado
 * btnReintentar: Para agregar el btnReintentar, que reinicia todos los parámetros del juego
 * text: texto de retroalimentación directa, que ayudará a saber si lo esta haciendo bien o mal
 * 
 * idPrimerPunto: Id del primer punto que es tocado en el ejercicio
 * idPuntoActual: Id del punto en el que partes, sería como decir "idPuntoAnterior" pues el punto recientemente tocado va a ser el del "id"
 * idPuntoSiguiente: Id del siguiente punto a tocar, para saber si está yendo en orden o no y así, sumar aciertos.
 * aciertos: número de aciertos. Es para terminar el juego, los aciertos deben de ser igual a la cantidad de puntos +1, así se verifica si terminó de dibujar
 * ultimoPunto: el último punto del arreglo cargado en una variable.
 * idRepetido: variable que no deja correr todas las verificaciones dentro del método, pues HandCursor llama por cuadro a este método, y eso genera algunos
 * problemas indeseados
 * reversa: permite saber si el juego está yendo en el orden esperado (false) o al reves (true)
 * segundoContacto: Permite saber que el juego ya hizo un segundo contacto, y así saber si ya se puede saber si está yendo en orden o no
 * 
 **/
public class Checador : MonoBehaviour
{

    #region Singleton
    //un pequeño singleton por aqui...
    private static Checador _instance;

    public static Checador Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Checador");
                go.AddComponent<Checador>();
            }

            return _instance;
        }
    }
    #endregion

    //Arreglo de puntos a utilizar. Aquí se agregan los puntos en la escena o ejercicio
    public Puntos[] puntos;
    // public HandCursor hd;
    //Objeto del DrawLine para llamar sus metodos
    public DrawLine draw;



    private float duracion;
    private float tiempoIntento;
    private bool updateTimer;


    int id;
    private int idPrimerPunto;
    private int idPuntoActual;
    private int idPuntoSiguiente;
    private int aciertos;
    private int ultimoPunto;
    private int idRepetido;
    private bool reversa;
    private bool segundoContacto;
    private bool terminar;

    public GameObject figura;

    public GameObject btnsiguiente;
    public GameObject txtRetroDirecta;

    public string[] sonidosError;
    public string[] sonidosRe;

    public datos datosF;


    //Parte del Singleton en unityCSharp
    void awake()
    {
        _instance = this;
    }

    //Inicialización de variables
    void Start()
    {
        //GameObject.Find("handcursor").GetComponent<HandCursor>().checador = this;
        //GameObject.Find("handcursor").GetComponent<HandCursor>().dl = GameObject.Find("DrawLine").GetComponent<DrawLine>();

        Time.timeScale = 1;

        //    this.figura = Resources.Load("Prefab/trianguloEjercicio") as GameObject;
        //        figura.transform.position = new Vector3(674, 232, 2);


        string palabra = EstadoJuego.estadoJuego.palabra;
        figura = Resources.Load("Prefab/" + palabra + "Ejercicio") as GameObject;
        AudioClip sonido;
        sonido = Resources.Load<AudioClip>("Sonidos/Figuras/"+palabra);
        GetComponent<AudioSource>().PlayOneShot(sonido);

        Instantiate(figura, new Vector3(600, 185, 0), Quaternion.identity);


       // Vector3 pos = GameObject.Find(palabra).GetComponent<Transform>().transform;
        //pos.x += 100;
        //pos.y += 20;

        //GameObject.Find("colisionador").GetComponent<Transform>().transform.localScale = GameObject.Find(palabra).GetComponent<Transform>().transform;


        //Vector3 posicion = GameObject.Find("colisionador").GetComponent<Transform>().transform.position;


        //    GameObject.Find(palabra).GetComponent<SpriteRenderer>().transform.localScale = pos;
        //  GameObject.Find(palabra).GetComponent<Transform>().position= posicion;
        // GameObject.Find("kk").GetComponent<BoxCollider>().size = (GameObject.Find(palabra).GetComponent<SpriteRenderer>().bounds.size);

        datosF = figura.GetComponent<datos>();

        puntos = datosF.puntos;


        updateTimer = true;
        duracion = 0.0f;
        terminar = true;
        idPrimerPunto = 0;
        idPuntoActual = 0;
        idPuntoSiguiente = 0;
        aciertos = 0;
        idRepetido = 0;
        reversa = false;
        segundoContacto = false;
        ultimoPunto = puntos.Length;
        GameObject.Find("DrawLine").GetComponent<DrawLine>().Seguir();


        //Lo que hace que aparezcan y desaparezcan los cuadros y el mensaje inmediato
        // btnReintentar.SetActive(true);

        GameObject.Find("btnSiguiente").SetActive(false);
        GameObject.Find("txtRetroDirecta").SetActive(false);





        //Color altColor = Color.black;

        //GameObject punto = GameObject.Find("boton1");


        int puntosLength = puntos.Length;


        //Suscribir este método (SeHizoContacto) al evento de Contacto en la clase Puntos.
        //Este mecanismo se usa para agregar observadores o quitarlos
        HandCursor.Contacto += SeHizoContacto;
    }

    void Update()
    {

        duracion += Time.deltaTime * 1;
        tiempoIntento += Time.deltaTime * 1;

        //UnityEngine.Debug.Log("tiempo del intento: " + tiempoIntento);
        if (tiempoIntento > 60)
        {
            //UnityEngine.Debug.Log("tiempo del intento excedido: " + tiempoIntento);
            fallo();

        }

    }

    /*
     * Método que verifica que hacer cada vez que el mouse hace contacto con el punto
     */
    public void SeHizoContacto(String idString)
    {

        id = int.Parse(idString);

        //antes que nada, verificar si ya termino bien el ejercicio
        if (aciertos == ultimoPunto + 1)
        {
            EjercicioExitoso();
        }

        if (idRepetido != id)
        {
            tiempoIntento = 0;


            if (idPrimerPunto == 0)
            {
                primerContacto(id);
            }

            //errores que pueden pasar despues del primer contacto, como conectar el 2 con el 4, o el 4 con el 2
            else if (segundoContacto == false)
            {
                UnityEngine.Debug.Log("1 Segundo contacto es: " + segundoContacto);
                if (id != ultimoPunto && id > (idPuntoActual + 1))
                {
                    if (idPuntoActual == 1)
                    {
                        UnityEngine.Debug.Log("fallo 1");
                        fallo();
                    }

                }
                else if (id >= (idPuntoActual + 2) || id <= (idPuntoActual - 2))
                {
                    UnityEngine.Debug.Log("id es: " + id);
                    if (id == 1)
                    {
                        UnityEngine.Debug.Log("2 Salio todo bien hasta aqui?");
                        segundoContacto = true;
                        reversa = esReversa(id);
                        if (reversa == true)
                        {
                            idPuntoSiguiente = id;
                            //if (idPuntoSiguiente == 0)
                            //{
                            //    idPuntoSiguiente = ultimoPunto;
                            //}
                            //Debug.Log("idPuntoSiguiente en reversa es: " + idPuntoSiguiente);
                        }
                    }
                    else if (id == ultimoPunto)
                    {
                        segundoContacto = true;
                        reversa = esReversa(id);
                        if (reversa == true)
                        {
                            idPuntoSiguiente = id;
                            //if (idPuntoSiguiente == 0)
                            //{
                            //    idPuntoSiguiente = ultimoPunto;
                            //}
                            //Debug.Log("idPuntoSiguiente en reversa es: " + idPuntoSiguiente);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("fallo 2");
                        fallo();
                    }

                }
                else
                {
                    UnityEngine.Debug.Log("2 Salio todo bien hasta aqui?");
                    segundoContacto = true;
                    reversa = esReversa(id);
                    if (reversa == true)
                    {
                        idPuntoSiguiente = id;
                        //if (idPuntoSiguiente == 0)
                        //{
                        //    idPuntoSiguiente = ultimoPunto;

                        //}
                        //Debug.Log("idPuntoSiguiente en reversa es: " + idPuntoSiguiente);
                    }
                }

            }


            if (segundoContacto == true)
            {
                UnityEngine.Debug.Log("3 Segundo contacto es: " + segundoContacto);

                if (reversa == true)
                {


                    if (id < idPuntoSiguiente || id > idPuntoActual)
                    {
                        if (id != ultimoPunto)
                        {
                            UnityEngine.Debug.Log("fallo 3");

                            fallo();
                        }

                    }
                    UnityEngine.Debug.Log("4 el error, estara antes de entrar al verificar contacto reversa?");
                    VerificarContactoReversa(id);

                }
                else
                {
                    //Sale este fallo cuando el punto va del 4 al 1
                    if (id > idPuntoSiguiente || id < idPuntoActual)
                    {
                        if (idPuntoSiguiente != 1)
                        {
                            UnityEngine.Debug.Log("fallo 4");
                            fallo();
                        }

                    }
                    verificarContacto(id);

                }

            }

        }
        idRepetido = id;

    }

    bool esReversa(int id)
    {
        if (idPuntoActual == 1)
        {

            if (id == ultimoPunto)
            {
                UnityEngine.Debug.Log("Hue1: reversa");
                return true;
            }
            else if (id == idPuntoActual + 1)
            {
                UnityEngine.Debug.Log("Hue1: no reversa");
                return false;
            }
        }
        else if (idPuntoActual == ultimoPunto)
        {

            if (id == 1)
            {
                UnityEngine.Debug.Log("Hue2: no reversa");
                return false;
            }
            else
            {
                UnityEngine.Debug.Log("Hue2: reversa");
                return true;
            }

        }
        else
        {

            if (id == idPuntoActual + 1)
            {
                UnityEngine.Debug.Log("Hue3: no reversa");
                return false;
            }
            else if (id < idPuntoActual)
            {
                UnityEngine.Debug.Log("Hue3: reversa");
                return true;
            }
        }
        return false;
    }

    void fallo()
    {
        int index = UnityEngine.Random.Range(0, 9);

        AudioClip sonido;
        sonido = Resources.Load<AudioClip>("Sonidos/EnCasoDeError/" + this.sonidosError[index]);
        GetComponent<AudioSource>().PlayOneShot(sonido);
        txtRetroDirecta.SetActive(true);




        UnityEngine.Debug.Log("Eso ha sido un fallo xd");
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





    /*
     * Método que se llama al primer contacto de un punto, inicializa todas las variables.
     * mientras dibujando = true, se dibujará
     */
    void primerContacto(int id)
    {
        //stopWatch.Start();
        //Decir quien es el primer punto y de ahi, irse punto por punto
        idPrimerPunto = id;
        idPuntoActual = id;
        idPuntoSiguiente = id + 1;
        if (idPuntoSiguiente > puntos.Length)
        {
            idPuntoSiguiente = 1;
        }
        aciertos++;
        retroPositiva();
        UnityEngine.Debug.Log("Primer punto = " + idPrimerPunto);



    }


    //aumenta los aciertos e indica cual sera el siguiente punto a conectar.
    void verificarContacto(int id)
    {
        //para este punto, el contacto ya fue verificado
        if (id == idPuntoSiguiente)
        {
            idPuntoSiguiente = id + 1;
            if (idPuntoSiguiente > ultimoPunto)
            {
                idPuntoSiguiente = 1;
            }
            idPuntoActual = id;

            UnityEngine.Debug.Log("No reversa: Punto actual= " + idPuntoActual + ", Siguiente punto= " + idPuntoSiguiente);       

            aciertos++;
            retroPositiva();
            UnityEngine.Debug.Log(aciertos);

        }

    }


    public void retroPositiva() {
        Material m = Resources.Load("Materials/puntoActivo") as Material;
        UnityEngine.Debug.Log(GameObject.Find("boton" + idPuntoActual).GetComponent<MeshRenderer>().material);
        UnityEngine.Debug.Log(m);
        GameObject.Find("boton" + idPuntoActual).GetComponent<MeshRenderer>().material = m;
        AudioClip sonido;
        sonido = Resources.Load<AudioClip>("Sonidos/EfectoPositivo2");
        GetComponent<AudioSource>().PlayOneShot(sonido);
    }




    void VerificarContactoReversa(int id)
    {

        //para este punto, el contacto ya fue verificado
        if (id == idPuntoSiguiente)
        {
            idPuntoSiguiente = id - 1;
            if (idPuntoSiguiente == 0)
            {
                idPuntoSiguiente = ultimoPunto;
            }
            idPuntoActual = id;

            UnityEngine.Debug.Log("Reversa: Punto actual= " + idPuntoActual + ", Siguiente punto= " + idPuntoSiguiente);
            aciertos++;
            retroPositiva();
            UnityEngine.Debug.Log(aciertos);

        }
    }

    /* Inicializa todas las variables como al principio
     */
    public void Reintentar()
    {
        EstadoJuego.estadoJuego.registrarIntento(duracion, 0);
        Invoke("ReintentarD", 1);
    }


    public void ReintentarD()
    {
        Material m = Resources.Load("Materials/punto") as Material;
        for (int i = 1; i <= puntos.Length; i++)
        {
            GameObject.Find("boton" + i).GetComponent<MeshRenderer>().material = m;

        }
        idRepetido = 0;
        idPrimerPunto = 0;
        idPuntoActual = 0;
        idPuntoSiguiente = 0;
        aciertos = 0;
        segundoContacto = false;
        reversa = false;
        //Aquí debería guardar el intento
        duracion = 0;
        draw.Redo();
    }

    /*
     * Funcionalidad que pase cuando termino bien el ejercicio
     */
    void EjercicioExitoso()
    {
        if (terminar)
        {
            GameObject go = GameObject.Find("Canvas");
            //Transform[] transform = go.GetComponentsInChildren(typeof(Transform), true);
            //foreach (Transform component in transform)
            //{
            //    if (component.name == "btnSiguiente")
            //    {

            //    }

            //}

            updateTimer = false;
            AudioClip sonido;
            int index = UnityEngine.Random.Range(0, 9);

            sonido = Resources.Load<AudioClip>("Sonidos/RetroPositiva/" + this.sonidosRe[index]);
            GetComponent<AudioSource>().PlayOneShot(sonido);
            txtRetroDirecta.SetActive(true);


            UnityEngine.Debug.Log("asddsadas");
            EstadoJuego.estadoJuego.registrarIntento(duracion, 1);
            //Por aquí debería guardar el intento
            UnityEngine.Debug.Log("Felicidades terminaste!");

            string palabra = EstadoJuego.estadoJuego.palabra;
            GameObject.Find(palabra).GetComponent<SpriteRenderer>().sprite = Resources.Load("Textures/"+palabra+"R", typeof(Sprite)) as Sprite;

            for (int i=1; i <= this.puntos.Length; i++) {

                GameObject.Find("boton" + i).SetActive(false);
            }
            GameObject.Find("btnReintentar").SetActive(false);
            GameObject.Find("btnSalir").SetActive(false);

            btnsiguiente.SetActive(true);
            txtRetroDirecta.SetActive(true);
            terminar = false;
            draw.Redo();
            duracion = 0;
            GameObject.Find("handcursor").GetComponent<HandCursor>().primeraVez = true;
            //Destroy(GameObject.Find("handcurso"));


        }
    }

    void OnDestroy()
    {
        HandCursor.Contacto -= SeHizoContacto;
    }
}
