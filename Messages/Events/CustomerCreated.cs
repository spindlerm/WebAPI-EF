using System;
using NServiceBus;

namespace Messages
{
    public class CustomerCreated : IEvent 
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
        public int Age { get; set; }

        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }

        public CommunicationPreferences CommunicationPreference { get; set; }

    };
}
