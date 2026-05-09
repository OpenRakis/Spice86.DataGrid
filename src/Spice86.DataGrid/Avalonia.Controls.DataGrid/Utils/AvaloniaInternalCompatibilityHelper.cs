// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

#nullable disable

using System.Reflection;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace Avalonia.Controls.Utils
{
    internal static class AvaloniaInternalCompatibilityHelper
    {
        public static Visual GetFocusedElement(InputElement inputElement)
        {
            TopLevel topLevel = TopLevel.GetTopLevel(inputElement);
            object focusManager = topLevel?.GetType().GetProperty("FocusManager", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(topLevel);
            if (focusManager == null)
            {
                return null;
            }

            // Use the overload that takes an explicit Type[] of parameters to disambiguate
            // between multiple GetFocusedElement overloads exposed by newer Avalonia versions
            // (Avalonia 12+ adds a GetFocusedElement(...) overload, which makes the lookup
            // by name alone throw AmbiguousMatchException).
            MethodInfo getFocusedElementMethod = focusManager.GetType().GetMethod(
                "GetFocusedElement",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                types: Type.EmptyTypes,
                modifiers: null);
            if (getFocusedElementMethod != null)
            {
                return getFocusedElementMethod.Invoke(focusManager, null) as Visual;
            }

            PropertyInfo focusedElementProperty = focusManager.GetType().GetProperty("FocusedElement", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return focusedElementProperty?.GetValue(focusManager) as Visual;
        }

        public static void NotifyDataContextProperty(StyledElement styledElement, bool isBeginUpdate)
        {
            PropertyInfo notifyingProperty = StyledElement.DataContextProperty.GetType().GetProperty("Notifying", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            object notifyingDelegate = notifyingProperty?.GetValue(StyledElement.DataContextProperty);
            MethodInfo invokeMethod = notifyingDelegate?.GetType().GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
            invokeMethod?.Invoke(notifyingDelegate, new object[] { styledElement, isBeginUpdate });
        }
    }
}