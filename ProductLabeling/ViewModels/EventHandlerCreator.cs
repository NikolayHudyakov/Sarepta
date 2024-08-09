﻿using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System;

namespace ProductLabeling.ViewModels
{
    public abstract class EventHandlerCreator
    {
        private readonly Dictionary<string, object> _eventHandlers = new();

        protected Action<T> GetEventHandler<T>(T property, [CallerArgumentExpression(nameof(property))] string propName = "")
        {
            PropertyInfo? prop = GetType().GetProperty(propName);
            Action<T> eventHandler = (T data) => prop?.SetValue(this, data);
            _eventHandlers.TryAdd(propName, eventHandler);
            return (Action<T>)_eventHandlers[propName];
        }
    }
}
