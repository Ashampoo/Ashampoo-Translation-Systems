using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.Dialogs;

/// <summary>
/// A dialog that allows the user to select a format.
/// </summary>
public partial class SelectFormatDialog : ComponentBase
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;

    private string? formatId;
    private bool error;

    private void Submit()
    {
        if (string.IsNullOrWhiteSpace(formatId))
        {
            error = true;
            return;
        }
        error = false;
        MudDialog.Close(DialogResult.Ok(formatId));
    }


    private void Cancel() => MudDialog.Cancel();
}