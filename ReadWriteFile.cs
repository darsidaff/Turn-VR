using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public static class ReadWriteFile
{
    public static int nPlayers = 50;

    [SerializableAttribute]
    public struct DataRecord
    {
        public string playerName;
        public int score;
    }

    private static string Path = Application.dataPath + "/StreamingAssets/Record.txt";  // Путь к файлу со списком рекордов

    public static List<GameObject> playersList = new List<GameObject>();                                         // Список из имен игроков (используется в скрипте PlayersList)

    public static DataRecord[] listPlayers = new DataRecord[nPlayers];                        // 10 игроков, входящих в список рекордов
    public static DataRecord PlayerInfo;                                                // Данные о текущем игроке, который играет в игру

    // Словарь для хранения ссылок на объекты "Клавиатура" и "Список достижений" (для их показа и скрытия)
    public static Dictionary<string, GameObject> refs = new Dictionary<string, GameObject>();


    // Добавление элемента в словарь (temp - ключ, obj - добавляемый объект)
    public static void AddRef(string temp, GameObject obj)
    {
        if (!refs.ContainsKey(temp)) refs.Add(temp, obj);  // Чтобы не добавить элементы с одинаковым ключом
    }


    // Размещение игрока в списке рекордов игроков в соответствии с набранными очками
    public static void InsertPlayerFromList()
    {
        int i = 0;
        while (i < 50)
        {
            if (PlayerInfo.score > listPlayers[i].score)
            {
                DataRecord temp1, temp2;
                temp1 = listPlayers[i];
                listPlayers[i] = PlayerInfo;
                i++;

                for (; i < 50; i++)
                {
                    temp2 = listPlayers[i];
                    listPlayers[i] = temp1;
                    temp1 = temp2;
                }
            }
            i++;
        }
        SaveToFile();  // Сохраняем обновленный список
    }


    // Вычисляем количество пробелов между именем игрока и набранными очками, чтобы таблица была ровная (имя максимум 15 символов и 7 пробелов до набранных очков)
    public static string LengthAlignment(string str)
    {
        string temp = "";
        int n = 22 - str.Length;
        //Debug.Log(n);
        for (int i = 1; i <= n; i++) temp += " ";
        return temp;
    }


    // Сохранение списка рекордов в файл
    public static void SaveToFile()
    {
        FileStream fout = File.OpenWrite(Path);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fout, listPlayers);
        fout.Close();
    }


    // Загрузка списка рекордов из файла
    public static void LoadFromFile()
    {
        if (File.Exists(Path))
        {
            FileStream fin = File.OpenRead(Path);
            BinaryFormatter bf = new BinaryFormatter();
            listPlayers = (DataRecord[])bf.Deserialize(fin);
            fin.Close();
        }
        else  // Если файл не найден, создаем и заполняем его пустыми значениями
        {
            for (int i = 0; i < 50; i++)
            {
                listPlayers[i].playerName = "---------------";
                listPlayers[i].score = 0;
            }
            SaveToFile();
        }
    }
}

