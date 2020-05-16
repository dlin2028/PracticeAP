using Microsoft.Extensions.Localization.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeAP.Data
{
    public class QuestionInfo
    {
        public string CourseName;
        public string[] Tags;
        public int Year;
        public int QuestionNumber;

        public QuestionInfo(string examName)
        {
            CourseName = examName;
        }
    }
}
