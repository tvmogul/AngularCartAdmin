'use strict';

// App Module: the name AngularStore matches the ng-app attribute in the main <html> tag
// the route provides parses the URL and injects the appropriate partial page
// Note: I don't recommend passing in'ui.bootstrap' as a dependency UNLESS you love update issues
//
var storeApp = angular.module('AngularStore', ['ngRoute', 'storeApp.config', 'slick', 'favicon', 'igTruncate', 'autoActive', 'ngWig']).
config(['$routeProvider', function ($routeProvider) {
    //alert("AngularStore");
    $routeProvider.
        when('/store', {
            templateUrl: 'ac_partials/editor-welcome.htm',
            controller: 'storeController'
        }).
        when('/product/:productSku', {
            templateUrl: 'ac_partials/editor-details.htm',
            controller: 'storeController'
        }).
        otherwise({
            redirectTo: '/store'
        });
    } 
]);


// I created a data service that provides the store and a shopping cart that
// will be shared by all views (instead of creating fresh ones for each view).
storeApp.factory('DataService', function ($http, $q, CONFIG) {

    // Store can be loadded from a JSON file, a local .mdf database, or a remote database.
    //'CF_DATA_FILE': 'ac_products/products.js',
    //'CF_DATA_LOCAL': '/crud.ashx?ac=getproducts&cn=local',
    //'CF_DATA_REMOTE': '/crud.ashx?ac=getproducts&cn=remote',

    var dataIndex = localStorage["data_src_index"];
    if (dataIndex == null || dataIndex == "undefined") {
        dataIndex = 0;
        localStorage["data_src_index"] = 0;
    }

    var dataSource = CONFIG.CF_DATA_FILE;
    if (dataIndex == 0) {
        dataSource = "/" + CONFIG.CF_DATA_FILE + "?rnd=" + new Date().getTime();
    }
    else if (dataIndex == 1) {
        dataSource = CONFIG.CF_DATA_LOCAL;
    }
    else if (dataIndex == 2) {
        dataSource = CONFIG.CF_DATA_REMOTE;
    }

    function Store() {
        var productsDeferred = $q.defer();
        this.products = productsDeferred.promise; //this.products is a promise

        $http.get(dataSource).success(function (data, status, headers, config) {
            var products = [];
            for (var i = 0, len = data.length; i < len; i++) {
                var prod = data[i];
                if (prod.storeid == "7cc6cb94-0938-4675-b84e-6b97ada53978") {
                    var _expecteddate = new Date(prod.expecteddate);
                    prod.expecteddate = _expecteddate;
                    products.push(prod);
                }
            }
            productsDeferred.resolve(products);
        }).error(function (data, status, headers, config) {
            alert("Please check you have updated ConnectionString with YOUR OWN information!");
            // Please note that in this sample project you need to update "remoteCartConnectionString" with your own data !!!
            //<add name="remoteCartConnectionString" connectionString="ADD_YOUR_CONNECTION_STRING\SQLEXPRESS;Initial Catalog=ACart;user=YOUR_USERNAME;pwd=YOUR_PASSWORD" providerName="System.Data.SqlClient"/>
        });
    }

    Store.prototype.getProduct = function (sku) {
        return this.products.then(function (products) {
            for (var i = 0; i < products.length; i++) { // MUST use products, it's the real value; this.products is a promise
                if (products[i].sku == sku) {
                    var _expecteddate = new Date(products[i].expecteddate);
                    products[i].expecteddate = _expecteddate;
                    return products[i];
                }  
            }
            return null;
        });
    };

    Store.prototype.getProducts = function () {
        return this.products.then(function (products) {
            var _expecteddate = new Date(products.expecteddate);
            products.expecteddate = _expecteddate;
            return products;
        });
    };

    // create store
    var myStore = new Store();

    // create shopping cart
    var myCart = new shoppingCart("AngularStore");

    // enable PayPal checkout
    // note: the second parameter identifies the merchant; in order to use the 
    // shopping cart with PayPal, you have to create a merchant account with 
    // PayPal. You can do that here:
    // https://www.paypal.com/webapps/mpp/merchant
    //myCart.addCheckoutParameters("PayPal", "paypaluser@youremail.com");
    myCart.addCheckoutParameters("PayPal", CONFIG.CF_PAYMENT_PAYPAL_BUYNOW);

    // enable Google Wallet checkout
    // note: the second parameter identifies the merchant; in order to use the 
    // shopping cart with Google Wallet, you have to create a merchant account with 
    // Google. You can do that here:
    // https://developers.google.com/commerce/wallet/digital/training/getting-started/merchant-setup
    //myCart.addCheckoutParameters("Google", "GooGle_Wallet_ID",
    myCart.addCheckoutParameters("Google", CONFIG.CF_PAYMENT_GOOGLE_WALLET_ID,
        {
            ship_method_name_1: "UPS Next Day Air",
            ship_method_price_1: "20.00",
            ship_method_currency_1: "USD",
            ship_method_name_2: "UPS Ground",
            ship_method_price_2: "15.00",
            ship_method_currency_2: "USD"
        }
    );

    // enable Stripe checkout
    // note: the second parameter identifies your publishable key; in order to use the 
    // shopping cart with Stripe, you have to create a merchant account with 
    // Stripe. You can do that here:
    // https://manage.stripe.com/register
    //myCart.addCheckoutParameters("Stripe", "pk_test_stripe",
    myCart.addCheckoutParameters("Stripe", CONFIG.CF_PAYMENT_STRIPE,
        {
            chargeurl: "https://localhost:1234/processStripe.aspx"
        }
    );

    // return data object with store and cart
    return {
        store: myStore,
        cart: myCart
    };
});

