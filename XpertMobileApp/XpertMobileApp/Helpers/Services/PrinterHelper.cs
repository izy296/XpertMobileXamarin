using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.DAL;
using XpertMobileApp.Views;
using XpertWebApi.Models;

namespace XpertMobileApp.Helpers.Services
{
    public class PrinterHelper
    {
        public async static  void PrintBL(List<Get_Print_VTE_TiketCaisse> data)
        {
            if (data == null) return;
            if (App.printerLocal != null && App.printerLocal.isConnected())
            {
                if (data.Count == 0) return;
     
                    App.printerLocal.setPrinter(13, 0);
                    App.printerLocal.setPrinter(13, 0);
                    App.printerLocal.setFont(0, 0, 1, 1, 0);
                    App.printerLocal.PrintText(data[0].NOM_PHARM + Environment.NewLine);
                    App.printerLocal.PrintText(data[0].ADRESSE_PHARM + Environment.NewLine);
                    App.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    App.printerLocal.setFont(0, 0, 0, 1, 0);
                    App.printerLocal.PrintText("N : " + data[0].NUM_VENTE + Environment.NewLine);
                    App.printerLocal.PrintText("Client : " + data[0].NOM_TIERS + Environment.NewLine);
                    string date = String.Format($"Date :{data[0].DATE_VENTE:dd/MM/yyyy}") + Environment.NewLine;
                    App.printerLocal.PrintText(date);
                    App.printerLocal.PrintText("Etablie par : " + data[0].CREATED_BY + Environment.NewLine);
                    App.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    App.printerLocal.setFont(0, 0, 0, 1, 0);
                    App.printerLocal.PrintText("Designation        Qte   Prix       MT " + Environment.NewLine);
                    App.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    string datvalue = "";
                    string designation = "";
                    foreach (Get_Print_VTE_TiketCaisse item in data)
                    {
                        designation = item.DESIGNATION_PRODUIT != null && item.DESIGNATION_PRODUIT.Length > 18 ? item.DESIGNATION_PRODUIT.Substring(0, 18) : item.DESIGNATION_PRODUIT;
                        datvalue = string.Format($"{designation,-18} {item.QUANTITE,-5:N1} {item.PRIX_VTE_TTC,-10:0.00} {item.MT_TTC,-12:0.00}") + Environment.NewLine;
                        App.printerLocal.PrintText(datvalue);
                    }
                    App.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    App.printerLocal.PrintText(string.Format($"                      Total : {data[0].TOTAL_TTC:0.00}") + Environment.NewLine);
                    string monyWord = Api.Services.XpertHelper.IsNullOrEmpty(data[0].MoneyWords) ? "" : data[0].MoneyWords.Replace('é', ' ');
                    App.printerLocal.PrintText(monyWord + Environment.NewLine);
                    App.printerLocal.PrintText(" " + Environment.NewLine);
                    App.printerLocal.PrintText(" " + Environment.NewLine);
            }
        }

        public async static void PrintEncaisse(List<Get_Print_ds_ViewTrsEncaiss> data)
        {
            if (data == null && data.Count>0) return;
            if (App.printerLocal != null && App.printerLocal.isConnected())
            {
                App.printerLocal.setPrinter(13, 0);
                App.printerLocal.setPrinter(13, 0);
                App.printerLocal.setFont(0, 0, 1, 1, 0);
                App.printerLocal.PrintText(data[0].NOM_PHARM + Environment.NewLine);
                App.printerLocal.PrintText(data[0].ADRESSE_PHARM + Environment.NewLine);
                App.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data[0].CODE_TYPE))
                {
                    App.printerLocal.PrintText("Recu de versement" + Environment.NewLine);
                }
                else
                {
                    App.printerLocal.PrintText("Recu de reglement" + Environment.NewLine);
                }
                App.printerLocal.setFont(0, 0, 0, 1, 0);
                App.printerLocal.PrintText("N : " + data[0].NUM_ENCAISS + Environment.NewLine);
                App.printerLocal.PrintText("Client : " + data[0].NOM_TIERS + Environment.NewLine);
                string date = String.Format($"Date :{data[0].DATE_ENCAISS:dd/MM/yyyy}") + Environment.NewLine;
                App.printerLocal.PrintText(date);
                App.printerLocal.PrintText("Etablie par : " + data[0].CREATED_BY + Environment.NewLine);
                App.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data[0].CODE_TYPE))
                {
                    App.printerLocal.PrintText($"Montant verse : {data[0].TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                else
                {
                    App.printerLocal.PrintText($"Montant regle : {data[0].TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                string monyWord = Api.Services.XpertHelper.IsNullOrEmpty(data[0].MoneyWords)?"": data[0].MoneyWords.Replace('é',' ');
                App.printerLocal.PrintText(monyWord + Environment.NewLine);
                if (data[0].AFFICHE_SOLDE == 1)
                {
                    App.printerLocal.PrintText($"Solde anterieur  : {data[0].SOLDE_ANTERIEUR,-10:0.00}" + Environment.NewLine);
                    App.printerLocal.PrintText($"Solde actuel: {data[0].SOLDE_ACTUEL,-10:0.00}" + Environment.NewLine);
                }
                App.printerLocal.PrintText(" " + Environment.NewLine);
                App.printerLocal.PrintText(" " + Environment.NewLine);
            }
        }

    }
}
