using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDS.ComLibrary;

namespace IDS.Forms
{
    public partial class frmAbout : Form
    {
        private IDSMain objMainfrm { get; set; }
        Communication _com = new Communication();
        DataSet _dsTagListXml = new DataSet();
        public frmAbout(IDSMain objfrm)
        {
            objMainfrm = objfrm;
            InitializeComponent();
            _dsTagListXml.ReadXml(Global.SysPath + "\\" + "TagList.xml");
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            try
            {
                if (Global.ProductModel == "1")
                {
                    lblProductName.Text = "IDS Batch Kilo";
                }
                if (Global.licenSeType == 0)
                {
                    lblLicenseType.Text = "Hardkey";
                }
                else
                {
                    lblLicenseType.Text = "Softkey";
                }
                lblSetupVersion.Text = Global.SetupVersion;
                float[] objImg = (float[])ReadMultipleValueFromPLC("plc_version", 1);

                //string  c = objImg.ToString();
                //int cou = c[1].Length;
                decimal plcVersion = 0.0M;
                plcVersion = Convert.ToDecimal(objImg[0]);
               
                lblProgramVersion.Text = objImg[0].ToString("N2");
            }
            catch (Exception ex)
            {
            }
        }
        private object ReadMultipleValueFromPLC(string tagName, int length)
        {
            try
            {
                DataRow[] dRow = _dsTagListXml.Tables[0].Select("Name = '" + tagName + "'");

                string area = dRow[0]["Area"].ToString();
                string address = Convert.ToString(dRow[0]["MemoryAddress"]);
                string dataType = dRow[0]["TypeName"].ToString();

                return _com.ReadMultipleValuesFromPLC(area, address, dataType, length);
            }
            catch (Exception ex)
            {
                return null;
                Global.Error_Log(ex.Message);
            }
        }
    }
}
