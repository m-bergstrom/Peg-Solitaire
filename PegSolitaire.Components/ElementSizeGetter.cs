using Microsoft.JSInterop;

namespace PegSolitaire.Components;

public class ElementSizeGetter : IAsyncDisposable
{
    private class Size
    {
        public double width { get; set; }
        public double height { get; set; }
    }

    private readonly Lazy<Task<IJSObjectReference>> _ModuleTask;

    public ElementSizeGetter(IJSRuntime jsRuntime)
    {
        _ModuleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/PegSolitaire.Components/scripts/domInterop.js").AsTask());
    }

    public async Task<(double? Width, double? Height)> GetElementSizeAsync(string id)
    {
        var module = await _ModuleTask.Value;
        var size = await module.InvokeAsync<Size>("getElementSize", id);
        return (size?.width, size?.height);
    }

    public async ValueTask DisposeAsync()
    {
        if (_ModuleTask.IsValueCreated)
        {
            var module = await _ModuleTask.Value;
            await module.DisposeAsync();
        }
    }
}