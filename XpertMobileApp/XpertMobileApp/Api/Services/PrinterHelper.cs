using Acr.UserDialogs;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert.Common;
using XpertMobileApp;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.SQLite_Managment;
using XpertMobileApp.Views;
using XpertMobileApp.Views.Helper;
using XpertWebApi.Models;

namespace XpertMobileSettingsPage.Helpers.Services
{
    public class PrinterHelper
    {
        public static bool printerReady = false;
        private static IPrinterSPRT printerLocal = DependencyService.Get<IPrinterSPRT>();
        private static readonly IBlueToothService _blueToothService = DependencyService.Get<IBlueToothService>();

        private static void updateConnected()
        {
            printerReady = printerLocal != null;
        }
        async static void eventConnecedUpdate(object sender, EventArgs e)
        {
            updateConnected();
        }

        public static List<XPrinter> GetBluetoothPrinters()
        {
            List<XPrinter> DeviceList = new List<XPrinter>();
            try
            {
                // Bluetooth printer
                var list = _blueToothService.GetDeviceList();
                
                DeviceList.Add(new XPrinter()
                {
                    Name = "",
                    Type = Printer_Type.Bluetooth
                }
                    );
                foreach (var item in list)
                {
                    XPrinter itm = new XPrinter()
                    {
                        Name = item,
                        Type = Printer_Type.Bluetooth
                    };
                    DeviceList.Add(itm);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
            }
            return DeviceList;
        }

        public static async Task GetPrinterInstance()
        {
            try
            {
                if (!printerLocal.IsInstanceReady() || Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    string printerToUse = App.Settings.PrinterName;

                    if (Constants.AppName == Apps.X_DISTRIBUTION)
                    {
                        var Liste = GetBluetoothPrinters();
                        var popupPrinter = new MultiPrinterSelector(Liste);
                        await PopupNavigation.Instance.PushAsync(popupPrinter);
                        var resPop = await popupPrinter.PopupClosedTask;
                        if (resPop != "Null")
                            printerToUse = resPop;

                        bool succes = printerLocal.GetPrinterInstance(eventConnecedUpdate, printerToUse);

                        if (!succes)
                        {
                            throw new Exception("Échec de la connexion à l'imprimante !");
                        }
                        else
                        {
                            if (!printerLocal.isConnected())
                            {
                                printerLocal.openConnection();
                            }
                        }
                        return;
                    }
                    else if (App.Settings.EnableMultiPrinter)
                    {
                        List<XPrinter> Liste;
                        if (Manager.isJson(App.Settings.MultiPrinterList))
                        {
                            Liste = Newtonsoft.Json.JsonConvert.DeserializeObject<List<XPrinter>>(App.Settings.MultiPrinterList);

                            if (Liste != null && Liste.Count != 0)
                            {
                                var popupPrinter = new MultiPrinterSelector(Liste);
                                PopupNavigation.Instance.PushAsync(popupPrinter);
                                var resPop = popupPrinter.PopupClosedTask;
                                if (resPop.Result != "Null")
                                    printerToUse = resPop.Result;
                            }
                            else Application.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_Msg_List_Impremant_Vide, AppResources.alrt_msg_Ok);

                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert(AppResources.alrt_msg_Alert, AppResources.txt_Msg_List_Impremant_Vide, AppResources.alrt_msg_Ok);
                        }
                    }
                    else
                    {
                        //if (Constants.AppName == Apps.X_DISTRIBUTION)
                        //{
                        //    PrinterSelector printerSelector = new PrinterSelector();
                        //    await PopupNavigation.Instance.PushAsync(printerSelector);


                        //}
                        bool succes = printerLocal.GetPrinterInstance(eventConnecedUpdate, printerToUse);

                        if (!succes)
                        {
                            throw new Exception("Échec de la connexion à l'imprimante !");
                        }
                        else
                        {
                            if (!printerLocal.isConnected())
                            {
                                printerLocal.openConnection();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(ex.Message,AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
            }
        }
        public async static Task PrintBL(View_VTE_VENTE data)
        {
            if (data == null) return;
            try
            {
                var sysParams = await AppManager.GetSysParams();
                SYS_USER user = await SQLite_Manager.getUserInfo(App.User.UserName);
                //  if (printerReady)
                //  {
                //  int stat=printerLocal.getCurrentStatus();

                //  if (stat == -5 && !printerLocal.isConnected())
                //  {
                //      printerLocal.openConnection();
                //   }
                //   else if(stat == -1)
                //   {
                //       GetPrinterInstance();
                //   }

                if (Constants.AppName == Apps.X_DISTRIBUTION)
                    printerLocal = DependencyService.Get<IPrinterSPRT>();

                await GetPrinterInstance();

                printerLocal.setPrinter(13, 0);
                printerLocal.setPrinter(13, 0);
                printerLocal.setFont(0, 0, 1, 1, 0);
                printerLocal.PrintText(sysParams.NOM_PHARM + Environment.NewLine);
                printerLocal.setFont(0, 0, 0, 1, 0);
                printerLocal.PrintText(sysParams.ADRESSE_PHARM + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                printerLocal.setFont(0, 0, 0, 1, 0);
                printerLocal.PrintText("N : " + data.NUM_VENTE + Environment.NewLine);
                printerLocal.PrintText("Client : " + data.NOM_TIERS + Environment.NewLine);
                string date = String.Format($"Date :{data.DATE_VENTE:dd/MM/yyyy}") + Environment.NewLine;
                printerLocal.PrintText(date);
                printerLocal.PrintText("Etablie par : " + data.CREATED_BY + Environment.NewLine);
                printerLocal.PrintText("Tel : " + user.TEL_USER + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                printerLocal.setFont(0, 0, 0, 1, 0);
                printerLocal.PrintText("Designation        Qte   Prix       MT " + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                string datvalue = "";
                string designation = "";
                if (data.Details != null)
                    foreach (var item in data.Details)
                    {
                        designation = item.DESIGNATION_PRODUIT != null && item.DESIGNATION_PRODUIT.Length > 18 ? item.DESIGNATION_PRODUIT.Substring(0, 18) : item.DESIGNATION_PRODUIT;
                        datvalue = string.Format($"{designation,-18} {item.QUANTITE,-5:N1} {item.PRIX_VTE_TTC,-10:0.00} {item.MT_TTC,-12:0.00}") + Environment.NewLine;
                        printerLocal.PrintText(datvalue);
                    }
                else
                {
                    foreach (var item in data.DetailsDistrib)
                    {
                        designation = item.DESIGNATION_PRODUIT != null && item.DESIGNATION_PRODUIT.Length > 18 ? item.DESIGNATION_PRODUIT.Substring(0, 18) : item.DESIGNATION_PRODUIT;
                        datvalue = string.Format($"{designation,-18} {item.QUANTITE,-5:N1} {item.PRIX_VTE_TTC,-10:0.00} {item.MT_TTC,-12:0.00}") + Environment.NewLine;
                        printerLocal.PrintText(datvalue);
                    }
                }
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                printerLocal.PrintText(string.Format($"Versement : {data.TOTAL_RECU:0.00}" + $"        Total : {data.TOTAL_TTC:0.00}") + Environment.NewLine);
                printerLocal.PrintText(string.Format($"Reste : {data.TOTAL_TTC - data.TOTAL_RECU:0.00}") + Environment.NewLine);

                var MoneyWords = ChiffresLettres.MoneyToLetter(data.TOTAL_TTC);
                string monyWord = XpertHelper.IsNullOrEmpty(MoneyWords) ? "" : MoneyWords.Replace('é', 'e');
                printerLocal.PrintText(monyWord + Environment.NewLine);
                printerLocal.PrintText(" " + Environment.NewLine);
                printerLocal.PrintText(" " + Environment.NewLine);

                //             }
                //             else
                //             {
                //                 await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, "L'imprimante n'a pas pu êtreconnectée!", AppResources.alrt_msg_Ok);
                //             }
                if (Constants.AppName == Apps.X_DISTRIBUTION)
                    printerLocal.closeConnection();

            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, ex.Message, AppResources.alrt_msg_Ok);
            }
            finally
            {
                //if (printerLocal.isConnected())
                //{
                //    printerLocal.closeConnection();
                //}
            }
        }


        public async static void PrintEncaisse(List<Get_Print_ds_ViewTrsEncaiss> data)
        {
            if (data == null) return;

            try
            {
                var sysParams = await AppManager.GetSysParams();

                //if (printerReady)
                //{
                //    if (!printerLocal.isConnected())
                //    {
                //        printerLocal.openConnection();
                //    }
                GetPrinterInstance();

                printerLocal.setPrinter(13, 0);
                printerLocal.setPrinter(13, 0);
                printerLocal.setFont(0, 0, 1, 1, 0);
                printerLocal.PrintText(data[0].NOM_PHARM + Environment.NewLine);
                printerLocal.PrintText(data[0].ADRESSE_PHARM + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data[0].CODE_TYPE))
                {
                    printerLocal.PrintText("Recu de versement" + Environment.NewLine);
                }
                else
                {
                    printerLocal.PrintText("Recu de reglement" + Environment.NewLine);
                }
                printerLocal.setFont(0, 0, 0, 1, 0);
                printerLocal.PrintText("N : " + data[0].NUM_ENCAISS + Environment.NewLine);
                printerLocal.PrintText("Client : " + data[0].NOM_TIERS + Environment.NewLine);
                string date = String.Format($"Date :{data[0].DATE_ENCAISS:dd/MM/yyyy}") + Environment.NewLine;
                printerLocal.PrintText(date);
                printerLocal.PrintText("Etablie par : " + data[0].CREATED_BY + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data[0].CODE_TYPE))
                {
                    printerLocal.PrintText($"Montant verse : {data[0].TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                else
                {
                    printerLocal.PrintText($"Montant regle : {data[0].TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                string monyWord = XpertHelper.IsNullOrEmpty(data[0].MoneyWords) ? "" : data[0].MoneyWords.Replace('é', 'e');
                printerLocal.PrintText(monyWord + Environment.NewLine);
                if (data[0].AFFICHE_SOLDE == 1)
                {
                    printerLocal.PrintText($"Solde anterieur  : {data[0].SOLDE_ANTERIEUR,-10:0.00}" + Environment.NewLine);
                    printerLocal.PrintText($"Solde actuel: {data[0].SOLDE_ACTUEL,-10:0.00}" + Environment.NewLine);
                }
                printerLocal.PrintText(" " + Environment.NewLine);
                printerLocal.PrintText(" " + Environment.NewLine);
                //}
                //else
                //{
                //    await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, "L'imprimante n'a pas pu êtreconnectée!", AppResources.alrt_msg_Ok);
                //}
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, ex.Message, AppResources.alrt_msg_Ok);
            }
            finally
            {
                //if (printerLocal.isConnected())
                //{
                //    printerLocal.closeConnection();
                //}
            }
        }

        public async static void PrintEncaisseOffline(View_TRS_ENCAISS data)
        {
            if (data == null) return;

            try
            {
                var sysParams = await AppManager.GetSysParams();

                //if (printerReady)
                //{
                //    if (!printerLocal.isConnected())
                //    {
                //        printerLocal.openConnection();
                //    }
                GetPrinterInstance();

                printerLocal.setPrinter(13, 0);
                printerLocal.setPrinter(13, 0);
                printerLocal.setFont(0, 0, 1, 1, 0);
                printerLocal.PrintText(sysParams.NOM_PHARM + Environment.NewLine);
                printerLocal.PrintText(sysParams.ADRESSE_PHARM + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data.CODE_TYPE))
                {
                    printerLocal.PrintText("Recu de versement" + Environment.NewLine);
                }
                else
                {
                    printerLocal.PrintText("Recu de reglement" + Environment.NewLine);
                }
                printerLocal.setFont(0, 0, 0, 1, 0);
                printerLocal.PrintText("N : " + data.NUM_ENCAISS + Environment.NewLine);
                printerLocal.PrintText("Client : " + data.NOM_TIERS + Environment.NewLine);
                string date = String.Format($"Date :{data.DATE_ENCAISS:dd/MM/yyyy}") + Environment.NewLine;
                printerLocal.PrintText(date);
                printerLocal.PrintText("Etablie par : " + data.CREATED_BY + Environment.NewLine);
                printerLocal.PrintText("-----------------------------------------------" + Environment.NewLine);
                if ("ENC".Equals(data.CODE_TYPE))
                {
                    printerLocal.PrintText($"Montant verse : {data.TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                else
                {
                    printerLocal.PrintText($"Montant regle : {data.TOTAL_ENCAISS,-10:0.00}" + Environment.NewLine);
                }
                var MoneyWords = ChiffresLettres.MoneyToLetter(data.TOTAL_ENCAISS);
                string monyWord = XpertHelper.IsNullOrEmpty(MoneyWords) ? "" : MoneyWords.Replace('é', 'e');
                printerLocal.PrintText(monyWord + Environment.NewLine);

                printerLocal.PrintText(" " + Environment.NewLine);
                printerLocal.PrintText(" " + Environment.NewLine);
                //}
                //else
                //{
                //    await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, "L'imprimante n'a pas pu êtreconnectée!", AppResources.alrt_msg_Ok);
                //}
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_Info, ex.Message, AppResources.alrt_msg_Ok);
            }
            finally
            {
                //if (printerLocal.isConnected())
                //{
                //    printerLocal.closeConnection();
                //}
            }
        }
    }
}

