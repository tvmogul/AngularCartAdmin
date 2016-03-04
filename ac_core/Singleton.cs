using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Security.Cryptography;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Linq;

//using QZFW = QZFramework;


    /// <summary>
    /// Summary description for CPSingleton
    /// Dim oSingleton As singleton = singleton.GetCurrentSingleton()
    /// singleton oSingleton = singleton.GetCurrentSingleton();
    /// </summary>
    [Serializable]
    public class singleton
    {
        //Name that will be used as key for Session object
        private const string SESSION_SINGLETON = "SINGLETON";

        //Dictionary<string, string> landingPages = new Dictionary<string, string>();

        static bool isStringInArray(string[] strArray, string key)
        {
            if (strArray.Contains(key))
            {
                return true;
            }
            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        string ww_ppBussiness = "your_email_address";
        public string ww_PPbusiness
        {
            get { return ww_ppBussiness; }
            set { ww_ppBussiness = value; }
        }

        // string invoice = "VisibleHair_" + System.Guid.NewGuid().ToString();
        // sb.Append("&invoice=" + invoice);
        string ww_invoice = string.Empty;
        public string ww_Invoice
        {
            get { return ww_invoice; }
            set { ww_invoice = value; }
        }


        // string custom = System.Guid.NewGuid().ToString();
        // sb.Append("&custom=" + custom);
        string ww_custom = string.Empty;
        public string ww_Custom
        {
            get { return ww_custom; }
            set { ww_custom = value; }
        }

        string ww_orderID = string.Empty;
        public string ww_OrderID
        {
            get { return ww_orderID; }
            set { ww_orderID = value; }
        }

        //currency_code=USD");
        //arParms[18] = new SqlParameter("@CurrencyCode", SqlDbType.Decimal);
        //arParms[18].Value = oSingleton.ZCurrencyCode;
        string ww_currencyCode = "USD";
        public string ww_CurrencyCode
        {
            get { return ww_currencyCode; }
            set { ww_currencyCode = value; }
        }

        double ww_uUnitPrice = .99d;
        public double ww_UnitPrice
        {
            get { return ww_uUnitPrice; }
            set { ww_uUnitPrice = value; }
        }


        int ww_units = 1;
        public int ww_Units
        {
            get { return ww_units; }
            set { ww_units = value; }
        }

        double ww_amount = 29.95d;
        public double ww_Amount
        {
            get { return ww_amount; }
            set { ww_amount = value; }
        }

        double ww_shipping = 6.95d;
        public double ww_Shipping
        {
            get { return ww_shipping; }
            set { ww_shipping = value; }
        }

        double ww_handling = 0.00d;
        public double ww_Handing
        {
            get { return ww_handling; }
            set { ww_handling = value; }
        }

        double ww_sh = 0.00d;
        public double ww_SH
        {
            get { return ww_sh; }
            set { ww_sh = value; }
        }

        double ww_total = 0.00d;
        public double ww_Total
        {
            get { return ww_total; }
            set { ww_total = value; }
        }

        double ww_taxRate = 0.00d;
        public double ww_TaxRate
        {
            get { return ww_taxRate; }
            set { ww_taxRate = value; }
        }

        string ww_taxState = string.Empty;
        public string ww_TaxState
        {
            get { return ww_taxState; }
            set { ww_taxState = value; }
        }

        string ww_promoDist = string.Empty;
        public string ww_PromoDist
        {
            get { return ww_promoDist; }
            set { ww_promoDist = value; }
        }

        string ww_promoTV = string.Empty;
        public string ww_PromoTV
        {
            get { return ww_promoTV; }
            set { ww_promoTV = value; }
        }

        string ww_promoWeb = string.Empty;
        public string ww_PromoWeb
        {
            get { return ww_promoWeb; }
            set { ww_promoWeb = value; }
        }

        string ww_shipFirstName = string.Empty;
        public string ww_ShipFirstName
        {
            get { return ww_shipFirstName; }
            set { ww_shipFirstName = value; }
        }

        string ww_shipLastName = string.Empty;
        public string ww_ShipLastName
        {
            get { return ww_shipLastName; }
            set { ww_shipLastName = value; }
        }

        string ww_shipAddress1 = string.Empty;
        public string ww_ShipAddress1
        {
            get { return ww_shipAddress1; }
            set { ww_shipAddress1 = value; }
        }

        string ww_shipAddress2 = string.Empty;
        public string ww_ShipAddress2
        {
            get { return ww_shipAddress2; }
            set { ww_shipAddress2 = value; }
        }

        string ww_shipCity = string.Empty;
        public string ww_ShipCity
        {
            get { return ww_shipCity; }
            set { ww_shipCity = value; }
        }

        string ww_shipStateProvince = string.Empty;
        public string ww_ShipStateProvince
        {
            get { return ww_shipStateProvince; }
            set { ww_shipStateProvince = value; }
        }

        string ww_shipPostalCode = string.Empty;
        public string ww_ShipPostalCode
        {
            get { return ww_shipPostalCode; }
            set { ww_shipPostalCode = value; }
        }

        string ww_shipCountryRegion = string.Empty;
        public string ww_ShipCountryRegion
        {
            get { return ww_shipCountryRegion; }
            set { ww_shipCountryRegion = value; }
        }

        //txtEveningPhoneS
        string ww_shipPhoneType = string.Empty;
        public string ww_ShipPhoneType
        {
            get { return ww_shipPhoneType; }
            set { ww_shipPhoneType = value; }
        }

        //txtDaytimePhoneS
        string ww_shipPhone = string.Empty;
        public string ww_ShipPhone
        {
            get { return ww_shipPhone; }
            set { ww_shipPhone = value; }
        }

        string ww_shipEmailAddress = string.Empty;
        public string ww_ShipEmailAddress
        {
            get { return ww_shipEmailAddress; }
            set { ww_shipEmailAddress = value; }
        }

        string ww_shipEmailVerify = string.Empty;
        public string ww_ShipEmailVerify
        {
            get { return ww_shipEmailVerify; }
            set { ww_shipEmailVerify = value; }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        
        
        bool _isOrderFaux = false;
        public bool ZIsOrderFaux
        {
            get { return _isOrderFaux; }
            set { _isOrderFaux = value; }
        }

        string[] rolesArray;
        public string[] ZRolesArray
        {
            get
            {
                try
                {
                    ZRolesArray = System.Web.Security.Roles.GetAllRoles();
                }
                catch (HttpException e)
                {
                }
                return rolesArray;
            }
            set { rolesArray = value; }
        }

        public bool ZIsInRoleEx(string key)
        {
            if (ZRolesArray.Contains(key))
            {
                return true;
            }
            return false;
        }

        bool _useDates = false;
        public bool ZUseDates
        {
            get { return _useDates; }
            set { _useDates = value; }
        }


        DateTime _startDate = DateTime.UtcNow;
        public DateTime ZStartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        DateTime _endDate = DateTime.Now;
        public DateTime ZEndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        string _startDateString = DateTime.UtcNow.ToShortDateString();
        public string ZStartDateString
        {
            get { return _startDateString; }
            set { _startDateString = value; }
        }

        string _endDateString = DateTime.Now.ToLongDateString();
        public string ZEndDateString
        {
            get { return _endDateString; }
            set { _endDateString = value; }
        }

        string _username = string.Empty;
        public string ZUserName
        {
            get { return _username; }
            set { _username = value; }
        }

        string _parameters = string.Empty;
        public string ZParameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        string[] _landingPages = new string[] { "default.aspx", "default2.aspx" };
        public string[] ZLandingPages
        {
            get { return _landingPages; }
            set { _landingPages = value; }
        }

        string _landingPage = "default.aspx";
        public string ZLandingPage
        {
            get { return _landingPage; }
            set { _landingPage = value; }
        }

        string _refHost = string.Empty;
        public string ZReferHost
        {
            get { return _refHost; }
            set { _refHost = value; }
        }

        string _refUrl = string.Empty;
        public string ZReferUrl
        {
            get { return _refUrl; }
            set { _refUrl = value; }
        }

        string _visitorID = string.Empty;
        public string ZVisitorID
        {
            get { return _visitorID; }
            set { _visitorID = value; }
        }

        string _orderID = string.Empty;
        public string ZOrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        string _adUrl = string.Empty;
        public string ZAdUrl
        {
            get { return _adUrl; }
            set { _adUrl = value; }
        }

        string _shipName = string.Empty;
        public string ShipName
        {
            get { return _shipName; }
            set { _shipName = value; }
        }


        // string invoice = "VisibleHair_" + System.Guid.NewGuid().ToString();
        // sb.Append("&invoice=" + invoice);
        string _invoice = string.Empty;
        public string ZInvoice
        {
            get { return _invoice; }
            set { _invoice = value; }
        }

        // string custom = System.Guid.NewGuid().ToString();
        // sb.Append("&custom=" + custom);
        string _custom = string.Empty;
        public string ZCustom
        {
            get { return _custom; }
            set { _custom = value; }
        }

        string _productName = string.Empty;
        public string ZProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        string _productCode = string.Empty;
        public string ZProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        string _productID = string.Empty;
        public string ZProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        
        string _ppBussiness= string.Empty;
        public string ZPPbusiness
        {
            get { return _ppBussiness; }
            set { _ppBussiness = value; }
        }

        string _ppItem_name = string.Empty;
        public string ZPPItem_name
        {
            get { return _ppItem_name; }
            set { _ppItem_name = value; }
        }

        string _ppItem_number = string.Empty;
        public string ZPPItem_number
        {
            get { return _ppItem_number; }
            set { _ppItem_number = value; }
        }

        //currency_code=USD");
        //arParms[18] = new SqlParameter("@CurrencyCode", SqlDbType.Decimal);
        //arParms[18].Value = oSingleton.ZCurrencyCode;
        string _currencyCode = "USD";
        public string ZCurrencyCode
        {
            get { return _currencyCode; }
            set { _currencyCode = value; }
        }

        double _unitPrice = 29.95d;
        public double ZUnitPrice
        {
            get { return _unitPrice; }
            set { _unitPrice = value; }
        }


        int _units = 1;
        public int ZUnits
        {
            get { return _units; }
            set { _units = value; }
        }

        double _amount = 29.95d;
        public double ZAmount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        double _shipping = 6.95d;
        public double ZShipping
        {
            get { return _shipping; }
            set { _shipping = value; }
        }

        double _handling = 0.00d;
        public double ZHanding
        {
            get { return _handling; }
            set { _handling = value; }
        }

        double _sh = 0.00d;
        public double ZSH
        {
            get { return _sh; }
            set { _sh = value; }
        }

        double _total = 0.00d;
        public double ZTotal
        {
            get { return _total; }
            set { _total = value; }
        }

        double _taxRate = 0.00d;
        public double ZTaxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; }
        }

        string _taxState = string.Empty;
        public string ZTaxState
        {
            get { return _taxState; }
            set { _taxState = value; }
        }

        string _hairColor = string.Empty;
        public string ZHairColor
        {
            get { return _hairColor; }
            set { _hairColor = value; }
        }

        string _promoDist = string.Empty;
        public string ZPromoDist
        {
            get { return _promoDist; }
            set { _promoDist = value; }
        }

        string _promoTV = string.Empty;
        public string ZPromoTV
        {
            get { return _promoTV; }
            set { _promoTV = value; }
        }

        string _promoWeb = string.Empty;
        public string ZPromoWeb
        {
            get { return _promoWeb; }
            set { _promoWeb = value; }
        }

        string _shipFirstName = string.Empty;
        public string ShipFirstName
        {
            get { return _shipFirstName; }
            set { _shipFirstName = value; }
        }

        string _shipLastName = string.Empty;
        public string ShipLastName
        {
            get { return _shipLastName; }
            set { _shipLastName = value; }
        }

        string _shipAddress1 = string.Empty;
        public string ShipAddress1
        {
            get { return _shipAddress1; }
            set { _shipAddress1 = value; }
        }

        string _shipAddress2 = string.Empty;
        public string ShipAddress2
        {
            get { return _shipAddress2; }
            set { _shipAddress2 = value; }
        }

        string _shipCity = string.Empty;
        public string ShipCity
        {
            get { return _shipCity; }
            set { _shipCity = value; }
        }

        string _shipStateProvince = string.Empty;
        public string ShipStateProvince
        {
            get { return _shipStateProvince; }
            set { _shipStateProvince = value; }
        }

        string _shipPostalCode = string.Empty;
        public string ShipPostalCode
        {
            get { return _shipPostalCode; }
            set { _shipPostalCode = value; }
        }

        string _shipCountryRegion = string.Empty;
        public string ShipCountryRegion
        {
            get { return _shipCountryRegion; }
            set { _shipCountryRegion = value; }
        }

        //txtEveningPhoneS
        string _shipPhoneType = string.Empty;
        public string ShipPhoneType
        {
            get { return _shipPhoneType; }
            set { _shipPhoneType = value; }
        }

        //txtDaytimePhoneS
        string _shipPhone = string.Empty;
        public string ShipPhone
        {
            get { return _shipPhone; }
            set { _shipPhone = value; }
        }

        string _shipEmailAddress = string.Empty;
        public string ShipEmailAddress
        {
            get { return _shipEmailAddress; }
            set { _shipEmailAddress = value; }
        }

        string _shipEmailVerify = string.Empty;
        public string ShipEmailVerify
        {
            get { return _shipEmailVerify; }
            set { _shipEmailVerify = value; }
        }


        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////


        protected System.Data.DataSet dsProduct = new System.Data.DataSet();

        string _billFirstName = string.Empty;
        public string BillFirstName
        {
            get { return _billFirstName; }
            set { _billFirstName = value; }
        }

        string _billLastName = string.Empty;
        public string BillLastName
        {
            get { return _billLastName; }
            set { _billLastName = value; }
        }

        string _billEmail = string.Empty;
        public string BillEmail
        {
            get { return _billEmail; }
            set { _billEmail = value; }
        }

        string _billEmailVerify = string.Empty;
        public string BillEmailVerify
        {
            get { return _billEmailVerify; }
            set { _billEmailVerify = value; }
        }

        string _billEveningPhone = string.Empty;
        public string BillEveningPhone
        {
            get { return _billEveningPhone; }
            set { _billEveningPhone = value; }
        }

        string _billDayPhone = string.Empty;
        public string BillDayPhone
        {
            get { return _billDayPhone; }
            set { _billDayPhone = value; }
        }

        string _billAddress1 = string.Empty;
        public string BillAddress1
        {
            get { return _billAddress1; }
            set { _billAddress1 = value; }
        }

        string _billAddress2 = string.Empty;
        public string BillAddress2
        {
            get { return _billAddress2; }
            set { _billAddress2 = value; }
        }

        string _billPostalCode = string.Empty;
        public string BillPostalCode
        {
            get { return _billPostalCode; }
            set { _billPostalCode = value; }
        }

        string _billCity = string.Empty;
        public string BillCity
        {
            get { return _billCity; }
            set { _billCity = value; }
        }

        string _billState = string.Empty;
        public string BillState
        {
            get { return _billState; }
            set { _billState = value; }
        }

        string _billCountry = string.Empty;
        public string BillCountry
        {
            get { return _billCountry; }
            set { _billCountry = value; }
        }


        string _HTTP_REFERER = string.Empty;
        public string ZHTTP_REFERER
        {
            get { return _HTTP_REFERER; }
            set { _HTTP_REFERER = value; }
        }

        string _ipAddress = string.Empty;
        public string ZIPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        string _browser = string.Empty;
        public string ZBrowser
        {
            get { return _browser; }
            set { _browser = value; }
        }

        string _cartID = string.Empty;
        public string ZCartID
        {
            get { return _cartID; }
            set { _cartID = value; }
        }

        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        string _cardType = string.Empty;
        public string CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        string _cardExpMonth = string.Empty;
        public string CardExpMonth
        {
            get { return _cardExpMonth; }
            set { _cardExpMonth = value; }
        }

        string _cardExpYear = string.Empty;
        public string CardExpYear
        {
            get { return _cardExpYear; }
            set { _cardExpYear = value; }
        }

        int _cardUnits = 1;
        public int CardUnits
        {
            get { return _cardUnits; }
            set { _cardUnits = value; }
        }

        decimal _cardPrice = (decimal)0.00;
        public decimal CardPrice
        {
            get { return _cardPrice; }
            set { _cardPrice = value; }
        }

        decimal _cardSH = (decimal)0.00;
        public decimal CardSH
        {
            get { return _cardSH; }
            set { _cardSH = value; }
        }

        decimal _cardTotal = (decimal)0.00;
        public decimal CardTotal
        {
            get { return _cardTotal; }
            set { _cardTotal = value; }
        }

        decimal _cardAmount = (decimal)0.00;
        public decimal CardAmount
        {
            get { return _cardAmount; }
            set { _cardAmount = value; }
        }

        string _cardFullName = string.Empty;
        public string CardFullName
        {
            get { return _cardFullName; }
            set { _cardFullName = value; }
        }

        string _cardNumber = string.Empty;
        public string CardNumber
        {
            get { return _cardNumber; }
            set { _cardNumber = value; }
        }

        string _cardSecurityCode = string.Empty;
        public string CardSecurityCode
        {
            get { return _cardSecurityCode; }
            set { _cardSecurityCode = value; }
        }

        string _cardOrderNumber = string.Empty;
        public string CardOrderNumber
        {
            get { return _cardOrderNumber; }
            set { _cardOrderNumber = value; }
        }

        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////


        //Variables to store data (used to be individual session key/value pairs)
        bool bIsBuyer = false;
        bool bIsSysop = false;
        bool bIsAdmin = false;

        string clientCode = "";
        string lastName = "";
        string firstName = "";
        bool bSortAscending = false;

        string sCulture = string.Empty;
        public string Culture
        {
            get { return sCulture; }
            set { sCulture = value; }
        }

        bool bActivateSite = false;
        public bool ActivateSite
        {
            get { return bActivateSite; }
            set { bActivateSite = value; }
        }

        bool bClient = false;
        public bool Client
        {
            get { return bClient; }
            set { bClient = value; }
        }

        int iUserid = 0;
        public int Userid
        {
            get { return iUserid; }
            set { iUserid = value; }
        }


        string sClientcode = string.Empty;
        public string Clientcode
        {
            get { return sClientcode; }
            set { sClientcode = value; }
        }


        public bool Buyer
        {
            get { return bIsBuyer; }
            set { bIsBuyer = value; }
        }


        public System.Data.DataSet Product
        {
            get { return dsProduct; }
            set { dsProduct = value; }
        }


        public bool Sysop
        {
            get { return bIsSysop; }
            set { bIsSysop = value; }
        }

        public bool Admin
        {
            get { return bIsAdmin; }
            set { bIsAdmin = value; }
        }

        public string ClientCode
        {
            get { return clientCode; }
            set { clientCode = value; }
        }


        public bool SortAscending
        {
            get { return bSortAscending; }
            set { bSortAscending = value; }
        }


        string _ProfileId = string.Empty;
        public string ProfileId
        {
            get { return _ProfileId; }
            set { _ProfileId = value; }
        }

        string _FromProfileId = string.Empty;
        public string FromProfileId
        {
            get { return _FromProfileId; }
            set { _FromProfileId = value; }
        }

        int _Sex = 1;
        public int Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        int _LookingFor = 0;
        public int LookingFor
        {
            get { return _LookingFor; }
            set { _LookingFor = value; }
        }

        //PartnerID
        string _PartnerID = string.Empty;
        public string PartnerID
        {
            get { return _PartnerID; }
            set { _PartnerID = value; }
        }

        string _Username = string.Empty;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        string _Password = string.Empty;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        string _PasswordHash = string.Empty;
        public string PasswordHash
        {
            get { return _PasswordHash; }
            set { _PasswordHash = value; }
        }


        string _Day = string.Empty;
        public string Day
        {
            get { return _Day; }
            set { _Day = value; }
        }

        string _Month = string.Empty;
        public string Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        string _Year = string.Empty;
        public string Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        int _EmailNewMsgs = 1;
        public int EmailNewMsgs
        {
            get { return _EmailNewMsgs; }
            set { _EmailNewMsgs = value; }
        }

        int _OptIn = 1;
        public int OptIn
        {
            get { return _OptIn; }
            set { _OptIn = value; }
        }

        DateTime _Birthday = DateTime.Now;
        public DateTime Birthday
        {
            get { return _Birthday; }
            set { _Birthday = value; }
        }

        string _Sign = string.Empty;
        public string Sign
        {
            get { return _Sign; }
            set { _Sign = value; }
        }

        int _Locked = 1;
        public int Locked
        {
            get { return _Locked; }
            set { _Locked = value; }
        }

        int _Blocked = 0;
        public int Blocked
        {
            get { return _Blocked; }
            set { _Blocked = value; }
        }


        bool _RememberUsername = false;
        public bool RememberUsername
        {
            get { return _RememberUsername; }
            set { _RememberUsername = value; }
        }

        string _sLinkType = "";
        public string ZLinkType
        {
            get { return _sLinkType; }
            set { _sLinkType = value; }
        }

        string _sCategory = "ALL";
        public string ZCategory
        {
            get { return _sCategory; }
            set { _sCategory = value; }
        }

        string _sFeedId = "";
        public string ZFeedId
        {
            get { return _sFeedId; }
            set { _sFeedId = value; }
        }


        #region ===================== TV STATIONS ===============================================================

        string _sFeedID = "00000000-0000-0000-0000-000000000000";
        public string ZFeedIDString
        {
            get { return _sFeedID; }
            set { _sFeedID = value; }
        }

        System.Guid _guidStationID = System.Guid.Empty;
        public System.Guid ZSStationIDGuid
        {
            get { return _guidStationID; }
            set { _guidStationID = value; }
        }

        string _sMediaType = "TVH";
        public string ZSMediaType
        {
            get { return _sMediaType; }
            set { _sMediaType = value; }
        }

        string _sCountry = "USA";
        public string ZSCountry
        {
            get { return _sCountry; }
            set { _sCountry = value; }
        }

        string _sState = "CA";
        public string ZSState
        {
            get { return _sState; }
            set { _sState = value; }
        }

        bool _sIsTest = false;
        public bool ZSIsTest
        {
            get { return _sIsTest; }
            set { _sIsTest = value; }
        }

        bool _sIsPerInquiry = false;
        public bool ZSIsPerInquiry
        {
            get { return _sIsPerInquiry; }
            set { _sIsPerInquiry = value; }
        }

        bool _sIsBought = false;
        public bool ZSIsBought
        {
            get { return _sIsBought; }
            set { _sIsBought = value; }
        }

        bool _sIsCallSign = false;
        public bool ZSIsCallSign
        {
            get { return _sIsCallSign; }
            set { _sIsCallSign = value; }
        }
        string _sCallSign = string.Empty;
        public string ZSCallSign
        {
            get { return _sCallSign; }
            set { _sCallSign = value; }
        }


        int _sPageCurrent = 1;
        public int ZSPageCurrent
        {
            get { return _sPageCurrent; }
            set { _sPageCurrent = value; }
        }

        int _sPageSize = 10;
        public int ZSPageSize
        {
            get { return _sPageSize; }
            set { _sPageSize = value; }
        }

        int _allclients = 0;
        public int ZSAllCients
        {
            get { return _allclients; }
            set { _allclients = value; }
        }

        string _sStringClientCodeID = string.Empty;
        public string ZSStringClientCodeID
        {
            get { return _sStringClientCodeID; }
            set { _sStringClientCodeID = value; }
        }

        //System.Guid _sGuidClientCodeID = System.Guid.Empty;
        //public System.Guid ZSGuidClientCodeID
        //{
        //    get { return _sGuidClientCodeID; }
        //    set { _sGuidClientCodeID = value; }
        //}

        int _allshows = 0;
        public int ZSAllShows
        {
            get { return _allshows; }
            set { _allshows = value; }
        }

        string _sStringShowCodeID = string.Empty;
        public string ZSStringShowCodeID
        {
            get { return _sStringShowCodeID; }
            set { _sStringShowCodeID = value; }
        }

        //System.Guid _sGuidShowCodeID = System.Guid.Empty;
        //public System.Guid ZSGuidShowCodeID
        //{
        //    get { return _sGuidShowCodeID; }
        //    set { _sGuidShowCodeID = value; }
        //}

        string _sBuyType = string.Empty;
        public string ZSBuyType
        {
            get { return _sBuyType; }
            set { _sBuyType = value; }
        }

        string _sBkStatus = string.Empty;
        public string ZSBkStatus
        {
            get { return _sBkStatus; }
            set { _sBkStatus = value; }
        }






        #endregion ================= END TV STATIONS ===========================================================








        //////////////////////////////////////////////////////
        //////////////////////////////////////////////////////
        //////////////////////////////////////////////////////

        ArrayList MessagesIDList = new ArrayList();
        public ArrayList messagesIDList
        {
            get
            {
                return MessagesIDList;
            }
            set
            {
                MessagesIDList = value;
            }
        }


        // Private constructor so cannot create an instance
        // without using correct method which is critical to 
        // implementing this as a singleton object which cannot 
        // be created from outside this class
        private singleton()
        {
        }

        //Create as a static method so this can be called using
        // just the class name (no object instance is required).
        // It simplifies other code because it will always return
        // the single instance of this class, either newly created
        // or from the session
        public static singleton GetCurrentSingleton()
        {
            singleton oSingleton;

            if (System.Web.HttpContext.Current.Session != null 
                && System.Web.HttpContext.Current.Session[SESSION_SINGLETON] != null)
            {
                //Retrieve the already instance that was already created
                oSingleton = (singleton)System.Web.HttpContext.Current.Session[SESSION_SINGLETON];
                
            }
            else
            {
                //No current session object exists, use private constructor to 
                // create an instance, place it into the session
                oSingleton = new singleton();
                if(System.Web.HttpContext.Current.Session != null)
                    System.Web.HttpContext.Current.Session[SESSION_SINGLETON] = oSingleton;
            }

            //if (null == System.Web.HttpContext.Current.Session[SESSION_SINGLETON])
            //{
            //    //No current session object exists, use private constructor to 
            //    // create an instance, place it into the session
            //    oSingleton = new singleton();
            //    System.Web.HttpContext.Current.Session[SESSION_SINGLETON] = oSingleton;
            //}
            //else
            //{
            //    //Retrieve the already instance that was already created
            //    oSingleton = (singleton)System.Web.HttpContext.Current.Session[SESSION_SINGLETON];
            //}

            //Return the single instance of this class that was stored in the session
            return oSingleton;
        }

        public static void Dispose()
        {
            //Cleanup this object so that GC can reclaim space
            System.Web.HttpContext.Current.Session.Remove(SESSION_SINGLETON);
        }

    }



public static class MyGlobals {
    public const string Prefix = "ID_"; // cannot change
    public static int Total = 1; // can change because not const
}

//string strStuff = MyGlobals.Prefix + "something";
//textBox1.Text = "total of " + MyGlobals.Total.ToString();

class CStationBreakConfig
{
    //List<string> myNewList = CStationBreakConfig.Clone(myOldList);

    static public List<T> Clone<T>(IEnumerable<T> oldList)
    {
        return new List<T>(oldList);
    }

    static public String GetConnectionString(string _connectionStringsName)
    {
        try
        {
            System.Configuration.ConnectionStringSettingsCollection config = System.Configuration.ConfigurationManager.ConnectionStrings;
            for (int i = 0; i < config.Count; i++)
            {
                if (config[i].Name.Equals(_connectionStringsName, StringComparison.OrdinalIgnoreCase))
                    return config[i].ToString();
            }
        }
        catch
        {
        }
        return String.Empty;
    }


}