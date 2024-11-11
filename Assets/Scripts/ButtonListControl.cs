using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System.IO;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ButtonListScrollbar : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;
    private Transform buttonparent;
    public InputField recherche;
    public ButtonListButton buttonScript;
    public Transform parentBoutons; // Parent des boutons de résultats
    private string path;
    private string connectionString;
    private string dbname;

    private List<GameObject> currentButtons = new List<GameObject>();
    public Text recent;

    void Awake()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
    }
    void Start()
    {
        dbname = "URI=file:" + Application.persistentDataPath + "/Patient.s3db";
        RemoveAllButton();
        GenButton();
    }

    public void GenButton()
    {
        Debug.Log("genbutton");
        int compteur = 0;
        recent.text = "Plus récents :";
        using (var connection = new SqliteConnection(dbname))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT nom, prenom, date, id FROM tablePatient ORDER BY id DESC ;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read() && compteur <3)
                    {

                        GameObject button = Instantiate(buttonTemplate, buttonparent) as GameObject;

                        button.SetActive(true);

                        buttonScript = button.GetComponent<ButtonListButton>();

                        if (buttonScript != null)
                        {
                            // Définissez le texte du bouton avec les données du lecteur
                            buttonScript.SetText(reader["nom"] + "\t\t" + reader["prenom"] + "\t\t" + reader["date"]);
                            // Associez l'identifiant du patient au bouton
                            buttonScript.patientId = (int)reader["id"];
                        }
                        button.transform.SetParent(buttonTemplate.transform.parent, false);
                       
                        currentButtons.Add(button);
                        compteur++;

                    }
                }
            }
            connection.Close();

        }
    }

    public void GenResearch()
    {
        string rechercheValue = recherche.text;
        recent.text = "Résultat :";
        if (rechercheValue == "")
        {
            RemoveAllButton();
            GenButton();
        }
        else
        {
            RemoveAllButton();
            using (var connection = new SqliteConnection(dbname))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    // Utiliser LOWER() pour rendre la recherche insensible à la casse
                    command.CommandText = "SELECT nom, prenom, date, prenomnom, id FROM tablePatient " +
                                          "WHERE LOWER(nom) LIKE '%' || LOWER(@rechercheValue) || '%' " +
                                          "OR LOWER(prenom) LIKE '%' || LOWER(@rechercheValue) || '%' " +
                                          "OR LOWER(date) LIKE '%' || LOWER(@rechercheValue) || '%' " +
                                          "OR LOWER(prenomnom) LIKE '%' || LOWER(@rechercheValue) || '%'"+"OR LOWER(nomprenom) LIKE '%' || LOWER(@rechercheValue) || '%';";
                    command.Parameters.AddWithValue("@rechercheValue", rechercheValue.ToLower());

                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            GameObject button = Instantiate(buttonTemplate, buttonparent) as GameObject;

                            button.SetActive(true);

                            button.GetComponent<ButtonListButton>().SetText(reader["nom"] + "\t\t" + reader["prenom"] + "\t\t" + reader["date"]);

                            button.GetComponent<ButtonListButton>().patientId = (int)reader["id"];

                            button.transform.SetParent(buttonTemplate.transform.parent, false);

                            currentButtons.Add(button);
                        }
                    }
                }
                connection.Close();

            }
            Debug.Log("genresarch done");
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
