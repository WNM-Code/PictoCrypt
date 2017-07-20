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
            for (int i = 0; i < keyLen; i++)
            {
                convKey.Add(splitKey[i]);
            }
        }

        public Bitmap encrypt(Bitmap i, String saveLocation, String name, String type)
        {
            int[] bounds = getBounds(i);
            Bitmap b;
            int[] randx = randomize(bounds[0]);
            int[] randy = randomize(bounds[1]);
            b = randomEnW(i, bounds[0], bounds[1], randx);
            b = randomEnH(b, bounds[0], bounds[1], randy);
            b = pushV(b, bounds[0], bounds[1]);
            b = pushH(b, bounds[0], bounds[1]);
            b.Save(@"" + saveLocation + name + "-encrypted" + type);
            return null;
        }

        public Bitmap decrypt(Bitmap i, String saveLocation, String name, String type)
        {
            int[] bounds = getBounds(i);
            Bitmap b;
            int[] randx = randomize(bounds[0]);
            int[] randy = randomize(bounds[1]);
            b = unPushH(i, bounds[0], bounds[1]);
            b = unPushV(b, bounds[0], bounds[1]);
            b = randomDeH(b, bounds[0], bounds[1], randx);
            b = randomDeW(b, bounds[0], bounds[1], randy);
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

        //Creates the mapping, utilizing the methods below
        private int[] randomize(int bound)
        {
            ulong[] flat = init(bound);
            flat = remove(flat);
            flat = fill(flat);
            int[] arr = new int[flat.Length];
            int i = 0;
            foreach(ulong a in flat)
            {
                arr[i] = (int)a -1;
                i++;
            }
            return arr;
        }

        //Fills the mapping with numbers based on a key that you put into it
        //this key was held in the containing class and is referenced as "key"
        private ulong[] init(int longSize)
        {
            ulong[] flat = new ulong[longSize];
            int i = 0;
            while (i < keyLen && i < longSize)
            {
                flat[i] = key[i];
                i++;
            }
            i = 0;
            int j = keyLen;
            int l = 0;
            while (j < longSize)
            {
                flat[j] = flat[l] + flat[l + 1];
                l++;
                j++;
            }
            for (i = 0; i < longSize; i++)
            {
                flat[i] = flat[i] % (ulong)longSize;
            }
            return flat;
        }

        //Replaces all duplicates from the mapping with -1
        private ulong[] remove(ulong[] flat)
        {
            int l = 0;
            int r = flat.Length;
            while (l < r)
            {
                for (int i = l + 1; i < r; i++)
                {
                    if (flat[l] == flat[i])
                    {
                        flat[i] = 0;
                    }
                }
                l++;
            }
            return flat;
        }

        //Fills the mapping with the missing numbers
        //Finds the numbers that were missing
        private ulong[] fill(ulong[] flat)
        {
            List<ulong> tempflat = new List<ulong>(flat);
            List<ulong> tempmissing = new List<ulong>();
            int m = 0;
            for (ulong i = 0; i < (ulong)flat.Length; i++)
            {
                if (!tempflat.Contains(i+1))
                {
                    tempmissing.Add(i+1);
                    m++;
                }
            }
            ulong[] missing = tempmissing.ToArray();
            int j = flat.Length-1;
            int k = 0;
            for (int i = 0; i < m;)
            {
                while (j > 0 && flat[j] != 0)
                {
                    j--;
                }
                if (j > 0)
                {
                    flat[j] = missing[i];
                    i++;
                }
                while (k < flat.Length && flat[k] != 0)
                {
                    k++;
                }
                if (k <= flat.Length - 1)
                {
                    flat[k] = missing[i];
                    i++;
                }
            }
            return flat;
        }

        //Method that encrypts the photo with the created mapping
        private Bitmap randomEnH(Bitmap i, int wid, int hei, int[] key)
        {
            Bitmap b = new Bitmap(i);
            for (int wloc = 0; wloc < wid; wloc++)
            {
                for (int hloc = 0; hloc < hei; hloc++)
                {
                    b.SetPixel(wloc, key[hloc], i.GetPixel(wloc, hloc));
                }
            }
            return b;
        }

        //Method that encrypts the photo with the created mapping
        private Bitmap randomEnW(Bitmap i, int wid, int hei, int[] key)
        {
            Bitmap b = new Bitmap(i);
            for (int hloc = 0; hloc < hei; hloc++) 
            {
                for (int wloc = 0; wloc < wid; wloc++)
                {
                    b.SetPixel(key[wloc], hloc, i.GetPixel(wloc, hloc));
                }
            }
            return b;
        }

        //Method that decrypts the photo with the created mapping
        private Bitmap randomDeH(Bitmap i, int wid, int hei, int[] key)
        {
            Bitmap b = new Bitmap(i);
            for (int hloc = 0; hloc < hei; hloc++)
            {
                for (int wloc = 0; wloc < wid; wloc++)
                {
                    b.SetPixel(wloc, hloc, i.GetPixel(wloc, key[hloc]));
                }
            }
            return b;
        }

        //Method that decrypts the photo with the created mapping
        private Bitmap randomDeW(Bitmap i, int wid, int hei, int[] key)
        {
            Bitmap b = new Bitmap(i);
            for (int hloc = 0; hloc < hei; hloc++)
            {
                for (int wloc = 0; wloc < wid; wloc++)
                {
                    b.SetPixel(wloc, hloc, i.GetPixel(key[wloc], hloc));
                }
            }
            return b;
        }
    }
}
