using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace IHK.Web.ViewComponents
{
    public class AdresseSearchResultComponent : ViewComponent
    {
        public AdresseSearchResultComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
