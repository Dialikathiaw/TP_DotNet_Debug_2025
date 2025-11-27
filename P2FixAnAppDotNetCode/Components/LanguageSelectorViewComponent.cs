using Microsoft.AspNetCore.Mvc;
using P2FixAnAppDotNetCode.Models.Services;

namespace P2FixAnAppDotNetCode.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ILanguageService languageService)
        {
            // The view expects a LanguageViewModel for the select binding.
            // We only use the ILanguageService here for switching logic, but
            // a view model must be provided to the view for correct binding.
            return View(new Models.ViewModels.LanguageViewModel());
        }
    }
}
