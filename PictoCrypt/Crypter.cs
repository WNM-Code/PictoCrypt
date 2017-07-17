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
        private List<int> convKey;
        private int keyLen;

        public Crypter(String key)
        {
            this.key = key;
            convKey = new List<int>();
        }

        public string getKey()
        {
            return this.key;
        }

        public void setKey(String key)
        {
            this.key = key;
            convKey.Clear();
            keyLen = key.Length;
            char[] splitKey = key.ToCharArray();
            for (int i = 0; i < key.Length; i++)
            {
                convKey.Add(splitKey[i]);
            }
        }

        public Bitmap encrypt(Bitmap i, String saveLocation, String name, String type)
        {
            int[] bounds = getBounds(i);
            Bitmap b;

            b = pushV(i, bounds[0], bounds[1]);
            b = pushH(b, bounds[0], bounds[1]);
            //b = push(i, bounds[0], bounds[1]);
            b.Save(@"" + saveLocation + name + "-encrypted" + type);
            return null;
        }

        public Bitmap decrypt(Bitmap i, String saveLocation, String name, String type)
        {
            int[] bounds = getBounds(i);
            Bitmap b;

            b = unPushH(i, bounds[0], bounds[1]);
            b = unPushV(b, bounds[0], bounds[1]);
            //b= unPush(i, bounds[0], bounds[1]);
            b.Save(@"" + saveLocation + name + "-decrypted" + type);
            return null;
        }

        private Bitmap push(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            //ToDo: Resolve overwrite issue
            for (int wloc = 0; wloc < wid; wloc++)
            {
                for (int hloc = 0; hloc < hei; hloc++)
                {
                    b.SetPixel((wloc + convKey[hloc % keyLen]) % wid, (hloc + convKey[wloc % keyLen]) % hei, a.GetPixel(wloc, hloc));
                }
            }
            return b;
        }
        private Bitmap unPush(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            for (int wloc = 0; wloc < wid; wloc++)
            {
                for (int hloc = 0; hloc < hei; hloc++)
                {
                    b.SetPixel(wloc, hloc, a.GetPixel((wloc + convKey[hloc % keyLen]) % wid, (hloc + convKey[wloc % keyLen]) % hei));
                }
            }
            return b;
        }
        private Bitmap pushV(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            for (int wloc = 0; wloc < wid; wloc++)
            {
                for(int hloc = 0; hloc < hei; hloc++)
                {
                    b.SetPixel(wloc, (hloc + (convKey[wloc % keyLen] * 40)) % hei, a.GetPixel(wloc,hloc));
                }
            }
            return b;
        }
        private Bitmap unPushV(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            //fix negative problem better
            for (int wloc = 0; wloc < wid; wloc++)
            {
                for (int hloc = 0; hloc < hei; hloc++)
                {
                    b.SetPixel(wloc, ((hloc - (convKey[wloc % keyLen]*40))+(hei*40)) % hei, a.GetPixel(wloc, hloc));
                }
            }
            return b;
        }
        private Bitmap pushH(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            for (int hloc = 0; hloc < hei; hloc++)
            {
                for (int wloc = 0; wloc < wid; wloc++)
                {
                    b.SetPixel((wloc + (convKey[hloc%keyLen]*40))%wid, hloc, a.GetPixel(wloc, hloc));
                }
            }
            return b;
        }
        private Bitmap unPushH(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            //fix negative problem better
            for (int wloc = 0; wloc < wid; wloc++)
            {
                for (int hloc = 0; hloc < hei; hloc++)
                {
                    b.SetPixel(((wloc - (convKey[hloc % keyLen]*40))+(wid*40)) % wid, hloc, a.GetPixel(wloc, hloc));
                }
            }
            return b;
        }
        private Bitmap shuffle(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            return b;
        }
        private Bitmap flip(Bitmap a, int wid, int hei)
        {
            Bitmap b = new Bitmap(a);
            return b;
        }
        
        /* gets the bounds of the image
         * returns an array of the bounds:
         * as {width, height}
         */
        private int[] getBounds(Bitmap i)
        {
            GraphicsUnit px = GraphicsUnit.Pixel;
            RectangleF r = i.GetBounds(ref px);
            int[] bounds = { (int)r.Width, (int)r.Height };
            return bounds;
        }
    }
}
