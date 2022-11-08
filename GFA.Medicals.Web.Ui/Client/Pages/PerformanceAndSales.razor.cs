using GFA.Medicals.Web.Ui.Shared.Models.Sales;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace GFA.Medicals.Web.Ui.Client.Pages
{
    public partial class PerformanceAndSales
    {
        TelerikChart PieChartRef { get; set; }
        TelerikChart EuropeChartRef { get; set; }
        TelerikChart WorldChartRef { get; set; }
        TelerikChart ColumnChartRef { get; set; }
        TelerikChart AreaChartRef { get; set; }
        TelerikCircularGauge UserFirstCirclGauge { get; set; }
        TelerikCircularGauge UserSecondCirclGaugeRef { get; set; }
        TelerikCircularGauge UserThirdCirclGaugeRef { get; set; }
        TelerikCircularGauge UserFourthCirclGaugeRef { get; set; }
        private List<SalesByDateViewModel> sales = new List<SalesByDateViewModel>();
        private List<SalesByDateViewModel> salesPerformance = new List<SalesByDateViewModel>();
        public string[] xAxisItems = new string[] { "Q1", "Q2", "Q3", "Q4" };
        public string[] xAxisItemsBarFirst = new string[] { "Sofia, Bulgaria", "Berlin, Germany", "Paris, France", "Madrid, Spain" };
        public string[] xAxisItemsBarSecond = new string[] { "Moscow, Russia", "Beijing, China", "Dubai, UAE", "Tokyo, Japan" };
        public object[] AxisCrossingValue = new object[] { -10 };

        protected override async Task OnInitializedAsync()
        {
            sales = await Http.GetFromJsonAsync<List<SalesByDateViewModel>>("api/sales/getsales");
            salesPerformance = await Http.GetFromJsonAsync<List<SalesByDateViewModel>>("api/sales/getsalesperformance");
        }

        void ItemResize()
        {
            PieChartRef.Refresh();
            EuropeChartRef.Refresh();
            WorldChartRef.Refresh();
            ColumnChartRef.Refresh();
            AreaChartRef.Refresh();
            UserFirstCirclGauge.Refresh();
            UserSecondCirclGaugeRef.Refresh();
            UserThirdCirclGaugeRef.Refresh();
            UserFourthCirclGaugeRef.Refresh();
        }
    }
}
