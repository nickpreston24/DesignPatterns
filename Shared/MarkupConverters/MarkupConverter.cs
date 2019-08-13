namespace Shared.MarkupConverters
{
    public interface IMarkupConverter
    {
        string ConvertXamlToHtml(string xamlText);

        string ConvertHtmlToXaml(string htmlText);

        string ConvertRtfToHtml(string rtfText);

        string ConvertHtmlToRtf(string htmlText);
    }

    public class MarkupConverter : IMarkupConverter
    {
        public string ConvertXamlToHtml(string xamlText) => HtmlFromXamlConverter.ConvertXamlToHtml(xamlText, false);

        public string ConvertHtmlToXaml(string htmlText) => HtmlToXamlConverter.ConvertHtmlToXaml(htmlText, true);

        public string ConvertRtfToHtml(string rtfText) => RtfToHtmlConverter.ConvertRtfToHtml(rtfText);

        public string ConvertHtmlToRtf(string htmlText) => HtmlToRtfConverter.ConvertHtmlToRtf(htmlText);
    }
}