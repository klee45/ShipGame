using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Reader
{
    public static List<StringPair> ReadPairedFile(TextAsset textAsset, string delimiter)
    {
        List<StringPair> pairs = new List<StringPair>();
        if (textAsset.text.Length > 0)
        {
            string[] lst = textAsset.text.Split('\n');
            foreach (string l in lst)
            {
                string[] words = l.Split(delimiter.ToCharArray());
                StringPair pair = new StringPair();
                pair.start = words[0].Trim();
                pair.end = words[1].Trim();
                pairs.Add(pair);
            }

            // PrintResults(pairs);
        }
        return pairs;
    }

    private static void PrintResults(List<StringPair> pairs)
    {
        string str = "";
        foreach (StringPair pair in pairs)
        {
            str += pair.start + ", " + pair.end + "\n";
        }
        Debug.Log(str);
    }

    [System.Serializable]
    public struct StringPair
    {
        public string start;
        public string end;
    }
}
