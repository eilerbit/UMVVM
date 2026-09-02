using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace Presentation.Common
{
    /// <summary>
    /// Minimal base for UI Toolkit runtime data-binding sources.
    /// ViewModels remain plain C# objects: they know about the binding contract,
    /// but never reference VisualElement instances or scene objects.
    /// </summary>
    public abstract class BindableViewModel : INotifyBindablePropertyChanged
    {
        public event EventHandler<BindablePropertyChangedEventArgs> propertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            BindingId bindingId = propertyName;
            propertyChanged?.Invoke(this, new BindablePropertyChangedEventArgs(bindingId));
        }
    }
}
