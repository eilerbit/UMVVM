using System;
using UnityEngine.UIElements;

namespace Presentation.Common
{
    public interface IScreenBinder : IDisposable
    {
        void Bind(VisualElement root);
    }
}
