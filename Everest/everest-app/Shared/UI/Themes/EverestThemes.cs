using System;
using MudBlazor;
using MudBlazor.Utilities;

namespace everest_app.Shared.UI.Themes
{
    public static class EverestThemes
    {
        public static readonly MudTheme Classic = new MudTheme()
        {
            Palette = new Palette()
            {
                Primary = new MudBlazor.Utilities.MudColor("#50abd6"),
                Secondary = new MudBlazor.Utilities.MudColor("#00c2a2"),
                AppbarBackground = new MudBlazor.Utilities.MudColor("50abd6"),
            },
            PaletteDark = new Palette()
            {
                Black = "#272c34",
                White = Colors.Shades.White,
                Primary = "#50abd6",
                PrimaryContrastText = Colors.Grey.Darken3,
                Secondary = new MudBlazor.Utilities.MudColor("#00c2a2"),
                SecondaryContrastText = Colors.Grey.Darken3,
                Tertiary = "#D67A50",
                TertiaryContrastText = Colors.Grey.Darken3,
                Info = Colors.Blue.Default,
                InfoContrastText = Colors.Grey.Darken3,
                Success = Colors.Green.Accent4,
                SuccessContrastText = Colors.Grey.Darken3,
                Warning = Colors.Orange.Default,
                WarningContrastText = Colors.Grey.Darken3,
                Error = "#b73a27",
                ErrorContrastText = Colors.Grey.Darken3,
                Dark = Colors.Grey.Darken3,
                DarkContrastText = Colors.Grey.Darken3,
                TextPrimary = Colors.Shades.White,
                TextSecondary = "#00c2a2",
                TextDisabled = new MudColor(Colors.Shades.Black).SetAlpha(0.38).ToString(MudColorOutputFormats.RGBA),
                ActionDefault = new MudColor(Colors.Shades.Black).SetAlpha(0.54).ToString(MudColorOutputFormats.RGBA),
                ActionDisabled = new MudColor(Colors.Shades.Black).SetAlpha(0.26).ToString(MudColorOutputFormats.RGBA),
                ActionDisabledBackground = new MudColor(Colors.Shades.Black).SetAlpha(0.12).ToString(MudColorOutputFormats.RGBA),
                Background = Colors.Grey.Darken3,
                BackgroundGrey = Colors.Grey.Darken4,
                Surface = Colors.Grey.Darken3,
                DrawerBackground = Colors.Grey.Darken3,
                DrawerText = Colors.Shades.White,
                DrawerIcon = Colors.Shades.White,
                AppbarBackground = "#50abd6",
                AppbarText = Colors.Grey.Darken3,
                LinesDefault = new MudColor(Colors.Shades.Black).SetAlpha(0.12).ToString(MudColorOutputFormats.RGBA),
                LinesInputs = Colors.Grey.Lighten1,
                TableLines = new MudColor(Colors.Grey.Lighten2).SetAlpha(1.0).ToString(MudColorOutputFormats.RGBA),
                TableStriped = new MudColor(Colors.Shades.Black).SetAlpha(0.02).ToString(MudColorOutputFormats.RGBA),
                TableHover = new MudColor(Colors.Shades.Black).SetAlpha(0.04).ToString(MudColorOutputFormats.RGBA),
                Divider = Colors.Grey.Lighten2,
                DividerLight = new MudColor(Colors.Shades.Black).SetAlpha(0.8).ToString(MudColorOutputFormats.RGBA),
                HoverOpacity = 0.06,
                GrayDefault = Colors.Grey.Default,
                GrayLight = Colors.Grey.Lighten1,
                GrayLighter = Colors.Grey.Lighten2,
                GrayDark = Colors.Grey.Darken1,
                GrayDarker = Colors.Grey.Darken2,
                OverlayDark = new MudColor("#212121").SetAlpha(0.5).ToString(MudColorOutputFormats.RGBA),
                OverlayLight = new MudColor(Colors.Shades.White).SetAlpha(0.5).ToString(MudColorOutputFormats.RGBA),
            },
        };
    }
}

