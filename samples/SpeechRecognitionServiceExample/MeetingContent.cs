using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTextWPFSample
{
    class myContent
    {
    public    static List<String> myContents = new List<string>();

        public static String getStrings() {
            String temp=String.Empty ;

            for (int i = 0; i < myContents.Count; i++) {
                if(i>0)
                    temp +=  "\n" + myContents[i];
                else
                   
                temp +=  myContents[i];

            }
         
            return temp;
           
        }
    }
        }
