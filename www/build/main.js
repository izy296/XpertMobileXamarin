webpackJsonp([2],{

/***/ 101:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__ = __webpack_require__(73);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__ = __webpack_require__(182);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




/*
  Generated class for the StatisticServiceProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
var DashboardServiceProvider = (function () {
    function DashboardServiceProvider(http, helperService) {
        this.http = http;
        this.helperService = helperService;
        this.BASE_URL = "XpertPharm.Rest.Api/api/";
        this.DASHBOARD_URL = "Dashboard/";
        this.MARGE_PAR_VENDEUR_URL = "MargeParVendeur";
        console.log('Hello StatisticServiceProvider Provider');
    }
    DashboardServiceProvider.prototype.initDashBoard = function () {
        var _this = this;
        var newBarChartLabels = [];
        this.getMargeParVendeur().subscribe(function (data) {
            console.log("dahsboard provider ", data[0].Exercice);
            data.forEach(function (el) {
                if (newBarChartLabels.find(function (e) { return e == el.Exercice; }) == undefined) {
                    newBarChartLabels.push(el.Exercice);
                }
            });
            _this.labelMargeParvendeur = newBarChartLabels;
            console.log("dashboard", _this.labelMargeParvendeur);
        }, function (error) {
            console.log(error);
        });
    };
    DashboardServiceProvider.prototype.getMargeParVendeur = function () {
        console.log("url ", this.helperService.networkAddress, this.helperService.networkAddress + this.BASE_URL + this.DASHBOARD_URL + this.MARGE_PAR_VENDEUR_URL);
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.DASHBOARD_URL + this.MARGE_PAR_VENDEUR_URL)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    return DashboardServiceProvider;
}());
DashboardServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__["a" /* HttpInterceptor */],
        __WEBPACK_IMPORTED_MODULE_1__helper_service_helper_service__["a" /* HelperServiceProvider */]])
], DashboardServiceProvider);

//# sourceMappingURL=dashboard-service.js.map

/***/ }),

/***/ 149:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EncaissementPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(14);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


/**
 * Generated class for the EncaissementPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var EncaissementPage = (function () {
    function EncaissementPage(navCtrl, navParams) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.Object = Object;
    }
    EncaissementPage.prototype.ionViewDidLoad = function () {
    };
    EncaissementPage.prototype.ngOnInit = function () {
        this.encaissement = this.navParams.data;
    };
    return EncaissementPage;
}());
EncaissementPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'page-encaissement',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\encaissements\encaissement\encaissement.html"*/'<!--\n\n  Generated template for the EncaissementPage page.\n\n\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n\n  Ionic pages and navigation.\n\n-->\n\n<ion-header>\n\n\n\n    <ion-navbar>\n\n      <ion-title>{{encaissement.CODE_ENCAISS}}</ion-title>\n\n    </ion-navbar>\n\n  \n\n  </ion-header>\n\n  \n\n  \n\n  <ion-content padding>\n\n  <ion-list>\n\n    <ion-item *ngFor="let key of Object.keys(encaissement) ">\n\n      {{ key }} <span float-right> {{ encaissement[key] }} </span>\n\n    </ion-item>\n\n  </ion-list>\n\n  </ion-content>\n\n  '/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\encaissements\encaissement\encaissement.html"*/,
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"], __WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavParams"]])
], EncaissementPage);

//# sourceMappingURL=encaissement.js.map

/***/ }),

/***/ 150:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HomePage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__form_encaissement_form_encaissement__ = __webpack_require__(65);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_menu_sommaire_menu_sommaire__ = __webpack_require__(278);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__ = __webpack_require__(52);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_ionic3_datepicker__ = __webpack_require__(102);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};







var HomePage = HomePage_1 = (function () {
    function HomePage(nav, popoverCtrl, encaisseService, helperService, popOverCtrl, toastCtrl) {
        this.nav = nav;
        this.popoverCtrl = popoverCtrl;
        this.encaisseService = encaisseService;
        this.helperService = helperService;
        this.popOverCtrl = popOverCtrl;
        this.toastCtrl = toastCtrl;
        this.MENU_COMPTE = "COMPTE";
        this.dateNow = new Date().toISOString();
        this.doughnutChartLabels = ['Decaissement', 'Encaissement',];
        this.doughnutChartData = [0, 0];
        this.doughnutChartType = 'doughnut';
        this.dataCompte = [];
        this.datesStatistic = this.helperService.datesStatistic;
        this.localDate = new Date();
        this.initDate = new Date();
        this.initDate2 = new Date(2015, 1, 1);
        this.disabledDates = [new Date(2017, 7, 14)];
        this.maxDate = new Date(new Date().setDate(new Date().getDate() + 30));
        this.min = new Date();
    }
    HomePage.prototype.closeDatepicker = function () {
        this.datepickerDirective.modal.dismiss();
    };
    HomePage.prototype.ionViewWillEnter = function () {
        this.sync();
    };
    HomePage.prototype.Log = function (stuff) {
        console.log(stuff);
    };
    HomePage.prototype.event = function (data) {
        this.dateEncaiss = data;
        console.log("dateencaiss ", this.dateEncaiss);
        this.localDate = data;
    };
    HomePage.prototype.setDate = function (date) {
        console.log(date);
        this.localDate = date;
    };
    HomePage.prototype.sync = function () {
        this.datesStatistic = this.helperService.datesStatistic;
        this.getStatistic();
        this.setDataChart();
    };
    HomePage.prototype.chartClicked = function (e) {
    };
    HomePage.prototype.chartHovered = function (e) {
    };
    HomePage.prototype.ngOnInit = function () {
        this.setDataChart();
        this.getCompte();
        this.datesStatistic = this.helperService.datesStatistic;
        if (this.helperService.datesStatistic != null) {
            this.getStatistic();
        }
    };
    HomePage.prototype.showMenuSommaire = function () {
        console.log("cree menu");
        var menu = this.popOverCtrl.create(__WEBPACK_IMPORTED_MODULE_2__components_menu_sommaire_menu_sommaire__["a" /* MenuSommaireComponent */]);
        menu.present({});
    };
    HomePage.prototype.getCompte = function () {
        var _this = this;
        this.encaisseService.getComptes().subscribe(function (data) {
            _this.dataCompte = data;
            console.log(_this.dataCompte);
        }, function (error) {
            var toast = _this.toastCtrl.create({
                message: 'Erreur Chargement Comptes ' + error,
                duration: 3000,
                position: 'bottom',
                cssClass: 'dark-trans',
                closeButtonText: 'OK',
                showCloseButton: true
            });
            toast.present();
        });
    };
    HomePage.prototype.delete = function (dateDebut, dateFin) {
        this.helperService.deleteDateStat(dateDebut, dateFin);
        console.log("delete");
        this.nav.setRoot(HomePage_1);
    };
    HomePage.prototype.getStatistic = function () {
        var _this = this;
        this.helperService.datesStatistic.map(function (e) { return __awaiter(_this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                this.encaisseService.getStatisticEncaiss(e.dateDebut.substring(0, 10), e.dateFin.substring(0, 10)).subscribe(function (result) {
                    if (result != null) {
                        result.map(function (el) {
                            switch (el.CODE_TYPE) {
                                case 'ENC':
                                    console.log("enc" + el.TOTAL_ENCAISS);
                                    e.ENC = (el.TOTAL_ENCAISS == null) ? 0 : el.TOTAL_ENCAISS;
                                    break;
                                case 'DEC':
                                    e.DEC = (el.TOTAL_ENCAISS == null) ? 0 : el.TOTAL_ENCAISS;
                                    break;
                            }
                        });
                    }
                }, function (error) {
                    var toast = _this.toastCtrl.create({
                        message: 'Erreur Statistique Encaisseement ' + error,
                        duration: 3000,
                        position: 'bottom',
                        cssClass: 'dark-trans',
                        closeButtonText: 'OK',
                        showCloseButton: true
                    });
                    toast.present();
                });
                return [2 /*return*/];
            });
        }); });
    };
    HomePage.prototype.setDataChart = function () {
        var _this = this;
        var data = [0, 0];
        this.encaisseService.getStatisticEncaiss(this.dateNow.substring(0, 10), this.dateNow.substring(0, 10)).subscribe(function (result) {
            if (result != null) {
                result.map(function (e) {
                    switch (e.CODE_TYPE) {
                        case 'ENC':
                            data[1] = (e.TOTAL_ENCAISS == null) ? 0 : e.TOTAL_ENCAISS;
                            break;
                        case 'DEC':
                            data[0] = (e.TOTAL_ENCAISS == null) ? 0 : e.TOTAL_ENCAISS;
                            break;
                    }
                    _this.doughnutChartData = data;
                });
            }
        }, function (error) {
            var toast = _this.toastCtrl.create({
                message: 'Erreur Statistique Encaisseement ' + error,
                duration: 3000,
                position: 'bottom',
                cssClass: 'dark-trans',
                closeButtonText: 'OK',
                showCloseButton: true
            });
            toast.present();
        });
    };
    HomePage.prototype.addEncaissementPage = function (codeType) {
        this.nav.push(__WEBPACK_IMPORTED_MODULE_0__form_encaissement_form_encaissement__["a" /* FormEncaissementPage */], { type: codeType });
        this.sync();
    };
    return HomePage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_4__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_6_ionic3_datepicker__["a" /* DatePickerDirective */]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_6_ionic3_datepicker__["a" /* DatePickerDirective */])
], HomePage.prototype, "datepickerDirective", void 0);
HomePage = HomePage_1 = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_4__angular_core__["Component"])({
        selector: 'page-home',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\home\home.html"*/'<!-- -->\n<ion-header>\n  <ion-navbar color="primary">\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n    <ion-title>\n      <strong>Accueil</strong>\n    </ion-title>\n    <ion-buttons end>\n      <button ion-button tappable (click)="sync()">\n        <ion-icon name="sync"></ion-icon>\n      </button>\n    </ion-buttons>\n  </ion-navbar>\n</ion-header>\n<ion-content padding class="animated fadeIn common-bg">\n  <ion-card class="border-bottom">\n    <ion-item tappable class="border-bottom ">\n      <span ion-text>\n        <strong>Sommaire </strong>\n      </span>\n      <ion-icon name="add" item-end Tappable (click)="showMenuSommaire()"></ion-icon>\n    </ion-item>\n    <ion-grid no-padding margin-top>\n      <ion-list>\n        <ion-item>\n          <div>\n            <h4 text-center> Aujourd\'hui </h4>\n            <ion-row margin-top>\n              <ion-col class="text-sm" width-70>\n                <span ion-text color="primary">\n                  <strong>Encaissement </strong>\n                </span>\n              </ion-col>\n              <ion-col class="text-sm" width-10 text-right>\n                <span ion-text color="primary">\n                  <strong>{{ doughnutChartData[1]| xpertCurrency :2}} </strong>\n                </span>\n              </ion-col>\n            </ion-row>\n            <ion-row margin-top>\n              <ion-col class="text-sm" width-70>\n                <span ion-text color="danger">\n                  <strong>Decaissement </strong>\n                </span>\n              </ion-col>\n              <ion-col class="text-sm" width-10 text-right>\n                <span ion-text color="danger">\n                  <strong>{{ doughnutChartData[0] | xpertCurrency : 2 }} </strong>\n                </span>\n              </ion-col>\n            </ion-row>\n          </div>\n        </ion-item>\n        <ion-item-sliding *ngFor="let date of datesStatistic">\n          <ion-item>\n            <div>\n              <h4 text-center> {{date.dateDebut | date : \'dd-MM-yyyy\'}} - {{ date.dateFin | date : \'dd-MM-yyyy\'}} </h4>\n              <ion-row>\n                <ion-col class="text-sm" width-70>\n                  <span ion-text color="primary">\n                    <strong>Encaissement </strong>\n                  </span>\n                </ion-col>\n                <ion-col class="text-sm" width-10 text-right>\n                  <span ion-text color="primary">\n                    <strong>{{ date.ENC | xpertCurrency :2 }} </strong>\n                  </span>\n                </ion-col>\n              </ion-row>\n              <ion-row margin-top>\n                <ion-col class="text-sm" width-70>\n                  <span ion-text color="danger">\n                    <strong>Decaissement </strong>\n                  </span>\n                </ion-col>\n                <ion-col class="text-sm" width-10 text-right>\n                  <span ion-text color="danger">\n                    <strong>{{ date.DEC | xpertCurrency : 2 }} </strong>\n                  </span>\n                </ion-col>\n              </ion-row>\n            </div>\n          </ion-item>\n          <ion-item-options side="right">\n            <button ion-button (click)="delete(date.dateDebut,date.dateFin)" color="danger">\n              <ion-icon name="trash"></ion-icon>\n            </button>\n          </ion-item-options>\n        </ion-item-sliding>\n      </ion-list>\n    </ion-grid>\n  </ion-card>\n  <ion-card class="border-bottom">\n    <ion-item tappable class="border-bottom">\n      <span ion-text>\n        <strong>Comptes</strong>\n      </span>\n      <ion-icon name="more" item-end Tappable></ion-icon>\n    </ion-item>\n    <ion-grid class="" no-padding margin-top>\n      <ion-list>\n        <ion-item>\n          <div>\n            <ion-row *ngFor="let compte of dataCompte">\n              <ion-col class="text-sm" width-70>\n                <span ion-text color="primary">\n                  <strong>{{ compte.DESIGN_COMPTE }} </strong>\n                </span>\n              </ion-col>\n              <ion-col class="text-sm" width-10 text-right>\n                <span ion-text color="primary" text-right>\n                  <strong>{{ compte.SOLDE_COMPTE | xpertCurrency : 2 }} </strong>\n                </span>\n              </ion-col>\n            </ion-row>\n          </div>\n        </ion-item>\n      </ion-list>\n    </ion-grid>\n  </ion-card>\n\n  <div style="display: block">\n    <canvas baseChart [data]="doughnutChartData" [labels]="doughnutChartLabels" [chartType]="doughnutChartType" (chartHover)="chartHovered($event)"\n      (chartClick)="chartClicked($event)">\n    </canvas>\n  </div>\n  <ion-fab bottom right>\n    <button ion-fab>\n      <ion-icon name="add"></ion-icon>\n    </button>\n    <ion-fab-list side="top">\n      <button ion-fab color="green" (click)="addEncaissementPage(\'ENC\')">\n        <ion-icon color="white" name="add"></ion-icon>\n      </button>\n      <button color="danger" ion-fab (click)="addEncaissementPage(\'DEC\')">\n        <ion-icon name="remove"></ion-icon>\n      </button>\n\n    </ion-fab-list>\n  </ion-fab>\n</ion-content>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\home\home.html"*/,
        providers: [__WEBPACK_IMPORTED_MODULE_6_ionic3_datepicker__["a" /* DatePickerDirective */]]
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_5_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["ToastController"]])
], HomePage);

