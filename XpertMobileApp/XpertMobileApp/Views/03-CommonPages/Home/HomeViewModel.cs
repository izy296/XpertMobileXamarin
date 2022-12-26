using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xpert;
using Xpert.Common.DAO;
using XpertMobileApp.Api;
using XpertMobileApp.Api.Managers;
using XpertMobileApp.Api.Models;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.Services;

namespace XpertMobileApp.ViewModels
{

    public class HomeViewModel : BaseViewModel
    {
        public ObservableCollection<TDB_SIMPLE_INDICATORS> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ObservableCollection<TRS_JOURNEES> Sessions { get; set; }
        public Command LoadSessionsCommand { get; set; }




        private TRS_JOURNEES currentSession;
        public TRS_JOURNEES CurrentSession
        {
            get { return currentSession; }
            set { SetProperty(ref currentSession, value); }
        }

        private decimal totalEncaiss;
        public decimal TotalEncaiss
        {
            get { return totalEncaiss; }
            set { SetProperty(ref totalEncaiss, value); }
        }

        private decimal totalDecaiss;
        public decimal TotalDecaiss
        {
            get { return totalDecaiss; }
            set { SetProperty(ref totalDecaiss, value); }
        }

        public HomeViewModel()
        {
            Title = AppResources.pn_home;

            Items = new ObservableCollection<TDB_SIMPLE_INDICATORS>();
            Sessions = new ObservableCollection<TRS_JOURNEES>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            LoadSessionsCommand = new Command(async () => await ExecuteLoadSessionsCommand());
        }