// the storeController contains two objects & the DataService:
// - store: contains the product list
// - cart: the shopping cart object
// - DataService: called to retrieve products from JSON file
storeApp.controller('storeController', function ($scope, $filter, $routeParams, $location, DataService, $sce, $timeout, $templateCache, CONFIG) {

    //alert("storeController");
    $scope.dataLoaded = false;

    var dataIndex = localStorage["data_src_index"];
    if (dataIndex == null || dataIndex == "undefined") {
        dataIndex = 0;
        localStorage["data_src_index"] = 0;
    }
    $scope.selectedDataIndex = dataIndex;


    /*#####################
    CONFIG
    ######################*/
    /* our global variabls */
    //$scope.DATA_SOURCE = CONFIG.CF_DATA_SOURCE;
    $scope.STORE_ID = CONFIG.CF_STORE_ID;
    $scope.STORE_PAGE = CONFIG.CF_STORE_PAGE;
    $scope.STORE_BG_IMAGE = CONFIG.CF_STORE_BG_IMAGE;
    $scope.DISTRIBUTOR_ID = CONFIG.CF_DISTRIBUTOR_ID;
    $scope.PAYMENT_PAYPAL_BUYNOW = CONFIG.CF_PAYMENT_PAYPAL_BUYNOW;
    $scope.PAYMENT_GOOGLE_WALLET_ID = CONFIG.CF_PAYMENT_GOOGLE_WALLET_ID;
    $scope.PAYMENT_STRIPE = CONFIG.CF_PAYMENT_STRIPE;
    $scope.PRODUCTS_FOLDER = CONFIG.CF_PRODUCTS_FOLDER;
    $scope.NAVBAR_THEME = CONFIG.CF_NAVBAR_THEME;
    $scope.NAVBAR_LOGO_IMAGE = CONFIG.CF_NAVBAR_LOGO_IMAGE;
    $scope.NAVBAR_LOGO_TEXT = CONFIG.CF_NAVBAR_LOGO_TEXT;
    $scope.NAVBAR_LOGO_LINK = CONFIG.CF_NAVBAR_LOGO_LINK;
    $scope.INSIDE_HEADER_SHOW = CONFIG.CF_INSIDE_HEADER_SHOW;
    $scope.INSIDE_HEADER_LINK = CONFIG.CF_INSIDE_HEADER_LINK;
    $scope.INSIDE_HEADER_IMAGE = CONFIG.CF_INSIDE_HEADER_IMAGE;
    $scope.INSIDE_HEADER_TITLE = CONFIG.CF_INSIDE_HEADER_TITLE;
    $scope.CAROUSEL_SHOW = CONFIG.CF_CAROUSEL_SHOW;
    $scope.CAROUSEL_AUTO_PLAY = CONFIG.CF_CAROUSEL_AUTO_PLAY;
    $scope.AN_CAROUSEL_IMG_VIDEO = CONFIG.CF_AN_CAROUSEL_IMG_VIDEO;
    $scope.AN_CAROUSEL_PILL = CONFIG.CF_AN_CAROUSEL_PILL;
    $scope.AN_STORE_IMG_VIDEO = CONFIG.CF_AN_STORE_IMG_VIDEO;
    $scope.AN_STORE_PILL = CONFIG.CF_AN_STORE_PILL;
    $scope.SYSTEM_NAME = CONFIG.CF_SYSTEM_NAME;
    $scope.SYSTEM_LANGUAGE = CONFIG.CF_SYSTEM_LANGUAGE;
    $scope.BASE_URL = CONFIG.CF_BASE_URL;
    $scope.API_URL = CONFIG.CF_API_URL;
    $scope.GOOGLE_ANALYTICS_ID = CONFIG.CF_GOOGLE_ANALYTICS_ID;

    $scope.filteredItems = [];
    $scope.groupedItems = [];
    $scope.pagedItems = [];

    $scope.currentPage = 1;
    $scope.pageSize = 8;

    $scope.products = [];
    $scope.slides = [];

    $scope.isActive = false;

    $scope.sections = [{ name: 'list', class: 'cbp-vm-icon cbp-vm-list' }];

    $scope.updateDisplay = function (section) {
        $scope.selected = section;
        $scope.isActive = !$scope.isActive;

        // let's flip our icons. 
        //<a href="#" class="cbp-vm-icon cbp-vm-grid cbp-vm-selected" data-view="cbp-vm-view-grid">Grid View</a>
        //<a href="#" class="cbp-vm-icon cbp-vm-list" data-view="cbp-vm-view-list">List View</a>
        if (section.class.toString() === 'cbp-vm-icon cbp-vm-grid') {
            $scope.sections = [{ name: 'list', class: 'cbp-vm-icon cbp-vm-list' }];
        }
        else {
            $scope.sections = [{ name: 'grid', class: 'cbp-vm-icon cbp-vm-grid' }];
        }
    }

    $scope.isSelected = function (section) {
        return $scope.selected === section;
    }

    $scope.fToggleOverlay = function () {
        $scope.overlayFlag = !$scope.overlayFlag; // toggle state of overlay flag.
    };

    // get store and cart from service
    $scope.store = DataService.store;
    $scope.cart = DataService.cart;

    // use routing to pick the selected product
    if ($routeParams.productSku != null) {
        //alert($routeParams.productSku);
        localStorage["productSku"] = $routeParams.productSku;
        $scope.product = $scope.store.getProduct($routeParams.productSku);
    }

    $scope.showDetails = function (e) {
        //event.stopPropagation();
        //event.preventDefault();
        //e.preventDefault();
        return false;

    };
    //////////////////////////////////////////////////////
    var imgDir = localStorage["img_dir"];
    if (imgDir == null || imgDir == 'undefined') {
        imgDir = "";
        localStorage["img_dir"] = imgDir;
    }
    $scope.imageDir = imgDir;
    $('#txtImageDir').val($scope.imageDir);

    $scope.setImageDirectory = function (e) {
        //event.stopPropagation();
        //event.preventDefault();
        //e.preventDefault();
        //localStorage["img_dir"] = JSON.stringify($('#txtImageDir').val().replace(/\/$/, ""));
        var z = $('#txtImageDir').val().replace(/\/$/, "");
        localStorage["img_dir"] = z;
        $('#txtImageDir').val(z);
        //e.preventDefault();
        $scope.imageDir = z;
        alert("Image Directort is set to: " + z);
        return false;
    };
    //////////////////////////////////////////////////////

    $scope.master = {};
    $scope.update = function (product) {

        $scope.master = angular.copy(product);
        var bFound = false;

        for (var i = 0; i < $scope.products.length; i++) {
            if ($scope.products[i].productid == $scope.product.productid) {
                bFound = true;
                $scope.products[i].storeid = "7cc6cb94-0938-4675-b84e-6b97ada53978"; 
                $scope.products[i].sku = $scope.product.sku;
                $scope.products[i].categoryname = $scope.product.categoryname;
                $scope.products[i].carousel = $scope.product.carousel;
                //////////////////////////////////////////////////////////////////////////
                // Bill SerGio - In order to maintain a uniform look in our shopping cart we MUST elminate all paragraph tags.
                $scope.products[i].carousel_caption = $scope.product.carousel_caption.replace(/<p\b[^>]*>/ig, '').replace(/<\/p\b[^>]*>/ig, '');

               // alert($scope.products[i].carousel_caption);

                $scope.products[i].productname = $scope.product.productname.replace(/<p\b[^>]*>/ig, '').replace(/<\/p\b[^>]*>/ig, '');
                $scope.products[i].header = $scope.product.header.replace(/<p\b[^>]*>/ig, '').replace(/<\/p\b[^>]*>/ig, '');
                $scope.products[i].shortdesc = $scope.product.shortdesc.replace(/<p\b[^>]*>/ig, '').replace(/<\/p\b[^>]*>/ig, '');
                $scope.products[i].description = $scope.product.description.replace(/<p\b[^>]*>/ig, '').replace(/<\/p\b[^>]*>/ig, '');
                //alert($scope.products[i].description);
                //////////////////////////////////////////////////////////////////////////
                $scope.products[i].link = $scope.product.link;
                $scope.products[i].linktext = $scope.product.linktext;
                $scope.products[i].imageurl = $scope.product.imageurl;
                $scope.products[i].imagename = $scope.product.imagename;
                $scope.products[i].tube = $scope.product.tube;
                $scope.products[i].videoid = $scope.product.videoid;
                $scope.products[i].showvideo = $scope.product.showvideo;
                $scope.products[i].unitprice = $scope.product.unitprice;
                $scope.products[i].saleprice = $scope.product.saleprice;
                $scope.products[i].unitsinstock = $scope.product.unitsinstock;
                $scope.products[i].unitsonorder = $scope.product.unitsonorder;
                $scope.products[i].reorderlevel = $scope.product.reorderlevel;
                $scope.products[i].expecteddate = $scope.product.expecteddate;
                $scope.products[i].discontinued = $scope.product.discontinued;
                $scope.products[i].notes = $scope.product.notes;
                $scope.products[i].faux = $scope.product.faux;
                $scope.products[i].sortorder = $scope.product.sortorder;
                break;
            }
        }

        if (!bFound) {
            var currentList = $scope.products;
            var newList = currentList.concat($scope.product);
            $scope.products = newList;
        }

        //var iselected = $("select[name='dataSourceSelect'] option:selected").index();
        var dataIndex = localStorage["data_src_index"];

        if (dataIndex == null || dataIndex == "undefined") {
            dataIndex = 0;
            localStorage["data_src_index"] = 0;
        }

        var jsonData;
        var dataUrl;
        if (dataIndex == 0) {
            jsonData = angular.toJson($scope.products);
            jsonData = JSON.stringify(JSON.parse(jsonData), null, 4);
            localStorage["grid_data"] = jsonData;
            dataUrl = "/crud.ashx?ac=save2file&cn=products";
        }
        else if (dataIndex == 1) {
            jsonData = angular.toJson($scope.product);
            dataUrl = "/crud.ashx?ac=update&cn=local";
        }
        else if (dataIndex == 2) {
            jsonData = angular.toJson($scope.product);
            dataUrl = "/crud.ashx?ac=update&cn=remote";
        }
        else {
            return;
        }

        // AUTHOR: Bill SerGio - To strip out Angular's unwanted "$$hashKey" 
        // I like a json file that is human readable!
        jsonData = JSON.stringify(JSON.parse(jsonData), null, 4);

        $.ajax({
            type: "POST"
            ,cache: false
            ,url: dataUrl
            ,data: jsonData
            ,contentType: "application/json"
            ,success: function (result) {
                //alert('changes saved!');
                location.reload();
            }
            ,error: function (xhr, ajaxOptions, thrownError) {
                //$("#output").html(xhr.responseText);
                //alert(xhr.responseText);
            }
        });
       
    };

    $scope.new = function () {
        //alert("new!");
        $scope.product = angular.copy($scope.master);
        //if we were using integers
        //options.data.productid = localData[localData.length - 1].productid + 1;
        $scope.product.productid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
        $scope.product.storeid = "7cc6cb94-0938-4675-b84e-6b97ada53978"; //options.data.storeid;
        $scope.product.sku = "";
        $scope.product.categoryname = "";
        $scope.product.productname = "";
        $scope.product.header = "";
        $scope.product.shortdesc = "";
        $scope.product.description = "";
        $scope.product.link = "";
        $scope.product.linktext = "";
        $scope.product.imageurl = "";
        $scope.product.imagename = "";
        $scope.product.tube = "youtube";
        $scope.product.videoid = "";
        $scope.product.carousel = true;
        $scope.product.carousel_caption = "";
        $scope.product.showvideo = false;
        $scope.product.unitprice = "0.00";
        $scope.product.saleprice = "0.00";
        $scope.product.unitsinstock = 1;
        $scope.product.unitsonorder = 1;
        $scope.product.reorderlevel = 1;
        $scope.product.expecteddate = "2016-01-01T05:00:00.000Z";
        $scope.product.discontinued = false;
        $scope.product.notes = "";
        $scope.product.faux = false;
        $scope.product.sortorder = 100;

    };
    $scope.reset = function () {
        //alert("reset!");
        $scope.product = angular.copy($scope.master);
    };
    $scope.delete = function () {
        if (!confirm('Are you sure you want to delete : ' + $scope.product.productid)) {
            return;
        }

        var bFound = false;
        var zid = "";
        for (var i = 0; i < $scope.products.length; i++) {
            if ($scope.products[i].productid === $scope.product.productid) {
                zid = $scope.product.productid;
                $scope.products.splice(i, 1);
                bFound = true;
                break;
            }
        }

        if (!bFound) {
            return;
        }

        //var iselected = $("select[name='dataSourceSelect'] option:selected").index();
        var dataIndex = localStorage["data_src_index"];
        if (dataIndex == null || dataIndex == "undefined") {
            dataIndex = 0;
            localStorage["data_src_index"] = 0;
        }

        var jsonData;
        var dataUrl;
        if (dataIndex == 0) {
            jsonData = angular.toJson($scope.products);
            jsonData = JSON.stringify(JSON.parse(jsonData), null, 4);
            localStorage["grid_data"] = jsonData;
            dataUrl = dataUrl = "/crud.ashx?ac=save2file&cn=products";
        }
        else if (dataIndex == 1) {
            jsonData = angular.toJson($scope.product);
            dataUrl = "/crud.ashx?ac=delete&cn=local";
        }
        else if (dataIndex == 2) {
            jsonData = angular.toJson($scope.product);
            dataUrl = "/crud.ashx?ac=delete&cn=remote";
        }
        else {
            return;
        }

        // AUTHOR: Bill SerGio - To strip out Angular's unwanted "$$hashKey" 
        // I like a json file that is human readable!
        jsonData = JSON.stringify(JSON.parse(jsonData), null, 4);

        $.ajax({
            type: "POST"
            , cache: false
            , url: dataUrl
            , data: jsonData
            , contentType: "application/json"
            , success: function (result) {
                //alert('changes saved!');
                location.reload();
            }
            , error: function (xhr, ajaxOptions, thrownError) {
                //$("#output").html(xhr.responseText);
                //alert(xhr.responseText);
            }
        });


    };
    $scope.reset();

    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
    //////////////////////////////////////////////////////

    $scope.setFilter = function (value) {
        $scope.carouselFilterClass = "blktext";
        $scope.showvideoFilterClass = "blktext";
        $scope.myFilter = value ? { categoryname: value } : '';
        //$scope.emptyStringFilter = $filter('filter')($scope.persons, { lastName: '' }, true);
    };

    $scope.carouselFilterClass = "blktext";
    $scope.showvideoFilterClass = "blktext";
    $scope.carouselFlag = false;
    $scope.carouselFilter = function () {
        $scope.carouselFlag = !$scope.carouselFlag; // toggle state of carousel flag.
        $scope.showvideoFilterClass = $scope.carouselFlag ? "blktext" : "blktext";
        $scope.carouselFilterClass = $scope.carouselFlag ? "redtext" : "blktext";
        $scope.myFilter = $scope.carouselFlag ? { carousel: $scope.carouselFlag } : '';
    };

    $scope.showvideoFlag = false;
    $scope.showvideoFilter = function () {
        $scope.showvideoFlag = !$scope.showvideoFlag; // toggle state of showvideo flag.
        $scope.carouselFilterClass = $scope.showvideoFlag ? "blktext" : "blktext";
        $scope.showvideoFilterClass = $scope.showvideoFlag ? "redtext" : "blktext";
        $scope.myFilter = $scope.showvideoFlag ? { showvideo: $scope.showvideoFlag } : '';
    };



    var rand = 1;
    $scope.initRand = function () {
        //rand = Math.floor((Math.random() * 2) + 1)
        rand = Math.floor((Math.random() * 3) + 1) - 1;
    }

    $scope.getRandomSpan = function () {
        return rand;
    }


    $scope.myArrayOfObjects = [
        { "tube": "youtube", "videoid": "HppJHKwCGCo" },
        { "tube": "youtube", "videoid": "5CYS0Ti9YC0" },
        { "tube": "youtube", "videoid": "92eR46ZbMAg" }
    ];

    $scope.data = {
        repeatSelect: null,
        availableOptions: [
            { "name": "YouTube", "id": "youtube" },
            { "name": "YouKu", "id": "youku" },
            { "name": "Vimeo", "id": "vimeo" },
            { "name": "DailyMotion", "id": "dailymotion" },
            { "name": "5min", "id": "5min" },
            { "name": "mtvnservices", "id": "cc" },
            { "name": "MetaCafe", "id": "metacafe" },
            { "name": "liveleak", "id": "liveleak" },
            { "name": "ebaumsworld", "id": "ebaumsworld" },
            { "name": "bliptv", "id": "bliptv" },
            { "name": "funnyordie", "id": "funnyordie" },
            { "name": "stupidvideos", "id": "stupidvideos" }
        ],
        selectedOption: { "id": "youtube", "name": "YouTube" }
    };

    $scope.dataSource = {
        dataSelect: null,
        dataSourceOptions: [
            { "text": "JSON File", "id": "file" },
            { "text": "Remote DB", "id": "remotedb" },
            { "text": "Local DB", "id": "local" }
            ],
        selectedOption: { "text": "youtube", "id": "YouTube" }
    };

    $scope.changeNavBar = function (css_name) {
        //event.stopPropagation();
        //event.preventDefault();
        var _path = "ac_css/" + css_name + ".css";
        _navbar_theme = css_name;
        localStorage["navbar_theme"] = _navbar_theme;
        $("#link_index").attr("href", _path);
        //alert("aaa");
        //return false;
    };

    $scope.getYouTubeImage = function () {
        //event.stopPropagation();
        //event.preventDefault();
        var _videoid = $("#videoid").val();
        var zpath = "https://img.youtube.com/vi/" + $("#videoid").val() + "/hqdefault.jpg";
        //if (_videoid.length > 0) {
        //    if (confirm('Do you want to download the YouTube Image?')) {

        //$('#fileToUpload').attr('value', zpath);
        document.getElementById('fileToUpload').ATTRIBUTE_NODE('value', zpath);
        document.getElementById('fileToUpload').click();

    };

    function uploadYouTubeImageComplete(evt) {
        alert("aaa");
    }

    $scope.FileUploadCtrl = function () {
        //function FileUploadCtrl($scope) {

        //============== DRAG & DROP =============
        var dropbox = document.getElementById("dropbox")
        $scope.dropText = 'Drop file here...'

        // init event handlers
        function dragEnterLeave(evt) {
            evt.stopPropagation()
            evt.preventDefault()
            $scope.$apply(function () {
                $scope.dropText = 'Drop file here...'
                $scope.dropClass = ''
            })
        }
        dropbox.addEventListener("dragenter", dragEnterLeave, false)
        dropbox.addEventListener("dragleave", dragEnterLeave, false)
        dropbox.addEventListener("dragover", function (evt) {
            evt.stopPropagation()
            evt.preventDefault()
            var clazz = 'not-available';
            var ok = evt.dataTransfer && evt.dataTransfer.types && evt.dataTransfer.types.indexOf('Files') >= 0;
            $scope.$apply(function () {
                $scope.dropText = ok ? 'Drop file here...' : 'Only Images are allowed!';
                $scope.dropClass = ok ? 'over' : 'not-available';
            })
        }, false)
        dropbox.addEventListener("drop", function (evt) {
            //console.log('drop evt:', JSON.parse(JSON.stringify(evt.dataTransfer)));
            evt.stopPropagation();
            evt.preventDefault();
            $scope.$apply(function () {
                $scope.dropText = 'Drop file here...'
                $scope.dropClass = ''
            })
            var files = evt.dataTransfer.files;
            if (files.length > 0) {
                $scope.$apply(function () {
                    $scope.files = []
                    for (var i = 0; i < files.length; i++) {
                        $scope.files.push(files[i])
                    }
                })
            }
        }, false)
        //============== DRAG & DROP =============

        $scope.setFiles = function (element) {
            $scope.$apply(function ($scope) {
                //console.log('files:', element.files);
                // Turn the FileList object into an Array
                $scope.files = []
                for (var i = 0; i < element.files.length; i++) {
                    $scope.files.push(element.files[i])
                }
                $scope.progressVisible = false
            });
        };

        $scope.uploadFile = function (event) {
            var fd = new FormData();
            for (var i in $scope.files) {
                fd.append("uploadedFile", $scope.files[i]);
            }

            //$('#txtImageDir').val($('#txtImageDir').val().replace(/\/$/, ""));
            var imgDir = localStorage["img_dir"];
            var uploadUrl = "ImageHandler.ashx?dir=" + imgDir;

            $.ajax({
                url: uploadUrl,
                dataType: 'text',
                cache: false,
                contentType: false,
                processData: false,
                data: fd,
                type: 'post',
                success: uploadComplete
            })
            .error(function () {
                alert("failed!")
            });
        }

        function uploadProgress(evt) {
            $scope.$apply(function () {
                if (evt.lengthComputable) {
                    $scope.progress = Math.round(evt.loaded * 100 / evt.total)
                } else {
                    $scope.progress = 'unable to compute'
                }
            })
        }

        function uploadComplete(evt) {

            var jsonData;
            $scope.$apply(function () {
                var newimage = $scope.files[0].name;
                var zpath = "";
                var imgDir = localStorage["img_dir"];
                if (imgDir.length > 0) {
                    zpath = imgDir + "/" + $scope.files[0].name;
                }
                else {
                    zpath = $scope.files[0].name;
                }
                newimage = zpath;

                for (var i = 0; i < $scope.products.length; i++) {
                    if ($scope.products[i].productid == $scope.product.productid) {
                        $scope.products[i].imagename = zpath;
                        $scope.product.imagename = zpath;
                        //alert($scope.products[i].imagename);
                        break;
                    }
                }

                var jsonData = angular.toJson($scope.products);
                // AUTHOR: Bill SerGio - To strip out Angular's unwanted "$$hashKey" 
                // I like a json file that is human readable!
                jsonData = JSON.stringify(JSON.parse(jsonData), null, 4);
                $scope.progressVisible = false;
                $scope.files.length = 0;
                
            })

            var dataUrl = "/crud.ashx?ac=save2file&cn=products";
            $.ajax({
                type: "POST"
                ,cache: false
                ,url: dataUrl
                ,data: jsonData
                ,contentType: "application/json"
                ,success: function (result) {
                    //alert('yes');
                    //that.refresh();
                    Location.reload();
                }
                ,error: function (xhr, ajaxOptions, thrownError) {
                    //$("#output").html(xhr.responseText);
                    //alert(xhr.responseText);
                    //that.cancelRow();
                }
            });
            
        }

        function uploadFailed(evt) {
            $scope.$apply(function () {
                $scope.progressVisible = false;
                $scope.files.length = 0;
            })
            alert("There was an error attempting to upload the file.")
        }

        function uploadCanceled(evt) {
            $scope.$apply(function () {
                $scope.progressVisible = false;
                $scope.files.length = 0;
            })
            alert("The upload has been canceled by the user or the browser dropped the connection.")
        }

        function MyMenu($scope) {
            $scope.name = 'eureka!';

        }

    }


    /*#####################
    DataService  Executes when AJAX call completes
    ######################*/
    DataService.store.getProducts().then(function (data) {

        // Build array for products
        $scope.products = data;

        // Build slides[] array for super slick carousel
        for (var i = 0, len = $scope.products.length; i < len; i++) {
            var prod = $scope.products[i];
            if (prod.storeid == "7cc6cb94-0938-4675-b84e-6b97ada53978") {
                if (prod.imagename.length < 1) {
                    prod.imagename = "nopic.png";
                }
                if (prod.carousel) {
                    $scope.slides.push(prod);
                }
            }
        }

        // We use: ng-if="dataLoaded" init-onload="false" data="dataLoaded" 
        // in the timeout function in order to get the old elements completly removed.
        // otherwise the old elements stay in the directive and the carousel breaks
        $timeout(function () {
            $scope.dataLoaded = true;
        });

        if ($routeParams.productSku != null) {
            var _sku = $routeParams.productSku.toString();
            //if (_sku.length > 0) {
            for (var i = 0, len = $scope.products.length; i < len; i++) {
                var prod = $scope.products[i];
                if (prod.sku === _sku) {
                    $scope.product = prod;
                }
            }
            //}
        }

        //////////////////////////////////////////////////////////////////////////////
        // Author: Bill SerGio, The Infomercial King
        // Substantially better than the TOTAL PIECE OF CRAP FUNCTION "getUrlVars" that throw errors like crazy!
        // Given a query string "?to=email&why=because&first=John&Last=smith"
        // getUrlVar("to")  will return "email"
        // getUrlVar("last") will return "smith"
        // To convert it to a jQuery plug-in, you could try something like this:
        //(function ($) {
        //    $.getUrlVar = function (key) {
        //        var result = new RegExp(key + "=([^&]*)", "i").exec(window.location.search);
        //        return result && unescape(result[1]) || "";
        //    };
        //})(jQuery);
        $scope.getUrlVar = function (key) {
            var result = new RegExp(key + "=([^&]*)", "i").exec(window.location.search);
            return result && unescape(result[1]) || "";
        }
        var _sku = $scope.getUrlVar('sku');
        if (_sku.length > 0) {
            for (var i = 0, len = $scope.products.length; i < len; i++) {
                var prod = $scope.products[i];
                if (prod.sku === _sku) {
                    DataService.cart.addItemUrl(prod.sku, prod.productname, prod.unitprice, +1);
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////

        $scope.forceAddItem = function (sku, productname, unitprice, quantity) {
            //slick.dataLoaded = true;
            //var sku = "BabyMop";
            //var productname = "Baby Mop";
            //var unitprice = "22.27";
            //var quantity = 1;

            DataService.cart.addItem(sku, productname, unitprice, quantity);

            $scope.$apply(function () {
                $scope.cart.sku = DataService.cart.sku;
                $scope.cart.productname = DataService.cart.productname;
                $scope.cart.unitprice = DataService.cart.unitprice;
            });
            return "item added";
        };


        $scope.pageCount = function () {
            return Math.ceil($scope.products.length / $scope.pageSize);
        };

        $scope.nextPage = function () {
            if ($scope.currentPage >= Math.ceil($scope.products.length / $scope.pageSize) - 1) {
                return true;
            }
            else {
                return false;
            }
        };

        //$scope.$watch('currentPage + pageSize', function () {
        //    var begin = (($scope.currentPage - 1) * $scope.pageSize);
        //    var end = begin + $scope.pageSize;
        //    $scope.filteredItems = $scope.products.slice(begin, end);
        //});

        var searchMatch = function (haystack, needle) {
            if (!needle) {
                return true;
            }
            return haystack.toLowerCase().indexOf(needle.toLowerCase()) !== -1;
        };
        $scope.filterCategory = function (categoryname) {
            //$('#searchfield').val('');
            $scope.filteredItems = $filter('filter')($scope.products, function (product) {
                for (var attr in product) {
                    if (searchMatch(product[categoryname], $scope.query))
                        return true;
                }
                return false;
            });
            $scope.currentPage = 0;
            $scope.groupedPages();
        };
        $scope.filterCategory = function (column, categoryname) {
            //$('#searchfield').val('');
            //alert(column);
            //alert(categoryname);
            $scope.filteredItems = $filter('filter')($scope.products, function (product) {
                for (var attr in product) {
                    if (searchMatch(product[column], categoryname))
                        return true;
                }
                return false;
            });
            $scope.currentPage = 0;
            $scope.groupedPages();
        };
        $scope.groupedPages = function () {
            $scope.pagedItems = [];
            for (var i = 0; i < $scope.filteredItems.length; i++) {
                if (i % $scope.pageSize === 0) {
                    $scope.pagedItems[Math.floor(i / $scope.pageSize)] = [$scope.filteredItems[i]];
                } else {
                    $scope.pagedItems[Math.floor(i / $scope.pageSize)].push($scope.filteredItems[i]);
                }
            }
        };

        // functions have been describe process the data for display
        $scope.filterCategory();
        $scope.search();
        ////////////////////////////////////////////////////////////////////////
        $scope.vchr_date = new Date($scope.product.expecteddate);

        $scope.getDate = function (item) {
            //var date = new Date(parseInt(jsonDate.substr(6)));
            //var date = eval(jsonDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
            return new Date(item);
        };

    }); /* END - DataService */

//});
}).config(['ngWigToolbarProvider', function (ngWigToolbarProvider) {
    ngWigToolbarProvider.setButtons(['bold', 'italic', 'link']);
}]);

/*#####################
MyMenu
######################*/
storeApp.controller('MyMenu', function ($scope, $filter, $location, CONFIG) {
    //alert("MyMenu");
    $scope.name = 'MyMenu';
    $scope.isCollapsed = false;
    $scope.dataLoaded = false;

    /*#####################
    CONFIG
    ######################*/
    /* our global variabls */
    $scope.DATA_SOURCE = CONFIG.DATA_SOURCE;
    $scope.STORE_ID = CONFIG.CF_STORE_ID;
    $scope.STORE_PAGE = CONFIG.CF_STORE_PAGE;
    $scope.STORE_BG_IMAGE = CONFIG.CF_STORE_BG_IMAGE;
    $scope.DISTRIBUTOR_ID = CONFIG.CF_DISTRIBUTOR_ID;
    $scope.PAYMENT_PAYPAL_BUYNOW = CONFIG.CF_PAYMENT_PAYPAL_BUYNOW;
    $scope.PAYMENT_GOOGLE_WALLET_ID = CONFIG.CF_PAYMENT_GOOGLE_WALLET_ID;
    $scope.PAYMENT_STRIPE = CONFIG.CF_PAYMENT_STRIPE;
    $scope.DATA_SOURCE = CONFIG.CF_DATA_SOURCE;
    $scope.PRODUCTS_FOLDER = CONFIG.CF_PRODUCTS_FOLDER;
    $scope.NAVBAR_THEME = CONFIG.CF_NAVBAR_THEME;
    $scope.NAVBAR_LOGO_IMAGE = CONFIG.CF_NAVBAR_LOGO_IMAGE;
    $scope.NAVBAR_LOGO_TEXT = CONFIG.CF_NAVBAR_LOGO_TEXT;
    $scope.NAVBAR_LOGO_LINK = CONFIG.CF_NAVBAR_LOGO_LINK;
    $scope.INSIDE_HEADER_SHOW = CONFIG.CF_INSIDE_HEADER_SHOW;
    $scope.INSIDE_HEADER_LINK = CONFIG.CF_INSIDE_HEADER_LINK;
    $scope.INSIDE_HEADER_IMAGE = CONFIG.CF_INSIDE_HEADER_IMAGE;
    $scope.INSIDE_HEADER_TITLE = CONFIG.CF_INSIDE_HEADER_TITLE;
    $scope.CAROUSEL_SHOW = CONFIG.CF_CAROUSEL_SHOW;
    $scope.CAROUSEL_AUTO_PLAY = CONFIG.CF_CAROUSEL_AUTO_PLAY;
    $scope.AN_CAROUSEL_IMG_VIDEO = CONFIG.CF_AN_CAROUSEL_IMG_VIDEO;
    $scope.AN_CAROUSEL_PILL = CONFIG.CF_AN_CAROUSEL_PILL;
    $scope.AN_STORE_IMG_VIDEO = CONFIG.CF_AN_STORE_IMG_VIDEO;
    $scope.AN_STORE_PILL = CONFIG.CF_AN_STORE_PILL;
    $scope.SYSTEM_NAME = CONFIG.CF_SYSTEM_NAME;
    $scope.SYSTEM_LANGUAGE = CONFIG.CF_SYSTEM_LANGUAGE;
    $scope.BASE_URL = CONFIG.CF_BASE_URL;
    $scope.API_URL = CONFIG.CF_API_URL;
    $scope.GOOGLE_ANALYTICS_ID = CONFIG.CF_GOOGLE_ANALYTICS_ID;

    if ($scope.CAROUSEL_SHOW) {
        $('#storeslider_wrapper').css('display', 'block');
    }
    else {
        $('#storeslider_wrapper').css('display', 'none');
    }

    if ($scope.INSIDE_HEADER_SHOW) {
        $('.inside_header').css('display', 'block');
    }
    else {
        $('.inside_header').css('display', 'none');
    }

    //if ($scope.STORE_BG_IMAGE.length > 0) {
    //    $("body").css('background-image', '');
    //    $("body").css("background", "#ffffff url(" + $scope.STORE_BG_IMAGE + ") no-repeat center center fixed");
    //    localStorage['bg_cart'] = $scope.STORE_BG_IMAGE;
    //}

    _navbar_theme = "navbar_dkred_gradient";
    if (localStorage["navbar_theme"]) {
        _navbar_theme = localStorage["navbar_theme"];
    } else {
        _navbar_theme = "navbar_dkred_gradient";
        localStorage["navbar_theme"] = "navbar_dkred_gradient";
    }
    var _path = "ac_css/" + _navbar_theme + ".css";
    $("#link_index").attr("href", _path);
    $scope.NAVBAR_THEME = _navbar_theme;

    $scope.showHideCarousel = function (event) {
        //event.stopPropagation();
        event.preventDefault();

        if ($('#storeslider_wrapper').css('display') === 'block') {
            $('.carousel_trim').css('display', 'none');
            $('#storeslider_wrapper').css('display', 'none');
        }
        else {
            $('.carousel_trim').css('display', 'block');
            $('#storeslider_wrapper').css('display', 'block');
            $("#storeslider").slick('slickPrev');
            $("#storeslider").slick('slickNext');
        }
    }

    $scope.changeBackgroundImage = function (event) {
        //event.stopPropagation();
        event.preventDefault();
        var x = 0
        for (x = 0; x < arBGs.length; x++) {
            if (_bgImage === arBGs[x]) { break; }
        }
        if (x + 1 < arBGs.length) {
            _bgImage = arBGs[x + 1];
        }
        else {
            x = 0;
            _bgImage = arBGs[x];
        }
        $("body").css('background-image', '');

        if (_bgImage === 'ac_img/bg0.jpg') {
            $("body").css("background-color", "#ffffff");
        }
        else {
            $("body").css("background", "#ffffff url(" + _bgImage + ") no-repeat center center fixed");
        }
        localStorage['bg_cart'] = _bgImage;
    }

    $scope.changeAnimation = function (effect_name) {
        var e = '';
        if ($scope.myModel === 'carousel_img_video') {
            e = '.carousel_img_video';
        }
        else if ($scope.myModel === 'carousel_pill') {
            e = '.carousel_pill';
        }
        else if ($scope.myModel === 'store_img_video') {
            e = '.store_img_video';
        }
        else if ($scope.myModel === 'store_pill') {
            //e = '.nav-pills li';
            e = '.store_pill';
        }
        if (e.length > 0) {
            $(e).removeClass(function (index, css) {
                return (css.match(/(^|\s)hvr-\S+/g) || []).join(' ');
            });
            $(e).addClass(effect_name);
        }
    };

    // Author: Bill SerGio - An elegant way to set the active tab is to use ng-controller 
    // to run a single controller outside of the ng-view as shown below.
    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

    //initiate an array to hold all active tabs
    $scope.activeTabs = [];

    //check if the tab is active
    $scope.isOpenTab = function (tab) {
        //event.stopPropagation();
        event.preventDefault();

        //check if this tab is already in the activeTabs array
        if ($scope.activeTabs.indexOf(tab) > -1) {
            //if so, return true
            return true;
        } else {
            //if not, return false
            return false;
        }
    }

    //function to 'open' a tab
    $scope.openTab = function (tab) {
        //event.stopPropagation();
        event.preventDefault();

        //check if tab is already open
        if ($scope.isOpenTab(tab)) {
            //if it is, remove it from the activeTabs array
            $scope.activeTabs.splice($scope.activeTabs.indexOf(tab), 1);
        } else {
            $scope.activeTabs = [];
            //if it's not, add it!
            $scope.activeTabs.push(tab);
        }
        return false;
    }

    //$scope.dnLink = function ($index) {
    //    //event.stopPropagation();
    //    event.preventDefault();
    //    setupDownloadLink = function (link, code) {
    //        link.href = 'data:text/plain;charset=utf-8,' + encodeURIComponent(code);
    //    };
    //    window.onload = function () {
    //        txt.value = main + '';
    //    }
    //}

    $scope.loadData = function ($index) {
        //event.stopPropagation();
        event.preventDefault();
        $scope.selectedDataIndex = $index;
        localStorage["data_src_index"] = $index;
        window.location.reload();
        return false;
    }

    //function to 'open' a tab
    $scope.openTab = function (tab) {
        //event.stopPropagation();
        event.preventDefault();

        //check if tab is already open
        if ($scope.isOpenTab(tab)) {
            //if it is, remove it from the activeTabs array
            $scope.activeTabs.splice($scope.activeTabs.indexOf(tab), 1);
        } else {
            $scope.activeTabs = [];
            //if it's not, add it!
            $scope.activeTabs.push(tab);
        }
        return false;
    }

    //var dataIndex = localStorage["data_src_index"];
    //if (dataIndex == null || dataIndex == "undefined") {
    //    dataIndex = 0;
    //    localStorage["data_src_index"] = 0;
    //}
    //$scope.selectedDataIndex = dataIndex;


    $scope.dataOptions = [
        { name: 'JSON Products File', showinfo: '', img: 'ac_img/_json_file.png' },
        { name: 'Local Database', showinfo: '', img: 'ac_img/_localdb.png' },
        { name: 'Remote Database', showinfo: '', img: 'ac_img/_remotedb.png' }
    ];

    //$scope.dataOptions.forEach(function (value, key) {
    //    alert("Key: " + (key) + "\nValue: " + value.value);
    //    //if (value.index == dataIndex) {
    //    //    alert("Key: " + (key) + "\nValue: " + value.value);
    //    //}
    //});

    //var dataSource = localStorage["data_src"];
    //if (dataSource == null || dataSource == "undefined") {
    //    dataSource = CONFIG.CF_DATA_FILE.value;
    //    localStorage["data_src"] = CONFIG.CF_DATA_FILE.value;
    //}
    //$scope.dataModel = dataSource;

    // create a radioButtonGroup for our apply effects options
    $scope.optActions = [
        { id: 'apply', name: 'apply effect', disabled: false, showinfo: '' },
        { id: 'remove', name: 'remove effect', disabled: false, showinfo: '' }
    ];
    $scope.modelAction = 'apply';
    $scope.idProperty = "id";
    $scope.nameProperty = "name";
    $scope.bootstrapSuffix = "x-success";
    $scope.disabledProperty = false;
    $scope.showinfoProperty = "";


    // create a radioButtonGroup for our apply effects options
    //{ id: 'carousel_img_video', name: 'carousel img', disabled: false, showinfo: 'You need to download and install the AngularJS Slick Carousel to apply effects to the Carousel!' },
    //{ id: 'carousel_pill', name: 'carousel pill', disabled: false, showinfo: 'You need to download and install the AngularJS Slick Carousel to apply effects to the Carousel!' }
    $scope.myOptions = [
        { id: 'store_img_video', name: 'store img', disabled: false, showinfo: '' },
        { id: 'store_pill', name: 'store pill', disabled: false, showinfo: '' },
        { id: 'carousel_img_video', name: 'carousel img', disabled: false, showinfo: '' },
        { id: 'carousel_pill', name: 'carousel pill', disabled: false, showinfo: '' }
    ];
    $scope.myModel = 'store_img_video';
    $scope.idProperty = "id";
    $scope.nameProperty = "name";
    $scope.bootstrapSuffix = "xs-success";
    $scope.disabledProperty = false;
    $scope.showinfoProperty = "";


    //$('.carousel_img_video').removeClass(function (index, css) {
    //    return (css.match(/(^|\s)hvr-\S+/g) || []).join(' ');
    //});
    //$('.carousel_pill').addClass($scope.AN_CAROUSEL_IMG_VIDEO);

    //alert($scope.AN_CAROUSEL_IMG_VIDEO);
    //$('.carousel_pill').removeClass(function (index, css) {
    //    return (css.match(/(^|\s)hvr-\S+/g) || []).join(' ');
    //});
    //$('.carousel_pill').addClass($scope.AN_CAROUSEL_PILL);

    //$('.store_img_video').removeClass(function (index, css) {
    //    return (css.match(/(^|\s)hvr-\S+/g) || []).join(' ');
    //});
    //$('.store_img_video').addClass($scope.AN_STORE_IMG_VIDEO);

    //$('.nav-pills li').removeClass(function (index, css) {
    //    return (css.match(/(^|\s)hvr-\S+/g) || []).join(' ');
    //});
    //$('.nav-pills li').addClass($scope.AN_STORE_PILL);



});

storeApp.directive('radioButtonGroup', function () {
    return {
        restrict: 'E',
        scope: { model: '=', options: '=', id: '=', name: '=', suffix: '=', disabled: '=', showinfo: '=' },
        controller: function ($scope) {
            $scope.activate = function (option, $event) {
                if (option.showinfo.length > 0) {
                    alert(option.showinfo);
                    return false;
                }
                $scope.model = option[$scope.id];
                // stop click event to avoid Bootstrap toggling "active" class
                if ($event.stopPropagation) {
                    $event.stopPropagation();
                }
                if ($event.preventDefault) {
                    $event.preventDefault();
                }
                $event.cancelBubble = true;
                $event.returnValue = false;
            };

            $scope.isActive = function (option) {
                return option[$scope.id] == $scope.model;
            };

            $scope.isDisabled = function (option) {
                return option[$scope.disabled] == $scope.model;
            };

            $scope.getName = function (option) {
                return option[$scope.name];
            }
        },
        template: "<button type='button' class='btn btn-{{suffix}}' " +
            "ng-class='{active: isActive(option)}'" +
            "ng-repeat='option in options' " +
            //The ng-disabled expression is evaluated in the present scope. Hence, 
            //you should NOT USE the extra interpolation with {{..}} which will not work:
            "ng-disabled=option.disabled ng-click='activate(option, $event)'><span ng-bind-html='getName(option) | unsafe'></span>" +
            "</button>"
    };
});

storeApp.directive('aDisabled', function ($compile) {
    return {
        restrict: 'A',
        priority: -99999,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.aDisabled, function (val, oldval) {
                if (!!val) {
                    element.unbind('click');
                } else if (oldval) {
                    element.bind('click', function () {
                        scope.$apply(attrs.ngClick);
                    });
                }
            });
        }
    };
});

storeApp.directive('noImageIcon', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            var defaultURL = attr.defaultURL || 'images/plaecholder.png';

            attr.$observe('ngSrc', function (newValue) {
                if (!newValue) {
                    element.attr('src', defaultURL);
                } else {
                    var image = new Image();
                    image.onerror = function () {
                        element.attr('src', defaultURL);
                    };
                    image.onload = function () {
                    };

                    image.src = newValue;
                }
            });
        }
    };
});

