using BrainBoost.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BrainBoost.Models
{
    public class Proxy
    {
        public void CheckPayment(CourseProgress courseProgress, Course course, ViewDataDictionary viewData)
        {
            if (courseProgress == null && course.Price == 0 || courseProgress != null)
            {
                viewData["NeedsPaying"] = "false";
            }
            else
            {
                viewData["NeedsPaying"] = "true";
            }
        }
    }
}
