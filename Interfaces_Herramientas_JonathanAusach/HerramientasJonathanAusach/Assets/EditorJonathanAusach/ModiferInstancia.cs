using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class ModiferInstancia : Editor
{

    private float currentScale = 1f;
    private GameObject objetoEnEdicion;

    void OnSceneGUI()
    {
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 pos = hit.point;

            // --- FASE 1: PRESIONAR (INSTANCIAR) ---
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                // Evitamos que Unity pierda el foco
                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

                // Creamos el objeto pero lo guardamos en 'objetoEnEdicion'
                objetoEnEdicion = PlaceObject(pos);

                e.Use();
            }

            // --- FASE 2: ARRASTRAR (ROTAR HACIA EL RATÓN) ---
            if (e.type == EventType.MouseDrag && e.button == 0 && objetoEnEdicion != null)
            {
                // Calculamos la dirección desde el objeto hacia donde está el ratón ahora
                Vector3 direccion = hit.point - objetoEnEdicion.transform.position;

                if (direccion.sqrMagnitude > 0.01f) // Evitar error si el ratón no se ha movido
                {
                    // Hacemos que el objeto mire hacia esa dirección (solo en el eje Y)
                    Quaternion nuevaRotacion = Quaternion.LookRotation(direccion);
                    objetoEnEdicion.transform.rotation = Quaternion.Euler(0, nuevaRotacion.eulerAngles.y, 0);
                }

                e.Use();
            }

            // --- FASE 3: SOLTAR (TERMINAR) ---
            if (e.type == EventType.MouseUp && e.button == 0)
            {
                // Liberamos el objeto y el control
                objetoEnEdicion = null;
                GUIUtility.hotControl = 0;
                e.Use();
            }

            // Si no estamos editando nada, mostrar el cubo verde de preview
            if (objetoEnEdicion == null)
            {
                DrawPreview(pos);
            }
        }

        SceneView.RepaintAll();
    }

    void DrawPreview(Vector3 position)
    {
        // Dibuja una caja o una malla transparente como preview
        Handles.color = new Color(0, 1, 0, 0.3f);
        Handles.DrawWireCube(position, Vector3.one * currentScale);
    }

    GameObject PlaceObject(Vector3 position)
    {
        LevelManager lm = (LevelManager)target;
        if (lm.miPrefab == null) return null;

        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(lm.miPrefab);

        // Configuración inicial
        newObj.transform.position = position;
        newObj.transform.localScale = Vector3.one * currentScale;

        if (lm.puntoDeSpawn != null)
            newObj.transform.SetParent(lm.puntoDeSpawn);

        Undo.RegisterCreatedObjectUndo(newObj, "Instanciar Objeto");

        return newObj; // Devolvemos la referencia
    }
}
