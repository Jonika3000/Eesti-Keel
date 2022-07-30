using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eesti_Keel
{
    internal class Words
    {
        public string word { get; set; }
        public string translate { get; set; }
        public Words(string word, string translate)
        {
            this.word = word;
            this.translate = translate;
        }
        public Words ()
        {

        }
    }
}