angular.module('igTruncate', []).filter('truncate', function () {
    return function (text, length, end) {
        if (text !== undefined) {
            if (isNaN(length)) {
                length = 10;
            }

            if (end === undefined) {
                end = "...";
            }

            if (text.length <= length || text.length - end.length <= length) {
                return text;
            } else {
                return String(text).substring(0, length - end.length) + end;
            }
        }
    };
});

storeApp.directive('fileModel', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.fileModel);
            var modelSetter = model.assign;

            element.bind('change', function () {
                scope.$apply(function () {
                    modelSetter(scope, element[0].files[0]);
                });
            });
        }
    };
}]);

// Bill SerGio - To remove template caching
storeApp.run(function ($rootScope, $templateCache) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (typeof (current) !== 'undefined') {
            $templateCache.remove(current.templateUrl);
        }
    });
});

storeApp.filter('htmlToPlaintext', function () {
    return function (text) {
        return text ? String(text).replace(/<[^>]+>/gm, '') : '';
    };
});

// AUTHOR: Bill SerGio
// Create a list of categoryname button to set categoryname filter
storeApp.filter('unique', function () {
    return function (collection, keyname) {
        var output = [],
            keys = [];

        angular.forEach(collection, function (item) {
            var key = item[keyname];
            if (keys.indexOf(key) === -1) {
                keys.push(key);
                output.push(item);
            }
        });
        return output;
    };
});

