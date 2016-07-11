using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using LinqToTwitter;
using Microsoft.ProjectOxford.Vision.Contract;
using SociOLRoom.Analytics.Core;
using SociOLRoom.Analytics.Models;
using Rectangle = System.Drawing.Rectangle;

namespace SociOLRoom.Analytics.Controllers
{


    public class HomeController : Controller
    {


        public ViewResult Index()
        {


            return View();
        }


        [OutputCache(Duration = 84600, Location = OutputCacheLocation.Any)]
        public async Task<FileResult> Picture(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var stream = await client.GetStreamAsync(url);

                    Bitmap bitmap = (Bitmap)Image.FromStream(stream);

                    Bitmap clone = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    using (Graphics gr = Graphics.FromImage(clone))
                    {
                        gr.DrawImage(bitmap, new Rectangle(0, 0, clone.Width, clone.Height));
                    }

                    var img = clone.AutoCrop();

                    FileContentResult result;

                    using (var memStream = new System.IO.MemoryStream())
                    {
                        img.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        result = this.File(memStream.GetBuffer(), "image/jpeg");
                    }

                    return result;

                }
            }
            catch (Exception)
            {
                return new FileContentResult(null, "image/jpeg");
            }
        }
    }
}