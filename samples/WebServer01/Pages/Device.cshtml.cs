using EasyIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace WebServer01.Pages;

[Authorize]
public class DeviceModel : PageModel
{
    [Required]
    [MaxLength(6)]
    [BindProperty]
    public string? Code { get; set; }

    private readonly IInteractionService _interactionService;

    public DeviceModel(IInteractionService interactionService)
    {
        _interactionService = interactionService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult?> OnPostValidateCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            return Page();
        }

        var result = await _interactionService.DeviceAuthorizeAsync(Code, User);

        if (!result.Valid)
        {
            ModelState.AddModelError("", "Invalid code");
            return Page();
        }

        //  TODO
        ModelState.AddModelError("", "Code applied");

        return Page();
    }
}
