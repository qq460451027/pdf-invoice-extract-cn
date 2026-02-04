using CodeTestClean.itext;
using NLog;

namespace CodeTestClean
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            PdfInvoiceProcessor reader = new PdfInvoiceProcessor(@"C:\Temp\xxxx.pdf");
            //pdf上标识区域   保存到原文件目录的文件名+"_marked.pdf"中
            reader.MarkTemplateArea();
            //读取标识区域内的文本内容
            logger.Info(reader.ReadTextByArea(reader.tagPositionDict.发票号码));
            logger.Info(reader.ReadTextByArea(reader.tagPositionDict.备注));

            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.发票号码));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.开票日期));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.销方名称));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.销方税号));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.购方名称));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.购方税号));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.价税合计));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.价税合计大写));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.备注));
            //    logger.Info(reader.ReadTextByArea(reader.tagPositionDict.明细表));

            Console.ReadKey();
        }
    }
}
