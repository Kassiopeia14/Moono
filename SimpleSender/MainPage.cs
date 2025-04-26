using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using modMnemosyneJSONModels;
using Windows.Networking.Sockets;

namespace SimpleSender;

internal partial record Messager(
    string Sender
)
{
    public string Receiver { get; set;}
    public string MessageText { get; set;}

    string serviceUri = "http://localhost:6729";

    public async Task SendMessage()
    {
        HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Sender", $"{Sender}");
        httpClient.DefaultRequestHeaders.Add("Receiver", $"{Receiver}");

        MessageItem message = new MessageItem()
        {
            Text = MessageText
        };

        string jsonString = JsonSerializer.Serialize(message);
        
        HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

        HttpResponseMessage result = await httpClient.PostAsync(serviceUri + "/api/Message", httpContent);
    }
}

internal partial record MainModel
{    
    public IState<Messager> Messager => State.Value(this, () => new Messager("moosender"));

    public async ValueTask Send()
    {
        var current = await Messager.Value();
        if (current is not null)
        {
            await current.SendMessage();
        }
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
                    .Text(() => vm.Messager.Sender, txt => $"Your username: {txt}"),
                new TextBox()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .PlaceholderText("Receiver username")
                    .Text(x => x.Binding(() => vm.Messager.Receiver).TwoWay()),
                new TextBox()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .TextAlignment(Microsoft.UI.Xaml.TextAlignment.Center)
                    .PlaceholderText("Type your message")
                    .Text(x => x.Binding(() => vm.Messager.MessageText).TwoWay()),
                new Button()
                    .Margin(12)
                    .HorizontalAlignment(HorizontalAlignment.Center)
                    .Command(() => vm.Send)
                    .Content("Send")
            )));

    }
}