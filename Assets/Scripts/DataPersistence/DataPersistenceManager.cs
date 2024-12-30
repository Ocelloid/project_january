using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour {
    [SerializeField] private string fileName;
    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler fileDataHandler;
    private void Awake() {
        if (Instance != null) {
            Debug.LogWarning("DataPersistenceManager already exists");
            return;
        }
        Instance = this;
        gameData = new GameData();
    }
    private void Start() {
        //https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }
    public void NewGame() {
        gameData = new GameData();
    }
    public void LoadGame() {
        this.gameData = fileDataHandler.Load();
        if (this.gameData == null) {
            NewGame();
        }
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) {
            dataPersistenceObject.LoadData(gameData);
        }
    }
    public void SaveGame() {
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects) {
            dataPersistenceObject.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }
    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistanceObjects() {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
