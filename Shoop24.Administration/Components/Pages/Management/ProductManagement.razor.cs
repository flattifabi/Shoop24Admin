using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Shoop24.Administration.Components.Dialogs;
using Shoop24.Core.Enums;
using Shoop24.Core.Models;
using ShoopCommunication;

namespace Shoop24.Administration.Components.Pages.Management
{
    public partial class ProductManagement : ComponentBase
    {
        [Inject] public ShoopClient _client { get; set; }
        [Inject] public ISnackbar _snackbar { get; set; }
        [Inject] public IDialogService _dialogService { get; set; }
        public bool _isLoading = false;
        public string _searchValue = string.Empty;
        public List<Product> _foundProducts = new List<Product>();
        public ProductManagement()
        {
            
        }
        
        public async Task ExecuteSearchCommand()
        {
            _isLoading = true;
            await InvokeAsync(StateHasChanged);
            var result = await _client.GetProductResolver().GetProductsBySearchValueAsyncAsync(_searchValue);
            if (result.ErrorCode != ShoopErrorCode.NoError)
            {
                _snackbar.Add($"Fehler beim laden der Produkte ({result.ErrorCode})", Severity.Error);
                return;
            }

            _isLoading = false;
            _foundProducts = result.Result;
            await InvokeAsync(StateHasChanged);
        }

        private async Task EditProduct(Product product)
        {
            var settings = new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Large
            };
            var dialogParameters = new DialogParameters()
            {
                ["Message"] = "Bearbeite das Produkt",
                ["Product"] = product
            };
            var dialogReference = await _dialogService.ShowAsync<EditProductDialog>("", dialogParameters);
            var result = await dialogReference.Result;
            if (!result.Canceled)
            {
                try
                {
                    var data = (Product)result.Data;
                    if (data == null)
                    {
                        _snackbar.Add("Daten konnten nicht geändert werden", Severity.Error);
                        return;
                    }

                    var changeResult = await _client.GetProductResolver().UpdateProductDetailsAsync(product.Id, data);
                    if (changeResult.ErrorCode != ShoopErrorCode.NoError)
                    {
                        _snackbar.Add("Produkt konnte nicht aktualisiert werden", Severity.Error);
                        return;
                    }

                    _snackbar.Add("Produkt wurde erfolgreich aktualisiert", Severity.Success);
                }
                catch (Exception e)
                {
                    
                }
            }
        }
    }
}