var HomePage_1;
//
//# sourceMappingURL=home.js.map

/***/ }),

/***/ 151:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__home_home__ = __webpack_require__(150);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_auth_service_auth_service__ = __webpack_require__(74);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};






var LoginPage = (function () {
    function LoginPage(nav, addrCtrl, menu, toastCtrl, authService, storage, helperService) {
        this.nav = nav;
        this.addrCtrl = addrCtrl;
        this.menu = menu;
        this.toastCtrl = toastCtrl;
        this.authService = authService;
        this.storage = storage;
        this.helperService = helperService;
        this.username = "";
        this.password = "";
        this.showOptions = true;
        this.TOKEN_KEY = 'token';
        this.REMEMBER_KEY = 'remember';
        this.USERNAME_KEY = 'username';
        this.PASSWORD_KEY = 'password';
        this.networkAddress = 'localhost';
        this.menu.swipeEnable(false);
    }
    LoginPage.prototype.OnSignin = function (form) {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.username = form.value.username;
                        this.password = form.value.password;
                        return [4 /*yield*/, this.authService.getAuthentification(this.username, this.password).subscribe(function (data) {
                                if (data.access_token != null) {
                                    _this.authService.setToken(data.access_token);
                                    _this.rememberMeSave(_this.username, _this.password);
                                    _this.nav.setRoot(__WEBPACK_IMPORTED_MODULE_4__home_home__["a" /* HomePage */]);
                                }
                            }, function (error) {
                                if (error == "invalid_grant") {
                                    var toast = _this.toastCtrl.create({
                                        message: "l'identifiant ou le mot de passe est incorrect",
                                        duration: 5000,
                                        position: 'bottom',
                                        closeButtonText: 'OK',
                                        showCloseButton: true
                                    });
                                    toast.present();
                                }
                                else {
                                    var toast = _this.toastCtrl.create({
                                        message: "l'application n'a pas pu se connecter au seveur",
                                        duration: 5000,
                                        position: 'bottom',
                                        closeButtonText: 'OK',
                                        showCloseButton: true
                                    });
                                    toast.present();
                                }
                            })];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    // login and go to home page
    // remember username and password
    LoginPage.prototype.rememberMeSave = function (username, password) {
        if (this.rememberMeToggle.value) {
            this.storage.set(this.REMEMBER_KEY, 1);
            this.storage.set(this.USERNAME_KEY, username);
            this.storage.set(this.PASSWORD_KEY, password);
        }
        else {
            this.storage.set(this.REMEMBER_KEY, 0);
            this.storage.set(this.USERNAME_KEY, " ");
            this.storage.set(this.PASSWORD_KEY, " ");
        }
    };
    LoginPage.prototype.setAddressNetwork = function () {
        var _this = this;
        this.networkAddress = this.helperService.networkAddress;
        var addresseInput = this.addrCtrl.create({
            title: 'Addresse du serveur',
            message: "Entrez l'adresse de votre serveur.",
            inputs: [
                {
                    name: 'addresse',
                    placeholder: 'address',
                    type: 'text',
                    value: this.networkAddress
                },
            ],
            buttons: [
                {
                    text: 'Cancel',
                    handler: function (data) {
                        console.log('Cancel clicked');
                    }
                },
                {
                    text: 'Save',
                    handler: function (data) {
                        _this.helperService.saveNetworkAddress(data.addresse);
                        var toast = _this.toastCtrl.create({
                            message: 'l\'adresse a etait mise à jour',
                            duration: 3000,
                            position: 'bottom',
                            cssClass: 'dark-trans',
                            closeButtonText: 'OK',
                            showCloseButton: true
                        });
                        toast.present();
                    }
                }
            ]
        });
        addresseInput.present();
    };
    LoginPage.prototype.loadLoginData = function () {
        var _this = this;
        this.storage.get(this.REMEMBER_KEY).then(function (val) {
            _this.remember = val;
            if (_this.remember == 1) {
                _this.storage.get(_this.USERNAME_KEY).then(function (val) {
                    _this.username = val;
                }).catch(function (error) { return error; });
                _this.storage.get(_this.PASSWORD_KEY).then(function (val) {
                    _this.password = val;
                }).catch(function (error) { return error; });
            }
        }).catch(function (error) { return error; });
    };
    LoginPage.prototype.ngOnInit = function () {
        this.loadLoginData();
    };
    return LoginPage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["ViewChild"])('rememberMe'),
    __metadata("design:type", Object)
], LoginPage.prototype, "rememberMeToggle", void 0);
LoginPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Component"])({
        selector: 'page-login',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\login\login.html"*/'<!-- -->\n<ion-content padding class="animated fadeIn login auth-page">\n  <div class="login-content">\n\n    <!-- Logo -->\n    <div padding-horizontal text-center class="animated fadeInDown">\n      <div class="logo"></div>\n      \n    </div>\n\n    <!-- Login form -->\n    <form #f="ngForm" class="list-form"  (ngSubmit)="OnSignin(f)">\n      <ion-item>\n        <ion-label floating>\n          <ion-icon name="contact" item-start class="text-primary"></ion-icon>\n          Username\n        </ion-label>\n        <ion-input type="text" [(ngModel)]="username" name="username" required username></ion-input>\n      </ion-item>\n\n      <ion-item>\n        <ion-label floating>\n          <ion-icon name="lock" item-start class="text-primary"></ion-icon>\n          Password\n        </ion-label>\n        <ion-input type="password" [(ngModel)]="password" name="password" required></ion-input>\n      </ion-item>\n      <ion-item no-lines>\n        <ion-label> se souvenir</ion-label>\n        <ion-toggle checked="true" #rememberMe></ion-toggle>\n      </ion-item>\n    </form>\n    <p text-right ion-text color="secondary" tappable (click)="setAddressNetwork()">\n      <strong>changer l\'address du serveur</strong>\n    </p>\n    <div>\n     \n      <button ion-button icon-start block color="dark" type="submit" (click)="OnSignin(f)" [disabled]="!f.valid">\n        <ion-icon name="log-in"></ion-icon>\n        Connexion\n      </button>\n    </div>\n  </div>\n</ion-content> '/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\login\login.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["AlertController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["MenuController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["ToastController"],
        __WEBPACK_IMPORTED_MODULE_5__providers_auth_service_auth_service__["a" /* AuthServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_1__ionic_storage__["b" /* Storage */],
        __WEBPACK_IMPORTED_MODULE_0__providers_helper_service_helper_service__["a" /* HelperServiceProvider */]])
], LoginPage);

//# sourceMappingURL=login.js.map

/***/ }),

/***/ 152:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_dashboard_service_dashboard_service__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic3_datepicker__ = __webpack_require__(102);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__form_encaissement_form_encaissement__ = __webpack_require__(65);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__providers_helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_encaiss_service_encaiss_service__ = __webpack_require__(52);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};








