using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * 
 * ESTE ES EL DEL PROYECTO A DONDE VA DIRIGIDO, NO LO OLVIDES
 * 
 * 
 */
public class HandCursor : MonoBehaviour
{

    private PXCMSenseManager sm;
    private PXCMHandCursorModule cursorModule;
    private PXCMCursorConfiguration cursorConfig;
    private PXCMSession session;
    private PXCMCursorData cursorData;
    private PXCMCursorData.GestureData gestureData;
    private PXCMPoint3DF32 adaptivePoints;
    private PXCMPoint3DF32 coordinates2d;
    private PXCMPoint3DF32 imagePoint; // prueba de concepto de un método nuevo ----------------------------------------
    private Ray ray;
    private RaycastHit hit;
    private bool isCursorPen = true;
    private bool isCursorSyllable = false;
    private bool llamarCorutina = true;
    private EstadoJuego estadoJuego;
    private float tiempoDeJuego = 0;
    private int terminar = 0;

    public Texture2D cursorImage;
    public Vector3 mousePos;
    public bool click;
    public static HandCursor me;
    public delegate void JugadorEmpiezaDibujar(String id);
    public static event JugadorEmpiezaDibujar Contacto;
    public Vector3 v3 = new Vector3();
    public bool primeraVez = true;
    public string pasoNombre;
    public GameObject bordePanel;
    public Texture2D[] manos = new Texture2D[4];

    public string silabaAgarrada;
    public string[] silabas;
    public int tamanioSilabas;
    public int aciertosSilabas = 0;
    public string txtPalabra;

    // variable para mantener conteo de un segundo en el juego
    public float tiempo;

    private void Awake()
    {
        if (me == null)
        {
            me = this;

        }
        else if (me != this)
        {
            Destroy(gameObject);
        }
        pasoNombre = SceneManager.GetActiveScene().name;
        if (pasoNombre == "paso 3")
        {
            // Se obtienen la palabra que se está jugando actualmente y el componente estado juego
            txtPalabra = EstadoJuego.estadoJuego.palabra;
            estadoJuego = EstadoJuego.estadoJuego;
            // Se inicia el arreglo de sílabas al tamaño necesario con sus sílabas necesarias
            silabas = estadoJuego.obtenerSilabasPalabra(txtPalabra);
            tamanioSilabas = silabas.Length;
            Debug.Log("Palabra que se está jugando actualmente: " + txtPalabra);
            Debug.Log("Valor de estado juego: " + estadoJuego);
            for (int i = 0; i < tamanioSilabas; i++)
            {
                Debug.Log("Arreglo de sílabas: " + silabas[i].ToString());
            }
            Debug.Log("Tamaño de sílabas: " + tamanioSilabas);
            // Se asigna la imagen al cursor principal
            cursorImage = Resources.Load("Textures/mano1", typeof(Texture2D)) as Texture2D;
            // Se reparten todas las piezas del tablero
            repartirPiezas();

            // Se reproduce el sonido acorde a la palabra en juego
            Debug.Log("Intentando hacer un sonido de: " + txtPalabra);
            AudioClip audioClip;
            audioClip = Resources.Load("Sonidos/Figuras/" + txtPalabra, typeof(AudioClip)) as AudioClip;
            AudioSource sonido = GameObject.Find("sonido").GetComponent<AudioSource>();
            reproducirSonido(sonido, audioClip);
            GameObject.Find("btnAudio").GetComponent<AudioSource>().clip = audioClip;

            // Se esconde el botón de siguiente puesto que no tiene funcionalidad a la hora de iniciar el juego
            GameObject.Find("btnSiguiente").gameObject.SetActive(false);
        }
    }


    private void Start()
    {
        Time.timeScale = 1;
        tiempoDeJuego += Time.deltaTime;
        ConfigureRealSense();
        Update();
    }


    void OnMouseEnter()
    {
        UnityEngine.Cursor.visible = true;
    }

    public void ConfigureRealSense()
    {
        // Create an instance of the SenseManager
        sm = PXCMSenseManager.CreateInstance();

        // Enable cursor tracking
        sm.EnableHandCursor();

        // Create a session of the RealSense
        session = PXCMSession.CreateInstance();

        // Get an instance of the hand cursor module
        cursorModule = sm.QueryHandCursor();
        // Get an instance of the cursor configuration
        cursorConfig = cursorModule.CreateActiveConfiguration();

        // Make configuration changes and apply them
        cursorConfig.EnableEngagement(true);
        cursorConfig.EnableAllGestures();
        cursorConfig.EnableAllAlerts();
        cursorConfig.ApplyChanges();

        // Initialize the SenseManager pipeline
        sm.Init();
    }

