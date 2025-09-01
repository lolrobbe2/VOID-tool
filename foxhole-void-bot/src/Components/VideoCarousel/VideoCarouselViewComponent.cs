using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class VideoCarouselViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var videoUrls = new List<string>
        {
            "https://www.youtube.com/embed/i-CYRlveLnU",
            "https://www.youtube.com/embed/HHLzJd2FOQQ",
            "https://www.youtube.com/embed/abdD3kgB4cg"
            // Add more URLs as needed
        };

        return View(videoUrls);
    }
}
