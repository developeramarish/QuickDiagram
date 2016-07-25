﻿using System;
using System.Windows.Media.Imaging;
using Codartis.SoftVis.VisualStudioIntegration.ImageExport;

namespace Codartis.SoftVis.VisualStudioIntegration.UI
{
    /// <summary>
    /// Defines the UI operations of the diagram control.
    /// </summary>
    public interface IUiServices
    {
        int FontSize { get; set; }
        Dpi ImageExportDpi { get; set; }

        void FitDiagramToView();
        void GetDiagramImage(Action<BitmapSource> imageCreatedCallback);
    }
}