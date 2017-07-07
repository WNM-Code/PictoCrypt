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
            char[] splitKey = key.ToCharArray();
            for (int i = 0; i < key.Length; i++)
            {
                convKey.Add(splitKey[i]);
            }

            foreach(int a in convKey)
            {
                Console.WriteLine(a);
            }
        }

        public Bitmap encrypt(Bitmap i, String saveLocation, String name, String type)
        {
            i.Save(@"" + saveLocation + name + "-encrypted" + type);
            return null;
        }

        public Bitmap decrypt(Bitmap i, String saveLocation, String name, String type)
        {
            i.Save(@"" + saveLocation + name + "-decrypted" + type);
            return null;
        }
    }
}