/**
 * Generated class for the DashboardPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var DashboardPage = (function () {
    function DashboardPage(nav, popoverCtrl, encaisseService, helperService, toastCtrl, dashboardService) {
        this.nav = nav;
        this.popoverCtrl = popoverCtrl;
        this.encaisseService = encaisseService;
        this.helperService = helperService;
        this.toastCtrl = toastCtrl;
        this.dashboardService = dashboardService;
        this.MENU_COMPTE = "COMPTE";
        this.dateNow = new Date().toISOString();
        this.doughnutChartLabels = ['Decaissement', 'Encaissement',];
        this.doughnutChartData = [0, 0];
        this.doughnutChartType = 'doughnut';
        this.dataCompte = [];
        this.datesStatistic = this.helperService.datesStatistic;
        this.localDate = new Date();
        this.initDate = new Date();
        this.initDate2 = new Date(2015, 1, 1);
        this.disabledDates = [new Date(2017, 7, 14)];
        this.maxDate = new Date(new Date().setDate(new Date().getDate() + 30));
        this.min = new Date();
        this.barChartOptions = {
            scaleShowVerticalLines: false,
            responsive: true
        };
        this.labelsMargeParVendeur = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
        this.dataMargeParVendeur = [
            { data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
            { data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B' }
        ];
        this.barChartLabels = this.dashboardService.labelMargeParvendeur;
        this.barChartType = 'bar';
        this.barChartLegend = true;
        this.barChartData = [{ data: [0, 0], label: '' },
            { data: [0, 0], label: '' }];
    }
    DashboardPage.prototype.closeDatepicker = function () {
        this.datepickerDirective.modal.dismiss();
    };
    DashboardPage.prototype.ionViewWillEnter = function () {
        this.sync();
    };
    DashboardPage.prototype.Log = function (stuff) {
        console.log(stuff);
    };
    DashboardPage.prototype.event = function (data) {
        this.dateEncaiss = data;
        this.localDate = data;
    };
    DashboardPage.prototype.setDate = function (date) {
        console.log(date);
        this.localDate = date;
    };
    DashboardPage.prototype.sync = function () {
        this.datesStatistic = this.helperService.datesStatistic;
        this.setDataChart();
    };
    DashboardPage.prototype.chartClicked = function (e) {
    };
    DashboardPage.prototype.chartHovered = function (e) {
    };
    DashboardPage.prototype.ngOnInit = function () {
        var _this = this;
        var newBarChartData = [];
        var newBarChartLabels = [];
        this.setDataChart();
        console.log("dashboard");
        this.dashboardService.getMargeParVendeur().subscribe(function (data) {
            console.log(data);
            data.forEach(function (el) {
                if (newBarChartData.find(function (e) { return e.label == el.CREATED_BY; }) == undefined) {
                    newBarChartData.push({ data: [], label: el.CREATED_BY });
                }
                newBarChartData.find(function (e) { return e.label == el.CREATED_BY; }).data.push(el.Sum_MARGE);
            });
            _this.barChartLabels = newBarChartLabels;
            _this.barChartData = newBarChartData;
        }, function (error) {
            console.log(error);
        });
    };
    DashboardPage.prototype.setDataChart = function () {
        var _this = this;
        var data = [0, 0];
        this.encaisseService.getStatisticEncaiss(this.dateNow.substring(0, 10), this.dateNow.substring(0, 10)).subscribe(function (result) {
            if (result != null) {
                result.map(function (e) {
                    switch (e.CODE_TYPE) {
                        case 'ENC':
                            data[1] = (e.TOTAL_ENCAISS == null) ? 0 : e.TOTAL_ENCAISS;
                            break;
                        case 'DEC':
                            data[0] = (e.TOTAL_ENCAISS == null) ? 0 : e.TOTAL_ENCAISS;
                            break;
                    }
                    _this.doughnutChartData = data;
                });
            }
        }, function (error) {
            var toast = _this.toastCtrl.create({
                message: 'Erreur Statistique Encaisseement ' + error,
                duration: 3000,
                position: 'bottom',
                cssClass: 'dark-trans',
                closeButtonText: 'OK',
                showCloseButton: true
            });
            toast.present();
        });
    };
    DashboardPage.prototype.addEncaissementPage = function (codeType) {
        this.nav.push(__WEBPACK_IMPORTED_MODULE_2__form_encaissement_form_encaissement__["a" /* FormEncaissementPage */], { type: codeType });
        this.sync();
    };
    return DashboardPage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_6__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_1_ionic3_datepicker__["a" /* DatePickerDirective */]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1_ionic3_datepicker__["a" /* DatePickerDirective */])
], DashboardPage.prototype, "datepickerDirective", void 0);
DashboardPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_6__angular_core__["Component"])({
        selector: 'page-dashboard',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\dashboard\dashboard.html"*/'<!-- -->\n<ion-header>\n  <ion-navbar color="primary">\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n    <ion-title>\n      <strong>Dashboard</strong>\n    </ion-title>\n    <ion-buttons end>\n      <button ion-button tappable (click)="sync()">\n        <ion-icon name="sync"></ion-icon>\n      </button>\n    </ion-buttons>\n  </ion-navbar>\n</ion-header>\n<ion-content padding class="animated fadeIn common-bg">\n  <div>\n    <div style="display: block">\n      <canvas baseChart   [datasets]="barChartData" [labels]="barChartLabels" [options]="barChartOptions" [legend]="barChartLegend"\n        [chartType]="barChartType" (chartHover)="chartHovered($event)" (chartClick)="chartClicked($event)"></canvas>\n      <h1 color="black" text-center class="text-1x">\n        <strong>Marge par Vendeur</strong>\n      </h1>\n    </div>\n  </div>\n  \n  <div style="display: block">\n    <canvas baseChart [data]="doughnutChartData" [labels]="doughnutChartLabels" [chartType]="doughnutChartType" (chartHover)="chartHovered($event)"\n      (chartClick)="chartClicked($event)">\n    </canvas>\n    <h1 color="black" text-center class="text-1x">\n      <strong>Encaissement / Decaissement</strong>\n    </h1>\n  </div>\n  \n  <ion-fab bottom right>\n    <button ion-fab>\n      <ion-icon name="add"></ion-icon>\n    </button>\n    <ion-fab-list side="top">\n      <button ion-fab color="green" (click)="addEncaissementPage(\'ENC\')">\n        <ion-icon color="white" name="add"></ion-icon>\n      </button>\n      <button color="danger" ion-fab (click)="addEncaissementPage(\'DEC\')">\n        <ion-icon name="remove"></ion-icon>\n      </button>\n    </ion-fab-list>\n  </ion-fab>\n</ion-content>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\dashboard\dashboard.html"*/,
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_5__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_4__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["ToastController"],
        __WEBPACK_IMPORTED_MODULE_0__providers_dashboard_service_dashboard_service__["a" /* DashboardServiceProvider */]])
], DashboardPage);

//# sourceMappingURL=dashboard.js.map

/***/ }),

/***/ 153:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return InventairePage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__ionic_native_barcode_scanner__ = __webpack_require__(234);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(14);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/**
 * Generated class for the InventairePage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var InventairePage = (function () {
    function InventairePage(navCtrl, navParams, barcodeScanner) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.barcodeScanner = barcodeScanner;
        this.code_barr = "code Barre";
    }
    InventairePage.prototype.ionViewDidLoad = function () {
        console.log('ionViewDidLoad InventairePage');
    };
    InventairePage.prototype.scanBarcode = function () {
        var _this = this;
        console.log("scan");
        this.barcodeScanner.scan().then(function (barcodeData) {
            console.log('Barcode data', barcodeData);
            _this.code_barr = barcodeData.text;
        }).catch(function (err) {
            console.log('Error', err);
        });
    };
    return InventairePage;
}());
InventairePage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Component"])({
        selector: 'page-inventaire',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\inventaire\inventaire.html"*/'<!-- -->\n<ion-header>\n  <ion-navbar color="primary">\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n    <ion-title>\n      <strong>Inventaire</strong>\n    </ion-title>\n  \n  </ion-navbar>\n</ion-header>\n<ion-content padding class="animated fadeIn common-bg">\n  <Button ion-button (click)="scanBarcode();"> scan</Button>\n  <p>{{ code_barr }}</p>\n</ion-content>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\inventaire\inventaire.html"*/,
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavParams"],
        __WEBPACK_IMPORTED_MODULE_0__ionic_native_barcode_scanner__["a" /* BarcodeScanner */]])
], InventairePage);

//# sourceMappingURL=inventaire.js.map

/***/ }),

/***/ 164:
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncatched exception popping up in devtools
	return Promise.resolve().then(function() {
		throw new Error("Cannot find module '" + req + "'.");
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = 164;

/***/ }),

/***/ 233:
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"../pages/dashboard/dashboard.module": [
		788,
		1
	],
	"../pages/inventaire/inventaire.module": [
		789,
		0
	]
};
function webpackAsyncContext(req) {
	var ids = map[req];
	if(!ids)
		return Promise.reject(new Error("Cannot find module '" + req + "'."));
	return __webpack_require__.e(ids[1]).then(function() {
		return __webpack_require__(ids[0]);
	});
};
webpackAsyncContext.keys = function webpackAsyncContextKeys() {
	return Object.keys(map);
};
webpackAsyncContext.id = 233;
module.exports = webpackAsyncContext;

/***/ }),

/***/ 278:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MenuSommaireComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/**
 * Generated class for the MenuSommaireComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
var MenuSommaireComponent = (function () {
    function MenuSommaireComponent(helperService, navCtrl) {
        this.helperService = helperService;
        this.navCtrl = navCtrl;
        this.dateDebut = new Date().toISOString();
        this.dateFin = new Date().toISOString();
    }
    MenuSommaireComponent.prototype.add = function () {
        console.log(this.dateDebut, this.dateFin);
        this.helperService.saveDateStatistic(this.dateDebut, this.dateFin);
        this.navCtrl.pop();
    };
    MenuSommaireComponent.prototype.onChange = function (event) {
        console.log(event);
    };
    return MenuSommaireComponent;
}());
MenuSommaireComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Component"])({
        selector: 'menu-sommaire',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\components\menu-sommaire\menu-sommaire.html"*/'<!-- Generated template for the MenuSommaireComponent component -->\n<div>\n  <form #f="ngForm" class="list-form" (ngSubmit)="add()">\n    <ion-list>\n      <ion-item>\n        <ion-label>Date debut</ion-label>\n        <ion-datetime displayFormat="DD-MM-YYYY" pickerFormat="DD-MM-YYYY" value="" ng-model [(ngModel)]="dateDebut" name="dateDebut"\n          required>\n        </ion-datetime>\n      </ion-item>\n      <ion-item>\n        <ion-label>Date Fin</ion-label>\n        <ion-datetime displayFormat="DD-MM-YYYY" pickerFormat="DD-MM-YYYY" value="" ng-model [(ngModel)]="dateFin" name="dateFin"\n          required>\n        </ion-datetime>\n      </ion-item>\n      <button ion-button icon-start block color="dark" type="submit" [disabled]="!f.valid">\n        <ion-icon name="add"></ion-icon>\n      </button>\n    </ion-list>\n  </form>\n</div>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\components\menu-sommaire\menu-sommaire.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["NavController"]])
], MenuSommaireComponent);

//# sourceMappingURL=menu-sommaire.js.map

/***/ }),

/***/ 279:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MenuEncaissementListComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_encaiss_service_encaiss_service__ = __webpack_require__(52);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__pages_form_encaissement_form_encaissement__ = __webpack_require__(65);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__pages_encaissements_encaissement_encaissement__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var MenuEncaissementListComponent = (function () {
    function MenuEncaissementListComponent(viewCtrl, navCtrl, app, modalCtrl, navParams, alertCtrl, encaisseService, toastCtrl) {
        this.viewCtrl = viewCtrl;
        this.navCtrl = navCtrl;
        this.app = app;
        this.modalCtrl = modalCtrl;
        this.navParams = navParams;
        this.alertCtrl = alertCtrl;
        this.encaisseService = encaisseService;
        this.toastCtrl = toastCtrl;
        this.encaissement = navParams.get('data');
        this.encaissementsPage = navParams.get('parent');
    }
    MenuEncaissementListComponent.prototype.showDetail = function () {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_3__pages_encaissements_encaissement_encaissement__["a" /* EncaissementPage */], this.encaissement);
    };
    MenuEncaissementListComponent.prototype.updateEncaissement = function () {
        this.navCtrl.pop();
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_2__pages_form_encaissement_form_encaissement__["a" /* FormEncaissementPage */], { 'encaissement': this.encaissement, 'update': true, 'encaissementsPage': this.encaissementsPage });
    };
    MenuEncaissementListComponent.prototype.deleteEncaissement = function () {
        var _this = this;
        var confirm = this.alertCtrl.create({
            message: 'voulez-vous supprimer cet encaissement ?',
            buttons: [
                {
                    text: 'ok',
                    handler: function () {
                        _this.encaisseService.deleteEncaissement(_this.encaissement.CODE_ENCAISS).subscribe(function (data) {
                            _this.toastCtrl.create({
                                message: "l'encaisseemnt a etait bien supprimer'",
                                duration: 5000,
                                position: 'bottom',
                                closeButtonText: 'OK',
                                showCloseButton: true
                            }).present();
                        }, function (error) {
                            _this.toastCtrl.create({
                                message: "Erreur : " + error + "est survenue",
                                duration: 5000,
                                position: 'bottom',
                                closeButtonText: 'OK',
                                showCloseButton: true
                            }).present();
                            console.log(error);
                        });
                    }
                },
                {
                    text: 'Annuler',
                    handler: function () {
                    }
                }
            ]
        });
        confirm.present();
    };
    return MenuEncaissementListComponent;
}());
MenuEncaissementListComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_4__angular_core__["Component"])({
        template: "\n    <ion-list>\n      <button ion-item  (click)=\"updateEncaissement()\">Modifier</button>\n      <button ion-item (click) =\"deleteEncaissement()\">Supprimer</button>\n      <button ion-item (click)=\"showDetail()\">Detail</button>\n    </ion-list>\n  "
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0_ionic_angular__["ViewController"],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["App"],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["ModalController"],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["NavParams"],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["AlertController"],
        __WEBPACK_IMPORTED_MODULE_1__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["ToastController"]])
], MenuEncaissementListComponent);

//# sourceMappingURL=menu-encaissement-list.js.map

/***/ }),

/***/ 280:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EncaissementsPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__models_tiers__ = __webpack_require__(733);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__form_encaissement_form_encaissement__ = __webpack_require__(65);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_menu_encaissement_list_menu_encaissement_list__ = __webpack_require__(279);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__encaissement_encaissement__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_encaiss_service_encaiss_service__ = __webpack_require__(52);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};









