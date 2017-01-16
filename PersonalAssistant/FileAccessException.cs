using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalAssistant
{
    public class FileAccessException : Exception
    {
        public FileAccessException(string message) : base(message)
        {
 
        }
    }
}