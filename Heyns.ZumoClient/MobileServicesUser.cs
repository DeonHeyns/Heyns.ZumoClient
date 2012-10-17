using System;

namespace Heyns.ZumoClient
{
    /// <summary>
    /// Represents and authenticated Windows Azure Mobile Services authenticated user.
    /// </summary>
    public class MobileServicesUser
    {
        // only want clients to get an id we handle the creation and setting of the user id
        public string UserId { get; private set; }

        internal MobileServicesUser(string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            this.UserId = userId;
        }
    }
}
