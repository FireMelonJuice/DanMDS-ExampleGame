using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Logging : MonoBehaviour
{
    private LogSystem logSystem;

    private float time;
    private bool isStarted = false;

    private List<string> logs = new List<string>();
    private StreamWriter sw;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.Find("LogSystem");
        logSystem = obj.GetComponent<LogSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            time += Time.deltaTime;
        }
    }

    public void InitLogging()
    {
        isStarted = true;
        logs.Clear();
        time = 0;
        sw = new StreamWriter("log.csv");
    }

    public void LogPlayer(string action, Vector2 position)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(time.ToString("0.000"));
        stringBuilder.Append(",");
        stringBuilder.Append(action);
        stringBuilder.Append(",");
        stringBuilder.Append(position.x.ToString("0.000000"));
        stringBuilder.Append(" ");
        stringBuilder.Append(position.y.ToString("0.000000"));
        stringBuilder.Append(";");
        string log = stringBuilder.ToString();

        /*
        logs.Add(log);
        if (logs.Count == 10)
        {

        }
        */

        //sw.WriteLine(log);
        logSystem.SendData(log);

        Debug.Log(log);
    }
}
