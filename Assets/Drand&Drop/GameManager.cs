using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Xml.Linq;
using UnityEngine.EventSystems;
using Mono.Data.Sqlite;
using System.Data;



public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Instance unique de GameManager

    public GameObject[] hearts; // Références aux objets des cœurs

    // Référence au SpriteManager pour obtenir les sprites de cœur
    public SpriteManager spriteManager;
    public SessionManager sessionManager;
    public GameManager gameManagercoeurcentre;
    
    public GameObject troiscoeur;

    public int heartsLeft; // Nombre de cœurs restants
    public GameObject boutonFin;
    private GameObject lastDroppedObject; // Dernier objet déposé

    public int idPatient;
  

    public GameObject CanvaFinDuJeu;
    public int nombreobjet;

    public GameObject popupstart;

    public Text avancee;
    public Text idjoueur;



    // Liste des objets bloqués
    public static List<GameObject> objetsBloques = new List<GameObject>();


    public void AddObjet(GameObject gameobject)
    {
        
        objetsBloques.Add(gameobject);
        
    }

    public bool Objetdansliste(GameObject gameobject)
    {
        
        if (objetsBloques.Contains(gameobject))
        {
            
            return true;
        }
        else
        {
            
            return false;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Debug.Log("Start");
        if (MainManager.Instance.currentidPatient < 0)
        {
            troiscoeur.SetActive(false);
        }
            //nombreobjet = 0;
            //this.avancee.text = "0/9";// Assurez-vous que nombreobjet est initialisé
            //MettreAJourTexteAvancee();
            afficherID();
        heartsLeft = 3;
        Debug.Log("start coeur" +heartsLeft);
        UpdateHearts();
        CanvaFinDuJeu.SetActive(false);
    }

   
    // Met à jour l'état des cœurs en fonction du nombre restant
    public void UpdateHearts()
    {
        Debug.Log("UpdateHearts");
        List<Sprite> heartSprites = spriteManager.HeartSprites; // Utilise la liste de sprites du SpriteManager

        for (int i = 0; i < hearts.Length; i++)
        {
            Image heartImage = hearts[i].GetComponent<Image>();
            if (i < heartsLeft)
                heartImage.sprite = heartSprites[0]; // Utilise le sprite plein
            else
                heartImage.sprite = heartSprites[1]; // Utilise le sprite vide
        }
        Debug.Log("coeur restant :" + heartsLeft);
    }

    // Perd un cœur
    public void LoseHeart(GameObject droppedObject)
    {
        if (MainManager.Instance.currentidPatient > -1)
        {
            Debug.Log("LoseHeart");
            Debug.Log("l'objet déposé est : " + droppedObject.name);
            Debug.Log("nombre de coeur avant: " + heartsLeft);
            heartsLeft--; // Diminue le nombre de cœurs restants
            sessionManager.AddErreur(droppedObject);
            // Assurez-vous que heartsLeft ne soit jamais inférieur à zéro
            if (heartsLeft < 0)
            {
                heartsLeft = 0;
            }
            Debug.Log("nombre de coeur après : " + heartsLeft);
            UpdateHearts();

            // Mettre à jour l'objet déposé
            lastDroppedObject = droppedObject;

            if (heartsLeft == 0)
            {
                if (boutonFin != null)
                {
                    boutonFin.SetActive(true);
                }
            }
        }
    }

    // Réinitialise les cœurs et désactive le dernier objet déposé
    public void ResetHearts()
    {
        heartsLeft = 3;
        UpdateHearts();

        if (lastDroppedObject != null)
        {
            lastDroppedObject.SetActive(false);
        }
    }
    


    public void Success()
    {
        Debug.Log("le jeu est fini avec succès");
        if (idPatient > -1)
        {
            sessionManager.StopTimer();
            sessionManager.EndSession(0);
        }
        Debug.Log("succes" + MainManager.Instance.currentidPatient);
        if (MainManager.Instance.currentidPatient < -1)
        {
            MainManager.Instance.currentidPatient = - MainManager.Instance.currentidPatient - 2;
        }

        CanvaFinDuJeu.SetActive(true);
    }

    public void Revoir()
    {
        Debug.Log("revoir");
        Debug.Log("revoir" + MainManager.Instance.currentidPatient);
        if (MainManager.Instance.currentidPatient != -1)
        {
            MainManager.Instance.currentidPatient = -2 - MainManager.Instance.currentidPatient;
        }
        SceneManager.LoadScene("Quel Sport");

    }

    public void Menu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void Jeux()
    {
        Debug.Log("Jeux");
        SceneManager.LoadScene("QuelTheme");
    }

    public void Exit()
    {
        Debug.Log("Exit");
        if (MainManager.Instance.currentidPatient < -1)
        {
            MainManager.Instance.currentidPatient = -MainManager.Instance.currentidPatient - 2;
        }
        Debug.Log(sessionManager.started);
        if (idPatient > -1 && sessionManager.started)
        {
            sessionManager.StopTimer();
            sessionManager.EndSession(1);
        }
        SceneManager.LoadScene("JeuThemeSport");

    }
    /*
    public void AugmenterNombre()
    {
        nombreobjet = nombreobjet + 1;
        MettreAJourTexteAvancee();
    }

    public void MettreAJourTexteAvancee()
    {
        if (avancee.text != null)
        {
            this.avancee.text = this.nombreobjet.ToString() + "/9";
            Debug.Log("Texte avancée mis à jour : " + this.avancee.text);
        }
        else
        {
            Debug.Log("avancee null");
        }
    }*/

    public void Restart()
    {
        if (idPatient > -1)
        {
            sessionManager.StopTimer();
            sessionManager.EndSession(2);
        }
        SceneManager.LoadScene("Quel Sport");
    }
    public void finjeux()
    {
        if (idPatient > -1)
        {
            sessionManager.StopTimer();
            sessionManager.EndSession(2);
        }
        SceneManager.LoadScene("QuelTheme");
    }
    public void CestParti()
    {
        popupstart.SetActive(false);
        idPatient = MainManager.Instance.currentidPatient;
        if (idPatient > -1)
        {
            sessionManager.StartTimer();
            sessionManager.Beginsession();
            gameManagercoeurcentre.heartsLeft = 3;
        }
    }

    public void afficherID()
    {
        string dbname;
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        if (MainManager.Instance != null)
        {
            
            int currentid = MainManager.Instance.currentidPatient;

            if (currentid == -1)
            {
                idjoueur.text = "Mode Libre";
            }
            else
            {
                if (currentid < -1)
                {
                    currentid = -currentid - 2;
                }

                idjoueur.text = "";
                using (var connection = new SqliteConnection(dbname))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT prenom, nom, date FROM tablePatient WHERE id = @id;";
                        command.Parameters.AddWithValue("@id", currentid);

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                idjoueur.text += reader["prenom"] + " " + reader["nom"] + "\n";
                                idjoueur.text += reader["date"];
                            }
                        }
                    }
                    connection.Close();

                }
            }
        }
        else
        {
            Debug.Log("MainManager.Instance est null");
        }
    }
}
