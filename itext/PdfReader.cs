using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeTestClean.itext
{
    internal class PdfInvoiceProcessor
    {
        private string _pdfPath;
        private Logger logger = LogManager.GetCurrentClassLogger();
        public PdfInvoiceTagPositionDictionary tagPositionDict;
        private PdfPage pdfPage;
        public PdfInvoiceProcessor(string pdfPath)
        {
            _pdfPath = pdfPath;
            var outputFile = _pdfPath + "_mark.pdf";
            using (var pdfDoc = new PdfDocument(new PdfReader(_pdfPath)))
            {
                pdfPage = pdfDoc.GetPage(1);
                tagPositionDict = new PdfInvoiceTagPositionDictionary(pdfPage.GetPageSize().GetWidth(), pdfPage.GetPageSize().GetHeight());
            }
        }


        public bool MarkTemplateArea(string outputFile=null) {
            try
            {
                if(string.IsNullOrWhiteSpace(outputFile))
                    outputFile = _pdfPath.Replace(".pdf","_mark.pdf");
                using (var pdfDoc = new PdfDocument(new PdfReader(_pdfPath), new PdfWriter(outputFile)))
                {
                    var pdfPage = pdfDoc.GetPage(1);
                    tagPositionDict = new PdfInvoiceTagPositionDictionary(pdfPage.GetPageSize().GetWidth(), pdfPage.GetPageSize().GetHeight());
                    pdfPage.MarkAreaByArea(tagPositionDict.发票号码);
                    pdfPage.MarkAreaByArea(tagPositionDict.开票日期);
                    pdfPage.MarkAreaByArea(tagPositionDict.购方名称);
                    pdfPage.MarkAreaByArea(tagPositionDict.购方税号);
                    pdfPage.MarkAreaByArea(tagPositionDict.销方名称);
                    pdfPage.MarkAreaByArea(tagPositionDict.销方税号);
                    pdfPage.MarkAreaByArea(tagPositionDict.明细表);
                    pdfPage.MarkAreaByArea(tagPositionDict.价税合计);
                    pdfPage.MarkAreaByArea(tagPositionDict.价税合计大写);
                    pdfPage.MarkAreaByArea(tagPositionDict.备注);
                }
                logger.Debug($"已标识PDF已保存到 {outputFile}");

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "标记PDF区域时发生错误");
                throw;
            }
        }
        public string ReadTextByArea(float x, float y, float width, float height)
        {
            try
            {
                //get page text by point
                IEventFilter filter = new TextRegionEventFilter(new iText.Kernel.Geom.Rectangle(x, y, width, height));
                var strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.FilteredTextEventListener(
                    new iText.Kernel.Pdf.Canvas.Parser.Listener.LocationTextExtractionStrategy(), filter);
                var currentPageText = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfPage, strategy);
                return currentPageText;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string ReadTextByArea(PdfInvoiceTagItem item,bool removeTag=true)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrEmpty(_pdfPath))
                throw new InvalidOperationException("PDF未指定.");

            try
            {
                //如有需要自行优化PDF打开方式
                using (var pdfDoc = new PdfDocument(new PdfReader(_pdfPath))){ 
                    var pdfPage = pdfDoc.GetPage(1);
                    if (pdfPage == null)
                        throw new InvalidOperationException("PDF文件未包含页");

                    var txt=PdfTextExtractor.GetTextFromPage(pdfPage);
                    IEventFilter filter = new TextRegionEventFilter(new iText.Kernel.Geom.Rectangle(item.X, item.Y, item.Width, item.Height));
                    var strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), filter);
                    var currentPageText = PdfTextExtractor.GetTextFromPage(pdfPage, strategy);
                    item.Text = currentPageText.TrimStart('：');
                    return currentPageText.RemoveTag();
                }

            }
            catch (Exception ex) when (!(ex is ArgumentNullException) && !(ex is InvalidOperationException))
            {
                // 重新抛出已知的异常，包装其他可能（如IO、解析错误）
                throw new Exception($"提取文本时出错: {ex.Message}", ex);
            }
        }
    }
    public static class PdfDocumentExtensions
    {
        public static bool MarkAreaByArea(this PdfPage page, PdfInvoiceTagItem item)
        {
            try
            {
                PdfCanvas pdfCanvas = new PdfCanvas(page);

                // 3. 保存绘图状态（在设置样式之前保存，是个好习惯）
                pdfCanvas.SaveState();

                // 4. 设置方框样式：红色边框，无填充
                pdfCanvas.SetStrokeColor(ColorConstants.RED)
                         .SetLineWidth(1f);

                // 5. 定义方框区域（使用传入的参数）
                iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(item.X, item.Y, item.Width, item.Height);

                // 6. 绘制矩形框
                pdfCanvas.Rectangle(rect);
                pdfCanvas.Stroke(); // 执行描边绘制

                // 7. 在方框下方添加标签文本
                pdfCanvas.BeginText();
                pdfCanvas.SetFontAndSize(PdfFontFactory.CreateFont("C:/Windows/Fonts/simhei.ttf"), 12);
                // 将标签定位在方框左下方稍低的位置
                pdfCanvas.MoveText(rect.GetX(), rect.GetY() - 15);
                pdfCanvas.ShowText(item.Name);
                pdfCanvas.EndText();

                // 8. 恢复绘图状态
                pdfCanvas.RestoreState();
                return true;
            }
            catch (Exception ex)
            {
                // 更详细的异常处理，建议记录日志或抛出更具体的自定义异常
                throw new InvalidOperationException($"标记PDF区域时发生错误: {ex.Message}", ex);
            }
            finally
            {

            }
        }
        public static string RemoveTag(this string input,bool debugLog=false) {
            string result = Regex.Replace(input.Trim(), @"^[\u4e00-\u9fa5].*?[：:]", "");
            result = Regex.Replace(result, @"\n[\u4e00-\u9fa5].*?[：:]", "").Trim('\n').Replace("\n","");
            if (debugLog) { 
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Debug($"正则替换标签  {input}=>{result} " );
            }
            return result;
        }
    }
}
