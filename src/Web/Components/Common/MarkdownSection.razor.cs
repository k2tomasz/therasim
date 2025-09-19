using Markdig;
using Microsoft.AspNetCore.Components;

namespace Therasim.Web.Components.Common;

public partial class MarkdownSection : ComponentBase
{
    private bool _markdownChanged = false;
    private string? _content;
    /// <summary>
    /// Gets or sets the Markdown content 
    /// </summary>
    [Parameter]
    public string? Content
    {
        get => _content;
        set
        {
            if (_content is not null && !_content.Equals(value))
            {
                _markdownChanged = true;
            }
            _content = value;
        }
    }

    [Parameter]
    public EventCallback OnContentConverted { get; set; }

    public MarkupString HtmlContent { get; private set; }

    protected override void OnInitialized()
    {
        if (Content is null)
        {
            throw new ArgumentException("You need to provide either Content or FromAsset parameter");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender || _markdownChanged)
        {
            _markdownChanged = false;

            // create markup from markdown source
            HtmlContent = MarkdownToMarkupString();
            StateHasChanged();

            // notify that content converted from markdown 
            if (OnContentConverted.HasDelegate)
            {
                await OnContentConverted.InvokeAsync();
            }
        }
    }

    /// <summary>
    /// Converts markdown, provided in Content or from markdown file stored as a static asset, to MarkupString for rendering.
    /// </summary>
    /// <returns>MarkupString</returns>
    private MarkupString MarkdownToMarkupString()
    {
        string? markdown = Content;
        return ConvertToMarkupString(markdown);
    }
    private static MarkupString ConvertToMarkupString(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var builder = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions();

            var pipeline = builder.Build();

            // Convert markdown string to HTML
            var html = Markdown.ToHtml(value, pipeline);

            // Return sanitized HTML as a MarkupString that Blazor can render
            return new MarkupString(html);
        }

        return new MarkupString();
    }
}

