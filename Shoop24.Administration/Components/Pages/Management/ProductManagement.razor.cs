using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Shoop24.Core.Enums;
using Shoop24.Core.Models;
using ShoopCommunication;

namespace Shoop24.Administration.Components.Pages.Management
{
    public partial class ProductManagement : ComponentBase
    {
        [Inject] public ShoopClient _client { get; set; }
        [Inject] public ISnackbar _snackbar { get; set; }
        public string _searchValue = string.Empty;
        public List<Product> _foundProducts = new List<Product>();
        public ProductManagement()
        {
            
        }

        public async Task ExecuteSearchCommand()
        {
            var result = await _client.GetProductResolver().GetProductsBySearchValueAsyncAsync(_searchValue);
            if (result.ErrorCode != ShoopErrorCode.NoError)
            {
                _snackbar.Add($"Fehler beim laden der Produkte ({result.ErrorCode})", Severity.Error);
                return;
            }
            _foundProducts = result.Result;
            await InvokeAsync(StateHasChanged);
        }
    }
}
