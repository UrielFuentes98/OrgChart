using OrgChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.ViewModels
{
    public class EmployeeWithImgString
    {
        public Employee Employee { get; set; }

        public string ImgDataString { get; set; }

        //Creates employee with image encoded string if entity has byte array of image data
        public static EmployeeWithImgString GetEmployeeWithImgString(Employee employee)
        {
            if (employee.EmployeePicture.Content != null)
            {
                string imageBase64Data = Convert.ToBase64String(employee.EmployeePicture.Content);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}",
                                        imageBase64Data);

                return new EmployeeWithImgString()
                { Employee = employee, ImgDataString = imageDataURL };
            }
            else
            {
                return new EmployeeWithImgString() { Employee = employee };
            }
        }
    }
}
