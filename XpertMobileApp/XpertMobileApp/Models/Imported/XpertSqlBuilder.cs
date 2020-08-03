using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using XpertMobileApp.Api.Services;

namespace Xpert.Common.DAO
{
    public enum XpertObjets
    {
        None
        ,
        SYS_OBLIGATION
        ,
        SYS_OBJET_PERMISSION
        ,
        SYS_OBJET_PWD
        ,
        SYS_TICKET
        ,
        SYS_USER
        ,
        SYS_GROUPE
        ,
        SYS_PARAMETRE
        ,
        TDB_ANALYSES
        ,
        TDB_ANALYSE_MVT_STK
        ,
        TDB_ANALYSE_MVT_STK_BY_PRODUIT
        ,
        TDB_ANALYSE_MENSUELLES_PRODUIT
        ,
        TDB_STAT_VTE_BY_USER
        ,
        TDB_STAT_NBR_VTE
        ,
        TDB_STAT_ACHAT_PAR_FOURNISSEUR
        ,
        SYS_FAVORIS_RUBBON
        ,
        SYS_OBJET_TRACE
        ,
        ACH_AVOIR
        ,
        ACH_COMMANDES
        ,
        ACH_FACTURE
        ,
        ACH_DOCUMENT
        ,
        ACH_RECEPTION
        ,
        ACH_RETOUR
        ,
        ACH_MANQUANTS
        ,
        ACH_RECLAMATIONS
        ,
        ACH_AUTORISER_RETOUR_FROM_STOCK
        ,
        ///////////XPERT_PRODUCTION /////////
        ACH_UPDATE_ENTETE
        ,
        ACH_UPDATE_DETAIL
        ,
        ACH_UPDATE_PRIX_HT
        ,
        ACH_RECEPTION_LISTE_AGRICULTEUR
        ,
        PRD_PRODUCTION_AGRICULTURE
        /////////////////////////////
        ,
        XAS_VENTE
        ,
        CFA_BORDEREAU
        ,
        CFA_ALLOW_IGNORE_DESTOCKAGE
        ,
        CFA_CENTRES
        ,
        CFA_MEDICAMENT
        ,
        FACTURE_CASNOS
        ,
        FACTURE_CHIFA
        ,
        FACTURE_CHIFA_DELETED
        ,
        View_TRS_ENCAISS_DETAIL
        ,
        CVM_VENTE
        ,
        CVM_CENTRE
        ,
        CVM_MEDECIN
        ,
        CVM_PATHOLOGIE
        ,
        CVM_SIT_FAM
        ,
        CVM_TYPE
        ,
        CVM_ASSURE
        ,
        CVM_AYD_DROIT
        ,
        CVM_BORDEREAU
        ,
        CVM_CARTE_CHRONIQUE
        ,
        CVM_DECOMPTE
        ,
        CVM_VTE_COMPTOIR
        ,
        CVM_DECOMPTE_CHRON
        ,
        CVM_FACTURE
        ,
        CVM_LISTE_NOIRE
        ,
        CVM_MALADE
        ,
        CVM_MALADE_CHRON
        ,
        CVM_PARAMETRAGE
        ,
        CVM_TYPE_DECOMPTE
        ,
        CVM_TYPE_FACTURE
        ,
        CVM_ACTION_LOGGER
        ,
        STK_AJUSTEMENT_DETAIL
        ,
        STK_ENTREE
        ,
        ENTREES_WEB
        ,
        BSE_PRODUIT_FORMES
        ,
        STK_PRODUITS
        ,
        STK_SORTIE
        ,
        STK_TRANSFERT
        ,
        STK_INVENTAIRE
        ,
        STK_STOCK
        ,
        STK_STOCK_BLOQUE
        ,
        STK_STOCK_UPDATE_QTT
        ,
        STK_SUIVI
        ,
        STK_ECHANGE
        ,
        BSE_PRODUIT_LABO
        ,
        BSE_PRODUIT_TYPE
        ,
        BSE_PRODUIT_DCI
        ,
        BSE_EMPLACEMENT
        ,
        View_BSE_MAGASIN
        ,
        BSE_COMPTE
        ,
        BSE_TIERS_FAMILLE
        ,
        BSE_TIERS_FAMILLE_SOLDE
        ,
        BSE_COMPTE_TYPE
        ,
        BSE_ENCAISS_MOTIFS
        ,
        BSE_TIERS_TYPE
        ,
        BSE_MEDECIN
        ,
        BSE_SPECIALITE_MEDECINE
        ,
        BSE_VALUES
        ,
        TRS_CREANCE
        ,
        View_TRS_CREDIT_PAIEMENT
        ,
        TRS_ENCAISS
        ,
        TRS_DECAISS
        ,
        TRS_JOURNEES
        ,
        TRS_JOURNEES_MAJ_MT_CLOTUR
        ,
        TRS_JOURNEES_MAJ_FOND_CAISSE
        ,
        TRS_JOURNEES_MAJ_MT_CLOTUR_ZERO
        ,
        TRS_TIERS
        ,
        TRS_CLIENT
        ,
        TRS_FOURNISSEUR
        ,
        TRS_CLIENT_FOURNISSEUR
        ,
        TRS_AUTRE
        ,
        TRS_TIERS_CHANGE_TYPE
        ,
        TRS_RESUME_SESSION
        ,
        TRS_DETAIL_SESSION
        ,
        TRS_ENCAISS_SANS_TIERS
        ,
        TRS_TIERS_FIDELITE
        ,
        View_TRS_VIREMENT
        ,
        VTE_AVOIR
        ,
        VTE_COMMANDE
        ,
        VTE_COMPTOIR
        ,
        VTE_PSYCHOTROP
        ,
        View_VTE_PSYCHOTROP_DETAIL
        ,
        VTE_FACTURE
        ,
        VTE_INSTANCE
        ,
        VTE_LIVRAISON
        ,
        VTE_PROFORMA
        ,
        VTE_RETOUR
        ,
        VTE_VENTE_CODEBARRE //prévilege pour Vente au comptoir uniquement avec code à barre
        ,
        View_VTE_BEST_CHOICE
        ,
        JOURNAL_VTE_COMMANDE_DETAIL
        ,
        View_VTE_JOURNAL_DETAIL
        ,
        JOURNAL_VTE_PROFORMA_DETAIL
        ,
        VTE_VENTE // idobjet pas dans les bll   
        ,
        VTE_ATTENTE //vente en attante pas relier au bll
        ,
        VTE_AUTORISER_RETOUR_FROM_STOCK
        ,
        VTE_USE_REMISE_WITH_PRIVILEGE
        ,
        ACH_ASSIST_COMMANDES // assistance commande n'etulise pas une bll mais elle a un previlege pour le menue mainform
        ,
        MAJ_VERSION // evidament pas de bll
        ,
        MAJ_FICHE // meme remarque
                  //, MAJ_A_PROPOS // meme remarque
        ,
        ETAT_CREANCE
        ,
        CFA_BENEFICIAIRE
        ,
        SYS_AUTHLIST
        ,
        SYS_FORM_CONFIG
        ,
        BSE_MOTIFS_VENTE_INSTANCE
        ,
        BSE_MOTIFS_ARCHIVE_INSTANCE
        ,
        SYS_CONFIGURATION_TABLEAUX_BORD
        ,
        BSE_PRODUIT_FAMILLE // enciennement FAMILLE
        ,
        View_STK_ECHANGE
        ,
        SYS_OPENTIROIRCAISSE // ajouter  pour le mainform
        ,
        TRS_TIROIRCAISSE_PREPAR_JOURNEE
        ,
        TRS_TIROIRCAISSE_CLOTURE_JOURNEE
        ,
        UP_PRODUIT
        ,
        VTE_BON_RETOUR
        ,
        STK_ARRIVAGE_INSTANCE
        ,
        ACH_DOCUMENT_DETAIL
        , // 
        ACH_COMMANDES_DETAILS
        ,
        VTE_COMPTOIR_TO_INSTANCE
        ,
        VTE_COMPTOIR_TO_CREDIT
        ,
        VTE_COMPTOIR_TO_CACHE
        ,
        STK_PRODUIT_FUSION
        ,
        SYS_EXPORT_XLS
        ,
        SYS_BASE_RESTORE
        ,
        SYS_BASE_CHANGE
        ,
        VTE_COMPTOIR_TRACE
        ,
        STK_STOCK_UPDATE_CB // Modification Code barre stock
        ,
        TDB_ANALYSES_CHIFR_VENDEUR
        ,
        TDB_ANALYSES_MARGE_VENDEUR
        ,
        TDB_CHIFFRE_DAFFAIRE_BY_MARGE
        ,
        VTE_ETAT_RECAP_VENTES_TAP_G50
        ,
        STK_ETAT_STOCK_FROM_DATE
        ,
        VTE_ETAT_RECAP_VENTES_TAP
        ,
        SYS_PRINT_CB_USER
        ,
        TRS_CATRE_FOURNISSUER
        ,
        RPT_JRN_VENTE_PRD
        ,
        RPT_JRN_VENTE_PRD_ACHAT
        ,
        RPT_JRN_VENTE_PRD_PPA
        ,
        RPT_JRN_VENTE_PRD_PPA_ACHAT
        ,
        TRS_ANNULATION_TRANSACTION_VC
        ,
        TRS_VALIDATION_TRANSACTION_VC
        ,
        rv_ORD_ANNEX_13 // annex 13 psychotrope
        ,
        rv_ORD_ANNEX_12
        ,
        ACH_FACTURE_PSYCHOTHROPE
        ,
        ACH_COMMANDES_PSYCHOTHROPE
        ,
        ENTREES_WEB_PSYCHOTHROPE
        ,
        VTE_ANNULER_VC
        ,
        VTE_SUPPRIMER_DETAIL_VC
        ,
        ACH_PARAMETRAGE_FACTURE_WEB
    }
    public enum Operator
    {
        EQUAL, NOT_EQUAL, NOT_EQUAL_WITH_TRIM, NOT_EQUAL_WITH_FIELD_ISNULL, EQUAL_WITH_FIELD_ISNULL, GREATER, LESS, GREATER_EQUAL, LESS_EQUAL, LIKE, LIKE_ANY, LIKE_RIGHT, LIKE_LEFT, NOT_LIKE_ANY, NOT_LIKE_RIGHT, NOT_LIKE_LEFT,
        BETWEEN, ISNULL,
        ISNOTNULL,
        IN,
        IN_WITH_FIELD_ISNULL,
        NOT_IN,
        GREATER_DATE_NOW,
        GREATER_DATE,
        GREATER_EQUAL_DATE_NOW,
        GREATER_EQUAL_DATE,
        EQUAL_DATE_NOW,
        LESS_DATE_NOW,
        LESS_DATE,
        LESS_EQUAL_DATE_NOW,
        LESS_EQUAL_DATE,        
        BETWEEN_DATE,BETWEEN_HOUR, 
        EQUAL_EMPTY, NOT_EMPTY,
        ISNULL_OR_EMPTY, ISNOT_NULL_OR_EMPTY,
        IS_NUMERIC, BeginsWith
    }
    public enum TypeConnector
    {
        AND, OR, NOT,
        NONE
    }
    public enum TypeParenthese
    {
        RIGHT, LEFT,
        NONE
    }

