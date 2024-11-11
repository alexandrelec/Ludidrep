using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;

public class GameManagerMS : MonoBehaviour
{
    public static GameManagerMS instance; // Instance unique de GameManager
    
    public GameObject[] hearts; // Références aux objets des cœurs

    // Référence au SpriteManager pour obtenir les sprites de cœur
    public SpriteManagerMS spriteManager;

    private int heartsLeft; // Nombre de cœurs restants
    public GameObject boutonFin;
    private GameObject lastDroppedObject; // Dernier objet déposé
    public bool popupactif;

    // Liste des objets bloqués
    public static List<GameObject> objetsBloques = new List<GameObject>();

    public Text idjoueur;

    public GameObject popupstart;
    public GameManagerMS gameManagercoeurcentre;

    public GameObject canvafindujeu;
    public GameObject troiscoeur;


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
        if (MainManager.Instance.currentidPatient < 0)
        {
            troiscoeur.SetActive(false);
        }
        popupactif = false;
        heartsLeft = 3;
        afficherID();
        Debug.Log("start hearts : " +  heartsLeft);
        UpdateHearts();
        popupactif |= true;
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
    }

    // Perd un cœur
    public void LoseHeart(GameObject droppedObject)
    {
        if (MainManager.Instance.currentidPatient > -1)
        {
            Debug.Log("hearts left before lose : " + heartsLeft);
            Debug.Log("LoseHeart");
            heartsLeft--; // Diminue le nombre de cœurs restants
            Debug.Log("hearstleft : " + heartsLeft);
            // Assurez-vous que heartsLeft ne soit jamais inférieur à zéro
            if (heartsLeft < 0)
            {
                heartsLeft = 0;
            }
            UpdateHearts();

            // Mettre à jour l'objet déposé
            lastDroppedObject = droppedObject;

            if (heartsLeft == 0)
            {
                if (lastDroppedObject != null)
                {
                    // Générez le nom du pop-up en concaténant "popUp" avec le nom de l'objet déposé
                    string popUpName = "popUp" + lastDroppedObject.name;

                    // Recherchez le pop-up correspondant par son nom
                    Transform popUpTransform = lastDroppedObject.transform.Find(popUpName);
                    if (popUpTransform != null)
                    {
                        popUpTransform.gameObject.SetActive(false);
                    }
                    else
                    {
                        Debug.LogWarning("Pop-up not found for the last dropped object: " + popUpName);
                    }
                }

                boutonFin.SetActive(true);
            }
        }
    }

    // Réinitialise les cœurs
    public void ResetHearts()
    {
        heartsLeft = hearts.Length;
        UpdateHearts();
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

    public void CestParti()
    {
        popupstart.SetActive(false);
        popupactif = false;
        int idPatient = MainManager.Instance.currentidPatient;
        Debug.Log("C'est parti");
        if (idPatient > -1 )
        {
            SessionManagerMS.InstanceMS.StartTimer();
            SessionManagerMS.InstanceMS.Beginsession();
            gameManagercoeurcentre.heartsLeft = 3;
            Debug.Log("c'est parti session");
        }
    }

    public void RestartChaud()
    {
        if (MainManager.Instance.currentidPatient > -1)
        {
            SessionManagerMS.InstanceMS.StopTimer();
            SessionManagerMS.InstanceMS.EndSession(2);
        }
        SessionManagerMS.InstanceMS.DestroyInstance();

        SceneManager.LoadScene("Niveau Chaud");
    }

    public void finjeux()
    {
        if (MainManager.Instance.currentidPatient > -1)
        {
            SessionManagerMS.InstanceMS.StopTimer();
            SessionManagerMS.InstanceMS.EndSession(2);
        }
        SessionManagerMS.InstanceMS.DestroyInstance();

        SceneManager.LoadScene("QuelTheme");
    }

    public void RestartFroid()
    {
        SceneManager.LoadScene("Niveau Froid");
    }


    public void Revoir()
    {
        Debug.Log("revoir");
        Debug.Log("revoir" + MainManager.Instance.currentidPatient);
        if (MainManager.Instance.currentidPatient != -1)
        {
            MainManager.Instance.currentidPatient = -2 - MainManager.Instance.currentidPatient;
        }
        SceneManager.LoadScene("Niveau Chaud");

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
        if (MainManager.Instance.currentidPatient > -1 && SessionManagerMS.InstanceMS.started)
        {
            SessionManagerMS.InstanceMS.StopTimer();
            SessionManagerMS.InstanceMS.EndSession(1);
        }
        SessionManagerMS.InstanceMS.DestroyInstance();

        SceneManager.LoadScene("JeuThemeSport");

    }

    public void Success()
    {
        Debug.Log("le jeu est fini avec succès");
        if (MainManager.Instance.currentidPatient > -1)
        {
            SessionManagerMS.InstanceMS.StopTimer();
            SessionManagerMS.InstanceMS.EndSession(0);
        }
        Debug.Log("succes" + MainManager.Instance.currentidPatient);
        if (MainManager.Instance.currentidPatient < -1)
        {
            MainManager.Instance.currentidPatient = -MainManager.Instance.currentidPatient - 2;
        }
        SessionManagerMS.InstanceMS.DestroyInstance();

        canvafindujeu.SetActive(true);
    }
}
