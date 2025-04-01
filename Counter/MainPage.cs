namespace Counter;

internal partial record Countable(int Count, int Step)
{
    public Countable Increment() => this with
    {
        Count = Count + Step
    };
}

internal partial record MainModel
{
    public IState<Countable> Countable => State.Value(this, () => new Countable(0, 1));

    public ValueTask IncrementCounter()
        => Countable.UpdateAsync(c => c?.Increment());
}

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.DataContext(new MainViewModel(), (page, vm) => page
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

                new TextBlock()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .Text(() => vm.Countable.Count, txt => $"Counter: {txt}"),
                new TextBox()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .PlaceholderText("Step Size")
                    .Text(x => x.Binding(() => vm.Countable.Step).TwoWay()),
                new Button()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Command(() => vm.IncrementCounter)
                    .Content("Increment Counter by Step Size")

            )));

    }
}
