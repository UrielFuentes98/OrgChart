using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChart.Models
{
    public class PictureRepository : IPictureRepository
    {
        private readonly OrgChartDbContext dbContext;

        public PictureRepository(OrgChartDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddPciture(EmployeePicture picture)
        {
            dbContext.Pictures.Add(picture);
            dbContext.SaveChanges();
        }
    }
}
