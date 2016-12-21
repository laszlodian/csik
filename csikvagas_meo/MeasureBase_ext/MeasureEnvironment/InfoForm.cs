using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using e77.MeasureBase.Sql;
using e77.MeasureBase.Model;
using e77.MeasureBase.Properties;

namespace e77.MeasureBase.MeasureEnvironment
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();

            if ( EnvironmentId.TheEnvironmentId == null
                || EnvironmentId.TheEnvironmentId.RoomId == null
                || EnvironmentId.TheEnvironmentId.RoomId == 0 )
            {
                _labelWorkplace.Text = Resources.NOT_SET;
            }
            else
            {
                _labelWorkplace.Text = IpThermo.IpThermo.RoomNames[EnvironmentId.TheEnvironmentId.RoomId.Value];
            }

            _tbTrace.Text = ""; //TODO_FGY show current trace
                        

            _labelPCName.Text = EnvironmentId.ComputerName;

            if (EnvironmentId.TheEnvironmentId != null)
            {
                _lbUserAccessRight.Text = e77User.TheUser.ValidAccessRights.ItemsToString();
                _labelSwVersion.Text = SwVersion.TheSwVersion.SwVersionCurrent;
            }
            else
            {
                _lbUserAccessRight.Text = "N/A";
                _labelSwVersion.Text = "N/A";
            }

            if(!e77User.LdapDenied)
                _lbUserFullName.Text = e77User.TheUser.FullName;
            else
                _lbUserFullName.Text = "N/A";
            

            if (MeasureCollectionBase.TheMeasures != null)
            {
                //show MeasureDatabase
                TreeNode mainNode = _treeDatabase.Nodes.Add(GetInfoStr(MeasureCollectionBase.TheMeasures));

                //items of main:
                AddDatabaseRoot(MeasureCollectionBase.TheMeasures, mainNode);

                //add measures
                foreach (MeasureRoot subMeas in MeasureCollectionBase.TheMeasures.Measures)
                {
                    TreeNode subMeasNode = _treeDatabase.Nodes.Add(GetInfoStr(subMeas));
                    AddDatabaseRoot(subMeas, subMeasNode);
                }
                _treeDatabase.ExpandAll();

                if (MeasureCollectionBase.TheMeasures.RoomTemperature.HasValue)
                    _labelRoomTemperature.Text = string.Format("{0}°C",
                        MeasureCollectionBase.TheMeasures.RoomTemperature.Value.ToString("F1"));
                else
                    _labelRoomTemperature.Hide();

                _lbMeasureSN.Text = MeasureCollectionBase.TheMeasures.SN == null ?
                        "N/A" : string.Format("'{0}'",  MeasureCollectionBase.TheMeasures.SN);
                _lbMeasureDate.Text = MeasureCollectionBase.TheMeasures.MeasureDate.ToString("yyyy-MM-dd HH:mm:ss");

                if (MeasureCollectionBase.TheMeasures.Sql != null
                    && MeasureCollectionBase.TheMeasures.Sql.RowId != SqlLowLevel.INVALID_ROW_ID)
                    _lbMeasureSqlId.Text = MeasureCollectionBase.TheMeasures.Sql.RowId.ToString();
                else
                    _lbMeasureSqlId.Text = "N/A";
                
                if(MeasureCollectionBase.TheMeasures is ICheckableBase)
                {
                    _lbMeasureResult.Text = (MeasureCollectionBase.TheMeasures as ICheckableBase).CheckResult.LocalizedStr();
                }
                else
                {
                    _lbMeasureResult.Hide();
                    _lbMeasureResultInfo.Hide();
                }
            }
            else
            {
                _tabPageMeasureDb.Hide();

                _labelRoomTemperature.Hide();

                _lbMeasureSN.Text = "N/A";
                _lbMeasureSqlId.Text = "N/A";
                _lbMeasureDate.Hide();
                _lbMeasureResult.Text = "N/A";

                _lbMeasureResult.Hide();
                _lbMeasureResultInfo.Hide();
 
            }
        }

        private static string GetInfoStr(object item_in)
        {
            if (item_in is ICheckableBase)
                return string.Format("{0} ({1})", item_in.ToString(), (item_in as ICheckableBase).CheckResult.LocalizedStr());
            else
                return item_in.ToString();
        }

        private static void AddDatabaseRoot(MeasureRoot measure_in, TreeNode dbNode_in)
        {
            foreach (string key in measure_in.Database_Keys)
            {
                TreeNode colKey = dbNode_in.Nodes.Add(key);
                foreach (object item in measure_in.Database_GetItems(key))
                {
                    TreeNode dataItemTree = colKey.Nodes.Add(GetInfoStr(item));

                    if (item is IChildInfo)
                        foreach (string s in (item as IChildInfo).ChildInfo)
                            dataItemTree.Nodes.Add(s);
                }
            }
        }

        private void BtnSaveTrace_Click(object sender, EventArgs e)
        {
            TraceHelper.MarkTraceFileForStore("UserRequest", true);
        }

    }
}