    public enum Sort
    {
        ASC, DESC
    }

    public enum TypesJoin
    {
        RIGHT, LEFT, INNER
    }

    public class XpertSqlBuilder
    {

        private string sqlQuery = "";
        private List<String> listQuotes = new List<string>();
        private Dictionary<string, object> listValues = new Dictionary<string, object>();
        public QueryInfos QueryInfos;
        public bool HasOperatorCond = false;

        public XpertSqlBuilder()
        {
            QueryInfos = new QueryInfos();
            this.InitQuery();
        }
        public void InitQuery()
        {
            QueryInfos.StringSelections = string.Empty;
            QueryInfos.StringCondition = string.Empty;
            this.HasOperatorCond = false;
            QueryInfos.StringGroupBy = string.Empty;
            QueryInfos.StringJoin = string.Empty;
            QueryInfos.StringOrderBy = string.Empty;
            QueryInfos.StringHaving = string.Empty;
            QueryInfos.StringPaging = string.Empty;
            this.listQuotes.Clear();
            this.listValues.Clear();
        }
        public void InitListValues()
        {
            this.listValues.Clear();
        }
        public void Add(string field, object value, bool addQuote)
        {
            if (this.listValues.ContainsKey(field))
            {
                this.listValues[field] = value;
            }
            else
            {
                this.listValues.Add(field, value);
            }
            if (addQuote)
            {
                this.AddQuotes(field);
            }
            else
            {
                this.RemoveQuotes(field);                
            }
                
        }

