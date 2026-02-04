using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTestClean.itext
{
    public class PdfInvoiceTagPositionDictionary
    {
        /*  rectDict = new Dictionary<string, float[]>();
            //point x1y1=30,44,    x2y2=580,100    
            rectDict.Add("备注", new float[] { 30, 44, 555, 60 });
            rectDict.Add("发票号", new float[] { 480, 410, 100, 15 });  //右上角坐标580,425
            rectDict.Add("开票日期", new float[] { });
            rectDict.Add("校验码", new float[] { 400, 370, 180, 15 }); //右上角坐标580,385*/
        public float PageWidth { get; set; }
        public float PageHeight { get; set; }
        public PdfInvoiceTagItem 发票号码 { get; set; }
        public PdfInvoiceTagItem 开票日期 { get; set; }
        public PdfInvoiceTagItem 购方名称 { get; set; }
        public PdfInvoiceTagItem 购方税号 { get; set; }
        public PdfInvoiceTagItem 销方名称 { get; set; }
        public PdfInvoiceTagItem 销方税号 { get; set; }
        public PdfInvoiceTagItem 明细表 { get; set; }
        //public PdfInvoiceTagItem 项目名称s { get; set; }
        //public PdfInvoiceTagItem 规格型号s { get; set; }
        //public PdfInvoiceTagItem 单位s { get; set; }
        //public PdfInvoiceTagItem 数量s { get; set; }
        //public PdfInvoiceTagItem 单价s { get; set; }
        //public PdfInvoiceTagItem 金额s { get; set; }
        //public PdfInvoiceTagItem 税率s { get; set; }
        //public PdfInvoiceTagItem 税额s { get; set; }
        public PdfInvoiceTagItem 价税合计 { get; set; }
        public PdfInvoiceTagItem 价税合计大写 { get; set; }
        public PdfInvoiceTagItem 备注 { get; set; }
        public PdfInvoiceTagPositionDictionary(float pageWidth, float pageHeight)
        {
            PageWidth = pageWidth;
            PageHeight = pageHeight;
            SetTagPosition();
            GenerateCurrentPdfPositionCollection();
        }
        public void SetTagPosition() {
            try
            {
                //样例发票高度为453  则
                //随便找一张发票先记录点位，默认参照点为左下角，其他参照点根据需要设置
                //活动区域                
                明细表= new PdfInvoiceTagItem() { Name = "发票明细", StartAnchorPoint = AnchorPoint.BottomLeft,EndAnchorPoint=AnchorPoint.TopLeft, X = 13, Y = 137, X1 = 582, Y1 = PageHeight - 147 };
                //上方固定区域
                //发票位置为样例发票高度453-左下角Y409=距离上边44,则实际Y坐标为PageHeight-44    结束位置为453-423=30，则实际结束Y坐标为PageHeight-30
                发票号码 = new PdfInvoiceTagItem() {Name="发票号码", StartAnchorPoint = AnchorPoint.TopLeft, X = 481, Y = PageHeight-44, X1 = 577, Y1 = PageHeight-30 };
                //开票日期为样张发票453-392=61，则实际Y坐标为PageHeight-61  结束位置为453-405=48，则实际结束Y坐标为PageHeight-48
                开票日期 = new PdfInvoiceTagItem() { Name = "开票日期", StartAnchorPoint = AnchorPoint.TopLeft, X = 480, Y =PageHeight- 61, X1 = 555, Y1 = PageHeight -48 };
                //购方名称为样张发票453-343=110，则实际Y坐标为PageHeight-110  结束位置为453-365=88，则实际结束Y坐标为PageHeight-88
                购方名称 = new PdfInvoiceTagItem() { Name = "购方名称", StartAnchorPoint = AnchorPoint.TopLeft, X = 55, Y = PageHeight - 110, X1 = 292, Y1 = PageHeight - 88  };
                //453-313=140，则实际Y坐标为PageHeight-140  结束位置为453-336=117，则实际结束Y坐标为PageHeight-117
                购方税号 = new PdfInvoiceTagItem() { Name = "购方税号", StartAnchorPoint = AnchorPoint.TopLeft, X = 153, Y =PageHeight- 140, X1 = 293, Y1 = PageHeight - 117 };
                //453-343=110，则实际Y坐标为PageHeight-110  结束位置为453-365=88，则实际结束Y坐标为PageHeight-88
                销方名称 = new PdfInvoiceTagItem() { Name = "销方名称", StartAnchorPoint = AnchorPoint.TopLeft, X = 339, Y = PageHeight - 110, X1 = 574, Y1 = PageHeight - 88 };
                销方税号= new PdfInvoiceTagItem() { Name = "销方税号", StartAnchorPoint = AnchorPoint.TopLeft, X = 437, Y = PageHeight - 140, X1 = 577, Y1 = PageHeight - 117  };
                
                //下方固定区域
                价税合计 = new PdfInvoiceTagItem() { Name = "价税合计", StartAnchorPoint = AnchorPoint.BottomLeft, X = 442, Y =102, X1 = 578, Y1 = 123 };
                价税合计大写= new PdfInvoiceTagItem() { Name = "价税合计大写", StartAnchorPoint = AnchorPoint.BottomLeft, X = 162, Y = 104, X1 =399, Y1 = 123 };
                备注= new PdfInvoiceTagItem() { Name = "备注", StartAnchorPoint = AnchorPoint.BottomLeft, X = 30, Y = 44, X1 = 585, Y1 = 103 };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PdfInvoiceTagPositionDictionary GenerateCurrentPdfPositionCollection() {
            try
            {
                //默认参照点为左下角，其他参照点根据需要设置
                //将AnchorPoint为TopLeft的根据PageWidth和PageHeight转换为左下角为参照点的,Width和Height
                //上方固定区域
                发票号码.Width = 发票号码.X1 - 发票号码.X;
                发票号码.Height = 发票号码.Y1 - 发票号码.Y;
                开票日期.Width = 开票日期.X1 - 开票日期.X;
                开票日期.Height = 开票日期.Y1 - 开票日期.Y;
                购方名称.Width = 购方名称.X1 - 购方名称.X;
                购方名称.Height = 购方名称.Y1 - 购方名称.Y;
                购方税号.Width = 购方税号.X1 - 购方税号.X;
                购方税号.Height = 购方税号.Y1 - 购方税号.Y;
                销方名称.Width = 销方名称.X1 - 销方名称.X;
                销方名称.Height = 销方名称.Y1 - 销方名称.Y;
                销方税号.Width = 销方税号.X1 - 销方税号.X;
                销方税号.Height = 销方税号.Y1 - 销方税号.Y;
                //活动表  左下开始计算，右上结束计算
                //明细表的Y坐标需要根据EndAnchorPoint来计算
                明细表.Width = 明细表.X1 - 明细表.X;
                明细表.Height = 明细表.Y1 - 明细表.Y;
                //下方固定区域
                价税合计.Width = 价税合计.X1 - 价税合计.X;
                价税合计.Height = 价税合计.Y1 - 价税合计.Y;
                价税合计大写.Width = 价税合计大写.X1 - 价税合计大写.X;
                价税合计大写.Height = 价税合计大写.Y1 - 价税合计大写.Y;
                备注.Width = 备注.X1 - 备注.X;
                备注.Height = 备注.Y1 - 备注.Y;
                return this;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class PdfInvoiceTagItem
    {
        public string Name { get; set; }
        /// <summary>
        /// 起始参照点
        /// </summary>
        public AnchorPoint StartAnchorPoint { get; set; }
        /// <summary>
        /// 结束参照点，只有会变动的区域需要设置
        /// </summary>
        public AnchorPoint EndAnchorPoint { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Text { get; set; }
    }
    /// <summary>
    /// PDF锚点/坐标参照位置   暂时未用上
    /// </summary>
    [Obsolete]
    public enum AnchorPoint
    {
        BottomLeft,
        BottomRight,        
        TopLeft,
        TopRight,
        Center
    }
}