var EncaissementsPage = (function () {
    function EncaissementsPage(navCtrl, navParams, encaisseService, popOverCtrl, toastCtrl) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.encaisseService = encaisseService;
        this.popOverCtrl = popOverCtrl;
        this.toastCtrl = toastCtrl;
        this.encaissementList = [];
        this.type = "ENC";
        this.isLoading = true;
        this.page = 1;
        this.perPage = 0;
        this.totalData = 0;
        this.totalPage = 0;
        this.localDateDebut = new Date();
        this.localDateFin = new Date();
        this.hours = new Date().getHours();
        this.dateDebut = null;
        this.dateFin = null;
        this.tiers = new __WEBPACK_IMPORTED_MODULE_0__models_tiers__["a" /* Tiers */]();
        this.filter = false;
        this.caissesList = [];
        this.motifsList = [];
        this.codeMotif = "all";
        this.codeCompte = "all";
    }
    EncaissementsPage.prototype.ionViewWillEnter = function () {
        this.getEncaissementSPerPage();
        console.log("will enter");
    };
    EncaissementsPage.prototype.ionViewDidEnter = function () {
        console.log("did enter");
    };
    EncaissementsPage.prototype.ionViewCanEnter = function () {
        console.log("can enter");
    };
    EncaissementsPage.prototype.getCaisses = function () {
        this.caissesList = this.encaisseService.caissesList;
    };
    // show filter menu
    EncaissementsPage.prototype.showMenuFilter = function () {
        this.filter = !(this.filter);
        if (this.filter == false) {
            //this.getEncaissementsPerPage();
        }
    };
    EncaissementsPage.prototype.filterEncaiss = function () {
        this.getEncaissementSPerPage();
    };
    // get the detail of encaiss and show it in EncaissementPage
    EncaissementsPage.prototype.getEncaissementDetail = function (encaissement) {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_4__encaissement_encaissement__["a" /* EncaissementPage */], encaissement);
    };
    EncaissementsPage.prototype.showMenu = function (event, encaissement) {
        var menu = this.popOverCtrl.create(__WEBPACK_IMPORTED_MODULE_2__components_menu_encaissement_list_menu_encaissement_list__["a" /* MenuEncaissementListComponent */], { data: encaissement, parent: this });
        menu.present({
            ev: event
        });
    };
    EncaissementsPage.prototype.ngOnInit = function () {
        this.getMotifs();
        this.getTiers();
        this.getCaisses();
    };
    EncaissementsPage.prototype.getTiers = function () {
        this.listTiers = this.encaisseService.tiersList;
    };
    EncaissementsPage.prototype.getMotifs = function () {
        this.motifsList = this.encaisseService.motifsList;
        console.log(this.motifsList);
    };
    EncaissementsPage.prototype.addEncaissementPage = function (codeType) {
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_1__form_encaissement_form_encaissement__["a" /* FormEncaissementPage */], { type: codeType });
    };
    EncaissementsPage.prototype.setDateDebut = function (date) {
        this.localDateDebut = date;
        this.dateDebut = this.localDateDebut.toISOString();
        console.log(this.localDateDebut.setHours(this.hours));
        this.dateDebut = this.localDateDebut.toISOString();
    };
    EncaissementsPage.prototype.setDateFin = function (date) {
        this.localDateFin = date;
        this.dateFin = this.localDateFin.toISOString();
        console.log(this.localDateFin.setHours(this.hours));
        this.dateFin = this.localDateFin.toISOString();
    };
    EncaissementsPage.prototype.getEncaissementSPerPage = function () {
        var _this = this;
        console.log("this getEncaissement Per Page");
        this.isLoading = true;
        this.content.scrollToTop();
        this.page = 1;
        if (!this.filter) {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page, null, null, this.tiers.CODE_TIERS, this.codeMotif, this.codeCompte)
                .subscribe(function (data) {
                _this.data = data;
                console.log("encaissement", data);
                _this.isLoading = false;
                _this.encaissementList = _this.data;
            }, function (error) {
                console.log(error);
            });
        }
        else {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page, this.localDateDebut.toDateString(), this.localDateFin.toDateString(), this.tiers.CODE_TIERS, this.codeMotif, this.codeCompte)
                .subscribe(function (data) {
                _this.data = data;
                _this.isLoading = false;
                _this.encaissementList = _this.data;
            }, function (error) {
                console.log(error);
            });
        }
    };
    EncaissementsPage.prototype.doInfinite = function (infiniteScroll) {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                this.page = this.page + 1;
                if (!this.filter) {
                    setTimeout(function () {
                        _this.encaisseService.getEncaissementsPerPage(_this.type, _this.page, null, null, _this.tiers.CODE_TIERS, _this.codeMotif, _this.codeCompte)
                            .subscribe(function (data) {
                            _this.data = data;
                            for (var i = 0; i < _this.data.length; i++) {
                                _this.encaissementList.push(_this.data[i]);
                            }
                        }, function (error) { return console.log(error); });
                        console.log('Async operation has ended');
                        infiniteScroll.complete();
                    }, 1000);
                }
                else {
                    setTimeout(function () {
                        _this.encaisseService.getEncaissementsPerPage(_this.type, _this.page, _this.localDateDebut.toDateString(), _this.localDateFin.toDateString(), _this.tiers.CODE_TIERS, _this.codeMotif, _this.codeCompte)
                            .subscribe(function (data) {
                            _this.data = data;
                            for (var i = 0; i < _this.data.length; i++) {
                                _this.encaissementList.push(_this.data[i]);
                            }
                        }, function (error) { return console.log(error); });
                        console.log('Async operation has ended');
                        infiniteScroll.complete();
                    }, 1000);
                }
                return [2 /*return*/];
            });
        });
    };
    EncaissementsPage.prototype.tiersChange = function (event) {
    };
    return EncaissementsPage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_6__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["Content"]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["Content"])
], EncaissementsPage.prototype, "content", void 0);
EncaissementsPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_6__angular_core__["Component"])({
        selector: 'page-encaissements',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\encaissements\encaissements.html"*/'<!-- -->\n\n<ion-header>\n\n    <ion-navbar color="primary">\n\n        <button ion-button menuToggle start>\n\n            <ion-icon name="menu"></ion-icon>\n\n        </button>\n\n\n\n        <ion-toolbar>\n\n            <ion-segment color="white" [(ngModel)]="type">\n\n                <ion-segment-button color="green" value="ENC" (click)="getEncaissementSPerPage()">\n\n                    <ion-icon color="white" name="trending-up"></ion-icon>\n\n                </ion-segment-button>\n\n                <ion-segment-button value="DEC" color="danger" (click)="getEncaissementSPerPage()">\n\n                    <ion-icon color="danger" name="trending-down"></ion-icon>\n\n                </ion-segment-button>\n\n                <ion-segment-button color="white" (click)="getEncaissementSPerPage()" value="All">\n\n                    <span>All</span>\n\n                </ion-segment-button>\n\n            </ion-segment>\n\n        </ion-toolbar>\n\n        <ion-buttons end>\n\n            <button ion-button tappable (click)="getEncaissementSPerPage()">\n\n                <ion-icon name="sync"></ion-icon>\n\n            </button>\n\n        </ion-buttons>\n\n    </ion-navbar>\n\n</ion-header>\n\n\n\n<ion-content padding class="animated fadeIn common-bg">\n\n    <ion-fab top left>\n\n        <button ion-fab mini (click)="showMenuFilter()">\n\n            <ion-icon name="options"></ion-icon>\n\n        </button>\n\n    </ion-fab>\n\n\n\n    <ion-list *ngIf="filter">\n\n        <ion-item>\n\n            <span ion-datepicker [modalOptions]="" (ionChanged)="setDateDebut($event);" [(value)]="localDateDebut" clear class="ScheduleDate"\n\n                float-right>\n\n                <span> {{localDateDebut | date : \'dd-MM-y\'}}\n\n                    <ion-icon name="clipboard" item-left></ion-icon>\n\n                </span>\n\n            </span>\n\n        </ion-item>\n\n        <ion-item>\n\n            <span ion-datepicker [modalOptions]="" (ionChanged)="setDateFin($event);" [(value)]="localDateFin" clear class="ScheduleDate"\n\n                float-right>\n\n                <span> {{localDateFin | date : \'dd-MM-y\'}}\n\n                    <ion-icon name="clipboard" item-left></ion-icon>\n\n                </span>\n\n            </span>\n\n        </ion-item>\n\n        <ion-item>\n\n            <ion-label>Caisse</ion-label>\n\n            <ion-select name="codeCompte" [(ngModel)]="codeCompte" required>\n\n                <ion-option value="all">\n\n                    all\n\n                </ion-option>\n\n                <ion-option *ngFor="let caisse of caissesList" [value]="caisse.CODE_COMPTE">\n\n                    {{ caisse.DESIGN_COMPTE }}\n\n                </ion-option>\n\n            </ion-select>\n\n        </ion-item>\n\n        <ion-item>\n\n            <ion-label>Motif</ion-label>\n\n            <ion-select name="codeMotif" [(ngModel)]="codeMotif" required>\n\n                <ion-option value="all">\n\n                    all\n\n                </ion-option>\n\n                <ion-option *ngFor="let motif of motifsList" [value]="motif.CODE_MOTIF ">\n\n                    {{ motif.DESIGN_MOTIF }}\n\n                </ion-option>\n\n            </ion-select>\n\n        </ion-item>\n\n        <ion-item>\n\n            <ion-label>Tiers</ion-label>\n\n            <select-searchable item-content name="tiers" [(ngModel)]="tiers" [items]="listTiers" itemValueField="CODE_TIERS" itemTextField="NOM_TIERS1"\n\n                [canSearch]="true" (onChange)="tiersChange($event)">\n\n            </select-searchable>\n\n        </ion-item>\n\n        <ion-item>\n\n            <button ion-button full (click)="filterEncaiss()">\n\n                <ion-icon name="search"></ion-icon>\n\n            </button>\n\n        </ion-item>\n\n    </ion-list>\n\n    <div *ngIf="isLoading" align-center>\n\n        <ion-spinner name="crescent">\n\n        </ion-spinner>\n\n    </div> \n\n    <ion-list [virtualScroll]="encaissementList">\n\n        <ion-card no-margin margin-bottom class="full-width" [ngClass]="(encaissement.CODE_TYPE  ==\'ENC\')?\'border-encaiss\':\'border-decaiss\'"\n\n            padding *virtualItem="let encaissement">\n\n            <ion-grid class="filters" no-padding>\n\n                <ion-row>\n\n                    <ion-col width-60>\n\n                        <span ion-text color="black">\n\n                            <strong class="text-sm">{{ encaissement.DESIGN_COMPTE }} </strong>\n\n                        </span>\n\n                    </ion-col>\n\n                    <ion-col width-40 text-right>\n\n                        <span ion-text color="primary">\n\n                            <strong class="text-sm">{{encaissement.TOTAL_ENCAISS | xpertCurrency}} </strong>\n\n                        </span>\n\n                    </ion-col>\n\n                    <ion-col width-10>\n\n                        <ion-icon float-right name="more" class="text-2x" Tappable (click)="showMenu($event,encaissement)">\n\n                        </ion-icon>\n\n                    </ion-col>\n\n                </ion-row>\n\n                <ion-row margin-top>\n\n                    <ion-col width-60>\n\n                        <span ion-text class="text-1x" color="black">\n\n                            <strong class="text-sm">{{encaissement.DESIGN_MOTIF}} </strong>\n\n                        </span>\n\n                    </ion-col>\n\n                    <ion-col width-40 text-right>\n\n                        <span ion-text color="primary">\n\n                            <strong class="text-sm">{{encaissement.DATE_ENCAISS | date : "dd-MM-y" }}</strong>\n\n                        </span>\n\n                    </ion-col>\n\n                    <ion-col width-10 text-right>\n\n\n\n                    </ion-col>\n\n                </ion-row>\n\n                <ion-row margin-top>\n\n                    <ion-col width-70>\n\n                        <span ion-text class="text-1x" color="black">\n\n                            <strong class="text-sm">{{encaissement.NOM_TIERS}} </strong>\n\n                        </span>\n\n                    </ion-col>\n\n                </ion-row>\n\n            </ion-grid>\n\n        </ion-card>\n\n    </ion-list>\n\n    <ion-infinite-scroll (ionInfinite)="doInfinite($event)">\n\n        <ion-infinite-scroll-content loadingSpinner="bubbles" loadingText="Loading more data..."></ion-infinite-scroll-content>\n\n    </ion-infinite-scroll>\n\n    <ion-fab bottom right>\n\n        <button ion-fab>\n\n            <ion-icon name="add"></ion-icon>\n\n        </button>\n\n        <ion-fab-list side="top">\n\n            <button ion-fab color="green" (click)="addEncaissementPage(\'ENC\')">\n\n                <ion-icon color="white" name="add"></ion-icon>\n\n            </button>\n\n            <button color="danger" ion-fab (click)="addEncaissementPage(\'DEC\')">\n\n                <ion-icon name="remove"></ion-icon>\n\n            </button>\n\n\n\n        </ion-fab-list>\n\n    </ion-fab>\n\n\n\n\n\n\n\n</ion-content>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\encaissements\encaissements.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavParams"],
        __WEBPACK_IMPORTED_MODULE_5__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["ToastController"]])
], EncaissementsPage);

