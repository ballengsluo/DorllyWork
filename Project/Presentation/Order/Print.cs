using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.IO;
using System.Web;

namespace project.Presentation.Order
{
    public static class WOPrint
    {
        public static string Path = HttpContext.Current.Server.MapPath("~/pdf") + "/";   
        static BaseFont bf = BaseFont.CreateFont(@"c:\Windows\fonts\SURSONG.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        static Font font20 = new Font(bf, 20, Font.BOLD);
        static Font font10 = new Font(bf, 10, Font.NORMAL);
  
        public static void Print(Document doc, Entity.Order.EntityWorkOrder bc)
        {
            try
            {
                PdfPTable Tit = new PdfPTable(1);
                Tit.DefaultCell.Padding = 3;
                float[] wid = { 1 };
                Tit.SetWidths(wid);
                Tit.WidthPercentage = 100;
                PdfPCell cell1 = new PdfPCell(new Paragraph("维修单", font20));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell1.Border = Rectangle.NO_BORDER;
                cell1.FixedHeight = 60;
                Tit.AddCell(cell1);
                doc.Add(Tit);

                PdfPTable PT1 = new PdfPTable(3);
                PT1.DefaultCell.Padding = 0;
                float[] wid1 = { 3, 2, 2 };
                PT1.SetWidths(wid1);
                PT1.WidthPercentage = 100;
                PT1.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell cell11 = new PdfPCell(new Paragraph("工单类型：" + bc.OrderTypeName, font10));
                cell11.HorizontalAlignment = Element.ALIGN_LEFT;
                cell11.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell11.Border = Rectangle.NO_BORDER;
                cell11.FixedHeight = 24;

                PdfPCell cell12 = new PdfPCell(new Paragraph(bc.OrderDate.ToString(" yyyy 年 MM 月 dd 日"), font10));
                cell12.HorizontalAlignment = Element.ALIGN_LEFT;
                cell12.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell12.Border = Rectangle.NO_BORDER;

                PdfPCell cell13 = new PdfPCell(new Paragraph("表单编号：" + bc.OrderNo, font10));
                cell13.HorizontalAlignment = Element.ALIGN_LEFT;
                cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell13.Border = Rectangle.NO_BORDER;

                PT1.AddCell(cell11);
                PT1.AddCell(cell12);
                PT1.AddCell(cell13);
                doc.Add(PT1);
                

                PdfPTable PT2 = new PdfPTable(8);
                PT2.DefaultCell.Padding = 0;
                float[] wid2 = { 5, 5, 3, 2, 3, 2, 2, 2 };
                PT2.SetWidths(wid2);
                PT2.WidthPercentage = 100;
                PT2.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell cell21 = new PdfPCell(new Paragraph("报修单位", font10));
                cell21.HorizontalAlignment = Element.ALIGN_CENTER;
                cell21.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell21.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell21.FixedHeight = 24;
                PdfPCell cell22 = new PdfPCell(new Paragraph(bc.CustName, font10));
                cell22.HorizontalAlignment = Element.ALIGN_LEFT;
                cell22.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell22.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell23 = new PdfPCell(new Paragraph("地点", font10));
                cell23.HorizontalAlignment = Element.ALIGN_CENTER;
                cell23.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell23.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell24 = new PdfPCell(new Paragraph(bc.Addr, font10));
                cell24.HorizontalAlignment = Element.ALIGN_LEFT;
                cell24.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell24.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell24.Colspan = 2;
                PdfPCell cell25 = new PdfPCell(new Paragraph("联系电话", font10));
                cell25.HorizontalAlignment = Element.ALIGN_CENTER;
                cell25.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell25.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell26 = new PdfPCell(new Paragraph(bc.LinkTel, font10));
                cell26.HorizontalAlignment = Element.ALIGN_LEFT;
                cell26.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell26.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell26.Colspan = 2;
                PT2.AddCell(cell21);
                PT2.AddCell(cell22);
                PT2.AddCell(cell23);
                PT2.AddCell(cell24);
                PT2.AddCell(cell25);
                PT2.AddCell(cell26);

                PdfPCell cell31 = new PdfPCell(new Paragraph("报修内容", font10));
                cell31.HorizontalAlignment = Element.ALIGN_CENTER;
                cell31.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell31.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell31.FixedHeight = 36;
                PdfPCell cell32 = new PdfPCell(new Paragraph(bc.OrderName, font10));
                cell32.HorizontalAlignment = Element.ALIGN_LEFT;
                cell32.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell32.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell32.Colspan = 7;
                PT2.AddCell(cell31);
                PT2.AddCell(cell32);


                PdfPCell cell41 = new PdfPCell(new Paragraph("报修时间", font10));
                cell41.HorizontalAlignment = Element.ALIGN_CENTER;
                cell41.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell41.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell41.FixedHeight = 24;
                PdfPCell cell42 = new PdfPCell(new Paragraph(bc.OrderDate.ToString("yyyy/MM/dd HH:mm"), font10));
                cell42.HorizontalAlignment = Element.ALIGN_LEFT;
                cell42.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell42.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell43 = new PdfPCell(new Paragraph("预约时间", font10));
                cell43.HorizontalAlignment = Element.ALIGN_CENTER;
                cell43.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell43.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell43.Colspan = 2;

                string appotime = "";
                if (bc.AppoIntTime.Year > 1901)
                    appotime = bc.AppoIntTime.ToString("yyyy/MM/dd HH:mm");
                PdfPCell cell44 = new PdfPCell(new Paragraph(appotime, font10));
                cell44.HorizontalAlignment = Element.ALIGN_LEFT;
                cell44.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell44.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell45 = new PdfPCell(new Paragraph("接单时间", font10));
                cell45.HorizontalAlignment = Element.ALIGN_CENTER;
                cell45.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell45.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;

                string resptime = "";
                if (bc.ResponseTime.Year > 1901)
                    resptime = bc.ResponseTime.ToString("yyyy/MM/dd HH:mm");
                PdfPCell cell46 = new PdfPCell(new Paragraph(resptime, font10));
                cell46.HorizontalAlignment = Element.ALIGN_LEFT;
                cell46.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell46.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell46.Colspan = 2;
                PT2.AddCell(cell41);
                PT2.AddCell(cell42);
                PT2.AddCell(cell43);
                PT2.AddCell(cell44);
                PT2.AddCell(cell45);
                PT2.AddCell(cell46);
                

                PdfPCell cell51 = new PdfPCell(new Paragraph("派单人", font10));
                cell51.HorizontalAlignment = Element.ALIGN_CENTER;
                cell51.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell51.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell51.FixedHeight = 24;
                PdfPCell cell52 = new PdfPCell(new Paragraph(bc.AlloUserName, font10));
                cell52.HorizontalAlignment = Element.ALIGN_LEFT;
                cell52.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell52.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell53 = new PdfPCell(new Paragraph("接单人", font10));
                cell53.HorizontalAlignment = Element.ALIGN_CENTER;
                cell53.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell53.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell53.Colspan = 2;
                PdfPCell cell54 = new PdfPCell(new Paragraph(bc.PersonName, font10));
                cell54.HorizontalAlignment = Element.ALIGN_LEFT;
                cell54.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell54.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell55 = new PdfPCell(new Paragraph("开工时间", font10));
                cell55.HorizontalAlignment = Element.ALIGN_CENTER;
                cell55.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell55.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell56 = new PdfPCell(new Paragraph("", font10));
                cell56.HorizontalAlignment = Element.ALIGN_LEFT;
                cell56.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell56.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell56.Colspan = 2;
                PT2.AddCell(cell51);
                PT2.AddCell(cell52);
                PT2.AddCell(cell53);
                PT2.AddCell(cell54);
                PT2.AddCell(cell55);
                PT2.AddCell(cell56);
                

                PdfPCell cell61 = new PdfPCell(new Paragraph("序号", font10));
                cell61.HorizontalAlignment = Element.ALIGN_CENTER;
                cell61.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell61.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell61.FixedHeight = 24;
                PdfPCell cell62 = new PdfPCell(new Paragraph("维修材料（项目）名称", font10));
                cell62.HorizontalAlignment = Element.ALIGN_CENTER;
                cell62.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell62.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell63 = new PdfPCell(new Paragraph("型号规格", font10));
                cell63.HorizontalAlignment = Element.ALIGN_CENTER;
                cell63.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell63.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell64 = new PdfPCell(new Paragraph("数量", font10));
                cell64.HorizontalAlignment = Element.ALIGN_CENTER;
                cell64.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell64.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell65 = new PdfPCell(new Paragraph("提供方", font10));
                cell65.HorizontalAlignment = Element.ALIGN_CENTER;
                cell65.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell65.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell66 = new PdfPCell(new Paragraph("材料费", font10));
                cell66.HorizontalAlignment = Element.ALIGN_CENTER;
                cell66.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell66.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell67 = new PdfPCell(new Paragraph("服务费", font10));
                cell67.HorizontalAlignment = Element.ALIGN_CENTER;
                cell67.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell67.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell68 = new PdfPCell(new Paragraph("备注", font10));
                cell68.HorizontalAlignment = Element.ALIGN_CENTER;
                cell68.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell68.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PT2.AddCell(cell61);
                PT2.AddCell(cell62);
                PT2.AddCell(cell63);
                PT2.AddCell(cell64);
                PT2.AddCell(cell65);
                PT2.AddCell(cell66);
                PT2.AddCell(cell67);
                PT2.AddCell(cell68);


                PdfPCell cell71 = new PdfPCell(new Paragraph("", font10));
                cell71.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell71.FixedHeight = 24;
                PdfPCell cell72 = new PdfPCell(new Paragraph("", font10));
                cell72.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell73 = new PdfPCell(new Paragraph("", font10));
                cell73.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell74 = new PdfPCell(new Paragraph("", font10));
                cell74.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell75 = new PdfPCell(new Paragraph("", font10));
                cell75.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell76 = new PdfPCell(new Paragraph("", font10));
                cell76.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell77 = new PdfPCell(new Paragraph("", font10));
                cell77.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell78 = new PdfPCell(new Paragraph("", font10));
                cell78.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PT2.AddCell(cell71);
                PT2.AddCell(cell72);
                PT2.AddCell(cell73);
                PT2.AddCell(cell74);
                PT2.AddCell(cell75);
                PT2.AddCell(cell76);
                PT2.AddCell(cell77);
                PT2.AddCell(cell78);

                PT2.AddCell(cell71);
                PT2.AddCell(cell72);
                PT2.AddCell(cell73);
                PT2.AddCell(cell74);
                PT2.AddCell(cell75);
                PT2.AddCell(cell76);
                PT2.AddCell(cell77);
                PT2.AddCell(cell78);

                PT2.AddCell(cell71);
                PT2.AddCell(cell72);
                PT2.AddCell(cell73);
                PT2.AddCell(cell74);
                PT2.AddCell(cell75);
                PT2.AddCell(cell76);
                PT2.AddCell(cell77);
                PT2.AddCell(cell78);

                PT2.AddCell(cell71);
                PT2.AddCell(cell72);
                PT2.AddCell(cell73);
                PT2.AddCell(cell74);
                PT2.AddCell(cell75);
                PT2.AddCell(cell76);
                PT2.AddCell(cell77);
                PT2.AddCell(cell78);


                PdfPCell cell81 = new PdfPCell(new Paragraph("材料、服务费合计", font10));
                cell81.HorizontalAlignment = Element.ALIGN_CENTER;
                cell81.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell81.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell81.FixedHeight = 24;
                PdfPCell cell82 = new PdfPCell(new Paragraph("             佰            拾            元            角            分 （¥：          元）         □现金      □转账", font10));
                cell82.HorizontalAlignment = Element.ALIGN_LEFT;
                cell82.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell82.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell82.Colspan = 7;
                PT2.AddCell(cell81);
                PT2.AddCell(cell82);

                
                PdfPCell cell91 = new PdfPCell(new Paragraph("维修处理情况", font10));
                cell91.HorizontalAlignment = Element.ALIGN_CENTER;
                cell91.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell91.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell91.FixedHeight = 84;
                cell91.Rowspan = 3;
                PdfPCell cell92 = new PdfPCell(new Paragraph("", font10));
                cell92.HorizontalAlignment = Element.ALIGN_LEFT;
                cell92.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell92.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell92.Rowspan = 3;
                PdfPCell cell93 = new PdfPCell(new Paragraph("用户意见", font10));
                cell93.HorizontalAlignment = Element.ALIGN_CENTER;
                cell93.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell93.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell93.Rowspan = 3;
                PdfPCell cell94 = new PdfPCell(new Paragraph("□满意    □基本满意    □不满意", font10));
                cell94.HorizontalAlignment = Element.ALIGN_LEFT;
                cell94.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell94.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell94.Colspan = 5;
                cell94.FixedHeight = 24;
                PdfPCell cell95 = new PdfPCell(new Paragraph("不满意原因：", font10));
                cell95.HorizontalAlignment = Element.ALIGN_LEFT;
                cell95.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell95.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell95.FixedHeight = 30;
                cell95.Colspan = 5;
                PdfPCell cell96 = new PdfPCell(new Paragraph("客户签名：", font10));
                cell96.HorizontalAlignment = Element.ALIGN_LEFT;
                cell96.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell96.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell96.FixedHeight = 30;
                cell96.Colspan = 5;

                PT2.AddCell(cell91);
                PT2.AddCell(cell92);
                PT2.AddCell(cell93);
                PT2.AddCell(cell94);
                PT2.AddCell(cell95);
                PT2.AddCell(cell96);
                


                PdfPCell cell101 = new PdfPCell(new Paragraph("完工时间", font10));
                cell101.HorizontalAlignment = Element.ALIGN_CENTER;
                cell101.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell101.Border = Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                cell101.FixedHeight = 27;
                PdfPCell cell102 = new PdfPCell(new Paragraph("", font10));
                cell102.HorizontalAlignment = Element.ALIGN_LEFT;
                cell102.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell102.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell103 = new PdfPCell(new Paragraph("财务签字", font10));
                cell103.HorizontalAlignment = Element.ALIGN_CENTER;
                cell103.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell103.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell104 = new PdfPCell(new Paragraph("", font10));
                cell104.HorizontalAlignment = Element.ALIGN_LEFT;
                cell104.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell104.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell105 = new PdfPCell(new Paragraph("客服签字", font10));
                cell105.HorizontalAlignment = Element.ALIGN_CENTER;
                cell105.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell105.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell106 = new PdfPCell(new Paragraph("", font10));
                cell106.HorizontalAlignment = Element.ALIGN_LEFT;
                cell106.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell106.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell107 = new PdfPCell(new Paragraph("工程签字", font10));
                cell107.HorizontalAlignment = Element.ALIGN_CENTER;
                cell107.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell107.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PdfPCell cell108 = new PdfPCell(new Paragraph("", font10));
                cell108.HorizontalAlignment = Element.ALIGN_LEFT;
                cell108.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell108.Border = Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                PT2.AddCell(cell101);
                PT2.AddCell(cell102);
                PT2.AddCell(cell103);
                PT2.AddCell(cell104);
                PT2.AddCell(cell105);
                PT2.AddCell(cell106);
                PT2.AddCell(cell107);
                PT2.AddCell(cell108);

                doc.Add(PT2);
            }
            catch { }
        }
        

    }

}
