using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public interface IPictureRepository
    {
        void AddPciture(EmployeePicture picture);

    }
}
