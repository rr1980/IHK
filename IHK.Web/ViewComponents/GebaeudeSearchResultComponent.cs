using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace IHK.Web.ViewComponents
{
    public class GebaeudeSearchResultComponent : ViewComponent
    {
        public GebaeudeSearchResultComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
