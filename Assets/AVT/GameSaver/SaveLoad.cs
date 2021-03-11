using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace AVT
{
	public class SaveLoad : Singleton<SaveLoad>
	{
		public void Awake()
		{
			if (PlayerPrefs.HasKey("old_player")) {
				Saver.Load();
			} else {
				Saver.Init();
				Saver.Save();
				PlayerPrefs.SetInt("old_player", 1);
			}
			Saver.saveFile.InitPermanentDataSetting();
		}

		public void OnApplicationQuit()
		{
			Saver.Save();
		}
	}

	public static class Saver
	{
		public static GameSaveFile saveFile;
		public static string fileName = "gamesave.gd";

		public static void Init()
		{
			saveFile = new GameSaveFile();
		}

		//it's static so we can call it from anywhere
		public static void Save()
		{
			BinaryFormatter bf = new BinaryFormatter();
			//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
			FileStream file = File.Create(Application.persistentDataPath + "/" + fileName); //you can call it anything you want
			bf.Serialize(file, saveFile);
			file.Close();
		}

		public static void Load()
		{
			if (File.Exists(Application.persistentDataPath + "/" + fileName)) {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/" + fileName, FileMode.Open);
				saveFile = (GameSaveFile)bf.Deserialize(file);
				file.Close();
			}
		}
	}
}