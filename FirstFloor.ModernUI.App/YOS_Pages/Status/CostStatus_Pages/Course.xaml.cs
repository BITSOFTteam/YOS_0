﻿using System.Windows.Controls;
using System;
using System.Windows;

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Linq;

namespace FirstFloor.ModernUI.App.YOS_Pages.Status.CostStatus_Pages
{

	public partial class Course: UserControl
    {
        private string strOraConn = "Data Source=ORCL;User Id=bitsoft;Password=bitsoft_";
        //private string strOraConn = "Data Source=MYORACLE;User Id=dba_soo;Password=tnalsl";
        //private OracleConnection Con = new OracleConnection();
        private DataSet LECTURE_DS = new DataSet("LECTURE_DS");
        private OracleCommandBuilder oraBuilder;
        private OracleDataAdapter Adpt;

        public Course()
        {
            InitializeComponent();

            #region 데이터 가져오기 및 DataGrid에 추가

            Adpt = new OracleDataAdapter("SELECT * FROM LECTURE", strOraConn);

            DataTable PERSON_dt = LECTURE_DS.Tables["LECTURE_dt"];

            oraBuilder = new OracleCommandBuilder(Adpt);

            Adpt.Fill(LECTURE_DS, "LECTURE_dt");
            DG_ST_C.ItemsSource = LECTURE_DS.Tables["LECTURE_dt"].DefaultView;
            CB_ST_C.ItemsSource = LECTURE_DS.Tables["LECTURE_dt"].DefaultView;
            DG_ST_C.CanUserAddRows = false;

            #endregion
        }

        private void CB_ST_C_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DG_ST_C.SelectedIndex = CB_ST_C.SelectedIndex;
        }

        private void Total_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //string tot = Convert.ToString(Total.Text);
                long tmoney = 0;
                //bool converter;
                string tot2 = string.Format("{0:F0}", Total.Text);
                long.TryParse(tot2, out tmoney);

			char[] charToTrim = { ',' };
			string result = Total.Text.Trim(charToTrim);
			double.TryParse(result, out totalMoney);

                Money.Text = string.Format("{0}", money.ToString("n0"));
                Commission.Text = string.Format("{0}", comm.ToString("n0"));
                BusinessExpenses.Text = string.Format("{0}", bexp.ToString("n0"));
                MembershipFees.Text = string.Format("{0}", mfee.ToString("n0"));
                CorporationProfit.Text = string.Format("{0}", cpro.ToString("n0"));
            }
            catch
            {

            }
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            searchDate();
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            searchDate();

            if (EndDate.SelectedDate <= StartDate.SelectedDate)
            {
                MessageBox.Show("시작일보다 빠름! 시작일 이후로 다시 선택해 주십시오.");
                EndDate.SelectedDate = null;
            }
        }

        private void searchDate()
        {
            string sd = String.Format("{0:yy-MM-dd}", StartDate.SelectedDate);
            string ed = String.Format("{0:yy-MM-dd}", EndDate.SelectedDate);

            string dynamicquery = "";

            if (sd != "" && ed != "")
            {

            }

            if (sd != "")
            {
                dynamicquery += "CLOSEDATE >= to_date('" + sd + "', 'yy-mm-dd')";
            }

            if (ed != "")
            {
                if (dynamicquery != "")
                    dynamicquery += " and ";
                else
                    dynamicquery += " ";

                dynamicquery += "STARTDATE <= to_date('" + ed + "', 'yy-mm-dd')";
            }

            if (dynamicquery != "")
                dynamicquery = " where " + dynamicquery;

            Adpt = new OracleDataAdapter("SELECT * FROM LECTURE" + dynamicquery, strOraConn);
            DataTable LECTURE_dt = LECTURE_DS.Tables["LECTURE_dt"];

            DG_ST_C.ItemsSource = null;
            CB_ST_C.ItemsSource = null;

            LECTURE_dt.Clear();

            oraBuilder = new OracleCommandBuilder(Adpt);

            Adpt.Fill(LECTURE_DS, "LECTURE_dt");
            DG_ST_C.ItemsSource = LECTURE_DS.Tables["LECTURE_dt"].DefaultView;
            CB_ST_C.ItemsSource = LECTURE_DS.Tables["LECTURE_dt"].DefaultView;
            DG_ST_C.CanUserAddRows = false;
        }
    }
}