        async Task ExecuteLoadSessionsCommand()
        {
            try
            {
                /*
                Sessions.Clear();
                var items = await WebServiceClient.GetSessionInfos();

                foreach (var item in items)
                {
                    Sessions.Add(item);

                    if (App.User.UserName == item.USER_SESSION)
                    {
                        this.CurrentSession = item;
                    }
                    
                }
                */
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(AppResources.err_msg_loadingDataError, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                UserDialogs.Instance.ShowLoading(AppResources.txt_Loading);

                ObservableCollection<TDB_SIMPLE_INDICATORS> res = new ObservableCollection<TDB_SIMPLE_INDICATORS>();

                if (Constants.AppName == Apps.X_DISTRIBUTION)
                {
                    /* Page Tournee */
                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Tournee).ToString(),
                        Title = "Tournee",
                        Color = "#FAEBCD",
                        CodeObjet = Xpert.XpertObjets.LIV_TOURNEE,
                        Action = Xpert.XpertActions.AcSelect,
                        IconImage = "tourneeIcon.png"
                    });

                    /* Page Livraison */
                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Livraison).ToString(),
                        Title = "Livraisons",
                        Color = "#F7C873",
                        CodeObjet = Xpert.XpertObjets.VTE_LIVRAISON,
                        Action = Xpert.XpertActions.AcSelect,
                        IconImage = "LivraisonMenuIcon.png"
                    });

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.TransfertStock).ToString(),
                        Title = "Charge./Décharge.",
                        Color = "#C9F0D6",
                        IconImage = "chargementDechargementIcon.png"
                    });

                    //res.Add(new TDB_SIMPLE_INDICATORS()
                    //{
                    //    CODE_ANALYSE = ((int)MenuItemType.Sessions).ToString(),
                    //    Title = "Sessions",
                    //    Color = "#ff6b81",
                    //    CodeObjet = Xpert.XpertObjets.TRS_RESUME_SESSION,
                    //    Action = Xpert.XpertActions.AcSelect,
                    //    IconImage = "sessionIcon.png"
                    //});

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Tiers).ToString(),
                        Title = AppResources.pn_Client,
                        Color = "#A8D08D",
                        IconImage = "clientIcon.png"
                    });

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Encaissements).ToString(),
                        Title = Constants.AppName != Apps.X_DISTRIBUTION ? "Versement" : "Paiement",
                        Color = "#F5F5F5",
                        IconImage = "encaissIcon.png"
                    });

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Produits).ToString(),
                        Title = "Produits",
                        Color = "#D5EEFF",
                        IconImage = "produitIcon.png"
                    });

                    foreach (var item in res)
                    {
                        if (item.HasPermission)
                            Items.Add(item);
                    }
                }
                else
                {
                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Achats).ToString(),
                        Title = AppResources.pn_Achats,
                        Color = "#91C1BA",
                        CodeObjet = Xpert.XpertObjets.ACH_DOCUMENT,
                        Action = Xpert.XpertActions.AcSelect
                    });

                    if (Constants.AppName == Apps.XPH_Mob)
                    {
                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.Psychotrop).ToString(),
                            Title = AppResources.pn_VtePsychotrop,
                            Color = "#91C1BA",
                            CodeObjet = Xpert.XpertObjets.VTE_PSYCHOTROP,
                            Action = Xpert.XpertActions.AcSelect
                        });
                    }

                    else if (Constants.AppName == Apps.XCOM_Mob)
                    {
                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.Manquants).ToString(),
                            Title = AppResources.pn_Manquants,
                            Color = "#91C1BA",
                            CodeObjet = Xpert.XpertObjets.ACH_MANQUANTS,
                            Action = Xpert.XpertActions.AcSelect
                        });
                    }


                    if (Constants.AppName == Apps.XCOM_Livraison)
                    {
                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.Livraison).ToString(),
                            Title = "Livraisons",
                            Color = "#91C1BA",
                            CodeObjet = Xpert.XpertObjets.VTE_LIVRAISON,
                            Action = Xpert.XpertActions.AcSelect
                        });
                    }
                    else
                    {
                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.VenteComptoir).ToString(),
                            Title = "Ventes comptoir",
                            Color = "#91C1BA",
                            CodeObjet = Xpert.XpertObjets.VTE_COMPTOIR,
                            Action = Xpert.XpertActions.AcSelect
                        });
                    }
                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Ventes).ToString(),
                        Title = "Ventes",
                        Color = "#91C1BA",
                        CodeObjet = Xpert.XpertObjets.VTE_VENTE,
                        Action = Xpert.XpertActions.AcSelect
                    });

                    if (Constants.AppName == Apps.XCOM_Livraison)
                    {
                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.Tournee).ToString(),
                            Title = "Tournée",
                            Color = "#D9EAE7",
                            Action = Xpert.XpertActions.AcSelect
                        });
                    }

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Sessions).ToString(),
                        Title = "Sessions",
                        Color = "#91C1BA",
                        CodeObjet = Xpert.XpertObjets.TRS_RESUME_SESSION,
                        Action = Xpert.XpertActions.AcSelect
                    });

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Encaissements).ToString(),
                        Title = "Encaiss / Decaiss",
                        Color = "#91C1BA",
                        CodeObjet = Xpert.XpertObjets.TRS_DECAISS,
                        Action = Xpert.XpertActions.AcSelect
                    });

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Produits).ToString(),
                        Title = "Produits",
                        Color = "#91C1BA"
                    });

                    res.Add(new TDB_SIMPLE_INDICATORS()
                    {
                        CODE_ANALYSE = ((int)MenuItemType.Tiers).ToString(),
                        Title = "Tiers",
                        Color = "#91C1BA"
                    });

                    if (Constants.AppName == Apps.XCOM_Livraison)
                    {
                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.Import).ToString(),
                            Title = "Import Donnée",
                            Color = "#8fc779"
                        });

                        res.Add(new TDB_SIMPLE_INDICATORS()
                        {
                            CODE_ANALYSE = ((int)MenuItemType.Export).ToString(),
                            Title = "Export Donnée",
                            Color = "#87CEEB",

                        });
                    }

                    Items.Clear();
                    foreach (var item in res)
                    {
                        if (item.HasPermission)
                            Items.Add(item);
                    }
                }


                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await UserDialogs.Instance.AlertAsync(AppResources.err_msg_loadingDataError, AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        protected QueryInfos GetFilterParams()
        {
            XpertSqlBuilder qb = new XpertSqlBuilder();
            qb.InitQuery();

            qb.AddCondition<TDB_SIMPLE_INDICATORS, string>(e => e.Profils, Operator.LIKE_ANY, App.User.UserGroup);

            qb.AddCondition<TDB_SIMPLE_INDICATORS, string>(e => e.AppNames, Operator.LIKE_ANY, Constants.AppName);

            qb.AddOrderBy<TDB_SIMPLE_INDICATORS, int>(e => e.ORDRE);

            return qb.QueryInfos;
        }
    }
}
