using System;

namespace ObligatorioTecnologia;

public partial class WebPageDate : ContentPage
{
    public WebPageDate(string url)
    {
        InitializeComponent();
        webView.Source = url;

    }
}