    private void Update()
    {
        tiempoDeJuego += Time.deltaTime;
        StartCoroutine("toUpdate");
    }
    PXCMImage image;
    IEnumerator toUpdate()
    {
        bool handInRange = true;

        if (sm.AcquireFrame(true).IsSuccessful())
        {
            // Hand and cursor tracking 
            cursorData = cursorModule.CreateOutput();
            adaptivePoints = new PXCMPoint3DF32();
            coordinates2d = new PXCMPoint3DF32();
            PXCMCursorData.BodySideType bodySide;

            cursorData.Update();

            // Probando el código de la documentación
            PXCMCapture.Sample sample;
            sample = sm.QueryHandCursorSample();
            image = sample.depth;


            // Check if alert data has fired
            for (int i = 0; i < cursorData.QueryFiredAlertsNumber(); i++)
            {
                PXCMCursorData.AlertData alertData;
                cursorData.QueryFiredAlertData(i, out alertData);

                if ((alertData.label == PXCMCursorData.AlertType.CURSOR_NOT_DETECTED) ||
                    (alertData.label == PXCMCursorData.AlertType.CURSOR_DISENGAGED) ||
                    (alertData.label == PXCMCursorData.AlertType.CURSOR_OUT_OF_BORDERS))
                {
                    handInRange = false;
                    Debug.Log("¡Tu mano no está en rango!");
                }
                else
                {
                    handInRange = true;
                    Debug.Log("¡Tu mano está en rango!");
                }
            }


            if (cursorData.IsGestureFired(PXCMCursorData.GestureType.CURSOR_CLICK, out gestureData))
            {
                click = true;
                Debug.Log("Click cambiando a true");
            }

            // Track hand cursor if it's within range
            int detectedHands = cursorData.QueryNumberOfCursors();

            if (detectedHands > 0)
            {
                // Retrieve the cursor data by order-based index
                PXCMCursorData.ICursor iCursor;
                cursorData.QueryCursorData(PXCMCursorData.AccessOrderType.ACCESS_ORDER_NEAR_TO_FAR,
                                           0,
                                           out iCursor);

                // Retrieve controlling body side (i.e., left or right hand)
                bodySide = iCursor.QueryBodySide();

                //Debug.Log("Resolución actual: " + Screen.currentResolution.height + ", " + Screen.currentResolution.width);



                adaptivePoints = iCursor.QueryAdaptivePoint();
                imagePoint = Camera.main.WorldToScreenPoint(iCursor.QueryCursorPointImage());

                //coordinates2d.x = (adaptivePoints.x * resWidth);
                //coordinates2d.y = (adaptivePoints.y * resHeight);
                //Debug.Log("Image point: " + imagePoint.x + ", " + imagePoint.y);
                coordinates2d.x = imagePoint.x;
                coordinates2d.y = resHeight - imagePoint.y;

                //Debug.Log("Current resolution: " + resWidth + "x" + resHeight);
                //Debug.Log("Coordinates2d: " + coordinates2d.x + ", " + coordinates2d.y);

                mousePos = coordinates2d;
                //imagePoint = Camera.main.WorldToScreenPoint(coordinates2d);
                mousePos = Camera.main.WorldToScreenPoint(imagePoint);
                //v3 = Camera.main.WorldToScreenPoint(mousePos);
                //Debug.Log("MousePos: " + mousePos); 
            }
            else
            {
                bodySide = PXCMCursorData.BodySideType.BODY_SIDE_UNKNOWN;
            }

            // Resume next frame processing
            cursorData.Dispose();
            sm.ReleaseFrame();
        }
        yield return null;
    }

    int resWidth = Screen.width;
    int resHeight = Screen.height;