//# sourceMappingURL=encaissements.js.map

/***/ }),

/***/ 35:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HelperServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};



var HelperServiceProvider = (function () {
    function HelperServiceProvider(storage) {
        this.storage = storage;
        this.NETWORK_ADDRESS_KEY = 'network_address';
        this.DATE_STATISTIC_KEY = 'Date_Statistic';
        this.datesStatistic = new Array();
        this.getDatesStatistic();
    }
    HelperServiceProvider.prototype.getNetworkAdress = function () {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.storage.get(this.NETWORK_ADDRESS_KEY).then(function (networkAddresse) {
                            _this.networkAddress = networkAddresse;
                            if (_this.networkAddress == null) {
                                _this.saveNetworkAddress("http://localhost/");
                            }
                        }, function (error) {
                            console.log(error);
                        })];
                    case 1:
                        _a.sent();
                        return [2 /*return*/, this.networkAddress];
                }
            });
        });
    };
    HelperServiceProvider.prototype.getDatesStatistic = function () {
        var _this = this;
        this.storage.get(this.DATE_STATISTIC_KEY).then(function (dates) {
            if (dates != null) {
                _this.datesStatistic = dates;
            }
        }, function (error) {
            console.log(error);
        });
    };
    HelperServiceProvider.prototype.deleteDateStat = function (dateDebut, dateFin) {
        return __awaiter(this, void 0, void 0, function () {
            var position;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        position = this.datesStatistic.findIndex(function (e) {
                            return (e.dateDebut == dateDebut && e.dateFin == dateFin);
                        });
                        this.datesStatistic.splice(position, 1);
                        return [4 /*yield*/, this.storage.set(this.DATE_STATISTIC_KEY, this.datesStatistic)];
                    case 1:
                        _a.sent();
                        this.getDatesStatistic();
                        return [2 /*return*/];
                }
            });
        });
    };
    HelperServiceProvider.prototype.saveNetworkAddress = function (networkAddress) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.storage.set(this.NETWORK_ADDRESS_KEY, networkAddress)];
                    case 1:
                        _a.sent();
                        this.getNetworkAdress();
                        return [2 /*return*/];
                }
            });
        });
    };
    HelperServiceProvider.prototype.saveDateStatistic = function (dateDebut, dateFin) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (this.datesStatistic == null)
                            console.log("--------------- null -----------");
                        this.datesStatistic.push({
                            dateDebut: dateDebut,
                            dateFin: dateFin
                        });
                        console.log("datastartistic", this.datesStatistic);
                        return [4 /*yield*/, this.storage.set(this.DATE_STATISTIC_KEY, this.datesStatistic)];
                    case 1:
                        _a.sent();
                        this.getDatesStatistic();
                        return [2 /*return*/];
                }
            });
        });
    };
    HelperServiceProvider.prototype.catchError = function (error) {
        console.log(error);
        return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].throw(error.json().error || "Server error");
    };
    HelperServiceProvider.prototype.logResponse = function (res) {
        console.log(res);
    };
    HelperServiceProvider.prototype.extractData = function (res) {
        return res.json();
    };
    return HelperServiceProvider;
}());
HelperServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__ionic_storage__["b" /* Storage */]])
], HelperServiceProvider);

//# sourceMappingURL=helper-service.js.map

/***/ }),

/***/ 413:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return XpertChartComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

/**
 * Generated class for the XpertChartComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
var XpertChartComponent = (function () {
    function XpertChartComponent() {
        this.barChartOptions = {
            scaleShowVerticalLines: false,
            responsive: true
        };
        this.barChartLabels = ["admin", "sysUser"];
        this.barChartType = 'bar';
        this.barChartLegend = true;
        this.barChartData = [{ data: [0, 0], label: '' },
            { data: [0, 0], label: '' }];
        console.log('Hello XpertChartComponent Component');
    }
    return XpertChartComponent;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", String)
], XpertChartComponent.prototype, "title", void 0);
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Array)
], XpertChartComponent.prototype, "barChartLabels", void 0);
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Array)
], XpertChartComponent.prototype, "barChartData", void 0);
XpertChartComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'xpert-chart',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\components\xpert-chart\xpert-chart.html"*/'<!-- Generated template for the XpertChartComponent component -->\n<div style="display: block">\n <canvas baseChart   [datasets]="barChartData" [labels]="barChartLabels" [options]="barChartOptions" [legend]="barChartLegend"\n    [chartType]="barChartType" ></canvas>\n  <h1 color="black" text-center class="text-1x">\n    <strong>{{ title }}</strong>\n  </h1>\n</div>\n'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\components\xpert-chart\xpert-chart.html"*/
    }),
    __metadata("design:paramtypes", [])
], XpertChartComponent);

//# sourceMappingURL=xpert-chart.js.map

/***/ }),

/***/ 414:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser_dynamic__ = __webpack_require__(415);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_module__ = __webpack_require__(419);



// this is the magic wand
Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["enableProdMode"])();
Object(__WEBPACK_IMPORTED_MODULE_0__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 419:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__ = __webpack_require__(73);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_dashboard_service_dashboard_service__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__ = __webpack_require__(152);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__ = __webpack_require__(52);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__ = __webpack_require__(732);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__components_menu_sommaire_menu_sommaire__ = __webpack_require__(278);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__pages_form_encaissement_form_encaissement__ = __webpack_require__(65);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__components_menu_encaissement_list_menu_encaissement_list__ = __webpack_require__(279);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__pages_encaissements_encaissement_encaissement__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__pages_encaissements_encaissements__ = __webpack_require__(280);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__angular_platform_browser__ = __webpack_require__(47);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__angular_common_http__ = __webpack_require__(734);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15_ng2_charts__ = __webpack_require__(735);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15_ng2_charts___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_15_ng2_charts__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__ionic_native_status_bar__ = __webpack_require__(410);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__ionic_native_splash_screen__ = __webpack_require__(411);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__ionic_native_keyboard__ = __webpack_require__(412);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__app_component__ = __webpack_require__(784);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__pages_settings_settings__ = __webpack_require__(785);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__pages_home_home__ = __webpack_require__(150);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_22__pages_login_login__ = __webpack_require__(151);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23__providers_helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_24__providers_auth_service_auth_service__ = __webpack_require__(74);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_25__angular_http__ = __webpack_require__(100);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_26_ionic_select_searchable__ = __webpack_require__(786);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_26_ionic_select_searchable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_26_ionic_select_searchable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_27_ionic3_datepicker__ = __webpack_require__(102);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_28__ionic_native_barcode_scanner__ = __webpack_require__(234);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_29__pages_inventaire_inventaire__ = __webpack_require__(153);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_30__Module_httpFactory__ = __webpack_require__(787);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_31__components_xpert_chart_xpert_chart__ = __webpack_require__(413);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
































// end import services
// end import services
// import pages
// end import pages
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_10__angular_core__["NgModule"])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_19__app_component__["a" /* MyApp */],
            __WEBPACK_IMPORTED_MODULE_20__pages_settings_settings__["a" /* SettingsPage */],
            __WEBPACK_IMPORTED_MODULE_21__pages_home_home__["a" /* HomePage */],
            __WEBPACK_IMPORTED_MODULE_22__pages_login_login__["a" /* LoginPage */],
            __WEBPACK_IMPORTED_MODULE_9__pages_encaissements_encaissements__["a" /* EncaissementsPage */],
            __WEBPACK_IMPORTED_MODULE_8__pages_encaissements_encaissement_encaissement__["a" /* EncaissementPage */],
            __WEBPACK_IMPORTED_MODULE_7__components_menu_encaissement_list_menu_encaissement_list__["a" /* MenuEncaissementListComponent */],
            __WEBPACK_IMPORTED_MODULE_6__pages_form_encaissement_form_encaissement__["a" /* FormEncaissementPage */],
            __WEBPACK_IMPORTED_MODULE_5__components_menu_sommaire_menu_sommaire__["a" /* MenuSommaireComponent */],
            __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
            __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__["a" /* DashboardPage */],
            __WEBPACK_IMPORTED_MODULE_29__pages_inventaire_inventaire__["a" /* InventairePage */],
            __WEBPACK_IMPORTED_MODULE_31__components_xpert_chart_xpert_chart__["a" /* XpertChartComponent */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_12__angular_platform_browser__["a" /* BrowserModule */],
            __WEBPACK_IMPORTED_MODULE_13__angular_common_http__["a" /* HttpClientModule */],
            __WEBPACK_IMPORTED_MODULE_26_ionic_select_searchable__["SelectSearchableModule"],
            __WEBPACK_IMPORTED_MODULE_11_ionic_angular__["IonicModule"].forRoot(__WEBPACK_IMPORTED_MODULE_19__app_component__["a" /* MyApp */], {
                scrollPadding: false,
                scrollAssist: true,
                autoFocusAssist: false
            }, {
                links: [
                    { loadChildren: '../pages/dashboard/dashboard.module#DashboardPageModule', name: 'DashboardPage', segment: 'dashboard', priority: 'low', defaultHistory: [] },
                    { loadChildren: '../pages/inventaire/inventaire.module#InventairePageModule', name: 'InventairePage', segment: 'inventaire', priority: 'low', defaultHistory: [] }
                ]
            }),
            __WEBPACK_IMPORTED_MODULE_14__ionic_storage__["a" /* IonicStorageModule */].forRoot({
                name: '__mydb',
                driverOrder: ['indexeddb', 'sqlite', 'websql']
            }),
            __WEBPACK_IMPORTED_MODULE_25__angular_http__["d" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_15_ng2_charts__["ChartsModule"],
            __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
            __WEBPACK_IMPORTED_MODULE_27_ionic3_datepicker__["b" /* DatePickerModule */],
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_11_ionic_angular__["IonicApp"]],
        entryComponents: [
            __WEBPACK_IMPORTED_MODULE_19__app_component__["a" /* MyApp */],
            __WEBPACK_IMPORTED_MODULE_20__pages_settings_settings__["a" /* SettingsPage */],
            __WEBPACK_IMPORTED_MODULE_21__pages_home_home__["a" /* HomePage */],
            __WEBPACK_IMPORTED_MODULE_22__pages_login_login__["a" /* LoginPage */],
            __WEBPACK_IMPORTED_MODULE_9__pages_encaissements_encaissements__["a" /* EncaissementsPage */],
            __WEBPACK_IMPORTED_MODULE_8__pages_encaissements_encaissement_encaissement__["a" /* EncaissementPage */],
            __WEBPACK_IMPORTED_MODULE_7__components_menu_encaissement_list_menu_encaissement_list__["a" /* MenuEncaissementListComponent */],
            __WEBPACK_IMPORTED_MODULE_6__pages_form_encaissement_form_encaissement__["a" /* FormEncaissementPage */],
            __WEBPACK_IMPORTED_MODULE_5__components_menu_sommaire_menu_sommaire__["a" /* MenuSommaireComponent */],
            __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__["a" /* DashboardPage */],
            __WEBPACK_IMPORTED_MODULE_29__pages_inventaire_inventaire__["a" /* InventairePage */]
        ],
        providers: [
            {
                provide: __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__["a" /* HttpInterceptor */],
                useFactory: __WEBPACK_IMPORTED_MODULE_30__Module_httpFactory__["a" /* HttpFactory */],
                deps: [__WEBPACK_IMPORTED_MODULE_25__angular_http__["f" /* XHRBackend */], __WEBPACK_IMPORTED_MODULE_25__angular_http__["e" /* RequestOptions */], __WEBPACK_IMPORTED_MODULE_24__providers_auth_service_auth_service__["a" /* AuthServiceProvider */]]
            },
            __WEBPACK_IMPORTED_MODULE_28__ionic_native_barcode_scanner__["a" /* BarcodeScanner */],
            __WEBPACK_IMPORTED_MODULE_16__ionic_native_status_bar__["a" /* StatusBar */],
            __WEBPACK_IMPORTED_MODULE_17__ionic_native_splash_screen__["a" /* SplashScreen */],
            __WEBPACK_IMPORTED_MODULE_18__ionic_native_keyboard__["a" /* Keyboard */],
            __WEBPACK_IMPORTED_MODULE_23__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_24__providers_auth_service_auth_service__["a" /* AuthServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
            __WEBPACK_IMPORTED_MODULE_1__providers_dashboard_service_dashboard_service__["a" /* DashboardServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_23__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        ]
    })
], AppModule);

//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 52:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EncaisseServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__ = __webpack_require__(73);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__auth_service_auth_service__ = __webpack_require__(74);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(1);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var EncaisseServiceProvider = (function () {
    function EncaisseServiceProvider(http, helperService, authService) {
        this.http = http;
        this.helperService = helperService;
        this.authService = authService;
        this.BASE_URL = "XpertPharm.Rest.Api/api/";
        this.ENCAISSEMENT_URL = "Encaissements/";
        this.ENCAISSEMENT_Per_Page_URL = "EncaissementsPerPage/";
        this.ADD_ENCAISSEMENT_URL = "addEncaissement";
        this.DELETE_ENCAISSEMENT_URL = "deleteEncaissement";
        this.MOTIFS_URL = "motifs/";
        this.CAISSES_URL = "caisses";
        this.COMPTES_URL = "comptes";
        this.TIERS_URL = "tiers";
        this.STATISTIC_URL = "statistic/";
        this.motifsList = [];
        this.tiersList = [];
        this.caissesList = [];
        this.loadCaisses();
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
    }
    EncaisseServiceProvider.prototype.getStatisticEncaiss = function (date_start, date_end) {
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.STATISTIC_URL + date_start + "/" + date_end)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getComptes = function () {
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.COMPTES_URL)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getEncaissements = function (type) {
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + type)
            .map(this.helperService.extractData)
            .do(this.helperService.logResponse)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getEncaissementsPerPage = function (type, page, dateDebut, dateFin, codeTiers, codeMotif, codeCompte) {
        var url;
        if (dateDebut == null && dateFin == null) {
            url = this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_Per_Page_URL + type + "/" + page + "/";
            console.log(url);
        }
        else {
            url = this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_Per_Page_URL + type + "/" + page + "/" + dateDebut + "/" + dateFin + "/" + codeTiers + "/" + codeMotif + "/" + codeCompte;
        }
        console.log("At encaiss service", this.authService.getToken());
        return this.http.get(url)
            .map(this.helperService.extractData)
            .do(this.helperService.logResponse)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getMotifs = function (type) {
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.MOTIFS_URL + type)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getTiers = function () {
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.TIERS_URL)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getCaisses = function () {
        return this.http.get(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.CAISSES_URL)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.addEncaissement = function (dateEncaiss, codeCompte, totalEncaiss, codeType, codeMotif, codeTiers) {
        var body = {
            "DATE_ENCAISS": dateEncaiss,
            "CODE_COMPTE": codeCompte,
            "TOTAL_ENCAISS": totalEncaiss,
            "CODE_TYPE": codeType,
            "CODE_MOTIF": codeMotif,
            "CODE_TIERS": codeTiers
        };
        return this.http.post(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.ADD_ENCAISSEMENT_URL, body)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.updateEncaissement = function (codeEncaiss, dateEncaiss, codeCompte, totalEncaiss, codeType, codeMotif, codeTiers) {
        var body = {
            "CODE_ENCAISS": codeEncaiss,
            "DATE_ENCAISS": dateEncaiss,
            "CODE_COMPTE": codeCompte,
            "TOTAL_ENCAISS": totalEncaiss,
            "CODE_TYPE": codeType,
            "CODE_MOTIF": codeMotif,
            "CODE_TIERS": codeTiers
        };
        return this.http.put(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL, body)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.deleteEncaissement = function (codeEncaiss) {
        console.log("in the method delete");
        var body = {
            "CODE_ENCAISS": codeEncaiss
        };
        return this.http.post(this.helperService.networkAddress + this.BASE_URL + this.ENCAISSEMENT_URL + this.DELETE_ENCAISSEMENT_URL, body)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.loadMotifs = function () {
        var _this = this;
        this.getMotifs("ENC").subscribe(function (data) {
            _this.motifsList = data;
            _this.loadTiers();
        }, function (error) {
            console.log(error);
            _this.loadMotifs();
        });
    };
    EncaisseServiceProvider.prototype.loadTiers = function () {
        var _this = this;
        this.getTiers().subscribe(function (data) {
            _this.tiersList = data;
        }, function (error) {
            console.log(error);
        });
    };
    EncaisseServiceProvider.prototype.loadCaisses = function () {
        var _this = this;
        this.getCaisses().subscribe(function (data) {
            _this.caissesList = data;
            _this.loadMotifs();
        }, function (error) {
            console.log(error);
        });
    };
    return EncaisseServiceProvider;
}());
EncaisseServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__["a" /* HttpInterceptor */],
        __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_1__auth_service_auth_service__["a" /* AuthServiceProvider */]])
], EncaisseServiceProvider);

