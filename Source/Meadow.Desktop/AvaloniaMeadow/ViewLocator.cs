using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaMeadow.ViewModels;
using System;

namespace AvaloniaMeadow
{
    public class ViewLocator : IDataTemplate
    {
        public bool Match(object data)
        {
            return data is ViewModelBase;
        }

        Control? ITemplate<object?, Control?>.Build(object? param)
        {
            var name = param.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }
    }
}