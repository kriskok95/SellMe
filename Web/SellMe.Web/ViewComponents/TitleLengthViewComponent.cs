namespace SellMe.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    public class TitleLengthViewComponent : ViewComponent
    {
        private const int StartIndex = 0;
        private const int StringLength = 15;

        public IViewComponentResult Invoke(string adTitle)
        {
            var result = adTitle.Length > 25 ? adTitle.Substring(StartIndex, StringLength) + "..." : adTitle;

            //TODO: Check why looking for view  different from "Default".
            return this.View("Default",result);
        }
    }
}
