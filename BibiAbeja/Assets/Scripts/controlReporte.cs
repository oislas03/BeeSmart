using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlReporte : MonoBehaviour
{


    List<Reporte> reportesObj = new List<Reporte>();
    double porcentaje = 0;
    List<string> fechas = new List<string>();
    List<double> tiempos = new List<double>();
    List<string> paths = new List<string>();
    string[] divisionFecha = new string[140];
    string[] divisionTiempo = new string[140];
    int inicio = 0;
    string cadenaFechas;
    string cadenaTiempos;
    int maximo = 0;
    int maximoTiempo = 0;
    int contFechas = 0;
    int indice = 0;
    int contTiempo2 = 0;
    int contFechas2 = 0;
    // Use this for initialization

    void Start()
    {
        EstadoJuego.estadoJuego.cargarTemas(GameObject.Find("DropDownListTemas").GetComponent<Dropdown>());
        EstadoJuego.estadoJuego.cargarNinios(GameObject.Find("DropDownListNinios").GetComponent<Dropdown>());
        for (int x=0; x<10; x++)
        {
            GameObject.Find("btnBoton" + x.ToString()).GetComponent<Button>().enabled = false;
            GameObject.Find("btnBoton" + x.ToString()).transform.localScale = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void botonClick()
    {
        for (int x = 0; x < 10; x++)
        {
            GameObject.Find("btnBoton" + x.ToString()).GetComponent<Button>().enabled = false;
            GameObject.Find("btnBoton" + x.ToString()).transform.localScale = new Vector3(0, 0, 0);
        }
        reportesObj = new List<Reporte>();
        fechas = new List<string>();
        tiempos = new List<double>();
        paths = new List<string>();
        divisionFecha = new string[140];
        divisionTiempo = new string[140];
        inicio = 0;
        cadenaFechas = "";
        cadenaTiempos = "";
        maximo = 0;
        maximoTiempo = 0;
        contFechas = 0;
        indice = 0;
        contTiempo2 = 0;
        contFechas2 = 0;
        porcentaje = 0;
        int index = GameObject.Find("DropDownListFiguras").GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = GameObject.Find("DropDownListFiguras").GetComponent<Dropdown>().options;
        int index2 = GameObject.Find("DropDownListNiveles").GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions2 = GameObject.Find("DropDownListNiveles").GetComponent<Dropdown>().options;
        int index3 = GameObject.Find("DropDownListTurno").GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions3 = GameObject.Find("DropDownListTurno").GetComponent<Dropdown>().options;
        int index4 = GameObject.Find("DropDownListNinios").GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions4 = GameObject.Find("DropDownListNinios").GetComponent<Dropdown>().options;
        if (index == 0 || index2 == 0 || index3 == 0 || index4==0)
        {
            GameObject.Find("txtFechas").GetComponent<Text>().text = "";
            GameObject.Find("txtDuracion").GetComponent<Text>().text = "";
            GameObject.Find("txtPorcentajeExito").GetComponent<Text>().text = "";
            GameObject.Find("txtFin").GetComponent<Text>().text = "";
            return;
        }
        else
        {
            string palabra = menuOptions[index].text;
            string nivel = menuOptions2[index2].text;
            string turno = menuOptions3[index3].text;
            Debug.Log(palabra);
            cargarReporte(index4,palabra, nivel, turno, porcentaje);
        }

    }

    public void cargarReporte(int ninio,string palabra,string nivel,string turno,double porcentaje)
    {
        reportesObj = EstadoJuego.estadoJuego.cargarReporteDelNinio(ninio,palabra,nivel,turno);
        porcentaje = EstadoJuego.estadoJuego.calcularPorcentaje(ninio,palabra,nivel,turno);

        foreach (Reporte reporte in reportesObj)
        {
            if (!fechas.Contains(reporte.fecha))
            {
                fechas.Add(reporte.fecha);
            }
        }

        foreach (Reporte reporte in reportesObj)
        {
            if (!tiempos.Contains(reporte.duracion))
            {
                tiempos.Add(reporte.duracion);
            }
        }

        foreach (Reporte reporte in reportesObj)
        {
            if (!paths.Contains(reporte.path))
            {
                paths.Add(reporte.path);
            }
        }

        string[] fecha = fechas.ToArray();
        double[] tiempo = tiempos.ToArray();
        string[] path = paths.ToArray();
        while (contFechas < fecha.Length)
        {
            cadenaFechas = "";
            cadenaTiempos = "";
            maximo += 10;
            while (contFechas < maximo)
            {
                if (contFechas == maximo - 10)
                {
                    cadenaFechas = fecha[contFechas];
                    cadenaTiempos = tiempo[contFechas].ToString();
                    contFechas++;
                }
                else
                {
                    if (contFechas == fecha.Length)
                    {
                        break;
                    }
                    else
                    {
                        cadenaFechas += "\n\n" + fecha[contFechas];
                        Debug.Log(cadenaFechas);
                        cadenaTiempos += "\n\n" + tiempo[contFechas].ToString();
                        contFechas++;
                    }
                }

            }
            //Aqui se guardan en partes de 10 en 10
            divisionFecha[contFechas2] = cadenaFechas;
            divisionTiempo[contFechas2] = cadenaTiempos;
            contFechas2++;
        }
        GameObject.Find("txtFechas").GetComponent<Text>().text = "";
        GameObject.Find("txtDuracion").GetComponent<Text>().text = "";
        GameObject.Find("txtPorcentajeExito").GetComponent<Text>().text = "";
        GameObject.Find("txtFechas").GetComponent<Text>().text = divisionFecha[0];
        GameObject.Find("txtDuracion").GetComponent<Text>().text = divisionTiempo[0];
        GameObject.Find("txtPorcentajeExito").GetComponent<Text>().text = porcentaje.ToString();
        GameObject.Find("txtFin").GetComponent<Text>().text = fechas.Count.ToString();
        for (int x = 0; x < fechas.Count; x++)
        {
            GameObject.Find("btnBoton" + x.ToString()).GetComponent<Button>().enabled = true;
            GameObject.Find("btnBoton" + x.ToString()).transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void cambiarDivision(int indice)
    {
        Debug.Log("Indice"+ this.indice +"tamaño:"+divisionFecha.Length);
        if ((this.indice < contTiempo2-1) && (this.indice + indice) > 0 && indice > 0)
        {
            this.indice += indice;
            GameObject.Find("txtFechas").GetComponent<Text>().text = divisionFecha[this.indice];
            GameObject.Find("txtDuracion").GetComponent<Text>().text = divisionTiempo[this.indice];

        }
        else if ((this.indice + indice < contTiempo2-1) && (this.indice + indice) >= 0 && indice <=0)
        {
            this.indice += indice;
            GameObject.Find("txtFechas").GetComponent<Text>().text = divisionFecha[this.indice];
            GameObject.Find("txtDuracion").GetComponent<Text>().text = divisionTiempo[this.indice];

        } 
    }

    public void seleccionarPalabra(string nombre)
    {
        int valor = GameObject.Find(nombre).GetComponent<Dropdown>().value;
        string valorReal = GameObject.Find(nombre).GetComponent<Dropdown>().options[valor].text;
        EstadoJuego.estadoJuego.cargarLista(valorReal, (GameObject.Find("DropDownListFiguras").GetComponent<Dropdown>()));
    }


    public void cargarImagen(int indiceBoton)
    {
        indiceBoton = indice==0? indiceBoton:(indice ) * 10 + indiceBoton;
        if (indiceBoton<paths.Count) {
         String itemPath = "C:/Users/IoTITSON/Documents/GitHub/BeeSmart/BibiAbeja/Assets/StreamingAssets/Screenshots/" + paths[indiceBoton];
            itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
            Debug.Log(itemPath);
            System.Diagnostics.Process.Start("explorer.exe", "/select/open," + itemPath);
        }
    }
}


