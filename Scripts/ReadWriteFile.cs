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

    private static string Path = Application.dataPath + "/StreamingAssets/Record.txt";  // Ïóòü ê ôàéëó ñî ñïèñêîì ðåêîðäîâ

    public static List<GameObject> playersList = new List<GameObject>();                                         // Ñïèñîê èç èìåí èãðîêîâ (èñïîëüçóåòñÿ â ñêðèïòå PlayersList)

    public static DataRecord[] listPlayers = new DataRecord[nPlayers];                        // 10 èãðîêîâ, âõîäÿùèõ â ñïèñîê ðåêîðäîâ
    public static DataRecord PlayerInfo;                                                // Äàííûå î òåêóùåì èãðîêå, êîòîðûé èãðàåò â èãðó

    // Ñëîâàðü äëÿ õðàíåíèÿ ññûëîê íà îáúåêòû "Êëàâèàòóðà" è "Ñïèñîê äîñòèæåíèé" (äëÿ èõ ïîêàçà è ñêðûòèÿ)
    public static Dictionary<string, GameObject> refs = new Dictionary<string, GameObject>();


    // Äîáàâëåíèå ýëåìåíòà â ñëîâàðü (temp - êëþ÷, obj - äîáàâëÿåìûé îáúåêò)
    public static void AddRef(string temp, GameObject obj)
    {
        if (!refs.ContainsKey(temp)) refs.Add(temp, obj);  // ×òîáû íå äîáàâèòü ýëåìåíòû ñ îäèíàêîâûì êëþ÷îì
    }


    // Ðàçìåùåíèå èãðîêà â ñïèñêå ðåêîðäîâ èãðîêîâ â ñîîòâåòñòâèè ñ íàáðàííûìè î÷êàìè
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
        SaveToFile();  // Ñîõðàíÿåì îáíîâëåííûé ñïèñîê
    }


    // Âû÷èñëÿåì êîëè÷åñòâî ïðîáåëîâ ìåæäó èìåíåì èãðîêà è íàáðàííûìè î÷êàìè, ÷òîáû òàáëèöà áûëà ðîâíàÿ (èìÿ ìàêñèìóì 15 ñèìâîëîâ è 7 ïðîáåëîâ äî íàáðàííûõ î÷êîâ)
    public static string LengthAlignment(string str)
    {
        string temp = "";
        int n = 22 - str.Length;
        //Debug.Log(n);
        for (int i = 1; i <= n; i++) temp += " ";
        return temp;
    }


    // Ñîõðàíåíèå ñïèñêà ðåêîðäîâ â ôàéë
    public static void SaveToFile()
    {
        FileStream fout = File.OpenWrite(Path);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fout, listPlayers);
        fout.Close();
    }


    // Çàãðóçêà ñïèñêà ðåêîðäîâ èç ôàéëà
    public static void LoadFromFile()
    {
        if (File.Exists(Path))
        {
            FileStream fin = File.OpenRead(Path);
            BinaryFormatter bf = new BinaryFormatter();
            listPlayers = (DataRecord[])bf.Deserialize(fin);
            fin.Close();
        }
        else  // Åñëè ôàéë íå íàéäåí, ñîçäàåì è çàïîëíÿåì åãî ïóñòûìè çíà÷åíèÿìè
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

