using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class RechercheJoueur : MonoBehaviour
{
    public InputField champRecherche;
    public GameObject boutonTemplate; // Utilisé pour instancier les boutons de résultats
    public Transform parentBoutons; // Parent des boutons de résultats
    private string connectionString = "URI=file:Patient.db"; // Chemin de votre base de données SQLite

    void Start()
    {
        RechercherJoueurs("");
    }

    public void RechercherJoueurs(string recherche)
    {
        // Effacer les résultats précédents
        EffacerResultats();

        // Écrire votre requête SQL pour rechercher les joueurs correspondants à la saisie
        string requeteSQL = "SELECT nom, prenom, dateNaissance FROM joueurs WHERE nom LIKE '%" + recherche + "%' OR prenom LIKE '%" + recherche + "%'";

        // Connexion à la base de données SQLite et exécution de la requête
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = requeteSQL;

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nom = reader.GetString(0);
                        string prenom = reader.GetString(1);
                        string dateNaissance = reader.GetString(2);

                        // Instancier un bouton de résultat
                        GameObject bouton = Instantiate(boutonTemplate) as GameObject;
                        bouton.SetActive(true);
                        bouton.GetComponent<ButtonListButton>().SetText(nom + " " + prenom + " - " + dateNaissance);

                        // Définir le parent du bouton de résultat
                        bouton.transform.SetParent(parentBoutons, false);
                    }
                }
            }

            connection.Close();
        }
    }
    public void OnSearchFieldUpdated(string searchTerm)
    {
        RechercherJoueurs(searchTerm);
    }

    // Fonction pour effacer les résultats précédents
    private void EffacerResultats()
    {
        foreach (Transform child in parentBoutons)
        {
            Destroy(child.gameObject);
        }
    }
}