        public void Add(string field, object value)
        {
            this.Add(field, value, true);
        }

        public void Add<T1, P>(Expression<Func<T1, P>> field, object fieldValue, bool addQuote=true)
        {
            if (XpertHelper.IsNullOrEmpty(fieldValue))
            {
                return;
            }
            string fieldName = XpertHelper.GetPropertyName(field);
            this.Add(fieldName, fieldValue, addQuote);
        }

        private string UpdateValue(string field, object value)
        {
            return this.UpdateValueQuote(value, listQuotes.Contains(field));
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

        public string BuildQueryDelete(string tableName)
        {
            this.sqlQuery = "Delete FROM " + tableName;
            this.sqlQuery += string.IsNullOrEmpty(QueryInfos.StringCondition) ? "" : " WHERE " + QueryInfos.StringCondition;
            return this.sqlQuery;
        }
        public string BuildQueryUpdate(string tableName)
        {
            this.sqlQuery = "";
            string key;
            object value;
            bool first = true;
            if (listValues.Count == 0)
            {
                return this.sqlQuery;
            }
            this.sqlQuery = "UPDATE " + tableName + " SET ";

            foreach (KeyValuePair<string, object> keyvalue in listValues)
            {
                key = keyvalue.Key;
                value = keyvalue.Value;
                if (value != null)
                {
                    if (first)
                    {
                        first = false;
                        this.sqlQuery += " " + key + " = "
                                + this.UpdateValue(key, value);
                    }
                    else
                    {
                        this.sqlQuery += " , " + key + " = "
                                + this.UpdateValue(key, value);
                    }
                }
            }
            this.sqlQuery += string.IsNullOrEmpty(QueryInfos.StringCondition) ? "" : " WHERE " + QueryInfos.StringCondition;
            return this.sqlQuery;
        }
        public string BuildQueryInsert(string table)
        {
            this.sqlQuery = "INSERT INTO " + table + " (";
            string key;
            object value;
            string v = "";
            bool first = true;
            foreach (string key1 in listValues.Keys)
            {
                this.AddQuotes(key1);
            }
            foreach (KeyValuePair<string, object> keyvalue in listValues)
            {
                key = keyvalue.Key;
                value = keyvalue.Value;
                if (value != null)
                {
                    if (first)
                    {
                        this.sqlQuery += " " + key;
                        v += " " + this.UpdateValue(key, value);
                        first = false;
                    }
                    else
                    {
                        this.sqlQuery += " , " + key;
                        v += " , " + this.UpdateValue(key, value);
                    }
                }
            }
            this.sqlQuery += " ) VALUES (" + v + ")";
            return this.sqlQuery;
        }
        public string BuildQueryInsertOrUpdate(string tableName)
        {
            string query = "IF EXISTS (" + this.BuildQuerySelect(tableName) + ")";
            query += this.BuildQueryUpdate(tableName);
            query += " ELSE " + this.BuildQueryInsert(tableName);
            return query;
        }
        public string BuildQuerySelect(XpertObjets tableName)
        {
            return BuildQuerySelect(tableName.ToString());
        }
        public string BuildQuerySelect<T1>()
        {
            return BuildQuerySelect(this.GetTableName<T1>());
        }
        public string BuildQuerySelect(string tableName)
        {
            try
            {
                if (String.IsNullOrEmpty(tableName))
                {
                    throw new Exception("From Table invalide");
                }
                this.sqlQuery = QueryInfos.StringSelections;
                if (String.IsNullOrEmpty(QueryInfos.StringSelections))
                {
                    this.sqlQuery = "SELECT * ";
                }
                else
                {
                    this.sqlQuery = QueryInfos.StringSelections;
                }
                this.sqlQuery += " FROM " + tableName;
                this.sqlQuery += " " + QueryInfos.StringJoin;
                this.sqlQuery += string.IsNullOrEmpty(QueryInfos.StringCondition) ? "" : " WHERE " + QueryInfos.StringCondition;
                this.sqlQuery += " " + QueryInfos.StringGroupBy;
                this.sqlQuery += " " + QueryInfos.StringOrderBy;
                this.sqlQuery += " " + QueryInfos.StringHaving;
                this.sqlQuery += " " + QueryInfos.StringPaging;
                return this.sqlQuery;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string BuildQuerySelectFunction()
        {
            try
            {
                this.sqlQuery = QueryInfos.StringSelections;
                return this.sqlQuery;
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        private string GetTableName<T1>()
        {
            return typeof(T1).Name;
        }

        public string GetPropertyFullName<T1, P>(Expression<Func<T1, P>> field)
        {
            return this.GetTableName<T1>() + "." + XpertHelper.GetPropertyName(field);
        }

        public void AddSelect<T1, P>(Expression<Func<T1, P>> field)
        {
            string fieldName = this.GetPropertyFullName(field);
            this.AddSelect(fieldName);
        }

        public void AddSelect_All<T1>()
        {
            this.AddSelect(this.GetTableName<T1>() + "." + "*");
        }

        public void AddSelectCountAll()
        {
            QueryInfos.StringSelections = "SELECT Count(*)";
        }

        public void AddSelect(string expression)
        {
            if (String.IsNullOrEmpty(QueryInfos.StringSelections))
            {
                QueryInfos.StringSelections = "SELECT " + expression;
            }
            else
            {
                QueryInfos.StringSelections += " , " + expression;
            }
        }

        public void AddSelect_RIGHT<T1, P>(Expression<Func<T1, P>> field, string valAdded, int length)
        {
            string fullName = this.GetPropertyFullName(field);
            string name = XpertHelper.GetPropertyName(field);
            this.AddSelect(string.Format("RIGHT('{0}'+RTRIM(ISNULL({1},'')),{2}) {3}", valAdded, fullName, length, name));
        }

        public void AddConditionOperator(TypeConnector _connector, TypeParenthese _parentese)
        {
            if (String.IsNullOrEmpty(QueryInfos.StringCondition))
            {
                if (_parentese.Equals(TypeParenthese.LEFT))
                    QueryInfos.StringCondition = " (";
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
                QueryInfos.StringCondition += cc;
            }
            this.HasOperatorCond = _connector != TypeConnector.NONE || _parentese == TypeParenthese.LEFT ;
        }
        public void AddCondition(string expression)
        {
            if (String.IsNullOrEmpty(QueryInfos.StringCondition))
                QueryInfos.StringCondition = " " + expression;
            else if (this.HasOperatorCond)
                QueryInfos.StringCondition += " " + expression;
            else
                QueryInfos.StringCondition += " AND " + expression;
            this.HasOperatorCond = false;
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
                    this.AddCondition("ISNULL("+fieldName+",'') = ''");
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
                    this.AddCondition("ISNUMERIC("+fieldName+") = 1");
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
                        this.AddCondition(fieldName + " >= "+""+"'" + XpertHelper.GetSQLDate(value1, "HH:mm") + "'");
                    }
                    else
                    {
                        this.AddCondition(fieldName + " BETWEEN " +"'" + XpertHelper.GetSQLDate(value1, "HH:mm") +"'"+
                                                    " AND " +"'"+ XpertHelper.GetSQLDate(value2, "HH:mm") + "'");
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
            if (String.IsNullOrEmpty(QueryInfos.StringJoin))
                QueryInfos.StringJoin = expression;
            else QueryInfos.StringJoin += " " + expression;
        }

        internal void AddJoinCond(string srcField, object value, bool quote)
        {
            value = this.UpdateValueQuote(value, quote);
            QueryInfos.StringJoin += string.Format(" AND  {0} = {1}", srcField, value);
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
            if (String.IsNullOrEmpty(QueryInfos.StringOrderBy))
            {
                QueryInfos.StringOrderBy = "ORDER BY " + fieldName+" "+sort;
            }
            else if (!QueryInfos.StringOrderBy.Contains(fieldName))
            {
                QueryInfos.StringOrderBy += " , " + fieldName+" "+ sort;
            }
        }
        public void AddGroupBy(string expression)
        {
            if (String.IsNullOrEmpty(QueryInfos.StringGroupBy))
                QueryInfos.StringGroupBy = "GROUP BY " + expression;
            else QueryInfos.StringGroupBy += " , " + expression;
        }

        internal void AddHaving(string expression)
        {
            if (String.IsNullOrEmpty(QueryInfos.StringHaving))
                QueryInfos.StringHaving = "HAVING " + expression;
            else QueryInfos.StringHaving = " AND " + expression;
        }

        public void AddPaging(int index, int nbrRows)
        {
            if (index > 0 && nbrRows > 0)
            {
                QueryInfos.StringHaving = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", index - 1, nbrRows);
            }
        }

        internal void AddQuotes(string key)
        {
            if (this.listQuotes.Contains(key)) return;
            this.listQuotes.Add(key);
        }
        internal void RemoveQuotes(string key)
        {
            if (this.listQuotes.Contains(key))
            {
                this.listQuotes.Remove(key);
            }
        }

        internal bool HasCondition()
        {
            return !String.IsNullOrEmpty(QueryInfos.StringCondition);
        }
        public string GetConditions()
        {
            return QueryInfos.StringCondition;
        }

        public object Get(string fieldName)
        {
            if (this.listValues.ContainsKey(fieldName))
                return this.listValues[fieldName];
            return null;
        }

        public void AddValueToField(string fieldName, object value)
        {
            string dd = Convert.ToString(value).Replace(",", ".");
            this.Add(fieldName, fieldName + "+" + dd, false);
        }

        internal void InitAddSelect()
        {
            QueryInfos.StringSelections = "";
        }

        public void SetQuery(string query)
        {
            this.sqlQuery = query;
        }        
    }
}
