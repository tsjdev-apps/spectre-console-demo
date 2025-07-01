using Spectre.Console;
using Spectre.Console.Json;
using System.Text.Json;

namespace SpectreConsoleSample.Utils;

/// <summary>
/// Helps with console output and input handling in the Spectre.Console application.
/// </summary>
internal static class ConsoleHelper
{
    /// <summary>    
    /// Defines the JSON serializer options used for formatting JSON output.
    /// </summary>    
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Clears the console and creates the header for the application.
    /// </summary>
    public static void ShowHeader()
    {
        AnsiConsole.Clear();


        Grid grid = new();
        grid.AddColumn();

        grid.AddRow(
            new FigletText("Spectre.Console")
                .Centered()
                .Color(Color.Red));

        grid.AddRow(
            Align.Center(
                new Panel(
                    "[red]Sample by Thomas Sebastian Jensen " +
                    "([link]https://www.tsjdev-apps.de[/])[/]")));

        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }


    /// <summary>
    /// Displays a prompt with the provided message and returns the user input.
    /// </summary>
    /// <param name="prompt">The prompt message.</param>
    /// <param name="showHeader">Indicator if the header should be shown.</param>
    /// <returns>The user input.</returns>
    public static string GetString(
        string prompt,
        bool showHeader = false)
        => PromptString(
            prompt,
            null,
            input => string.IsNullOrWhiteSpace(input)
                ? ValidationResult.Error("[red]Please enter a value.[/]")
                : ValidationResult.Success(),
            showHeader);


    /// <summary>
    /// Displays a prompt with the provided message and returns the user input.
    /// </summary>
    /// <param name="prompt">The prompt message.</param>
    /// <param name="defaultValue">An optional default value.</param>
    /// <param name="showHeader">Indicator if the header should be shown.</param>
    /// <returns>The user input.</returns>
    public static string GetString(
        string prompt,
        string defaultValue, 
        bool showHeader = false)
        => PromptString(
            prompt,
            defaultValue,
            input
                => string.IsNullOrWhiteSpace(input)
                    ? ValidationResult.Error("[red]Please enter a value.[/]")
                    : ValidationResult.Success(),
            showHeader);


    /// <summary> 
    /// Displays a prompt with the provided message and returns the user input,
    /// validates for an URL.
    /// </summary>
    /// <param name="prompt">The prompt message.</param>
    /// <param name="showHeader">Indicator if the header should be shown.</param>
    /// <returns>The user input.</returns>
    public static string GetUrl(
        string prompt, 
        bool showHeader = false)
        => PromptString(
            prompt,
            null,
            input =>
                {
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return ValidationResult.Error("[red]Please enter a value.[/]");
                    }

                    if (!Uri.TryCreate(input, UriKind.Absolute, out var uriResult) ||
                        (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                    {
                        return ValidationResult.Error("[red]Please enter a valid URL starting with http or https.[/]");
                    }

                    return ValidationResult.Success();
                },
            showHeader);


    /// <summary>
    /// Writes an object as JSON to the
    /// console with a header.
    /// </summary>
    /// <param name="obj">The object to display.</param>
    /// <param name="header">The header of the panel.</param>
    public static void WriteJson(
        object obj, 
        string header)
    {
        AnsiConsole.Write(
            new Panel(
                new JsonText(
                    JsonSerializer.Serialize(obj, jsonSerializerOptions)))
                        .Header(header)
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(Color.Yellow));
    }


    /// <summary>
    /// Writes the specified text to the console with a new line.
    /// </summary>
    /// <param name="text">The text to write.</param>
    public static void WriteToConsoleLine(
        string text)
    {
        AnsiConsole.MarkupLine($"[white]{text}[/]");
    }


    /// <summary>
    /// Writes the specified text in red to the console with a new line.
    /// </summary>
    /// <param name="text">The text to write.</param>
    public static void WriteErrorToConsoleLine(
        string text)
    {
        AnsiConsole.MarkupLine($"[red]{text}[/]");
    }


    /// <summary>
    /// Writes the specified text to the console.
    /// </summary>
    /// <param name="text">The text to write.</param>    
    public static void WriteToConsole(
        string text)
    {
        AnsiConsole.Markup($"[white]{text}[/]");
    }


    /// <summary>
    /// Writes the specified text in red to the console.
    /// </summary>
    /// <param name="text">The text to write.</param>    
    public static void WriteErrorToConsole(
        string text)
    {
        AnsiConsole.Markup($"[red]{text}[/]");
    }


    /// <summary>
    /// Gets a string input from the user with validation and optional default value.
    /// </summary>
    /// <param name="prompt">The prompt message.</param>
    /// <param name="defaultValue">An optional default value.</param>
    /// <param name="validator">An optional validator.</param>
    /// <param name="showHeader">Indicator if the header should be shown.</param>
    /// <returns></returns>    
    private static string PromptString(
        string prompt,
        string? defaultValue = null,
        Func<string, ValidationResult>? validator = null,
        bool showHeader = false)
    {
        if (showHeader)
        {
            ShowHeader();
        }


        TextPrompt<string> textPrompt =
            new TextPrompt<string>(prompt)
                .PromptStyle("white");

        if (!string.IsNullOrEmpty(defaultValue))
        {
            textPrompt = textPrompt.DefaultValue(defaultValue);
        }

        if (validator != null)
        {
            textPrompt = 
                textPrompt
                    .ValidationErrorMessage("[red]Invalid input[/]")
                    .Validate(validator);
        }

        return AnsiConsole.Prompt(textPrompt);
    }
}
