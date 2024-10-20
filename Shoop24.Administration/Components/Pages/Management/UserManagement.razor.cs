using Microsoft.AspNetCore.Components;
using Shoop24.Administration.Services;
using ShoopCommunication;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shoop24.Administration.Components.Pages.Management
{
    public partial class UserManagement : ComponentBase
    {
        private string _searchInput = string.Empty;
        
        public UserManagement() 
        {
            ShoopFactory.GetUserResolver().Methode();
        }
        public async Task dijdii()
        {
            ShoopClient client = new ShoopClient(null);

            var result =  await client.GetBusinessResolver().GetBusinessAccountsAsync();
            _ = result.Result.First().SupermarketOwners;
        }
    }
}
