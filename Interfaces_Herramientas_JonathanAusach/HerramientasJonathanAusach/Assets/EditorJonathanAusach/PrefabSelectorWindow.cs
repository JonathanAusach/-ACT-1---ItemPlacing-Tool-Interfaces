using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabSelectorWindow : EditorWindow
{
    private List<GameObject> prefabs = new List<GameObject>();
    private Vector2 scrollPos;
    private string folderPath = "Assets/EditorJonathanAusach/Prefabs"; // Carpeta donde buscará
    private float iconSize = 80f;
    bool[] EstaMarcado;
    private int indexSeleccionado = -1;

    [MenuItem("Herramientas/Selector de Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<PrefabSelectorWindow>("Selector de Prefabs");
    }

    private void OnEnable()
    {
        CargarPrefabs();
    }

    void CargarPrefabs()
    {
        prefabs.Clear();
        // Busca todos los assets de tipo GameObject en la carpeta especificada
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path));
        }
        EstaMarcado = new bool[prefabs.Count];
    }

    void OnGUI()
    {
        GUILayout.Label("Galería de Prefabs", EditorStyles.boldLabel);

        if (GUILayout.Button("Refrescar Carpeta")) CargarPrefabs();

        EditorGUILayout.Space();

        // Control de tamaño de iconos
        iconSize = EditorGUILayout.Slider("Tamaño", iconSize, 40f, 150f);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // Calcular cuántos iconos caben por fila
        int columns = Mathf.Max(1, Mathf.FloorToInt(position.width / (iconSize + 10)));

        

        GUILayout.BeginVertical();
        for (int i = 0; i < prefabs.Count; i += columns)
        {
            GUILayout.BeginHorizontal();
            for (int j = 0; j < columns; j++)
            {
                int index = i + j;
                if (index < prefabs.Count)
                {
                    DrawPrefabButton(prefabs[index], index, indexSeleccionado == index);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    void DrawPrefabButton(GameObject prefab, int index, bool seleccionado)
    {
        Color originalColor = GUI.backgroundColor;
        // Obtener la miniatura del prefab
        Texture2D preview = AssetPreview.GetAssetPreview(prefab);

        GUIContent content = new GUIContent(preview, prefab.name);

        if (seleccionado)
        {
            GUI.backgroundColor = new Color(0f, 0.5f, 1f);
        }

        if (GUILayout.Button(content, GUILayout.Width(iconSize), GUILayout.Height(iconSize)))
        {
            indexSeleccionado = index;
            // Lógica al hacer clic: Seleccionarlo en el proyecto
            Selection.activeObject = prefab;
            EditorGUIUtility.PingObject(prefab);
            EnviarPrefabAlManager(folderPath,prefab);
            //Debug.Log($"Seleccionado: {prefab.name}. ¡Ahora puedes arrastrarlo a la escena!");
        }

        GUI.backgroundColor = originalColor;
    }

    void EnviarPrefabAlManager(string path, GameObject prefab)
    {
        // Buscamos el LevelManager que está en la escena
        LevelManager manager = GameObject.FindObjectOfType<LevelManager>();

        if (manager != null)
        {
            manager.miPrefab = prefab;
            // Seleccionamos el objeto en la jerarquía para que se vea su Custom Editor
            Selection.activeGameObject = manager.gameObject;
            Debug.Log("Prefab enviado al Manager: " + prefab.name);
        }
        else
        {
            Debug.LogError("No hay un LevelManager en la escena.");
        }
    }
}