using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xpert.Common.DAO;
using Xpert.Common.WSClient.Helpers;
using Xpert.Common.WSClient.Model;
using Xpert.Common.WSClient.Services;
using XpertMobileApp.Api.Services;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Api.ViewModels
{
    public class CrudBaseViewModel2<T1, TView> : BaseViewModel
    where T1 : new()
    where TView : new()
    {
        internal ICurdService<TView> service;

        internal XpertSqlBuilder qb = new XpertSqlBuilder();

        public bool LoadSummaries { get; set; } = false;
        public ObservableCollection<SAMMUARY> Summaries { get; set; }

        public int PageSize = 10;

        private int elementsCount;

        bool isLoadExtrasBusy = false;
        public bool IsLoadExtrasBusy
        {
            get { return isLoadExtrasBusy; }
            set { SetProperty(ref isLoadExtrasBusy, value); }
        }

        decimal elementsSum;
        public decimal ElementsSum
        {
            get { return elementsSum; }
            set { SetProperty(ref elementsSum, value); }
        }

        public InfiniteScrollCollection<TView> Items { get; set; }

        public TView SelectedItem { get; set; }

        public Command LoadItemsCommand { get; set; }

        public Command LoadExtrasDataCommand { get; set; }

        public Command AddItemCommand { get; set; }

        public Command DeleteItemCommand { get; set; }

        public Command UpdateItemCommand { get; set; }

        public CrudBaseViewModel2()
        {
            InitConstructor();
            qb = new XpertSqlBuilder();
        }


        protected virtual QueryInfos GetFilterParams()
        {
            qb.InitQuery();
            return qb.QueryInfos;
        }

        protected virtual void OnAfterLoadItems(IEnumerable<TView> list)
        {

        }

        protected virtual string ContoleurName
        {
            get
            {
                return typeof(T1).Name;
            }
        }

        protected virtual void InitConstructor()
        {
            string ctrlName = ContoleurName;
            service = new CrudService<TView>(App.RestServiceUrl, ContoleurName, App.User.Token);
            Summaries = new ObservableCollection<SAMMUARY>();

            // Listing
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // Ajout
            AddItemCommand = new Command<TView>(async (TView item) => await ExecuteAddItemCommand(item));
            MessagingCenter.Subscribe<MsgCenter, TView>(this, MCDico.ADD_ITEM, async (obj, item) =>
            {
                AddItemCommand.Execute(item);
            });

            // Supression
            DeleteItemCommand = new Command<string>(async (string idElem) => await ExecuteDeleteItemCommand(idElem));
            MessagingCenter.Subscribe<MsgCenter, TView>(this, MCDico.DELETE_ITEM, async (obj, item) =>
            {
                DeleteItemCommand.Execute(item);
            });

            // Modification
            UpdateItemCommand = new Command<TView>(async (TView item) => await ExecuteUpdateItemCommand(item));
            MessagingCenter.Subscribe<MsgCenter, TView>(this, MCDico.UPDATE_ITEM, async (obj, item) =>
            {
                UpdateItemCommand.Execute(item);
            });

            // chargement infini
            Items = new InfiniteScrollCollection<TView>
            {
                OnLoadMore = async () =>
                {
                    try 
                    {
                        bool isconnected = await App.IsConected();
                        if (!isconnected) 
                        {
                            await UserDialogs.Instance.AlertAsync(AppResources.alrt_msg_NoConnexion, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                            return new List<TView>();
                        }
                        
                        IsBusy = true;

                        elementsCount = await service.ItemsCount(GetFilterParams());

                        // load the next page
                        var page = (Items.Count / PageSize) + 1;

                        var items = await service.SelectByPage(GetFilterParams(), page, PageSize);
                        Summaries.Clear();
                        if (LoadSummaries && elementsCount > 0)
                        {
                            var res = await service.ItemsSums(GetFilterParams());

                            foreach (var item in res)
                            {
                                Summaries.Add(new SAMMUARY()
                                {
                                    key = TranslateExtension.GetTranslation(item.Key),
                                    Value = item.Value.ToString("N2")
                                });
                            }
                        }

                        OnAfterLoadItems(items);

                        IsBusy = false;

                        // return the items that need to be added
                        return items;
                    } 
                    catch(Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(ex.Message, AppResources.alrt_msg_Alert, AppResources.alrt_msg_Ok);
                        return new List<TView>();
                    }
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < elementsCount;
                }
            };
        }

        internal async Task GetItemsSum()
        {
            ElementsSum = await service.ItemsSum(GetFilterParams());
        }

        internal async Task<SortedDictionary<string, decimal>> GetItemsSums()
        {
            var result = await service.ItemsSums(GetFilterParams());
            return result;
        }

        internal virtual async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Items.Clear();
                await Items.LoadMoreAsync();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task ExecuteUpdateItemCommand(TView item)
        {
            if (IsBusy)
                return;

            try
            {
                if (await App.IsConected())
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    await service.UpdateItemAsync(item);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        public async Task ExecuteDeleteItemCommand(string codeItem)
        {
            if (IsBusy)
                return;

            try
            {
                if (await App.IsConected())
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    await service.DeleteItemAsync(codeItem);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                IsBusy = false;
            }
        }

        public async Task ExecuteAddItemCommand(TView item)
        {
            if (IsBusy)
                return;

            try
            {
                if (await App.IsConected())
                {
                    IsBusy = true;
                    UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                    await service.AddItemAsync(item);
                    await UserDialogs.Instance.AlertAsync("L'ajout a été effectuée avec succès!", AppResources.alrt_msg_Alert,
    AppResources.alrt_msg_Ok);
                }
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
            }
            finally
            {
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }
        }

        public virtual void ClearFilters()
        {
            qb.InitQuery();
        }

        #region QUERY BUILDER METHDOES
        public string TableView { get; set; }
        public string GetPropertyFullName<T1, P>(Expression<Func<T1, P>> field)
        {
            return XpertHelper.GetTableName<T1>() + "." + XpertHelper.GetPropertyName(field);
        }
        public string GetPropertyFullName<P>(Expression<Func<TView, P>> field)
        {
            return TableView + "." + XpertHelper.GetPropertyName(field);
        }

        public void AddSelect<T1, P>(Expression<Func<T1, P>> field)
        {
            string fieldName = this.GetPropertyFullName(field);
            this.AddSelect(fieldName);
        }

        public void AddSelect_All<T1>()
        {
            this.AddSelect(XpertHelper.GetTableName<T1>() + "." + "*");
        }

        public void AddSelectCountAll()
        {
            qb.QueryInfos.StringSelections = "SELECT Count(*)";
        }

        public void AddSelect(string expression)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringSelections))
            {
                qb.QueryInfos.StringSelections = "SELECT " + expression;
            }
            else
            {
                qb.QueryInfos.StringSelections += " , " + expression;
            }
        }

        public void AddSelect_RIGHT<T1, P>(Expression<Func<T1, P>> field, string valAdded, int length)
        {
            string fullName = this.GetPropertyFullName(field);
            string name = XpertHelper.GetPropertyName(field);
            this.AddSelect(string.Format("RIGHT('{0}'+RTRIM(ISNULL({1},'')),{2}) {3}", valAdded, fullName, length, name));
        }

        public void AddConditionOperator(TypeParenthese _typeParenthese)
        {
            this.AddConditionOperator(TypeConnector.NONE, _typeParenthese);
        }

        public void AddConditionOperator(TypeConnector _typeConnector)
        {
            this.AddConditionOperator(_typeConnector, TypeParenthese.NONE);
        }

        public void AddConditionOperator(TypeConnector _connector, TypeParenthese _parentese)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringCondition))
            {
                if (_parentese.Equals(TypeParenthese.LEFT))
                    qb.QueryInfos.StringCondition = " (";
            }
            else
            {
                string cc = "";
                if (!_connector.Equals(TypeConnector.NONE))
                    cc = " " + _connector + " ";
                if (_parentese.Equals(TypeParenthese.LEFT))
                    cc += "(";
                else if (_parentese.Equals(TypeParenthese.RIGHT))
                    cc = ")" + cc;
                qb.QueryInfos.StringCondition += cc;
            }
            this.qb.HasOperatorCond = _connector != TypeConnector.NONE || _parentese == TypeParenthese.LEFT;
        }
        public void AddCondition(string expression)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringCondition))
                qb.QueryInfos.StringCondition = " " + expression;
            else if (this.qb.HasOperatorCond)
                qb.QueryInfos.StringCondition += " " + expression;
            else
                qb.QueryInfos.StringCondition += " AND " + expression;
            this.qb.HasOperatorCond = false;
        }
        public void AddCondition(string fieldName, object valueField)
        {
            this.AddCondition(fieldName, valueField, true);
        }
        public void AddCondition(string fieldName, Operator oper)
        {
            if (XpertHelper.IsNullOrEmpty(fieldName)) return;
            switch (oper)
            {
                case Operator.ISNULL_OR_EMPTY:
                    this.AddCondition("ISNULL(" + fieldName + ",'') = ''");
                    break;
                case Operator.ISNOT_NULL_OR_EMPTY:
                    this.AddCondition("ISNULL(" + fieldName + ",'') <> ''");
                    break;
                case Operator.ISNULL:
                    this.AddCondition(fieldName + " IS NULL");
                    break;
                case Operator.ISNOTNULL:
                    this.AddCondition(fieldName + " IS NOT NULL");
                    break;
                case Operator.NOT_EMPTY:
                    this.AddCondition(fieldName + " <>''");
                    break;
                case Operator.GREATER_DATE_NOW:
                    this.AddCondition(fieldName + " > CONVERT(DATE,GETDATE())");
                    break;
                case Operator.GREATER_EQUAL_DATE_NOW:
                    this.AddCondition(fieldName + " >= CONVERT(DATE,GETDATE())");
                    break;
                case Operator.EQUAL_DATE_NOW:
                    this.AddCondition(fieldName + " = CONVERT(DATE,GETDATE())");
                    break;
                case Operator.LESS_DATE_NOW:
                    this.AddCondition(fieldName + " < CONVERT(DATE,GETDATE())");
                    break;
                case Operator.LESS_EQUAL_DATE_NOW:
                    this.AddCondition(fieldName + " <= CONVERT(DATE,GETDATE())");
                    break;
                case Operator.IS_NUMERIC:
                    this.AddCondition("ISNUMERIC(" + fieldName + ") = 1");
                    break;
            }
        }

        public void AddCondition(string fieldName, object valueField,
                bool quote)
        {
            if (XpertHelper.IsNullOrEmpty(valueField))
            {
                return;
            }

            this.AddCondition(fieldName + " = " + this.UpdateValueQuote(valueField, quote));
        }

        public void AddCondition_NotEmpty<T1, P>(Expression<Func<T1, P>> field, object fieldValue)
        {
            if (XpertHelper.IsEmptyCode(fieldValue))
            {
                this.AddCondition("1=0");
            }
            else
            {
                string fieldName1 = this.GetPropertyFullName(field);
                this.AddCondition(fieldName1, fieldValue);
            }
        }

        public void AddCondition<P>(Expression<Func<TView, P>> field, Operator oper,
        object valueField)
        {
            this.AddCondition<TView, P>(field, oper, valueField);
        }
        public void AddCondition<T1, P>(Expression<Func<T1, P>> field, object fieldValue)
        {
            string fieldName1 = this.GetPropertyFullName(field);
            this.AddCondition(fieldName1, fieldValue);
        }

        public void AddCondition<T1, P>(Expression<Func<T1, P>> field, Operator oper, object value1, bool quote = true)
        {
            string fieldName1 = this.GetPropertyFullName(field);
            this.AddCondition(fieldName1, oper, value1, quote);
        }

        public void AddCondition_Is_Null<T1, P>(Expression<Func<T1, P>> field, Expression<Func<T1, P>> fieldIsNull, object value, Operator op = Operator.EQUAL)
        {
            string fieldIsNullName = this.GetPropertyFullName(fieldIsNull);
            this.AddCondition_Is_Null(field, fieldIsNullName, value);

        }
        public void AddCondition_Is_Null<T1, P>(Expression<Func<T1, P>> field, object valueIsNull, object value, Operator oper = Operator.EQUAL, bool quote = false)
        {
            if (quote)
            {
                valueIsNull = this.UpdateValueQuote(valueIsNull, quote);
            }

            string fieldName1 = "ISNULL(" + this.GetPropertyFullName(field) + ", " + valueIsNull + ")";

            this.AddCondition(fieldName1, oper, value, quote);
        }

        public void AddCondition<P>(Expression<Func<TView, P>> field, Operator oper, object value1, object value2)
        {
            this.AddCondition<TView, P>(field, oper, value1, value2);
        }
        public void AddCondition<T1, P>(Expression<Func<T1, P>> field, Operator oper, object value1, object value2)
        {
            string fieldName = this.GetPropertyFullName(field);
            this.AddCondition(fieldName, oper, value1, value2);
        }
        public void AddCondition(string fieldName, Operator oper, object value1, object value2)
        {
            if (XpertHelper.IsNullOrEmpty(fieldName)) return;
            if (XpertHelper.IsNullOrEmpty(value1) && XpertHelper.IsNullOrEmpty(value2)) return;
            switch (oper)
            {
                case Operator.BETWEEN:
                    if (XpertHelper.IsNullOrEmpty(value1))
                    {
                        this.AddCondition(fieldName + " <= " + XpertHelper.GetSqlValue(value2));
                    }
                    else if (XpertHelper.IsNullOrEmpty(value2))
                    {
                        this.AddCondition(fieldName + " >= " + XpertHelper.GetSqlValue(value1));
                    }
                    else
                    {
                        this.AddCondition(fieldName + " BETWEEN " + XpertHelper.GetSqlValue(value1) +
                                                    " AND " + XpertHelper.GetSqlValue(value2));
                    }
                    break;
                case Operator.BETWEEN_DATE:
                    fieldName = "CONVERT(CHAR(8)," + fieldName + ",112)";
                    if (!XpertHelper.IsNullOrEmpty(value1))
                    {
                        this.AddCondition(fieldName + " >= " + XpertHelper.GetSqlValue(value1));
                    }
                    if (!XpertHelper.IsNullOrEmpty(value2))
                    {
                        this.AddCondition(fieldName + " <= " + XpertHelper.GetSqlValue(value2));
                    }
                    break;

                case Operator.BETWEEN_HOUR:
                    fieldName = "CONVERT(CHAR(8)," + fieldName + ",108)";
                    if (XpertHelper.IsNullOrEmpty(value1))
                    {
                        this.AddCondition(fieldName + " <= " + "" + "'" + XpertHelper.GetSQLDate(value2, "HH:mm") + "'");
                    }
                    else if (XpertHelper.IsNullOrEmpty(value2))
                    {
                        this.AddCondition(fieldName + " >= " + "" + "'" + XpertHelper.GetSQLDate(value1, "HH:mm") + "'");
                    }
                    else
                    {
                        this.AddCondition(fieldName + " BETWEEN " + "'" + XpertHelper.GetSQLDate(value1, "HH:mm") + "'" +
                                                    " AND " + "'" + XpertHelper.GetSQLDate(value2, "HH:mm") + "'");
                    }
                    break;
            }
        }

        public void AddCondition_WithConvertDate(string fieldName, Operator oper,
                object valueField)
        {
            this.AddCondition(string.Format("CONVERT(char(8),{0},112)", fieldName), oper, valueField, false);
        }


        public void AddCondition(string fieldName, Operator oper,
                object valueField)
        {
            this.AddCondition(fieldName, oper, valueField, true);
        }
        public void AddCondition(string fieldName, Operator oper, object valueField, bool quote)
        {
            if (oper == Operator.EQUAL_EMPTY)
            {
                if (XpertHelper.IsNull(valueField))
                {
                    return;
                }
                this.AddCondition(fieldName + " = " + this.UpdateValueQuote(valueField, quote));
                return;
            }
            if (XpertHelper.IsNullOrEmpty(valueField))
            {
                return;
            }

            if (XpertHelper.IsNullOrEmpty(valueField.ToString().Trim()))
            {
                return;
            }
            switch (oper)
            {
                case Operator.EQUAL:
                    this.AddCondition(fieldName + " = " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.NOT_EQUAL:
                    this.AddCondition(fieldName + " <> " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.NOT_EQUAL_WITH_FIELD_ISNULL:
                    this.AddCondition("ISNULL(" + fieldName + ",'')" + " <> " + this.UpdateValueQuote(valueField, quote));
                    return;
                case Operator.EQUAL_WITH_FIELD_ISNULL:
                    this.AddCondition("ISNULL(" + fieldName + ",'')" + " = " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.NOT_EQUAL_WITH_TRIM:
                    this.AddCondition("RTRIM(LTRIM(" + fieldName + ")) <> " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.GREATER:
                    this.AddCondition(fieldName + " > " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.GREATER_EQUAL:
                    this.AddCondition(fieldName + " >= " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.LESS:
                    this.AddCondition(fieldName + " < " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.LESS_EQUAL:
                    this.AddCondition(fieldName + " <= " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.LIKE:
                    this.AddCondition(fieldName + " LIKE " + this.UpdateValueQuote(valueField, quote));
                    return;

                case Operator.LIKE_ANY:
                    this.AddCondition(fieldName + " LIKE " + this.UpdateValueQuote("%" + valueField.ToString().Trim().Replace(' ', '%') + "%", quote));
                    return;

                case Operator.LIKE_RIGHT:
                    this.AddCondition(fieldName + " LIKE " + this.UpdateValueQuote(valueField + "%", quote));
                    return;

                case Operator.LIKE_LEFT:
                    this.AddCondition(fieldName + " LIKE " + this.UpdateValueQuote("%" + valueField, quote));
                    return;

                case Operator.NOT_LIKE_ANY:
                    this.AddCondition(fieldName + " NOT LIKE " + this.UpdateValueQuote("%" + valueField.ToString().Trim().Replace(' ', '%') + "%", quote));
                    return;

                case Operator.NOT_LIKE_RIGHT:
                    this.AddCondition(fieldName + " NOT LIKE " + this.UpdateValueQuote(valueField + "%", quote));
                    return;

                case Operator.NOT_LIKE_LEFT:
                    this.AddCondition(fieldName + " NOT LIKE " + this.UpdateValueQuote("%" + valueField, quote));
                    return;

                case Operator.IN:
                    string condIn = XpertHelper.GetValues(valueField, ',');
                    if (!string.IsNullOrEmpty(condIn))
                        this.AddCondition(fieldName + " IN (" + condIn + ")");
                    return;
                case Operator.IN_WITH_FIELD_ISNULL:
                    string condIn_WITH_FIELD_ISNULL = XpertHelper.GetValues(valueField, ',');
                    condIn_WITH_FIELD_ISNULL += ",''";
                    if (!string.IsNullOrEmpty(condIn_WITH_FIELD_ISNULL))
                        this.AddCondition("ISNULL(" + fieldName + ",'')" + " IN (" + condIn_WITH_FIELD_ISNULL + ")");
                    return;
                case Operator.NOT_IN:
                    string condNotIn = XpertHelper.GetValues(valueField, ',');
                    if (!string.IsNullOrEmpty(condNotIn))
                        this.AddCondition(fieldName + " NOT IN (" + condNotIn + ")");
                    return;

                case Operator.GREATER_DATE:
                    fieldName = "CONVERT(CHAR(8)," + fieldName + ",112)";
                    if (!XpertHelper.IsNullOrEmpty(valueField))
                    {
                        this.AddCondition(fieldName + " > " + XpertHelper.GetSqlValue(valueField));
                    }
                    return;

                case Operator.LESS_DATE:
                    fieldName = "CONVERT(CHAR(8)," + fieldName + ",112)";
                    if (!XpertHelper.IsNullOrEmpty(valueField))
                    {
                        this.AddCondition(fieldName + " < " + XpertHelper.GetSqlValue(valueField));
                    }
                    return;

                default:
                    this.AddCondition(fieldName + " = " + this.UpdateValueQuote(valueField, quote));
                    return;
            }
        }
        public void AddJoin(string srcField, string srctTable, string destTable)
        {
            this.AddJoin(srctTable, srcField, destTable, srcField);
        }

        public void AddJoin(string srctTable, string srcField, string destTable, string destField)
        {
            this.AddJoin(srctTable, srcField, destTable, destField, TypesJoin.LEFT);
        }

        public void AddJoin(string tableName1, string fieldName1, string tableName2,
                string fieldName, TypesJoin joinType)
        {
            this.AddJoin(" " + joinType + " JOIN " + tableName2 + " ON " + tableName1 + "."
                    + fieldName1 + " = " + tableName2 + "." + fieldName + " ");
        }

        public void AddJoin(string expression)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringJoin))
                qb.QueryInfos.StringJoin = expression;
            else qb.QueryInfos.StringJoin += " " + expression;
        }

        internal void AddJoinCond(string srcField, object value, bool quote)
        {
            value = this.UpdateValueQuote(value, quote);
            qb.QueryInfos.StringJoin += string.Format(" AND  {0} = {1}", srcField, value);
        }

        public void AddOrderBy<T1, P>(Expression<Func<T1, P>> field)
        {
            this.AddOrderBy(GetPropertyFullName(field));
        }

        public void AddOrderBy<T1, P>(Expression<Func<T1, P>> field, Sort sort)
        {
            this.AddOrderBy(GetPropertyFullName(field), sort);
        }

        internal void AddOrderBy(string fieldName)
        {
            this.AddOrderBy(fieldName, Sort.ASC);

        }
        public void AddOrderBy(string fieldName, Sort sort)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringOrderBy))
            {
                qb.QueryInfos.StringOrderBy = "ORDER BY " + fieldName + " " + sort;
            }
            else if (!qb.QueryInfos.StringOrderBy.Contains(fieldName))
            {
                qb.QueryInfos.StringOrderBy += " , " + fieldName + " " + sort;
            }
        }
        public void AddGroupBy(string expression)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringGroupBy))
                qb.QueryInfos.StringGroupBy = "GROUP BY " + expression;
            else qb.QueryInfos.StringGroupBy += " , " + expression;
        }

        internal void AddHaving(string expression)
        {
            if (String.IsNullOrEmpty(qb.QueryInfos.StringHaving))
                qb.QueryInfos.StringHaving = "HAVING " + expression;
            else qb.QueryInfos.StringHaving = " AND " + expression;
        }

        public void AddPaging(int index, int nbrRows)
        {
            if (index > 0 && nbrRows > 0)
            {
                qb.QueryInfos.StringPaging = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", index - 1, nbrRows);
            }
        }

        private string UpdateValueQuote(object value1, bool quote)
        {
            if (value1 is DateTime)
            {
                if (value1 == null) return null;
                string value = Convert.ToDateTime(value1).ToString("yyyyMMdd HH:mm:ss");
                return "'" + value + "'";
            }

            else if ((value1 is string) || (value1 is char) || (value1 is bool))
            {
                string value = value1.ToString();
                value = value.Replace("'", "''");
                if (quote)
                {
                    return "'" + value + "'";
                }
                return value;
            }
            return value1.ToString().Replace(",", ".");
        }

        #endregion  Query builder


        internal async Task<bool> UpdateItem(TView item) 
        {
            if (IsBusy)
                return false;

            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var res = await service.UpdateItemAsync(item);
                UserDialogs.Instance.HideLoading();

                return res;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
