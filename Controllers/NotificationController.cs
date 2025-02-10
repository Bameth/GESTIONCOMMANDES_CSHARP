using GESTIONCOMMANDES.services;
using Microsoft.AspNetCore.Mvc;

public class NotificationController : Controller
{
    private readonly SmsService _smsService;

    public NotificationController(SmsService smsService)
    {
        _smsService = smsService;
    }

    [HttpPost]
    public async Task<IActionResult> EnvoyerSms(string numeroTelephone, string message)
    {
        try
        {
            await _smsService.SendSmsAsync(numeroTelephone, message);
            TempData["SuccessMessage"] = "SMS envoyé avec succès !";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Erreur lors de l'envoi du SMS : {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}
