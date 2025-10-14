using System.Windows;

namespace Tp2.Behaviors
{
    /// <summary>
    /// Ferme une Window lorsque la propriété attachée DialogResult change.
    /// </summary>
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(null, OnDialogResultChanged));

        public static void SetDialogResult(Window target, bool? value)
            => target.SetValue(DialogResultProperty, value);

        public static bool? GetDialogResult(Window target)
            => (bool?)target.GetValue(DialogResultProperty);

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                var result = e.NewValue as bool?;
                if (result.HasValue)
                {
                    window.DialogResult = result;
                    window.Close();
                }
            }
        }
    }
}
