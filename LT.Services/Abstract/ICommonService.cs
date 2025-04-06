using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.Services.Abstract
{
    public interface ICommonService
    {
        string MD5Hash(string input);
        string SaveIamge(string imagePath, IFormFile formFile);
    }
}
