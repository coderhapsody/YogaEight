using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoGym.Data;

namespace GoGym.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new FitnessEntities();
            Header h = new Header();
            h.Name = "Header A";
            
            Detail d = new Detail();
            d.Description = "Detail A";

            d.Header = h;
            
            context.Add(d);

            context.Add(h);
 

            context.SaveChanges();

            //Microsoft.VisualBasic.Financial.Pmt()
        }
    }
}
