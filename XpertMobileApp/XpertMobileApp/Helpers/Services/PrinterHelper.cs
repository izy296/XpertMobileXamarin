using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Views;
using XpertWebApi.Models;

namespace XpertMobileSettingsPage.Helpers.Services
{
    public class PrinterHelper
    {
        public async static  void PrintBL(List<Get_Print_VTE_TiketCaisse> data)
        {
            if (data == null) return;
            if (SettingsPage.printerLocal != null && SettingsPage.printerLocal.isConnected())
            {
                if (data.Count == 0) return;

                    SettingsPage.printerLocal.setPrinter(13, 0);
                    SettingsPage.printerLocal.setPrinter(13, 0);
                    SettingsPage.printerLocal.setFont(0, 0, 1, 1, 0);
                    SettingsPage.printerLocal.PrintText(data[0].NOM_PHARM + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText(data[0].ADRESSE_PHARM + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    SettingsPage.printerLocal.setFont(0, 0, 0, 1, 0);
                    SettingsPage.printerLocal.PrintText("N : " + data[0].NUM_VENTE + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText("Client : " + data[0].NOM_TIERS + Environment.NewLine);
                    string date = String.Format($"Date :{data[0].DATE_VENTE:dd/MM/yyyy}") + Environment.NewLine;
                    SettingsPage.printerLocal.PrintText(date);
                    SettingsPage.printerLocal.PrintText("Etablie par : " + data[0].CREATED_BY + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    SettingsPage.printerLocal.setFont(0, 0, 0, 1, 0);
                    SettingsPage.printerLocal.PrintText("Designation        Qte   Prix       MT " + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    string datvalue = "";
                    string designation = "";
                    foreach (Get_Print_VTE_TiketCaisse item in data)
                    {
                        designation = item.DESIGNATION_PRODUIT != null && item.DESIGNATION_PRODUIT.Length > 18 ? item.DESIGNATION_PRODUIT.Substring(0, 18) : item.DESIGNATION_PRODUIT;
                        datvalue = string.Format($"{designation,-18} {item.QUANTITE,-5:N1} {item.PRIX_VTE_TTC,-10:0.00} {item.MT_TTC,-12:0.00}") + Environment.NewLine;
                        SettingsPage.printerLocal.PrintText(datvalue);
                    }
                    SettingsPage.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText(string.Format($"                      Total : {data[0].TOTAL_TTC:0.00}") + Environment.NewLine);
                    string monyWord = XpertHelper.IsNullOrEmpty(data[0].MoneyWords) ? "" : data[0].MoneyWords.Replace('é', 'e');
                    SettingsPage.printerLocal.PrintText(monyWord + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText(" " + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText(" " + Environment.NewLine);
            }
        }

        public async static void PrintEncaisse(List<Get_Print_ds_ViewTrsEncaiss> data)
        {
            if (data == null && data.Count>0) return;
            if (SettingsPage.printerLocal != null && SettingsPage.printerLocal.isConnected())
            {
                SettingsPage.printerLocal.setPrinter(13, 0);
                SettingsPage.printerLocal.setPrinter(13, 0);
                SettingsPage.printerLocal.setFont(0, 0, 1, 1, 0);
                SettingsPage.printerLocal.PrintText(data[0].NOM_PHARM + Environment.NewLine);
                SettingsPage.printerLocal.PrintText(data[0].ADRESSE_PHARM + Environment.NewLine);
                SettingsPage.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data[0].CODE_TYPE))
                {
                    SettingsPage.printerLocal.PrintText("Recu de versement" + Environment.NewLine);
                }
                else
                {
                    SettingsPage.printerLocal.PrintText("Recu de reglement" + Environment.NewLine);
                }
                SettingsPage.printerLocal.setFont(0, 0, 0, 1, 0);
                SettingsPage.printerLocal.PrintText("N : " + data[0].NUM_ENCAISS + Environment.NewLine);
                SettingsPage.printerLocal.PrintText("Client : " + data[0].NOM_TIERS + Environment.NewLine);
                string date = String.Format($"Date :{data[0].DATE_ENCAISS:dd/MM/yyyy}") + Environment.NewLine;
                SettingsPage.printerLocal.PrintText(date);
                SettingsPage.printerLocal.PrintText("Etablie par : " + data[0].CREATED_BY + Environment.NewLine);
                SettingsPage.printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data[0].CODE_TYPE))
                {
                    SettingsPage.printerLocal.PrintText($"Montant verse : {data[0].TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                else
                {
                    SettingsPage.printerLocal.PrintText($"Montant regle : {data[0].TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                string monyWord = XpertHelper.IsNullOrEmpty(data[0].MoneyWords)?"": data[0].MoneyWords.Replace('é','e');
                SettingsPage.printerLocal.PrintText(monyWord + Environment.NewLine);
                if (data[0].AFFICHE_SOLDE == 1)
                {
                    SettingsPage.printerLocal.PrintText($"Solde anterieur  : {data[0].SOLDE_ANTERIEUR,-10:0.00}" + Environment.NewLine);
                    SettingsPage.printerLocal.PrintText($"Solde actuel: {data[0].SOLDE_ACTUEL,-10:0.00}" + Environment.NewLine);
                }
                SettingsPage.printerLocal.PrintText(" " + Environment.NewLine);
                SettingsPage.printerLocal.PrintText(" " + Environment.NewLine);
            }
        }

    }
}