storeApp.directive('scrollTo', ['ScrollTo', function (ScrollTo) {
    return {
        restrict: "AC",
        compile: function () {

            return function (scope, element, attr) {
                element.bind("click", function (event) {
                    event.preventDefault();
                    ScrollTo.idOrName(attr.scrollTo, attr.offset);
                });
            };
        }
    };
}])
  .service('ScrollTo', ['$window', 'ngScrollToOptions', function ($window, ngScrollToOptions) {

      this.idOrName = function (idOrName, offset, focus) {//find element with the given id or name and scroll to the first element it finds
          var document = $window.document;

          if (!idOrName) {//move to top if idOrName is not provided
              $window.scrollTo(0, 0);
          }

          if (focus === undefined) { //set default action to focus element
              focus = true;
          }

          //check if an element can be found with id attribute
          var el = document.getElementById(idOrName);
          if (!el) {//check if an element can be found with name attribute if there is no such id
              el = document.getElementsByName(idOrName);

              if (el && el.length)
                  el = el[0];
              else
                  el = null;
          }

          if (el) { //if an element is found, scroll to the element
              if (focus) {
                  el.focus();
              }

              ngScrollToOptions.handler(el, offset);
          }

          //otherwise, ignore
      }

  }])
  .provider("ngScrollToOptions", function () {
      this.options = {
          handler: function (el, offset) {
              if (offset) {
                  var top = $(el).offset().top - offset;
                  window.scrollTo(0, top);
              }
              else {
                  el.scrollIntoView();
              }
          }
      };
      this.$get = function () {
          return this.options;
      };
      this.extend = function (options) {
          this.options = angular.extend(this.options, options);
      };
  });