//# sourceMappingURL=encaiss-service.js.map

/***/ }),

/***/ 65:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FormEncaissementPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_encaiss_service_encaiss_service__ = __webpack_require__(52);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(14);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



/**
 * Generated class for the FormEncaissementPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */
var FormEncaissementPage = (function () {
    function FormEncaissementPage(navCtrl, navParams, encaisseService, toastCtrl) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.encaisseService = encaisseService;
        this.toastCtrl = toastCtrl;
        this.update = false;
        this.method = "Ajouter";
        this.motifsList = [];
        this.caissesList = [];
        this.dateEncaiss = new Date().toISOString();
        this.hours = new Date().getHours();
        this.localDate = new Date();
        this.encaissementsPage = this.navParams.get('encaissementsPage');
        console.log("modification page : ", this.encaissementsPage.page);
    }
    FormEncaissementPage.prototype.ionViewDidLoad = function () {
    };
    FormEncaissementPage.prototype.ionViewWillEnter = function () {
    };
    FormEncaissementPage.prototype.setDate = function (date) {
        this.localDate = date;
        this.dateEncaiss = this.localDate.toISOString();
        console.log(this.localDate.setHours(this.hours));
        this.dateEncaiss = this.localDate.toISOString();
    };
    FormEncaissementPage.prototype.getMotifs = function () {
        this.motifsList = this.encaisseService.motifsList;
    };
    FormEncaissementPage.prototype.getTiers = function () {
        this.listTiers = this.encaisseService.tiersList;
    };
    FormEncaissementPage.prototype.getCaisses = function () {
        this.caissesList = this.encaisseService.caissesList;
    };
    FormEncaissementPage.prototype.goBack = function () {
        this.navCtrl.pop();
        console.log("go back");
    };
    FormEncaissementPage.prototype.updateEncaissement = function () {
        var _this = this;
        this.encaisseService.updateEncaissement(this.encaissement.CODE_ENCAISS, this.dateEncaiss, this.codeCompte, this.totalEncaiss, this.codeType, this.codeMotif, this.tiers.CODE_TIERS).subscribe(function (data) {
            _this.encaissementsPage.getEncaissementSPerPage();
            _this.navCtrl.pop();
        }, function (error) {
            var toast = _this.toastCtrl.create({
                message: 'Erreur ' + error,
                duration: 3000,
                position: 'bottom',
                cssClass: 'dark-trans',
                closeButtonText: 'OK',
                showCloseButton: true
            });
            toast.present();
        });
    };
    FormEncaissementPage.prototype.addEncaissement = function () {
        var _this = this;
        this.encaisseService.addEncaissement(this.dateEncaiss, this.codeCompte, this.totalEncaiss, this.codeType, this.codeMotif, this.tiers.CODE_TIERS).subscribe(function (data) {
            _this.encaissementsPage.getEncaissementSPerPage();
            _this.navCtrl.pop();
        });
    };
    FormEncaissementPage.prototype.ngOnInit = function () {
        /// test if we are in the update or add Encaissment
        this.update = this.navParams.get('update');
        if (this.update) {
            this.initUpdateForm();
        }
        else {
            this.codeType = this.navParams.get('type');
            //this.initAddForm();
        }
        this.initForm();
    };
    FormEncaissementPage.prototype.initForm = function () {
        this.initTitle();
        this.getMotifs();
        this.getTiers();
        this.getCaisses();
    };
    FormEncaissementPage.prototype.initUpdateForm = function () {
        this.method = 'Modifier';
        this.encaissement = this.navParams.get('encaissement');
        console.log('--------------init update FORM ------------------');
        console.log(this.encaissement);
        console.log('---------------------------------');
        this.codeType = this.encaissement.CODE_TYPE;
        this.dateEncaiss = this.encaissement.DATE_ENCAISS;
        this.totalEncaiss = this.encaissement.TOTAL_ENCAISS;
        this.tiers = {
            CODE_TIERS: this.encaissement.CODE_TIERS,
            NOM_TIERS: this.encaissement.NOM_TIERS
        };
        this.codeCompte = this.encaissement.CODE_COMPTE;
        this.codeMotif = this.encaissement.CODE_MOTIF;
    };
    FormEncaissementPage.prototype.initTitle = function () {
        if (this.codeType == 'ENC') {
            this.title = "Encaissement";
        }
        else {
            this.title = "Decaissement";
        }
    };
    FormEncaissementPage.prototype.validate = function () {
        if (this.update) {
            this.updateEncaissement();
        }
        else {
            this.addEncaissement();
        }
    };
    FormEncaissementPage.prototype.tiersChange = function (event) {
    };
    return FormEncaissementPage;
}());
FormEncaissementPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Component"])({
        selector: 'page-form-encaissement',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\form-encaissement\form-encaissement.html"*/'<!--\n  Generated template for the EncaissementsPage page.\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n  Ionic pages and navigation.\n-->\n<ion-header>\n    <ion-navbar>\n        <ion-title>{{ method }} {{ title }}</ion-title>\n        <ion-buttons left>\n            <button ion-button (click)="goBack()">\n                <ion-icon name="arrow-back"></ion-icon>\n            </button>\n        </ion-buttons>\n    </ion-navbar>\n</ion-header>\n<ion-content padding>\n    <form #f="ngForm" class="list-form" (ngSubmit)="validate()">\n        <ion-list>\n            <!--  <ion-item>\n                <ion-label>Date</ion-label>\n                <ion-datetime displayFormat="DD-MM-YYYY" pickerFormat="DD-MM-YYYY" value="" ng-model [(ngModel)]="dateEncaiss" name="dateEncaiss"\n                    required></ion-datetime>\n            </ion-item> -->\n            <ion-item>\n                <span ion-datepicker [modalOptions]="" (ionChanged)="setDate($event);" [(value)]="localDate" clear class="ScheduleDate" float-right>\n                    <span> {{localDate | date : \'dd-MM-y\'}}\n                        <ion-icon name="clipboard" item-left></ion-icon>\n                    </span>\n                </span>\n            </ion-item>\n            <ion-item>\n                <ion-label>Motif</ion-label>\n                <ion-select name="codeMotif" [(ngModel)]="codeMotif" required>\n                    <ion-option *ngFor="let motif of motifsList" [value]="motif.CODE_MOTIF ">\n                        {{ motif.DESIGN_MOTIF }}\n                    </ion-option>\n                </ion-select>\n            </ion-item>\n            <ion-item>\n                <ion-label>Caisse</ion-label>\n                <ion-select name="codeCompte" [(ngModel)]="codeCompte" required>\n                    <ion-option *ngFor="let caisse of caissesList" [value]="caisse.CODE_COMPTE">\n                        {{ caisse.DESIGN_COMPTE }}\n                    </ion-option>\n                </ion-select>\n            </ion-item>\n            <ion-item>\n                <ion-label>Tiers</ion-label>\n                <select-searchable item-content name="tiers" [(ngModel)]="tiers" [items]="listTiers" itemValueField="CODE_TIERS" itemTextField="NOM_TIERS"\n                    [canSearch]="true" (onChange)="tiersChange($event)">\n                </select-searchable>\n            </ion-item>\n            <ion-item>\n                <ion-label>Montant : </ion-label>\n                <ion-input name="totalEncaiss" type="number" placeholder="entrez le montant" [(ngModel)]="totalEncaiss" required></ion-input>\n            </ion-item>\n            <button ion-button icon-start block color="dark" type="submit" [disabled]="!f.valid">\n                <ion-icon name="add"></ion-icon>\n                {{ method }}\n            </button>\n        </ion-list>\n    </form>\n</ion-content>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\form-encaissement\form-encaissement.html"*/,
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavParams"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavParams"]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_0__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["ToastController"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["ToastController"]) === "function" && _d || Object])
], FormEncaissementPage);

