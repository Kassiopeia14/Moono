namespace Counter;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(new StackPanel()
            .VerticalAlignment(VerticalAlignment.Center)
            //.HorizontalAlignment(HorizontalAlignment.Center)
            .Children(                    
                new Image()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Width(150)
                    .Height(150)
                    .Source("ms-appx:///Assets/Images/logo.png"),

                new TextBox()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .PlaceholderText("Step Size")
                    .Text("1"),
                new TextBlock()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .Text("Counter: 1"),
                new Button()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Content("Increment Counter by Step Size")

            ));

    }
}
