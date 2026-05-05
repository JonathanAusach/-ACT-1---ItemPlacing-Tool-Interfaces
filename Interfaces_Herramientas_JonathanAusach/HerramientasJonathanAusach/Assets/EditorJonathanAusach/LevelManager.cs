using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Permitir que otros scripts accedan a él con LevelManager.Instance
    public static LevelManager Instance { get; private set; }

    [Header("Configuración de Nivel")]
    public GameObject miPrefab;
    //variable para decicir si es hijo o no
    public Transform puntoDeSpawn;
    public int enemigosNecesarios = 5;

    private void Awake()
    {
        // Asegurarse de que solo haya un LevelManager
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SumarPunto()
    {
        enemigosNecesarios--;
        if (enemigosNecesarios <= 0) FinalizarNivel();
    }

    void FinalizarNivel()
    {
        Debug.Log("¡Nivel Ganado!");
        // Aquí podrías cargar la siguiente escena
    }
}