storeApp.filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };
});

storeApp.directive('embedVideo', function ($sce) {
    return {
        restrict: 'EA',
        scope: { tube: '=', code: '=' },
        replace: true,
        template: '<div class="video"><iframe src="{{url}}" frameborder="0" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe></div>',
        link: function (scope) {
            //console.log('here');
            //document.cookie="VISITOR_INFO1_LIVE=oKckVSqvaGw; path=/; domain=.youtube.com";window.location.reload();
            scope.url = "about:blank";
            scope.$watch('code', function (videoidVal) {
                if (videoidVal) {
                    if (scope.tube === 'youtube') {
                        scope.url = $sce.trustAsResourceUrl("http://www.youtube.com/embed/" + videoidVal);
                    }
                    else if (scope.tube === 'youku') {
                        scope.url = $sce.trustAsResourceUrl("http://player.youku.com/embed/" + videoidVal);
                    }
                    else if (scope.tube === 'vimeo') {
                        scope.url = $sce.trustAsResourceUrl("http://player.vimeo.com/video/" + videoidVal);
                    }
                    else if (scope.tube === 'dailymotion') {
                        scope.url = $sce.trustAsResourceUrl("http://www.dailymotion.com/embed/video/" + videoidVal);
                    }
                    else if (scope.tube === '5min') {
                        scope.url = $sce.trustAsResourceUrl("http://embed.5min.com/" + videoidVal);
                    }
                    else if (scope.tube === 'cc') {
                        scope.url = $sce.trustAsResourceUrl("http://media.mtvnservices.com/embed/" + videoidVal);
                    }
                    else if (scope.tube === 'metacafe') {
                        scope.url = $sce.trustAsResourceUrl("http://www.metacafe.com/embed/" + videoidVal);
                    }
                    else if (scope.tube === 'liveleak') {
                        scope.url = $sce.trustAsResourceUrl("http://www.liveleak.com/ll_embed?f=" + videoidVal);
                    }
                    else if (scope.tube === 'ebaumsworld') {
                        scope.url = $sce.trustAsResourceUrl("http://www.ebaumsworld.com/media/embed/" + videoidVal);
                    }
                    else if (scope.tube === 'bliptv') {
                        scope.url = $sce.trustAsResourceUrl("http://blip.tv/play/" + videoidVal);
                    }
                    else if (scope.tube === 'funnyordie') {
                        scope.url = $sce.trustAsResourceUrl("http://www.funnyordie.com/embed/" + videoidVal);
                    }
                    else if (scope.tube === 'stupidvideos') {
                        scope.url = $sce.trustAsResourceUrl("http://www.stupidvideos.com/embed/?video=" + videoidVal);
                    }
                    scope.$apply();
                }
            });
        }
    };
});