    void OnGUI()
    {
        // Cuadrado y posición en la pantalla que ayuda a dibujar el lápiz.
        Rect posa = new Rect(mousePos.x + 2, resHeight - mousePos.y - 23, cursorImage.width, cursorImage.height);
        GUI.Label(posa, cursorImage);

        ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            // Acciones a ejecutar para la escena del paso 1
            if (pasoNombre == "paso 1")
            {
                if (hit.collider.tag == "salir")
                {
                    SceneManager.LoadScene("eligeTema");
                }
                else if (hit.collider.tag == "punto")
                {
                    var nombre = hit.collider.gameObject.name;
                    nombre = nombre.Substring(5);
                    // Revisa que haya un objeto suscrito al evento
                    if (Contacto != null)
                    {
                        // llama al evento del observador
                        Contacto(nombre);
                    }

                }
                if (hit.collider.tag == "areaDibujable")
                {
                    GameObject.Find("DrawLine").GetComponent<DrawLine>().Dibujar(coordinates2d);
                    Debug.Log("Coordinates2d en HandCursor: " + coordinates2d.x + ", " + coordinates2d.y);
                }
            }
            else if (pasoNombre == "paso 2")
            {
                if (hit.collider.tag == "salir")
                {
                    SceneManager.LoadScene("eligeTema");
                }
                else if (hit.collider.tag == "punto")
                {
                    var nombre = hit.collider.gameObject.name;
                    nombre = nombre.Substring(5);
                    // Revisa que haya un objeto suscrito al evento
                    if (Contacto != null)
                    {
                        // llama al evento del observador
                        Contacto(nombre);
                    }

                }
                if (hit.collider.tag == "areaDibujable")
                {
                    GameObject.Find("BgImage").GetComponent<ChecadorLetras>().nuevoDibujar(coordinates2d);
                }
            }

            // Acciones a ejecutar para la escena del paso 3
            if (pasoNombre == "paso 3")
            {
                if (hit.collider != null)
                {
                    if (hit.transform.tag == "silabaLetra")
                    {
                        // al hacer un click se ejecuta el evento de "arrastrar" la sílaba
                        //if (click)
                        //{
                        //}
                        //click = false;
                        if (hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite == null)
                        {
                            if (llamarCorutina)
                            {
                                devolver_pieza();
                                llamarCorutina = false;
                            }
                        }
                        else if (isCursorPen)
                        {
                            if (llamarCorutina)
                            {
                                agarrar_pieza();
                                llamarCorutina = false;
                            }
                            else
                            {
                                reproducirManoGif();
                            }
                        }
                    }
                    else if (hit.transform.tag == "silabaEspacio")
                    {
                        if (isCursorSyllable)
                        {
                            if (llamarCorutina)
                            {
                                colocar_pieza();
                                llamarCorutina = false;
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Método que permite repartir las piezas al azar en el tablero de piezas, si las piezas necesarias no son iguales al tamaño máximo
    /// de piezas posibles en el tablero, estos espacios se llenan con piezas al azar que no necesariamente corresponden a las piezas
    /// correctas necesarias para ganar.
    /// </summary>
    public void repartirPiezas()
    {
        // Se declara un arreglo de sprites para guardar las imagenes a cargar
        Sprite[] arregloSprites = new Sprite[5];
        string[] piezasAlAzar = { "cu", "trian", "tan", "es", "cua" };
        // Se itera cuantas veces se necesite para llenar el tamaño de sílabas posibles
        for (int i = 0; i < 5; i++)
        {
            // Primero se añaden las piezas que corresponden
            if (i < tamanioSilabas)
            {
                arregloSprites[i] = (Resources.Load("Sprites/" + silabas[i].ToString(), typeof(Sprite)) as Sprite);
            }
            // Si se requieren menos espacios de los máximos, se llenan los espacios disponibles con piezas al azar
            else if (i >= tamanioSilabas && i < 5)
            {
                int random = UnityEngine.Random.Range(0, 4);
                arregloSprites[i] = (Resources.Load("Sprites/" + piezasAlAzar[random].ToString(), typeof(Sprite)) as Sprite);
            }
        }
        // Se crea una lista de índices para cargar al azar los índices de los espacios en blanco
        List<int> espacioIndices = new List<int>();
        espacioIndices.AddRange(new int[] { 1, 2, 3, 4, 5 });
        // Se cargan los sprites dentro de las piezas necesitadas
        for (int i = 0; i < 5; i++)
        {
            // Si no hay elementos nulos, se establecen los espacios vacíos al valor del sprite en la escena
            if (arregloSprites[i] != null)
            {
                // Se busca un índice entre 1 y 5
                int num = UnityEngine.Random.Range(0, espacioIndices.Count);
                // Se establece ese número al azar como el índice para buscar en el arreglo de sprites
                int indice = espacioIndices[num];
                // Se encuentra el espacio en blanco para poner un sprite en la pieza de acuerdo a los valores al azar de 1 a 5
                GameObject espacio = GameObject.Find("SilabaLetras" + indice);
                // Se establece el sprite encontrado en la pieza vacía encontrada
                SpriteRenderer imagenSilaba = espacio.GetComponent<SpriteRenderer>();
                imagenSilaba.sprite = arregloSprites[i];
                //Por último se elimina el índice que ya usamos una vez de las posibles variables índice que se pueden elegir
                espacioIndices.Remove(indice);
            }
        }

        // Se desactivan los espacios para fichas que no son necesarios
        int espaciosDesactivables = 5 - tamanioSilabas; int x = 5; // Variables para manejar los espacios a desactivar y los nombres de estos Ej: SilabaEspacio5
        for (int i = 0; i < espaciosDesactivables; i++)
        {
            string nombre = "SilabaEspacio" + x.ToString();
            GameObject.Find(nombre).SetActive(false);
            x--;
        }
    }

    /// <summary>
    /// Corrutina que permite llamar al método de agarrarPieza.
    /// </summary>
    public void agarrar_pieza()
    {
        StartCoroutine("agarrarPieza");
    }

    /// <summary>
    /// Método que permite agarrar una pieza con el cursor del tablero de piezas.
    /// </summary>
    /// <returns></returns>
    IEnumerator agarrarPieza()
    {
        if (isCursorPen)
        {
            float tiempoRestante = tiempo;
            while (tiempoRestante > 0)
            {
                Debug.Log("Tiempo restante en agarrar pieza: " + tiempoRestante);
                yield return new WaitForSeconds(1);
                tiempoRestante--;
            }

            ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject != null)
                {
                    // Se obtiene el objeto al que toca el Raycast
                    GameObject objetoSilaba = hit.transform.gameObject;
                    // Se obtiene el nombre del source image que tiene la sílaba que se tocó
                    string silaba = objetoSilaba.GetComponent<SpriteRenderer>().sprite.name;
                    silabaAgarrada = silaba;
                    // Se carga la nueva textura y se le da el tamaño adecuado
                    Texture2D imagen = Resources.Load("Textures/" + silaba, typeof(Texture2D)) as Texture2D;
                    cursorImage = imagen;

                    // Atributos de semáforo cambian de estado
                    isCursorPen = false;
                    isCursorSyllable = true;
                    //llamarCorutina = true;

                    StartCoroutine("tiempoLibre");

                    // Se desactiva la imagen de la sílaba que se agarró con el cursor
                    objetoSilaba.GetComponent<SpriteRenderer>().sprite = null;
                }
            }
            else
            {
                llamarCorutina = true;
            }
        }
        else
        {
            Debug.Log("El cursor es una sílaba");
        }
        yield return null;
    }

    /// <summary>
    /// Corrutina que permite llamar al método de colocarPieza.
    /// </summary>
    public void colocar_pieza()
    {
        StartCoroutine("colocarPieza");
    }

    /// <summary>
    /// Método que permite colocar una pieza con el cursor dentro de los espacios vacíos en el pizarrón.
    /// </summary>
    /// <returns></returns>
    IEnumerator colocarPieza()
    {
        if (isCursorSyllable)
        {
            float tiempoRestante = tiempo;
            while (tiempoRestante > 0)
            {
                Debug.Log("Tiempo restante en colocar pieza: " + tiempoRestante);
                yield return new WaitForSeconds(1);
                tiempoRestante--;

            }
            ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject != null)
                {
                    // Se obtiene el GameObject del espacio vacío, después se coloca la pieza del cursor sobre él
                    GameObject espacioVacio = hit.transform.gameObject;

                    int indice = int.Parse(espacioVacio.name.Substring(13));
                    indice -= 1;
                    if (silabas[indice].Equals(silabaAgarrada))
                    {
                        SpriteRenderer imagenSilaba = espacioVacio.GetComponent<SpriteRenderer>();
                        imagenSilaba.sprite = Resources.Load("Sprites/" + silabaAgarrada, typeof(Sprite)) as Sprite;

                        // Una vez colocada la pieza en su lugar, se devuelve al cursor la imagen de la pluma
                        cursorImage = Resources.Load("Textures/mano1", typeof(Texture2D)) as Texture2D;

                        // Se aumentan los aciertos para saber cuándo terminará el ejercicio
                        aciertosSilabas++;

                        // Atributos de semáforo cambian de estado
                        isCursorPen = true;
                        isCursorSyllable = false;
                        //llamarCorutina = true;

                        StartCoroutine("tiempoLibre");

                        if (aciertosSilabas == tamanioSilabas)
                            ganar();
                        else
                            Debug.Log("Aún no has ganado");
                    }
                    else
                    {
                        // Se utiliza un índice al azar para asignar una de dos retroalimentaciones de error auditiva.
                        string audioError = "";
                        int random = UnityEngine.Random.Range(1, 3);
                        if (random == 1)
                            audioError = "Oh-Oh";
                        else if (random == 2)
                            audioError = "Ups";
                        AudioClip audioClip;
                        audioClip = Resources.Load("Sonidos/EnCasoDeError/" + audioError, typeof(AudioClip)) as AudioClip;
                        AudioSource sonido = GameObject.Find("sonido").GetComponent<AudioSource>();
                        reproducirSonido(sonido, audioClip);

                        StartCoroutine("tiempoLibre");
                        // Se establece en positivo la llamada a una corrutina
                        //llamarCorutina = true;
                    }
                }
            }
            else
            {
                llamarCorutina = true;
            }
        }
        else
        {
            Debug.Log("El cursor es una pluma");
        }
        yield return null;
    }

    /// <summary>
    /// Corrutina que permite llamar al método devolverPieza.
    /// </summary>
    public void devolver_pieza()
    {
        StartCoroutine("devolverPieza");
    }

    /// <summary>
    /// Método que permite devolver una pieza al tablero de piezas.
    /// </summary>
    /// <returns></returns>
    IEnumerator devolverPieza()
    {
        float tiempoRestante = tiempo;
        while (tiempoRestante > 0)
        {
            Debug.Log("Tiempo restante en colocar pieza: " + tiempoRestante);
            yield return new WaitForSeconds(1);
            tiempoRestante--;

        }
        ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject != null)
            {
                // Se obtiene el GameObject del espacio vacío, después se coloca la pieza del cursor sobre él
                GameObject espacioVacio = hit.transform.gameObject;
                SpriteRenderer imagenSilaba = espacioVacio.GetComponent<SpriteRenderer>();
                imagenSilaba.sprite = Resources.Load("Sprites/" + silabaAgarrada, typeof(Sprite)) as Sprite;

                // Una vez colocada la pieza en su lugar, se devuelve al cursor la imagen de la pluma
                cursorImage = Resources.Load("Textures/mano1", typeof(Texture2D)) as Texture2D;

                // Atributos de semáforo cambian de estado
                isCursorPen = true;
                isCursorSyllable = false;
                //llamarCorutina = true;
                StartCoroutine("tiempoLibre");
            }
        }
        else
        {
            llamarCorutina = true;
        }
        yield return null;
    }

