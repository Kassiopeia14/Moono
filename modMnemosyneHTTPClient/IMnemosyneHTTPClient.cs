using modMnemosyneJSONModels;

namespace modMnemosyneHTTPClient;

public interface IMnemosyneHTTPClient
{
    Task PostMessage(string sender, string receiver, MessageItem message);

    Task<string?> GetTest();
}
