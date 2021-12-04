using System.Net;
using System.Threading;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class RestClient : Assets.IObservable
{
    private string URL;
    private string lastResponse = "default";

    private Mutex mutex = new Mutex();

    private HashSet<Assets.IObserver> observers = new HashSet<Assets.IObserver>();

    public RestClient(string URL)
    {
        this.URL = URL;
    }

    public IEnumerator POST(string data)
    {
        yield return null;
        try
        {
            WebRequest request = WebRequest.Create(URL);
            request.Method = "POST";
            request.Timeout = 30000;
            request.ContentType = "text/plain";

            byte[] sent = Encoding.GetEncoding(65001).GetBytes(data);
            request.ContentLength = sent.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(sent, 0, sent.Length);
            }

            WebResponse Response = request.GetResponse();

            using (Stream receivedStream = Response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(receivedStream, Encoding.UTF8))
                {
                    mutex.WaitOne();
                    lastResponse = sr.ReadToEnd();
                    mutex.ReleaseMutex();
                    NotifyObservers(lastResponse);
                }
            }
        }
        catch (System.Exception)
        {

        }
    }

    public void AddObserver(Assets.IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(Assets.IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers(object s)
    {
        foreach (var observer in observers)
        {
            observer.Update(s);
        }
    }
}
