using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class videoPlay : MonoBehaviour
{


    public MovieTexture movieclips;

    private MeshRenderer meshRenderer;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = movieclips;

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        movieclips.Play();

        Invoke("cargarJuego", movieclips.audioClip.length);
    }

    private void cargarJuego()
    {

        SceneManager.LoadScene("seleccionarUsuario");

    }

}