    /// <summary>
    /// Al realizar una acción con el cursor sobre las piezas, se tiene que esperar el tiempo especificado antes de poder realizar
    /// una nueva acción.
    /// </summary>
    /// <returns></returns>
    IEnumerator tiempoLibre()
    {
        float tiempoDeEspera = 1.0f;
        while (tiempoDeEspera > 0)
        {
            Debug.Log("Tiempo de espera para una nueva acción del cursor: " + tiempoDeEspera);
            yield return new WaitForSeconds(1);
            tiempoDeEspera--;
        }
        llamarCorutina = true;
        yield return null;
    }

    /// <summary>
    /// Método que se llama cuando se determina que el jugador ha terminado la actividad.
    /// </summary>
    public void ganar()
    {
        Debug.Log("Has ganado " + tiempoDeJuego.ToString());
        EstadoJuego.estadoJuego.registrarIntento(tiempoDeJuego, 1);
    }

    /// <summary>
    /// Método que permite reproducir un audio en tiempo de ejecución de acuerdo a los parámetros recibidos.
    /// </summary>
    /// <param name="sonido">Componente que reproducirá el sonido</param>
    /// <param name="audioClip">Audio que se reproducirá</param>
    public void reproducirSonido(AudioSource sonido, AudioClip audioClip)
    {
        sonido.clip = audioClip;
        sonido.Play();
    }

    /// <summary>
    /// Método que permite reproducir un arreglo de texturas con forma de mano para simular una animación con el cursor.
    /// </summary>
    /// <param name="framesPerSecond"></param>
    public void reproducirManoGif()
    {
        float index = Time.time * 10.0f;
        index = index % manos.Length;
        cursorImage = manos[(int)index];
    }
}