var _a, _b, _c, _d;
//# sourceMappingURL=form-encaissement.js.map

/***/ }),

/***/ 73:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HttpInterceptor; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_auth_service_auth_service__ = __webpack_require__(74);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(100);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var HttpInterceptor = (function (_super) {
    __extends(HttpInterceptor, _super);
    function HttpInterceptor(backend, defaultOptions, authService) {
        var _this = _super.call(this, backend, defaultOptions) || this;
        _this.authService = authService;
        _this.token = 'token';
        return _this;
    }
    HttpInterceptor.prototype.request = function (url, options) {
        return _super.prototype.request.call(this, url, options);
    };
    /* Performs a request with `get` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    HttpInterceptor.prototype.get = function (url, options) {
        return _super.prototype.get.call(this, url, this.getRequestOptionArgs(options));
    };
    /* Performs a request with `post` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    HttpInterceptor.prototype.post = function (url, body, options) {
        return _super.prototype.post.call(this, url, body, this.getRequestOptionArgs(options));
    };
    /* Performs a request with `put` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    HttpInterceptor.prototype.put = function (url, body, options) {
        return _super.prototype.put.call(this, url, body, this.getRequestOptionArgs(options));
    };
    /* Performs a request with `delete` http method.
    * @param url
    * @param options
    * @returns {Observable<>}
    */
    HttpInterceptor.prototype.delete = function (url, options) {
        return _super.prototype.delete.call(this, url, this.getRequestOptionArgs(options));
    };
    HttpInterceptor.prototype.getRequestOptionArgs = function (options) {
        if (options == null) {
            options = new __WEBPACK_IMPORTED_MODULE_1__angular_http__["e" /* RequestOptions */]();
        }
        if (options.headers == null) {
            options.headers = new __WEBPACK_IMPORTED_MODULE_1__angular_http__["b" /* Headers */]();
        }
        options.headers.append('Content-Type', 'application/json');
        options.headers.append('Authorization', 'Bearer ' + this.authService.token);
        return options;
    };
    // /**
    //  * Before any Request.
    //  */
    // private beforeRequest(): void {
    //   //this.notifyService.showPreloader();
    // }
    //
    /**
     * After any request.
     */
    HttpInterceptor.prototype.afterRequest = function () {
        //this.notifyService.hidePreloader();
        console.log("after request");
    };
    return HttpInterceptor;
}(__WEBPACK_IMPORTED_MODULE_1__angular_http__["c" /* Http */]));
HttpInterceptor = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* ConnectionBackend */], __WEBPACK_IMPORTED_MODULE_1__angular_http__["e" /* RequestOptions */], __WEBPACK_IMPORTED_MODULE_0__providers_auth_service_auth_service__["a" /* AuthServiceProvider */]])
], HttpInterceptor);

//# sourceMappingURL=httpInterceptor.js.map

/***/ }),

/***/ 732:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return XpertCurrencyPipe; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(40);
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};


/**
 * Generated class for the XpertCurrencyPipe pipe.
 *
 * See https://angular.io/api/core/Pipe for more info on Angular Pipes.
 */
var XpertCurrencyPipe = (function (_super) {
    __extends(XpertCurrencyPipe, _super);
    function XpertCurrencyPipe() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * Mise en forme du chiffre =>  "0.00"
     */
    XpertCurrencyPipe.prototype.transform = function (value) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        // si chiffre null renvoyer 0 sionon Mettre en forme
        return (value != null) ? _super.prototype.transform.call(this, value, '1.2-2').replace(/,/g, ' ') : "0.00";
    };
    return XpertCurrencyPipe;
}(__WEBPACK_IMPORTED_MODULE_1__angular_common__["DecimalPipe"]));
XpertCurrencyPipe = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Pipe"])({
        name: 'xpertCurrency',
    }),
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({})
], XpertCurrencyPipe);

//# sourceMappingURL=xpert-currency.js.map

/***/ }),

/***/ 733:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Tiers; });
var Tiers = (function () {
    function Tiers() {
        this.CODE_TIERS = "all";
        this.NOM_TIERS = "all";
    }
    return Tiers;
}());

//# sourceMappingURL=tiers.js.map

/***/ }),

/***/ 74:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs__ = __webpack_require__(423);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__(100);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};





/*
  Generated class for the AuthServiceProvider provider.
  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
var AuthServiceProvider = (function () {
    function AuthServiceProvider(http, helperService, storage) {
        this.http = http;
        this.helperService = helperService;
        this.storage = storage;
        this.BASE_URL = "XpertPharm.Rest.Api/api/";
        this.TOKEN_URL = "token";
        this.GRANT_TYPE = "password";
        this.token = "token";
        this.TOKEN_KEY = 'token';
    }
    AuthServiceProvider.prototype.getAuthentification = function (username, password) {
        this.headers = new __WEBPACK_IMPORTED_MODULE_4__angular_http__["b" /* Headers */]();
        this.headers.append('Content-Type', 'application/json');
        return this.http.post(this.helperService.networkAddress + this.BASE_URL + this.TOKEN_URL, 'username=' + username + '&password=' + password + '&grant_type=' + this.GRANT_TYPE).do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(function (error) {
            console.log(error);
            return __WEBPACK_IMPORTED_MODULE_1_rxjs__["Observable"].throw(error.json().error || "Server error");
        });
    };
    AuthServiceProvider.prototype.setToken = function (token) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this.token = token;
                        this.headers = new __WEBPACK_IMPORTED_MODULE_4__angular_http__["b" /* Headers */]();
                        this.headers.append('Content-Type', 'application/json');
                        this.headers.append('Authorization', 'bearer ' + this.token);
                        this.authorisationOptions = new __WEBPACK_IMPORTED_MODULE_4__angular_http__["e" /* RequestOptions */]({ headers: this.headers });
                        console.log("---------------------------------");
                        console.log('Token set ', this.getToken());
                        console.log("---------------------------------");
                        return [4 /*yield*/, this.storage.set(this.TOKEN_KEY, this.token)];
                    case 1:
                        _a.sent();
                        this.storage.get(this.TOKEN_KEY).then(function (val) {
                            console.log("----------------------------");
                            console.log("the value from Auth service : " + val);
                            console.log("----------------------------");
                        });
                        return [2 /*return*/];
                }
            });
        });
    };
    AuthServiceProvider.prototype.getToken = function () {
        console.log("log in getToken");
        return this.token;
    };
    AuthServiceProvider.prototype.getAuthorisationToken = function () {
        return this.authorisationOptions;
    };
    return AuthServiceProvider;
}());
AuthServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_4__angular_http__["c" /* Http */],
        __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_0__ionic_storage__["b" /* Storage */]])
], AuthServiceProvider);

//# sourceMappingURL=auth-service.js.map

/***/ }),

/***/ 765:
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"./af": 287,
	"./af.js": 287,
	"./ar": 288,
	"./ar-dz": 289,
	"./ar-dz.js": 289,
	"./ar-kw": 290,
	"./ar-kw.js": 290,
	"./ar-ly": 291,
	"./ar-ly.js": 291,
	"./ar-ma": 292,
	"./ar-ma.js": 292,
	"./ar-sa": 293,
	"./ar-sa.js": 293,
	"./ar-tn": 294,
	"./ar-tn.js": 294,
	"./ar.js": 288,
	"./az": 295,
	"./az.js": 295,
	"./be": 296,
	"./be.js": 296,
	"./bg": 297,
	"./bg.js": 297,
	"./bm": 298,
	"./bm.js": 298,
	"./bn": 299,
	"./bn.js": 299,
	"./bo": 300,
	"./bo.js": 300,
	"./br": 301,
	"./br.js": 301,
	"./bs": 302,
	"./bs.js": 302,
	"./ca": 303,
	"./ca.js": 303,
	"./cs": 304,
	"./cs.js": 304,
	"./cv": 305,
	"./cv.js": 305,
	"./cy": 306,
	"./cy.js": 306,
	"./da": 307,
	"./da.js": 307,
	"./de": 308,
	"./de-at": 309,
	"./de-at.js": 309,
	"./de-ch": 310,
	"./de-ch.js": 310,
	"./de.js": 308,
	"./dv": 311,
	"./dv.js": 311,
	"./el": 312,
	"./el.js": 312,
	"./en-au": 313,
	"./en-au.js": 313,
	"./en-ca": 314,
	"./en-ca.js": 314,
	"./en-gb": 315,
	"./en-gb.js": 315,
	"./en-ie": 316,
	"./en-ie.js": 316,
	"./en-il": 317,
	"./en-il.js": 317,
	"./en-nz": 318,
	"./en-nz.js": 318,
	"./eo": 319,
	"./eo.js": 319,
	"./es": 320,
	"./es-do": 321,
	"./es-do.js": 321,
	"./es-us": 322,
	"./es-us.js": 322,
	"./es.js": 320,
	"./et": 323,
	"./et.js": 323,
	"./eu": 324,
	"./eu.js": 324,
	"./fa": 325,
	"./fa.js": 325,
	"./fi": 326,
	"./fi.js": 326,
	"./fo": 327,
	"./fo.js": 327,
	"./fr": 328,
	"./fr-ca": 329,
	"./fr-ca.js": 329,
	"./fr-ch": 330,
	"./fr-ch.js": 330,
	"./fr.js": 328,
	"./fy": 331,
	"./fy.js": 331,
	"./gd": 332,
	"./gd.js": 332,
	"./gl": 333,
	"./gl.js": 333,
	"./gom-latn": 334,
	"./gom-latn.js": 334,
	"./gu": 335,
	"./gu.js": 335,
	"./he": 336,
	"./he.js": 336,
	"./hi": 337,
	"./hi.js": 337,
	"./hr": 338,
	"./hr.js": 338,
	"./hu": 339,
	"./hu.js": 339,
	"./hy-am": 340,
	"./hy-am.js": 340,
	"./id": 341,
	"./id.js": 341,
	"./is": 342,
	"./is.js": 342,
	"./it": 343,
	"./it.js": 343,
	"./ja": 344,
	"./ja.js": 344,
	"./jv": 345,
	"./jv.js": 345,
	"./ka": 346,
	"./ka.js": 346,
	"./kk": 347,
	"./kk.js": 347,
	"./km": 348,
	"./km.js": 348,
	"./kn": 349,
	"./kn.js": 349,
	"./ko": 350,
	"./ko.js": 350,
	"./ky": 351,
	"./ky.js": 351,
	"./lb": 352,
	"./lb.js": 352,
	"./lo": 353,
	"./lo.js": 353,
	"./lt": 354,
	"./lt.js": 354,
	"./lv": 355,
	"./lv.js": 355,
	"./me": 356,
	"./me.js": 356,
	"./mi": 357,
	"./mi.js": 357,
	"./mk": 358,
	"./mk.js": 358,
	"./ml": 359,
	"./ml.js": 359,
	"./mn": 360,
	"./mn.js": 360,
	"./mr": 361,
	"./mr.js": 361,
	"./ms": 362,
	"./ms-my": 363,
	"./ms-my.js": 363,
	"./ms.js": 362,
	"./mt": 364,
	"./mt.js": 364,
	"./my": 365,
	"./my.js": 365,
	"./nb": 366,
	"./nb.js": 366,
	"./ne": 367,
	"./ne.js": 367,
	"./nl": 368,
	"./nl-be": 369,
	"./nl-be.js": 369,
	"./nl.js": 368,
	"./nn": 370,
	"./nn.js": 370,
	"./pa-in": 371,
	"./pa-in.js": 371,
	"./pl": 372,
	"./pl.js": 372,
	"./pt": 373,
	"./pt-br": 374,
	"./pt-br.js": 374,
	"./pt.js": 373,
	"./ro": 375,
	"./ro.js": 375,
	"./ru": 376,
	"./ru.js": 376,
	"./sd": 377,
	"./sd.js": 377,
	"./se": 378,
	"./se.js": 378,
	"./si": 379,
	"./si.js": 379,
	"./sk": 380,
	"./sk.js": 380,
	"./sl": 381,
	"./sl.js": 381,
	"./sq": 382,
	"./sq.js": 382,
	"./sr": 383,
	"./sr-cyrl": 384,
	"./sr-cyrl.js": 384,
	"./sr.js": 383,
	"./ss": 385,
	"./ss.js": 385,
	"./sv": 386,
	"./sv.js": 386,
	"./sw": 387,
	"./sw.js": 387,
	"./ta": 388,
	"./ta.js": 388,
	"./te": 389,
	"./te.js": 389,
	"./tet": 390,
	"./tet.js": 390,
	"./tg": 391,
	"./tg.js": 391,
	"./th": 392,
	"./th.js": 392,
	"./tl-ph": 393,
	"./tl-ph.js": 393,
	"./tlh": 394,
	"./tlh.js": 394,
	"./tr": 395,
	"./tr.js": 395,
	"./tzl": 396,
	"./tzl.js": 396,
	"./tzm": 397,
	"./tzm-latn": 398,
	"./tzm-latn.js": 398,
	"./tzm.js": 397,
	"./ug-cn": 399,
	"./ug-cn.js": 399,
	"./uk": 400,
	"./uk.js": 400,
	"./ur": 401,
	"./ur.js": 401,
	"./uz": 402,
	"./uz-latn": 403,
	"./uz-latn.js": 403,
	"./uz.js": 402,
	"./vi": 404,
	"./vi.js": 404,
	"./x-pseudo": 405,
	"./x-pseudo.js": 405,
	"./yo": 406,
	"./yo.js": 406,
	"./zh-cn": 407,
	"./zh-cn.js": 407,
	"./zh-hk": 408,
	"./zh-hk.js": 408,
	"./zh-tw": 409,
	"./zh-tw.js": 409
};
function webpackContext(req) {
	return __webpack_require__(webpackContextResolve(req));
};
function webpackContextResolve(req) {
	var id = map[req];
	if(!(id + 1)) // check for number or string
		throw new Error("Cannot find module '" + req + "'.");
	return id;
};
webpackContext.keys = function webpackContextKeys() {
	return Object.keys(map);
};
webpackContext.resolve = webpackContextResolve;
module.exports = webpackContext;
webpackContext.id = 765;

