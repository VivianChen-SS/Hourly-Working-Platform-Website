using final_project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace final_project.Repositories
{
    public class ImageRepository
    {
        private readonly finalprojectEntities db = new finalprojectEntities();

        //public int UploadImageInDataBase(HttpPostedFileBase file, ImageViewModels imageViewModels)
        public EMPLOYER UploadImageInDataBase(HttpPostedFileBase file, EMPLOYER employer)
        {
            employer.ImageData = ConvertToBytes(file);
            return employer;
        }

        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}