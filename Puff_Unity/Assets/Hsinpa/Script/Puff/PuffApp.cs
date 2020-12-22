using Hsinpa.View;
using ObserverPattern;
using Puff.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PuffApp : Singleton<PuffApp>
{
    [SerializeField]
    private ModelsManager _models;
    public ModelsManager models => _models;

    protected PuffApp() { } // guarantee this will be always a singleton only - can't use the constructor!

    private Subject subject;


    private Observer[] observers = new Observer[0];

    private void Awake()
    {
        subject = new Subject();

        RegisterAllController(subject);

        Init();
    }

    private void Start()
    {
        Notify(EventFlag.Event.GameStart);
    }

    public void Notify(string entity, params object[] objects)
    {
        subject.notify(entity, objects);
    }

    public void Init()
    {
        Modals.instance.CloseAll();
        models.SetUp();
    }

    private void RegisterAllController(Subject p_subject)
    {
        Transform ctrlHolder = transform.Find("Controller");

        if (ctrlHolder == null) return;

        observers = transform.GetComponentsInChildren<Observer>();

        foreach (Observer observer in observers)
        {
            subject.addObserver(observer);
        }
    }


    public T GetObserver<T>() where T : Observer
    {
        foreach (Observer observer in observers)
        {
            if (observer.GetType() == typeof(T)) return (T)observer;
        }

        return default(T);
    }

    private void OnApplicationQuit()
    {

    }
}