var favApp = angular.module("favicon", []);
favApp.filter("favicon", function () {
    var provider = "https://www.google.com/s2/favicons?domain=%s";

    return function (url) {
        return provider.replace(/%s/g, url);
    }
})
.directive("favicon", ["faviconFilter", function (faviconFilter) {
    return {
        restrict: "EA",
        replace: true,
        template: '<img ng-src="{{faviconUrl}}" alt="{{description}}">',
        scope: {
            url: "=",
            description: "="
        },
        link: function ($scope, element, attrs) {
            $scope.$watch("url", function (value) {
                $scope.faviconUrl = faviconFilter(value);
            });
        }
    }
} ]);



// AUTHOR: Bil SerGio
// Angular doesn't automatically scroll to the top when loading a new view, 
// it just keeps the current scroll position.
// The script below checks every 200ms if the new DOM is fully loaded and then 
// scrolls to the top and stops checking. I tried without this 200ms loop and it 
// sometimes failed to scroll because the page was just not completely displayed.
storeApp.run(function ($rootScope, $window) {

    $rootScope.$on('$routeChangeSuccess', function () {

        var interval = setInterval(function () {
            if (document.readyState == 'complete') {
                $window.scrollTo(0, 0);
                clearInterval(interval);
            }
        }, 200);

    });
});



