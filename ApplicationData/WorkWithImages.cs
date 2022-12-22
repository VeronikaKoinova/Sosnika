using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFApplicationOptika.ApplicationData
{
    public partial class Products
    {
        public string InsideImage
        {
            get
            {
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Resources\\" + ImageProduct))
                {
                    return "/Resources/" + ImageProduct; 
                }
                else
                {
                    return "/Resources/picture.jpg";
                }
            }
        }
        public Brush CountZero
        {
            get
            {
                if (InStock <= 0)
                {
                    return Brushes.Gray;
                }
                else
                {
                    return Brushes.Transparent;
                }
            }
        }
    }
}
