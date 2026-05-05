using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JonathanAusach : MonoBehaviour
{
    int id;
    int life;
    int dany;
    List<JonathanAusach> CountsObjects = new List<JonathanAusach>(); 

    //Getters
    public int getId() { return id; }
    public int getLife() { return life; }
    public int getDany() {  return dany;}

    public int jonathanAusaches() {  return CountsObjects.Count; } 
    

    //Setters
    public void setId(int id) { this.id = id; }
    public void setLife(int life) {  this.life = life; }
    public void setDany(int dany) {  this.dany = dany; }


    //Lista eleimniar componente o añadir componnete
    void newElementLista(JonathanAusach MiNuevoJonathan) { CountsObjects.Add(MiNuevoJonathan); }
    void RemoveComponent(JonathanAusach MiElement)
    {
        for (int i = 0; i < CountsObjects.Count; i++)
        {
            if (CountsObjects[i] == MiElement)
            {
                CountsObjects.RemoveAt(i);
            }
        }
    }

    private void OnEnable()
    {
        newElementLista(this);
        for (int i = 0; i < CountsObjects.Count;i++)
        {
            setId(i);
        }
    }

    private void OnDisable()
    {
        RemoveComponent(this);
    }

}