using Microsoft.AspNetCore.Components;
using ShoopCommunication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shoop24.Administration.Components.Pages.Management
{
    public partial class UserManagement : ComponentBase
    {
        /* Ready for use */
        [Inject] private ShoopClient _client { get; set; }
        public string _searchInput = string.Empty;
        
        public UserManagement() 
        {
            
        }
        
    }
}