// For later use in login
//function MyController($scope) {
//    $scope.val = "PA";
//    $scope.states = [
//        { name: "Alabama", value: "AL" },
//        { name: "Alaska", value: "AK" },
//        { name: "Arizona", value: "AZ" },
//        { name: "Arkansas", value: "AR" },
//        { name: "California", value: "CA" },
//        { name: "Colorado", value: "CO" },
//        { name: "Connecticut", value: "CT" },
//        { name: "Delaware", value: "DE" },
//        { name: "Florida", value: "FL" },
//        { name: "Georgia", value: "GA" },
//        { name: "Hawaii", value: "HI" },
//        { name: "Idaho", value: "ID" },
//        { name: "Illinois", value: "IL" },
//        { name: "Indiana", value: "IN" },
//        { name: "Iowa", value: "IA" },
//        { name: "Kansas", value: "KS" },
//        { name: "Kentucky", value: "KY" },
//        { name: "Louisiana", value: "LA" },
//        { name: "Maine", value: "ME" },
//        { name: "Maryland", value: "MD" },
//        { name: "Massachusetts", value: "MA" },
//        { name: "Michigan", value: "MI" },
//        { name: "Minnesota", value: "MN" },
//        { name: "Mississippi", value: "MS" },
//        { name: "Missouri", value: "MO" },
//        { name: "Montana", value: "MT" },
//        { name: "Nebraska", value: "NE" },
//        { name: "Nevada", value: "NV" },
//        { name: "New Hampshire", value: "NH" },
//        { name: "New Jersey", value: "NJ" },
//        { name: "New Mexico", value: "NM" },
//        { name: "New York", value: "NY" },
//        { name: "North Carolina", value: "NC" },
//        { name: "North Dakota", value: "ND" },
//        { name: "Ohio", value: "OH" },
//        { name: "Oklahoma", value: "OK" },
//        { name: "Oregon", value: "OR" },
//        { name: "Pennsylvania", value: "PA" },
//        { name: "Rhode Island", value: "RI" },
//        { name: "South Carolina", value: "SC" },
//        { name: "South Dakota", value: "SD" },
//        { name: "Tennessee", value: "TN" },
//        { name: "Texas", value: "TX" },
//        { name: "Utah", value: "UT" },
//        { name: "Vermont", value: "VT" },
//        { name: "Virginia", value: "VA" },
//        { name: "Washington", value: "WA" },
//        { name: "West Virginia", value: "WV" },
//        { name: "Wisconsin", value: "WI" },
//        { name: "Wyoming", value: "WY" }];
//};

//$scope.forceAddItem = function (sku, productname, unitprice, quantity) {
//    DataService.cart.addItemUrl(sku, productname, unitprice, quantity);
//};

//projectsApp.factory("Project", function ($http) {
//    var json = $http.get("project.json").then(function (response) {
//        return response.data;
//    });

//    var Project = function (data) {
//        if (data) angular.copy(data, this);
//    };

//    Project.query = function () {
//        return json.then(function (data) {
//            return data.map(function (project) {
//                return new Project(project);
//            });
//        })
//    };

//    Project.get = function (id) {
//        return json.then(function (data) {
//            var result = null;
//            angular.forEach(data, function (project) {
//                if (project.CF_STORE_ID == id) result = new Project(project);
//            });
//            return result;
//        })
//    };

//    return Project;
//});