using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.Dialogs;

/// <summary>
/// A dialog that allows the user to configure options for a format.
/// </summary>
public partial class ConfigureFormatOptionsDialog : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;


    /// <summary>
    /// The format options to configure.
    /// </summary>
    [Parameter]
    public FormatOptions FormatOptions { get; init; } = default!;

    private MudForm? form;

    private void Submit()
    {
        form?.Validate();
        if (!form?.IsValid ?? true) return;
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel()
    {
        form?.ResetValidation();
        MudDialog.Cancel();
    }
}