using System.Collections.Generic;
using System.IO;
using UnityEngine;

    public static class CSVLoader
    {
        private static string cashedLine;
        
        public static List<Dictionary<string, string>> LoadCSV(string filePath)
        {
            var data = new List<Dictionary<string, string>>();

            using StreamReader reader = new StreamReader(filePath);
            cashedLine = "";
            var headers = reader.ReadLine()?.Split(',');
            bool reachedEnd = false;

            while (!reachedEnd)
            {
                if (reader.EndOfStream) reachedEnd = true;
                if (headers != null)
                {
                    var values = Split(reader, headers.Length);
                    var entry = new Dictionary<string, string>();
                
                    if (values != null)
                    {
                        for (int i = 0; i < headers.Length && i < values.Length; i++)
                        {
                            entry[headers[i]] = values[i];
                        } 
                    }
                    data.Add(entry);
                }
            }

            return data;
        }

        //Note: this will still cause errors if the last collumn contains a line jump (\n)
        private static string[] Split(StreamReader reader, int collumnCount)
        {
            int currentID = 0;
            string[] splitData = new string[collumnCount];
            string raw = string.IsNullOrEmpty(cashedLine)? reader.ReadLine() : cashedLine;

            bool foundQuote = false;
            string data = "";

            while (currentID < collumnCount)
            {
                if(raw == null) break;
                
                for (int i = 0; i < raw.Length; i++)
                {
                    if (raw[i] == ',' && !foundQuote)
                    {
                        splitData[currentID] = data;
                        Debug.Log($"[{currentID}] {data}");
                        currentID++;
                        data = "";
                    }
                    else if (raw[i] == '\"')
                    {
                        if (IsQuote(raw, i, foundQuote))
                        {
                            data += raw[i];
                            i++;
                        }
                        else foundQuote = !foundQuote;
                    }
                    else
                    {
                        data += raw[i];
                    }
                }

                if (currentID < collumnCount - 1)
                {
                    data += "\n";
                    raw = reader.ReadLine();
                }
                else
                {
                    CheckNextLine(reader, ref data, foundQuote);
                    splitData[currentID] = data;
                    currentID++;
                    data = "";
                }
            }

            return splitData;
        }

        private static bool IsQuote(string raw, int id,bool foundQuote) => foundQuote && id + 1 < raw.Length && raw[id + 1] == '\"';

        private static void CheckNextLine(StreamReader reader, ref string currentData,bool insideQuote)
        {
            if (reader.EndOfStream) return;
            bool foundQuote = insideQuote;
            string raw = reader.ReadLine();
            string potentialData = "";
            bool isNewLine = false;

            while (!isNewLine)
            {
                potentialData += "\n";
                for (int i = 0; i < raw.Length; i++)
                {
                    if (raw[i] == ',' && !foundQuote)
                    {
                        isNewLine = true;
                        break;
                    }
                    if (raw[i] == '\"')
                    {
                        if (IsQuote(raw, i, foundQuote))
                        {
                            potentialData += raw[i];
                            i++;
                        }
                        else foundQuote = !foundQuote;
                    }
                    else
                    {
                        potentialData += raw[i];
                    }
                }

                if (!isNewLine)
                {
                    currentData += potentialData;
                    potentialData = "";
                    raw = reader.ReadLine(); 
                }
                else cashedLine = raw;
            }
        }
    }