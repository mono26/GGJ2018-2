// Copyright (c) What a Box Creative Studio. All rights reserved.

using System;
using UnityEngine;

public class GameException : Exception
{
    public GameException() : base() { }
    public GameException(string _message) : base(_message) { }

    protected virtual string FormatMessage()
    {
        string message;
        if (Message == null || Message == "") {
            message = "The game encountered an exception.";
        }
        else {
            message = Message;
        }
        return message;
    }

    private void LogMessage(string _messageToLog)
    {
        Debug.LogWarning(_messageToLog);
        return;
    }

    public void DisplayException()
    {
        string messageToLog = FormatMessage();
        LogMessage(messageToLog);
        return;
    }
}

public class MissingComponentException : GameException
{
    private Type missingComponent;
    private GameObject gameObjectWithException;

    public MissingComponentException() : base("Missing component exception.") { }
    public MissingComponentException(string _message) : base(_message) { }
    // TODO refactorization when we only pass a gameObject.
    public MissingComponentException(GameObject _gameObjectWithException, Type _missingComponent) : base()
    {
        missingComponent = _missingComponent;
        gameObjectWithException = _gameObjectWithException;
        return;
    }

    protected override string FormatMessage()
    {
        string message;
        if (Message == null || Message == "")
            message = "The GameObject " + gameObjectWithException.name + " has a missing component: " + missingComponent.ToString();
        else
            message = Message + missingComponent.ToString();
        return message;
    }
}

public class EventException : GameException
{
    Type eventType;

    public EventException() : base("Event Exception") { }
    public EventException(Type _eventType) : base()
    {
        eventType = _eventType;
        return;
    }
    public EventException(Type _eventType, string _message) : base(_message)
    {
        eventType = _eventType;
        return;
    }

    protected override string FormatMessage()
    {
        string message;
        if (Message == null || Message == "")
            message = "Event of type: " + eventType + " encountered a exception.";
        else
            message = eventType.Name + Message;
        return message;
    }
}