/***/ }),

/***/ 784:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyApp; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__pages_inventaire_inventaire__ = __webpack_require__(153);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_dashboard_service_dashboard_service__ = __webpack_require__(101);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__ = __webpack_require__(152);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_helper_service_helper_service__ = __webpack_require__(35);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__pages_encaissements_encaissements__ = __webpack_require__(280);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__ionic_native_status_bar__ = __webpack_require__(410);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__ionic_native_splash_screen__ = __webpack_require__(411);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__ionic_native_keyboard__ = __webpack_require__(412);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__pages_home_home__ = __webpack_require__(150);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__pages_login_login__ = __webpack_require__(151);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};












var MyApp = (function () {
    function MyApp(platform, statusBar, splashScreen, keyboard, helperService, dashBoardService) {
        this.platform = platform;
        this.statusBar = statusBar;
        this.splashScreen = splashScreen;
        this.keyboard = keyboard;
        this.helperService = helperService;
        this.dashBoardService = dashBoardService;
        this.rootPage = __WEBPACK_IMPORTED_MODULE_11__pages_login_login__["a" /* LoginPage */];
        this.initializeApp();
        this.appMenuItems = [
            { title: 'Home', component: __WEBPACK_IMPORTED_MODULE_10__pages_home_home__["a" /* HomePage */], icon: 'home' },
            { title: 'Encaissements', component: __WEBPACK_IMPORTED_MODULE_4__pages_encaissements_encaissements__["a" /* EncaissementsPage */], icon: 'logo-usd' },
            { title: 'Dashboard', component: __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__["a" /* DashboardPage */], icon: 'stats' },
            { title: 'Inventaire', component: __WEBPACK_IMPORTED_MODULE_0__pages_inventaire_inventaire__["a" /* InventairePage */], icon: 'clipboard' }
        ];
    }
    MyApp.prototype.initializeApp = function () {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.platform.ready().then(function () {
                            // Okay, so the platform is ready and our plugins are available.
                            //*** Control Splash Screen
                            //this.splashScreen.show();
                            // this.splashScreen.hide();
                            _this.helperService.getNetworkAdress().then(function () {
                                _this.dashBoardService.initDashBoard();
                            });
                            //*** Control Status Bar
                            _this.statusBar.styleDefault();
                            _this.statusBar.overlaysWebView(false);
                            //*** Control Keyboard
                            _this.keyboard.disableScroll(true);
                        })];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    MyApp.prototype.openPage = function (page) {
        // Reset the content nav to have just this page
        // we wouldn't want the back button to show in this scenario
        this.nav.setRoot(page.component);
    };
    MyApp.prototype.logout = function () {
        this.nav.setRoot(__WEBPACK_IMPORTED_MODULE_11__pages_login_login__["a" /* LoginPage */]);
    };
    return MyApp;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_5__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_6_ionic_angular__["Nav"]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_6_ionic_angular__["Nav"])
], MyApp.prototype, "nav", void 0);
MyApp = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_5__angular_core__["Component"])({template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\app\app.html"*/'<ion-menu side="left" id="authenticated" [content]="content">\n  <ion-header>\n    <ion-toolbar class="user-profile">\n      <ion-grid>\n        <ion-row>\n          <ion-col padding-top col-8>\n            <h2 ion-text class="no-margin bold text-white">\n              Mohammed Ouassim\n            </h2>\n            <span ion-text color="light">Admin</span>\n          </ion-col>\n        </ion-row>\n        <ion-row no-padding class="other-data">\n          <ion-col no-padding class="column">\n            <button ion-button icon-left small full color="light" menuClose (click)="logout()">\n              <ion-icon name="log-out"></ion-icon>\n              Deconnexion\n            </button>\n          </ion-col>\n        </ion-row>\n      </ion-grid>\n    </ion-toolbar>\n  </ion-header>\n  <ion-content color="primary">\n    <ion-list class="user-list">\n      <button ion-item menuClose class="text-1x" *ngFor="let menuItem of appMenuItems" (click)="openPage(menuItem)">\n        <ion-icon item-left [name]="menuItem.icon" color="primary"></ion-icon>\n        <span ion-text color="primary">{{menuItem.title}}</span>\n      </button>\n    </ion-list>\n  </ion-content>\n</ion-menu>\n<ion-nav [root]="rootPage" #content swipeBackEnabled="false"></ion-nav>'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\app\app.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_6_ionic_angular__["Platform"],
        __WEBPACK_IMPORTED_MODULE_7__ionic_native_status_bar__["a" /* StatusBar */],
        __WEBPACK_IMPORTED_MODULE_8__ionic_native_splash_screen__["a" /* SplashScreen */],
        __WEBPACK_IMPORTED_MODULE_9__ionic_native_keyboard__["a" /* Keyboard */],
        __WEBPACK_IMPORTED_MODULE_3__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_1__providers_dashboard_service_dashboard_service__["a" /* DashboardServiceProvider */]])
], MyApp);

//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 785:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SettingsPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(14);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__login_login__ = __webpack_require__(151);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var SettingsPage = (function () {
    function SettingsPage(nav) {
        this.nav = nav;
    }
    // logout
    SettingsPage.prototype.logout = function () {
        this.nav.setRoot(__WEBPACK_IMPORTED_MODULE_2__login_login__["a" /* LoginPage */]);
    };
    return SettingsPage;
}());
SettingsPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'page-settings',template:/*ion-inline-start:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\settings\settings.html"*/'<!-- -->\n<ion-header class="no-shadow">\n\n  <ion-navbar class="no-border">\n    <ion-title>\n      <ion-icon name="cog" class="text-primary"></ion-icon>\n      <span class="text-primary">Settings</span>\n    </ion-title>\n  </ion-navbar>\n\n</ion-header>\n\n<ion-content class="common-bg">\n  <!-- User settings-->\n  <ion-item-group>\n    <ion-item-divider color="secondary" class="bold">User Settings</ion-item-divider>\n    <ion-item>\n      <ion-label>Language</ion-label>\n      <ion-select  cancelText="Cancel" okText="OK">\n        <ion-option value="en-US" selected="true">English (US)</ion-option>\n        <ion-option value="en-GB">English (UK)</ion-option>\n        <ion-option value="en-CA">English (CA)</ion-option>\n        <ion-option value="en-AU">English (AU)</ion-option>\n        <ion-option value="en-IN">English (IN)</ion-option>\n        <ion-option value="pt-BR">Portuguese (BR)</ion-option>\n        <ion-option value="pt-PT">Portuguese (PT)</ion-option>\n        <ion-option value="es-ES">Spanish (ES)</ion-option>\n        <ion-option value="es-AR">Spanish (AR)</ion-option>\n        <ion-option value="es-CO">Spanish (CO)</ion-option>\n        <ion-option value="es-CL">Spanish (CL)</ion-option>\n        <ion-option value="es-MX">Spanish (MX)</ion-option>\n        <ion-option value="zh-CN">Chinese (CN)</ion-option>\n        <ion-option value="zh-TW">Chinese (TW)</ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label>Currency</ion-label>\n      <ion-select cancelText="Cancel" okText="OK">\n        <ion-option value="USD" selected="true">U.S Dollar (US$)</ion-option>\n        <ion-option value="EUR">Euro (€)</ion-option>\n        <ion-option value="GBP">Pound (£)</ion-option>\n        <ion-option value="BRL">Brazilian Real (R$)</ion-option>\n        <ion-option value="CNY">Chinese Yuan</ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label>Units</ion-label>\n      <ion-select  cancelText="Cancel" okText="OK">\n        <ion-option value="M" selected="true">Miles (ft²)</ion-option>\n        <ion-option value="K">Kilometers (m²)</ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label>Notifications?</ion-label>\n      <ion-toggle checked="true"></ion-toggle>\n    </ion-item>\n  </ion-item-group>\n  <!-- App settings-->\n  <ion-item-group>\n    <ion-item-divider color="secondary" class="bold">App Settings</ion-item-divider>\n    <ion-item>\n      <span>Clear Private Data</span>\n    </ion-item>\n    <ion-item>\n      <ion-label>Push Notifications?</ion-label>\n      <ion-toggle checked="false"></ion-toggle>\n    </ion-item>\n    <ion-item>\n      <span>Privacy Policy</span>\n    </ion-item>\n  </ion-item-group>  \n\n  <!--sign out button-->\n  <button ion-button color="primary" full tappable (click)="logout()">LOG OUT</button>\n\n</ion-content>\n'/*ion-inline-end:"C:\Users\wassim\Desktop\testXpertMobile\XpertMobile\src\pages\settings\settings.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"]])
], SettingsPage);

//# sourceMappingURL=settings.js.map

/***/ }),

/***/ 787:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = HttpFactory;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__httpInterceptor__ = __webpack_require__(73);

function HttpFactory(xhrBackend, requestOptions, authService) {
    return new __WEBPACK_IMPORTED_MODULE_0__httpInterceptor__["a" /* HttpInterceptor */](xhrBackend, requestOptions, authService);
}
//# sourceMappingURL=httpFactory.js.map

/***/ })

},[414]);
//# sourceMappingURL=main.js.map