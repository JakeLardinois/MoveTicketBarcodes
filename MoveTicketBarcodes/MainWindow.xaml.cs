using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;
using GenCode128;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Printing;


namespace MoveTicketBarcodes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Drawing.Printing.PrintDocument objPrintDocument;
        private const int BARCODEWEIGHT = 1;
        private const int BARCODELABELWIDTH = 150;//Barcode label width in pixels 150px = 2inches
        private const int BARCODELABELLENGTH = 300;//Barcode label length in pixels 300px = 4inches
        private const int MAXBARCODEHIEGHT = 20;
        private const int LEFTMARGIN = 5;
        private const string BARCODEHEADER = "QSF-7.1.A REV: 1-30-12";
        private const string FONTNAME = "Arial";


        public MainWindow()
        {
            InitializeComponent();

            objPrintDocument = new System.Drawing.Printing.PrintDocument();
            this.objPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.objPrintDocument_PrintPage);

            PopulateInstalledPrintersToComboBox();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            int intCopies = 0;
            bool blnIntegerEntered = (int.TryParse(txtQuantity.Text, out intCopies));

            if (cmbInstalledPrinters.Text != string.Empty && blnIntegerEntered)
            {
                objPrintDocument.PrinterSettings.PrinterName = cmbInstalledPrinters.Text;
                objPrintDocument.PrinterSettings.Copies = short.Parse(txtQuantity.Text);
                objPrintDocument.Print();
            }
            else
                if (blnIntegerEntered)
                    MessageBox.Show("You Must Select a Printer!", "Printer Selection", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                    MessageBox.Show("You didn't enter a numeric quantity to print!", "Numeric Quantity Needed", MessageBoxButton.OK, MessageBoxImage.Error);
                
            
        }

        private void objPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            using (Graphics g = e.Graphics)
            {
                using (Font fnt = new Font(FONTNAME, 16))
                {
                    StringBuilder objStringBuilder = new StringBuilder();


                    using (Font fntHeader = new Font(FONTNAME, 8))
                    {
                        g.DrawString(BARCODEHEADER, fntHeader, System.Drawing.Brushes.Black, 120, 0);
                    }

                    objStringBuilder.Append(string.Format("Item:{0}", txtItem.Text));
                    g.DrawString(objStringBuilder.ToString(), fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 10);
                    objStringBuilder.Clear();

                    /*I changed the below code to draw on a rectangle that is drawn on the label. This was the only way that I could control the hieght of the barcode. Else it would grow 
                     * wider when more data was placed in the textbox.  I also multiplied the label length by a multiplier because the barcode length that was specified was shorter than the
                     * label length specified.  So by using a multiplier, I figured that the length of barcode vs length of label would be proportional.*/
                    g.DrawImage(Code128Rendering.MakeBarcodeImage(txtItem.Text, BARCODEWEIGHT, true), new System.Drawing.Rectangle(LEFTMARGIN, 35, (int)(BARCODELABELLENGTH * .9), MAXBARCODEHIEGHT));
                    //g.DrawImage(Code128Rendering.MakeBarcodeImage(txtItem.Text, BARCODEWEIGHT, true), 5, 35);

                    objStringBuilder.Append(string.Format("Job:{0}", txtJob.Text));
                    g.DrawString(objStringBuilder.ToString(), fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 55);
                    objStringBuilder.Clear();

                    g.DrawImage(Code128Rendering.MakeBarcodeImage(txtJob.Text, BARCODEWEIGHT, true), new System.Drawing.Rectangle(LEFTMARGIN, 80, (int)(BARCODELABELLENGTH * .9), MAXBARCODEHIEGHT));
                    //g.DrawImage(Code128Rendering.MakeBarcodeImage(txtJob.Text, BARCODEWEIGHT, true), 5, 80);

                    g.DrawString("Emp#'s:________________________", fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 120);

                    g.DrawString("Qty:_________", fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 155);

                    g.DrawString("NextOper:__________", fnt, System.Drawing.Brushes.Black, 155, 155);
                }
            }
        }

        private void btnMakeBarcode_Click(object sender, RoutedEventArgs e)
        {
            /*I had to make a bitmap image in memory so that i could create a graphics object to work with.  for some reason I had to scale the image up from the width specified in pixels for the 
             * label length and width.  Even though label width and legth are given in pixels.  I'm thinking it has something to do with screen resolutions...*/
            System.Drawing.Image objImage = new Bitmap((int)(BARCODELABELWIDTH * 5), (int)(BARCODELABELLENGTH * 5));


            txtError.Text = "";

            try
            {
                using (Graphics objGraphics = Graphics.FromImage(objImage))
                {
                    //I needed to paint the background of the label white, else the background default is black.
                    objGraphics.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, (int)(BARCODELABELWIDTH * 5), (int)(BARCODELABELLENGTH * 5));

                    using (Font fnt = new Font("Arial", 8))
                    {
                        StringBuilder objStringBuilder = new StringBuilder();


                        using (Font fntHeader = new Font(FONTNAME, 8))
                        {
                            objGraphics.DrawString(BARCODEHEADER, fntHeader, System.Drawing.Brushes.Black, 120, 0);
                        }
                        
                        objStringBuilder.Append(string.Format("Item:{0}", txtItem.Text));
                        objGraphics.DrawString(objStringBuilder.ToString(), fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 10);
                        objStringBuilder.Clear();

                        objGraphics.DrawImage(Code128Rendering.MakeBarcodeImage(txtItem.Text, BARCODEWEIGHT, true), new System.Drawing.Rectangle(LEFTMARGIN, 35, (int)(BARCODELABELLENGTH * 1.3), MAXBARCODEHIEGHT));
                        //objGraphics.DrawImage(Code128Rendering.MakeBarcodeImage(txtItem.Text, BARCODEWEIGHT, true), LEFTMARGIN, 35);

                        objStringBuilder.Append(string.Format("Job:{0}", txtJob.Text));
                        objGraphics.DrawString(objStringBuilder.ToString(), fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 55);
                        objStringBuilder.Clear();

                        objGraphics.DrawImage(Code128Rendering.MakeBarcodeImage(txtJob.Text, BARCODEWEIGHT, true), new System.Drawing.Rectangle(LEFTMARGIN, 80, (int)(BARCODELABELLENGTH * 1.3), MAXBARCODEHIEGHT));
                        //objGraphics.DrawImage(Code128Rendering.MakeBarcodeImage(txtJob.Text, BARCODEWEIGHT, true), LEFTMARGIN, 80);

                        objGraphics.DrawString("Emp#'s:________________________________", fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 120);

                        objGraphics.DrawString("Qty:___________", fnt, System.Drawing.Brushes.Black, LEFTMARGIN, 145);

                        objGraphics.DrawString("NextOper:_______________", fnt, System.Drawing.Brushes.Black, 95, 145);
                    }
                }
                chkActualSize.IsChecked = true;
                imgBarcode.Source = objImage.ToBitmapImage();
            }
            catch (Exception ex)
            {
                txtError.Text = ex.ToString();
            }
        }

        private void PopulateInstalledPrintersToComboBox()
        {
            StringBuilder objStringBuilder = new StringBuilder();


            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                objStringBuilder.Append(PrinterSettings.InstalledPrinters[i]);
                cmbInstalledPrinters.Items.Add(objStringBuilder.ToString());
                objStringBuilder.Clear();
            }
        }

        private void cmbInstalledPrinters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbInstalledPrinters.SelectedIndex != -1)
            {
                objPrintDocument.PrinterSettings.PrinterName = cmbInstalledPrinters.Text;
            }
        }
    }
}
