using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public string TranslateNumToRichText(int n, int len = 1)
    {
        string res = "";
        Stack<int> reverseNum = new Stack<int>();
        while (n > 0)
        {
            reverseNum.Push(n % 10);
            n = n / 10;
        }
        while (reverseNum.Count < len)
            reverseNum.Push(0);
        while (reverseNum.Count > 0)
        {
            res += $"<sprite={reverseNum.Pop()}>";
        }
        return res;
    }
    static public string TranslateSecToRichText(float t)
    {
        int sec = Mathf.FloorToInt(t);
        int ms = Mathf.FloorToInt((t - sec) * 100);
        int min = sec / 60;
        sec = sec % 60;
        return Utils.TranslateNumToRichText(min, 2) + ":" + Utils.TranslateNumToRichText(sec, 2) + ":" + Utils.TranslateNumToRichText(ms, 2);
    }
}
