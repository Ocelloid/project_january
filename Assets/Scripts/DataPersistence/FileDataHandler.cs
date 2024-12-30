using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDataHandler {
    private string dataDirPath = "";
    private string dataFileName = "";
    public FileDataHandler(string dataDirPath, string dataFileName) {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load() {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = new GameData();
        if (File.Exists(fullPath)) {
            try {
                string dataToLoad = "";
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader streamReader = new StreamReader(fileStream)) {
                        dataToLoad = streamReader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            } catch (Exception e) {
                Debug.LogError(e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data) {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {
                    streamWriter.Write(dataToStore);
                }
            }
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }
}
