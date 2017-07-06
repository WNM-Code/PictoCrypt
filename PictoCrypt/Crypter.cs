using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PictoCrypt
{
    class Crypter
    {
        private String key;

        public Crypter(String key)
        {
            this.key = key;
        }

        public string getKey()
        {
            return this.key;
        }

        public void setKey(String key)
        {
            this.key = key;
        }

        public Bitmap Encrypt(Bitmap i, String saveLocation)
        {
            //for (int j = 0; j < 100; j++)
            //{
            //    for (int k = 0; k < 100; k++)
            //    {
            //        i.SetPixel(j, k, Color.FromArgb(255, 19, 15, 255));
            //    }
            //}
            i.Save(@"" + saveLocation + ".png");
            return null;
        }
    }
}
