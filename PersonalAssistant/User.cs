using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalAssistant
{
    public class User
    {
        /// <summary>
        /// User's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier for user
        /// </summary>
        public int Id
        {
            get;
            private set;
        }


        /// <summary>
        /// Create a new user.
        /// </summary>
        private User()
        {
            
        }

        /// <summary>
        /// Create a user with a name
        /// </summary>
        /// <param name="name">User's name</param>
        public User(string name) : this()
        {
            Name = name;
        }
    }
}