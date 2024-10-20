using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic.CompilerServices;
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

        private string[] searchValues = new[] { "Bier", "Fleisch", "Wurst", "Nudeln" };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Random r = new Random();
                var searchValue = searchValues[r.Next(searchValues.Count())];
                _isLoading = true;
                await InvokeAsync(StateHasChanged);
                var result = await _client.GetProductResolver().GetProductsBySearchValueAsyncAsync(searchValue);
                if (result.ErrorCode != ShoopErrorCode.NoError)
                {
                    _snackbar.Add($"Fehler beim laden der Produkte ({result.ErrorCode})", Severity.Error);
                    return;
                }

                _isLoading = false;
                _foundProducts = result.Result;
                await InvokeAsync(StateHasChanged);
            }
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
            if (result != null && !result.Canceled)
            {
                var data = (Product)result.Data!;
                var changeResult = await _client.GetProductResolver().UpdateProductDetailsAsync(product.Id, data);
                if (changeResult != null && changeResult.ErrorCode != ShoopErrorCode.NoError)
                {
                    _snackbar.Add("Produkt konnte nicht aktualisiert werden", Severity.Error);
                    return;
                }

                _snackbar.Add("Produkt wurde erfolgreich aktualisiert", Severity.Success);
                
            }
        }

        private async Task DeleteProduct(Product product)
        {
            var settings = new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Large
            };
            var dialogParameters = new DialogParameters()
            {
                ["Message"] = $"Möchtes du das Product {product.Name} wirklich löschen?",
            };
            var dialogReference = await _dialogService.ShowAsync<AskDialog>("", dialogParameters);
            var result = await dialogReference.Result;
            if (!result.Canceled)
            {
                var deleteResult = await _client.GetProductResolver().DeleteProductByIdAsync(product.Id);
                if (deleteResult.ErrorCode != ShoopErrorCode.NoError)
                {
                    _snackbar.Add($"Produkt konnte nicht gelöscht werden ({deleteResult.ErrorCode})", Severity.Error);
                    return;
                }

                _snackbar.Add($"Produkt {product.Name} wurde erfolgreich gelöscht", Severity.Success);
                _foundProducts.Remove(product);
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
