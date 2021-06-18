using System;
using System.ComponentModel;

namespace webapi.Models
{
    public class Customer
    {
        public enum CommunicationPreferences
        {
            None,
            Email,
            SMS
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }

        [DefaultValue(CommunicationPreferences.None)]
        public CommunicationPreferences CommunicationPreference { get; set; }
        public int Age { get; set; }
    };
}
