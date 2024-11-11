using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class HistoriqueManager : MonoBehaviour
{
    public int idPatient;
    public Text date;
    public Text prenomnom;

    public Text erreur;

    
    [SerializeField]
    private GameObject buttonTemplate;
    private Transform buttonparent;
    public ButtonHistoButton buttonScript;


    private string dbname;
    private string dberreurname;
    public string nomdujeu;
    private List<GameObject> currentButtons = new List<GameObject>();

    public GameObject fenetreSport;
    public GameObject fenetreSac;
    public Button buttonSport;
    public Button buttonSac;
    public Sprite spritegris; 
    public Sprite spriteblanc;
    

    private string currentwindows;

    // Start is called before the first frame update
    void Start()
    {
        currentwindows = "Quel sport";
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        if (MainManager.Instance != null)
        {
            idPatient = MainManager.Instance.currentidPatient;
            Debug.Log("idPatient : " + idPatient);
            date.text = "";
            prenomnom.text = "";


            using (var connection = new SqliteConnection(dbname))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT prenom, nom, date FROM tablePatient WHERE id = @id;";
                    Debug.Log("idPatient : " + idPatient);
                    command.Parameters.AddWithValue("@id", idPatient);

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prenomnom.text += reader["prenom"] + " " + reader["nom"];
                            date.text += reader["date"];

                        }
                    }
                }
                connection.Close();

            }
        }

        bouttonQuelSport();
        historiquesport("Quel Sport");
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Retour()
    {
        SceneManager.LoadScene("Patient");
    }

    public void bouttonQuelSport()
    {
        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreur.s3db";
        Debug.Log("boutton quel sport");
        nomdujeu = "Quel Sport";
        if (currentwindows == "Mon sac")
        {
            fenetreSport.SetActive(true);
            fenetreSac.SetActive(false);
            buttonSac.image.sprite = spritegris;
            buttonSport.image.sprite = spriteblanc;
            currentwindows = "Quel sport";
            erreur.text = "";
            historiquesport(nomdujeu);
            
           
        }
    }

    public void bouttonMonSac()
    {
        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreur.s3db";
        Debug.Log("boutton mon sac");
        nomdujeu = "Mon Sac";
        if (currentwindows == "Quel sport")
        {
            fenetreSport.SetActive(false);
            fenetreSac.SetActive(true);
            buttonSac.image.sprite = spriteblanc;
            buttonSport.image.sprite = spritegris;
            currentwindows = "Mon sac";
            erreur.text = "";
            historiquesport(nomdujeu);
            
        }
    }

    /*
    public void historiquesport(string nomdujeu)
    {
        titreerreur.text = "Erreurs sur le jeu : " + nomdujeu;
        erreur.text = "";

        if (MainManager.Instance != null)
        {
            idPatient = MainManager.Instance.currentidPatient;
            Debug.Log("idPatient : " + idPatient);

            try
            {
                using (var connection = new SqliteConnection(dbname))
                {
                    connection.Open();

                    List<string> dates = new List<string>();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT DISTINCT date FROM tableErreur WHERE id = @id AND jeu = @jeu ORDER BY SUBSTR(date, 7, 4) || '-' || SUBSTR(date, 4, 2) || '-' || SUBSTR(date, 1, 2) DESC;";
                        command.Parameters.AddWithValue("@id", idPatient);
                        command.Parameters.AddWithValue("@jeu", nomdujeu);

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Debug.Log(reader["date"]);
                                dates.Add(reader["date"].ToString());
                                
                                
                            }
                        }
                    }

                    foreach (string currentdate in dates)
                    {
                        erreur.text += currentdate + "\n";

                        using (var command2 = connection.CreateCommand())
                        {
                            command2.CommandText = "SELECT erreur FROM tableErreur WHERE id = @id AND jeu = @jeu AND date = @date;";
                            command2.Parameters.AddWithValue("@id", idPatient);
                            command2.Parameters.AddWithValue("@jeu", nomdujeu);
                            command2.Parameters.AddWithValue("@date", currentdate);

                            using (IDataReader reader2 = command2.ExecuteReader())
                            {
                                while (reader2.Read())
                                {
                                    erreur.text += " - " + reader2["erreur"].ToString() + "\n";
                                }
                            }
                        }
                    }

                    Debug.Log("erreur affichée");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Erreur lors de l'accès à la base de données : " + ex.Message);
            }
        }
    }
    */

    public void historiquesport(string nomdujeu)
    {
        RemoveAllButton();
        Debug.Log("affiche historique :" + nomdujeu);
        idPatient = MainManager.Instance.currentidPatient;
        dberreurname = "URI=file:" + Application.persistentDataPath + "/Erreursession.s3db";

        using (var connection = new SqliteConnection(dberreurname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT date, idsession FROM tableErreursession WHERE idpatient = @idpatient AND jeu = @jeu ORDER BY SUBSTR(date, 7, 4) || '-' || SUBSTR(date, 4, 2) || '-' || SUBSTR(date, 1, 2) || ' ' || SUBSTR(date, 12, 2) || ':' || SUBSTR(date, 15, 2) DESC; ";
                command.Parameters.AddWithValue("@idpatient", idPatient);
                command.Parameters.AddWithValue("@jeu", nomdujeu);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        GameObject button = Instantiate(buttonTemplate, buttonparent) as GameObject;

                        button.SetActive(true);

                        buttonScript = button.GetComponent<ButtonHistoButton>();

                        if (buttonScript != null)
                        {
                            // Définissez le texte du bouton avec les données du lecteur
                            buttonScript.SetText(reader["date"].ToString());
                            // Associez l'identifiant du patient au bouton
                            buttonScript.sessionId = Convert.ToInt32(reader["idsession"]);
                            
                        }
                        button.transform.SetParent(buttonTemplate.transform.parent, false);
                        currentButtons.Add(button);
                    }
                }
            }
            connection.Close();
        }
    }


    public void RemoveAllButton()
    {
        foreach (GameObject button in currentButtons)
        {
            Destroy(button);
        }
        currentButtons.Clear();
    }
}
