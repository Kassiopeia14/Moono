using System.Text;
using System.Text.Json;
using modMnemosyneJSONModels;

namespace SimpleSender;

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

    public async ValueTask IncrementSimpleSender()
    {
        HttpClient httpClient = new HttpClient();
        
        string 
            sender = "SENDER",
            receiver = "RECEIVER",
            serviceUri = "http://localhost:6729";

        Random random = new Random();

        MessageItem message = new MessageItem
        {
            Text = "BUBA" + random.Next(0, 100).ToString()
        };

        httpClient.DefaultRequestHeaders.Add("Sender", $"{sender}");
        httpClient.DefaultRequestHeaders.Add("Receiver", $"{receiver}");

        string jsonString = JsonSerializer.Serialize(message);
        
        HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage result = await httpClient.PostAsync(serviceUri + "/api/Message", httpContent);

        await Countable.UpdateAsync(c => c?.Increment());
    }
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
                    .Text(() => vm.Countable.Count, txt => $"SimpleSender: {txt}"),
                new TextBox()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .PlaceholderText("Step Size")
                    .Text(x => x.Binding(() => vm.Countable.Step).TwoWay()),
                new Button()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Command(() => vm.IncrementSimpleSender)
                    .Content("Send")

            )));

    }
}
