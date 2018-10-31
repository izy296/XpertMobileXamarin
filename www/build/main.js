webpackJsonp([0],{

/***/ 145:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__ = __webpack_require__(56);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__helper_service_helper_service__ = __webpack_require__(26);
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
        this.BASE_URL = "api";
        this.DASHBOARD_URL = "Dashboard";
        this.MARGE_PAR_VENDEUR_URL = "MargeParVendeur";
    }
    DashboardServiceProvider.prototype.initDashBoard = function () {
    };
    DashboardServiceProvider.prototype.getMargeParVendeur = function (date_start, date_end) {
        console.log("url :", this.helperService.networkAddress + this.BASE_URL + this.DASHBOARD_URL + this.MARGE_PAR_VENDEUR_URL + '/' + date_start.toISOString().substring(0, 10) + '/' + date_end.toISOString().substring(0, 10) + '/');
        var url = this.helperService.createLink(this.BASE_URL, this.DASHBOARD_URL, this.MARGE_PAR_VENDEUR_URL, date_start.toISOString().substring(0, 10), date_end.toISOString().substring(0, 10));
        console.log("createLink dashboar durl : ", url);
        return this.http.get(url)
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

/***/ 146:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EncaissementsPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__models_tiers__ = __webpack_require__(414);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__form_encaissement_form_encaissement__ = __webpack_require__(91);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__components_menu_encaissement_list_menu_encaissement_list__ = __webpack_require__(413);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__models_motif__ = __webpack_require__(415);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__components_menu_filter_menu_filter__ = __webpack_require__(416);
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
    function EncaissementsPage(navCtrl, navParams, encaisseService, popOverCtrl, toastCtrl, helperService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.encaisseService = encaisseService;
        this.popOverCtrl = popOverCtrl;
        this.toastCtrl = toastCtrl;
        this.helperService = helperService;
        this.encaissementList = [];
        this.type = "All";
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
        this.motif = new __WEBPACK_IMPORTED_MODULE_6__models_motif__["a" /* Motif */]();
        this.tiersAll = new __WEBPACK_IMPORTED_MODULE_0__models_tiers__["a" /* Tiers */]();
        this.filter = false;
        this.filterActivated = false;
        this.caissesList = [];
        this.codeCompte = "all";
        this.idCaisse = "all";
    }
    EncaissementsPage.prototype.scrolling = function (event) {
        // si le filtre est activer est que on est dans le haut de la page on affiche le menu filtre
        /*   console.log(event);
          if(this.filterActivated && event.scrollTop<220 && event.directionY =="up"){
              this.showMenuFilter();
          } */
    };
    EncaissementsPage.prototype.scrollComplete = function (event) {
    };
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
    // show filter menu
    EncaissementsPage.prototype.showMenuFilter = function () {
        var _this = this;
        var menuFilter = this.popOverCtrl.create(__WEBPACK_IMPORTED_MODULE_8__components_menu_filter_menu_filter__["a" /* MenuFilterComponent */], {
            dateDebut: this.localDateDebut,
            dateFin: this.localDateFin,
            motif: this.motif,
            tiers: this.tiers,
            codeCompte: this.codeCompte
        });
        menuFilter.present();
        menuFilter.onDidDismiss(function (data) {
            console.log("data from popover : ", data);
            if (data.filter) {
                _this.localDateDebut = data.dateDebut;
                console.log("date debut ", data.dateDebut);
                console.log("date fin ", data.dateFin);
                _this.localDateFin = data.dateFin;
                _this.tiers = data.tiers;
                _this.codeCompte = data.compte;
                _this.motif = data.motif;
                _this.filter = true;
                _this.getEncaissementSPerPage();
            }
            else {
                _this.filter = false;
                _this.getEncaissementSPerPage();
            }
        });
    };
    EncaissementsPage.prototype.ngOnInit = function () {
        if (this.navParams.get('id_caisse'))
            this.idCaisse = this.navParams.get('id_caisse');
    };
    EncaissementsPage.prototype.showMenu = function (event, encaissement) {
        console.log("presss ");
        var menu = this.popOverCtrl.create(__WEBPACK_IMPORTED_MODULE_2__components_menu_encaissement_list_menu_encaissement_list__["a" /* MenuEncaissementListComponent */], { data: encaissement, parent: this });
        menu.present({
            ev: event
        });
    };
    EncaissementsPage.prototype.addEncaissementPage = function (codeType, fab) {
        fab.close();
        this.navCtrl.push(__WEBPACK_IMPORTED_MODULE_1__form_encaissement_form_encaissement__["a" /* FormEncaissementPage */], { type: codeType });
    };
    EncaissementsPage.prototype.getEncaissementSPerPage = function () {
        var _this = this;
        this.isLoading = true;
        this.content.scrollToTop();
        this.page = 1;
        if (!this.filter) {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page, this.idCaisse, null, null, "all", "all", "all")
                .subscribe(function (data) {
                _this.data = data;
                _this.isLoading = false;
                _this.encaissementList = _this.data;
            }, function (error) {
                console.log(error);
                _this.helperService.showNotifError(error);
            });
        }
        else {
            this.encaisseService.getEncaissementsPerPage(this.type, this.page, this.idCaisse, this.localDateDebut.toDateString(), this.localDateFin.toDateString(), this.tiers.CODE_TIERS, this.motif.CODE_MOTIF, this.codeCompte)
                .subscribe(function (data) {
                _this.data = data;
                _this.isLoading = false;
                _this.encaissementList = _this.data;
            }, function (error) {
                console.log(error);
                _this.helperService.showNotifError(error);
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
                        _this.encaisseService.getEncaissementsPerPage(_this.type, _this.page, _this.idCaisse, null, null, "all", "all", "all")
                            .subscribe(function (data) {
                            _this.data = data;
                            for (var i = 0; i < _this.data.length; i++) {
                                _this.encaissementList.push(_this.data[i]);
                            }
                            console.log(_this.encaissementList.length);
                        }, function (error) { return _this.helperService.showNotifError(error); });
                        console.log('Async operation has ended');
                        infiniteScroll.complete();
                    }, 1000);
                }
                else {
                    setTimeout(function () {
                        _this.encaisseService.getEncaissementsPerPage(_this.type, _this.page, _this.idCaisse, _this.localDateDebut.toDateString(), _this.localDateFin.toDateString(), _this.tiers.CODE_TIERS, _this.motif.CODE_MOTIF, _this.codeCompte)
                            .subscribe(function (data) {
                            _this.data = data;
                            for (var i = 0; i < _this.data.length; i++) {
                                _this.encaissementList.push(_this.data[i]);
                            }
                            console.log(_this.encaissementList.length);
                        }, function (error) {
                            _this.helperService.showNotifError(error);
                        });
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
    Object(__WEBPACK_IMPORTED_MODULE_5__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["Content"]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["Content"])
], EncaissementsPage.prototype, "content", void 0);
EncaissementsPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_5__angular_core__["Component"])({
        selector: 'page-encaissements',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\encaissements\encaissements.html"*/'<ion-header>\n\n    <ion-navbar color="primary">\n\n        <button ion-button menuToggle start>\n\n            <ion-icon name="menu"></ion-icon>\n\n        </button>\n\n        <ion-toolbar>\n\n            <ion-segment color="white" [(ngModel)]="type">\n\n                <ion-segment-button color="white" (click)="getEncaissementSPerPage()" value="All">\n\n                    <span>Tous</span>\n\n                </ion-segment-button>\n\n                <ion-segment-button color="green" value="ENC" (click)="getEncaissementSPerPage()">\n\n                    <ion-icon color="white" name="trending-up"></ion-icon>\n\n                </ion-segment-button>\n\n                <ion-segment-button value="DEC" color="danger" (click)="getEncaissementSPerPage()">\n\n                    <ion-icon color="danger" name="trending-down"></ion-icon>\n\n                </ion-segment-button>\n\n            </ion-segment>\n\n        </ion-toolbar>\n\n        <ion-buttons end>            \n\n            <button ion-button tappable (click)="showMenuFilter()">\n\n                <ion-icon name="ios-search"></ion-icon>\n\n            </button>\n\n        </ion-buttons>\n\n    </ion-navbar>\n\n</ion-header>\n\n<ion-content padding class="animated fadeIn common-bg"  (ionScroll)="scrolling($event)" (ionScrollEnd)="scrollComplete($event)">\n\n    <div *ngIf="isLoading" align-center>\n\n        <ion-spinner name="crescent">\n\n        </ion-spinner>\n\n    </div>\n\n    <ion-list [virtualScroll]="encaissementList">\n\n        <ion-grid *virtualItem="let encaissement" class="encaiss" [ngClass]="(encaissement.CODE_TYPE  ==\'ENC\')?\'border-encaiss\':\'border-decaiss\'"\n\n        (press)="showMenu($event,encaissement)">\n\n        <encaiss [encaissement]="encaissement">\n\n        </encaiss>\n\n        </ion-grid>\n\n  </ion-list>\n\n  <!--     <ion-list [virtualScroll]="encaissementList">\n\n        <ion-grid *virtualItem="let encaissement" class="encaiss" [ngClass]="(encaissement.CODE_TYPE  ==\'ENC\')?\'border-encaiss\':\'border-decaiss\'"\n\n            (press)="showMenu($event,encaissement)">\n\n            <ion-row>\n\n                <ion-col>\n\n                    <span ion-text class="text-big" color="black">{{encaissement.DESIGN_MOTIF | titlecase}} </span>\n\n                </ion-col>\n\n                <ion-col float-right>\n\n                    <strong ion-text class="text-big" color="danger" float-right>{{encaissement.TOTAL_ENCAISS |\n\n                        xpertCurrency}}\n\n                    </strong>\n\n                </ion-col>\n\n            </ion-row>\n\n            <ion-row>\n\n                <ion-col>\n\n                    <strong class="text-sm">{{ encaissement.DESIGN_COMPTE | titlecase }} </strong>\n\n                </ion-col>\n\n                <ion-col float-right>\n\n                    <strong class="text-sm" float-right>{{encaissement.DATE_ENCAISS | date : "dd-MM-y,HH:mm:ss"\n\n                        }}</strong>\n\n                </ion-col>\n\n            </ion-row>\n\n            <ion-row>\n\n                <ion-col col-8>\n\n                    <strong class="text-sm">{{encaissement.NOM_TIERS | titlecase}} </strong>\n\n                </ion-col>\n\n                <ion-col>\n\n                    <strong ion-text class="text-sm" float-right>{{encaissement.CREATED_BY | titlecase}}</strong>\n\n                </ion-col>\n\n                <hr size=1 align="center" width="80%">\n\n            </ion-row>\n\n        </ion-grid>\n\n    </ion-list> -->\n\n    <ion-infinite-scroll (ionInfinite)="doInfinite($event)">\n\n        <ion-infinite-scroll-content loadingSpinner="bubbles" loadingText="Loading more data..."></ion-infinite-scroll-content>\n\n    </ion-infinite-scroll>\n\n    <ion-fab bottom right #fabAddEncaiss>\n\n        <button ion-fab>\n\n            <ion-icon name="add"></ion-icon>\n\n        </button>\n\n        <ion-fab-list side="top">\n\n            <button color="danger" ion-fab (click)="addEncaissementPage(\'DEC\',fabAddEncaiss)">\n\n                <ion-icon name="remove"></ion-icon>\n\n            </button>\n\n            <button ion-fab color="green" (click)="addEncaissementPage(\'ENC\',fabAddEncaiss)">\n\n                <ion-icon color="white" name="add"></ion-icon>\n\n            </button>\n\n        </ion-fab-list>\n\n    </ion-fab>\n\n</ion-content>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\encaissements\encaissements.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavParams"],
        __WEBPACK_IMPORTED_MODULE_4__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["ToastController"],
        __WEBPACK_IMPORTED_MODULE_7__providers_helper_service_helper_service__["a" /* HelperServiceProvider */]])
], EncaissementsPage);

//# sourceMappingURL=encaissements.js.map

/***/ }),

/***/ 149:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HomePage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__form_encaissement_form_encaissement__ = __webpack_require__(91);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ionic3_datepicker__ = __webpack_require__(421);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__encaissements_encaissements__ = __webpack_require__(146);
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







var HomePage = (function () {
    function HomePage(nav, popoverCtrl, encaisseService, helperService, toastCtrl) {
        this.nav = nav;
        this.popoverCtrl = popoverCtrl;
        this.encaisseService = encaisseService;
        this.helperService = helperService;
        this.toastCtrl = toastCtrl;
        this.MENU_COMPTE = "COMPTE";
        this.dateNow = new Date().toISOString();
        this.doughnutChartLabels = ['Decaissement', 'Encaissement',];
        this.doughnutChartData = [0, 0];
        this.doughnutChartType = 'doughnut';
        this.dataCompte = [];
        this.session = [];
        this.datesStatistic = this.helperService.datesStatistic;
        this.localDate = new Date();
        this.initDate = new Date();
        this.initDate2 = new Date(2015, 1, 1);
        this.disabledDates = [new Date(2017, 7, 14)];
        this.maxDate = new Date(new Date().setDate(new Date().getDate() + 30));
        this.min = new Date();
        this.showSession = false;
        this.showEncaissement = false;
    }
    HomePage.prototype.closeDatepicker = function () {
        this.datepickerDirective.modal.dismiss();
    };
    HomePage.prototype.ionViewWillEnter = function () {
        this.sync();
    };
    HomePage.prototype.Log = function (stuff) {
    };
    HomePage.prototype.event = function (data) {
        this.dateEncaiss = data;
        this.localDate = data;
    };
    HomePage.prototype.setDate = function (date) {
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
        this.getSession();
        this.setDataChart();
        this.getCompte();
        this.datesStatistic = this.helperService.datesStatistic;
        if (this.helperService.datesStatistic != null) {
            this.getStatistic();
        }
    };
    HomePage.prototype.getCompte = function () {
        var _this = this;
        this.encaisseService.getComptes().subscribe(function (data) {
            _this.dataCompte = data;
            console.log(_this.dataCompte);
        }, function (error) {
            _this.helperService.showNotifError(error);
        });
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
                                    e.ENC = (el.TOTAL_ENCAISS == null) ? 0 : el.TOTAL_ENCAISS;
                                    break;
                                case 'DEC':
                                    e.DEC = (el.TOTAL_ENCAISS == null) ? 0 : el.TOTAL_ENCAISS;
                                    break;
                            }
                        });
                    }
                }, function (error) {
                    _this.helperService.showNotifError("statistique " + error);
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
            _this.helperService.showNotifError(error);
        });
    };
    HomePage.prototype.addEncaissementPage = function (codeType, fab) {
        fab.close();
        this.nav.push(__WEBPACK_IMPORTED_MODULE_0__form_encaissement_form_encaissement__["a" /* FormEncaissementPage */], { type: codeType });
        this.sync();
    };
    HomePage.prototype.showSessionDetail = function (id_caisse) {
        this.nav.setRoot(__WEBPACK_IMPORTED_MODULE_6__encaissements_encaissements__["a" /* EncaissementsPage */], { id_caisse: id_caisse });
    };
    HomePage.prototype.getSession = function () {
        var _this = this;
        this.encaisseService.getSessions().subscribe(function (data) {
            console.log("session  : ------------", data);
            _this.session = data;
            _this.showSession = true;
            console.log("showSession --------------------> ", _this.showSession);
        }, function (error) { return _this.helperService.showNotifError("session : " + error); });
    };
    return HomePage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_5_ionic3_datepicker__["a" /* DatePickerDirective */]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_5_ionic3_datepicker__["a" /* DatePickerDirective */])
], HomePage.prototype, "datepickerDirective", void 0);
HomePage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["Component"])({
        selector: 'page-home',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\home\home.html"*/'<!-- -->\n<ion-header>\n  <ion-navbar color="primary">\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n    <ion-title>\n      <strong>Accueil</strong>\n    </ion-title>\n    <ion-buttons end>\n      <button ion-button tappable (click)="sync()">\n        <ion-icon name="sync"></ion-icon>\n      </button>\n    </ion-buttons>\n  </ion-navbar>\n</ion-header>\n<ion-content padding class="animated fadeIn common-bg">\n  <ion-card class="border-bottom">\n    <ion-item tappable class="border-bottom ">\n      <span ion-text>\n        <strong>Session </strong>\n      </span>\n    </ion-item>\n    <div *ngIf="!showSession" text-center>\n      <ion-spinner > </ion-spinner>\n    </div>  \n    <ion-grid *ngFor=" let sess of session" (press)="showSessionDetail(sess.ID_CAISSE)">\n      <ion-row>\n        <ion-col>\n          <span ion-text class="text-big" color="black">Utilisateur</span>\n        </ion-col>\n        <ion-col float-right>\n          <strong ion-text class="text-big" float-right>{{ sess.DEBUTEE_PAR }} </strong>\n        </ion-col>\n      </ion-row>\n      <ion-row>\n        <ion-col>\n          <strong class="text-sm">Compte</strong>\n        </ion-col>\n        <ion-col float-right>\n          <strong class="text-sm" float-right>{{ sess.DESIGN_COMPTE }}</strong>\n        </ion-col>\n      </ion-row>\n      <ion-row>\n        <ion-col col-8>\n          <strong class="text-sm">Montant Encaisser </strong>\n        </ion-col>\n        <ion-col>\n          <strong ion-text class="text-sm" color="danger" float-right>{{ sess.MONT_ENCAISSEMENT | xpertCurrency }}\n          </strong>\n        </ion-col>\n        <hr size=1 align="center" width="60%">\n      </ion-row>\n    </ion-grid>\n  </ion-card>\n  <ion-card class="border-bottom">\n    <ion-item tappable class="border-bottom ">\n      <span ion-text>\n        <strong>Encaissements</strong>\n      </span>\n    </ion-item>\n    <ion-grid no-padding margin-top>\n      <ion-list>\n        <ion-item>\n          <div>\n            <h4 text-center> Aujourd\'hui </h4>\n            <ion-row margin-top>\n              <ion-col class="text-sm" width-70>\n                <span ion-text color="primary">\n                  <strong>Encaissement </strong>\n                </span>\n              </ion-col>\n              <ion-col class="text-sm" width-10 text-right>\n                <span ion-text color="primary">\n                  <strong>{{ doughnutChartData[1]| xpertCurrency :2}} </strong>\n                </span>\n              </ion-col>\n            </ion-row>\n            <ion-row margin-top>\n              <ion-col class="text-sm" width-70>\n                <span ion-text color="danger">\n                  <strong>Decaissement </strong>\n                </span>\n              </ion-col>\n              <ion-col class="text-sm" width-10 text-right>\n                <span ion-text color="danger">\n                  <strong>{{ doughnutChartData[0] | xpertCurrency : 2 }} </strong>\n                </span>\n              </ion-col>\n            </ion-row>\n          </div>\n        </ion-item>\n      </ion-list>\n    </ion-grid>\n  </ion-card>\n  <div style="display: block">\n    <canvas baseChart [data]="doughnutChartData" [labels]="doughnutChartLabels" [chartType]="doughnutChartType"\n      (chartHover)="chartHovered($event)" (chartClick)="chartClicked($event)">\n    </canvas>\n  </div>\n  <ion-card class="border-bottom">\n    <ion-item tappable class="border-bottom">\n      <span ion-text>\n        <strong>Comptes</strong>\n      </span>\n    </ion-item>\n    <ion-grid class="" no-padding margin-top>\n      <ion-list>\n        <ion-item>\n          <div>\n            <ion-row *ngFor="let compte of dataCompte">\n              <ion-col class="text-sm" width-70>\n                <span ion-text color="primary">\n                  <strong>{{ compte.DESIGN_COMPTE }} </strong>\n                </span>\n              </ion-col>\n              <ion-col class="text-sm" width-10 text-right>\n                <span ion-text color="primary" text-right>\n                  <strong>{{ compte.SOLDE_COMPTE | xpertCurrency : 2 }} </strong>\n                </span>\n              </ion-col>\n            </ion-row>\n          </div>\n        </ion-item>\n      </ion-list>\n    </ion-grid>\n  </ion-card>\n  <ion-fab bottom right #fabAddEncaiss>\n    <button ion-fab>\n      <ion-icon name="add"></ion-icon>\n    </button>\n    <ion-fab-list side="top">\n      <button color="danger" ion-fab (click)="addEncaissementPage(\'DEC\',fabAddEncaiss)">\n        <ion-icon name="remove"></ion-icon>\n      </button>\n      <button ion-fab color="green" (click)="addEncaissementPage(\'ENC\',fabAddEncaiss)">\n        <ion-icon color="white" name="add"></ion-icon>\n      </button>\n    </ion-fab-list>\n  </ion-fab>\n</ion-content>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\home\home.html"*/,
        providers: [__WEBPACK_IMPORTED_MODULE_5_ionic3_datepicker__["a" /* DatePickerDirective */]]
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_4_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_4_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_2__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_4_ionic_angular__["ToastController"]])
], HomePage);

//
//# sourceMappingURL=home.js.map

/***/ }),

/***/ 152:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LoginPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__home_home__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_auth_service_auth_service__ = __webpack_require__(93);
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
    function LoginPage(nav, addrCtrl, menu, toastCtrl, authService, storage, helperService, events) {
        this.nav = nav;
        this.addrCtrl = addrCtrl;
        this.menu = menu;
        this.toastCtrl = toastCtrl;
        this.authService = authService;
        this.storage = storage;
        this.helperService = helperService;
        this.events = events;
        this.username = "";
        this.password = "";
        this.showOptions = true;
        this.TOKEN_KEY = 'token';
        this.REMEMBER_KEY = 'remember';
        this.USERNAME_KEY = 'username';
        this.PASSWORD_KEY = 'password';
        this.networkAddress = 'localhost';
        this.message_connexion = '';
        this.connexionBool = true;
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
                                    _this.getAccountDetail();
                                    _this.rememberMeSave(_this.username, _this.password);
                                    _this.nav.setRoot(__WEBPACK_IMPORTED_MODULE_4__home_home__["a" /* HomePage */]);
                                }
                            }, function (error) {
                                if (error == "invalid_grant") {
                                    _this.helperService.showNotifError("l'identifiant ou le mot de passe est incorrect");
                                }
                                else {
                                    _this.helperService.showNotifError("l'application n'a pas pu se connecter au seveur");
                                }
                            })];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    LoginPage.prototype.getAccountDetail = function () {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.authService.getAccountDetail().subscribe(function (data) {
                            console.log("Account detail", data);
                            _this.helperService.USERNAME = data.UserId;
                            _this.events.publish('user:username', _this.helperService.USERNAME);
                        }, function (error) {
                            _this.helperService.showNotifError(error);
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
    LoginPage.prototype.testConnexion = function () {
        var _this = this;
        console.log("enter in the testconnexion method");
        this.authService.testConnexion().subscribe(function (data) {
            if (data == "ok") {
                _this.message_connexion = "vous etes bien connectee";
                _this.connexionBool = true;
            }
        }, function (error) {
            _this.message_connexion = "verifier que vous avez entrez la bonne address serveur";
            _this.connexionBool = false;
            console.log(error);
        });
    };
    LoginPage.prototype.setAddressNetwork = function () {
        var _this = this;
        this.networkAddress = this.helperService.networkAddress;
        var addresseInput = this.addrCtrl.create({
            title: 'Adresse du serveur',
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
                    text: 'Annuler',
                    handler: function (data) {
                        console.log('Cancel clicked');
                    }
                },
                {
                    text: 'Save',
                    handler: function (data) {
                        _this.helperService.saveNetworkAddress(data.addresse);
                        _this.helperService.showNotifSuccess("L'adresse a bien été mise à jour");
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
        selector: 'page-login',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\login\login.html"*/'<!-- -->\n<ion-content padding class="animated fadeIn login auth-page">\n  <div class="login-content">\n\n    <!-- Logo -->\n    <div padding-horizontal text-center class="animated fadeInDown">\n      <div class="logo"></div>\n\n    </div>\n\n    <!-- Login form -->\n    <form #f="ngForm" class="list-form" (ngSubmit)="OnSignin(f)">\n      <ion-item>\n        <ion-label floating>\n          <ion-icon name="contact" item-start class="text-primary"></ion-icon>\n         Nom d\'utilisateur\n        </ion-label>\n        <ion-input type="text" [(ngModel)]="username" name="username" required username></ion-input>\n      </ion-item>\n\n      <ion-item>\n        <ion-label floating>\n          <ion-icon name="lock" item-start class="text-primary"></ion-icon>\n          Mot de passe\n        </ion-label>\n        <ion-input type="password" [(ngModel)]="password" name="password" required></ion-input>\n      </ion-item>\n      <ion-item no-lines>\n        <ion-label>Se souvenir</ion-label>\n        <ion-toggle checked="true" #rememberMe></ion-toggle>\n      </ion-item>\n    </form>\n    <p text-right ion-text color="secondary" tappable (click)="setAddressNetwork()">\n      <strong>Changer l\'adresse du serveur</strong>\n    </p>\n    <div text-center>\n      <button ion-button text-center color="secondary" round small (click)="testConnexion()"> Test Connexion</button>\n      <p [ngClass]="connexionBool ? \'green\':\'red\'">{{ message_connexion}}</p>\n    </div>\n    <div>\n\n      <button ion-button icon-start block color="dark" type="submit" (click)="OnSignin(f)" [disabled]="!f.valid">\n        <ion-icon name="log-in"></ion-icon>\n        Connexion\n      </button>\n    </div>\n  </div>\n</ion-content>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\login\login.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["AlertController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["MenuController"],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["ToastController"],
        __WEBPACK_IMPORTED_MODULE_5__providers_auth_service_auth_service__["a" /* AuthServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_1__ionic_storage__["b" /* Storage */],
        __WEBPACK_IMPORTED_MODULE_0__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["Events"]])
], LoginPage);

//# sourceMappingURL=login.js.map

/***/ }),

/***/ 163:
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
webpackEmptyAsyncContext.id = 163;

/***/ }),

/***/ 233:
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
webpackEmptyAsyncContext.id = 233;

/***/ }),

/***/ 26:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HelperServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_ionic_angular__ = __webpack_require__(13);
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
    function HelperServiceProvider(storage, toastCtrl) {
        this.storage = storage;
        this.toastCtrl = toastCtrl;
        this.NETWORK_ADDRESS_KEY = 'network_address';
        this.DATE_STATISTIC_KEY = 'Date_Statistic';
        this.USERNAME = ' ';
        this.ROLE = ' ';
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
                            _this.showNotifError("Network Address :" + error);
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
            _this.showNotifError(" statistique :" + error);
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
        return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].throw(error.json().ExceptionMessage || "Server error");
    };
    HelperServiceProvider.prototype.logResponse = function (res) {
        console.log(res);
    };
    HelperServiceProvider.prototype.extractData = function (res) {
        return res.json();
    };
    HelperServiceProvider.prototype.showNotifSuccess = function (success) {
        var toast = this.toastCtrl.create({
            message: success,
            duration: 3000,
            position: 'bottom',
            cssClass: 'dark-trans',
            closeButtonText: 'OK',
            showCloseButton: true
        });
        toast.present();
    };
    HelperServiceProvider.prototype.showNotifError = function (error) {
        var toast = this.toastCtrl.create({
            message: 'Erreur ' + error,
            duration: 3000,
            position: 'bottom',
            cssClass: 'dark-trans',
            closeButtonText: 'OK',
            showCloseButton: true
        });
        toast.present();
    };
    HelperServiceProvider.prototype.createLink = function () {
        var argume = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            argume[_i] = arguments[_i];
        }
        var strUrl = this.networkAddress;
        argume.forEach(function (element) {
            strUrl += element + "/";
        });
        console.log("the utrl WITH CREATE LINK ", strUrl);
        return strUrl;
    };
    return HelperServiceProvider;
}());
HelperServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__ionic_storage__["b" /* Storage */],
        __WEBPACK_IMPORTED_MODULE_3_ionic_angular__["ToastController"]])
], HelperServiceProvider);

//# sourceMappingURL=helper-service.js.map

/***/ }),

/***/ 274:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_chart_js__ = __webpack_require__(275);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_chart_js___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_chart_js__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__providers_dashboard_service_dashboard_service__ = __webpack_require__(145);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ionic_pullup__ = __webpack_require__(408);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__models_datachart__ = __webpack_require__(778);
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
    function DashboardPage(nav, popoverCtrl, dashboardService, encaissementService, helperService) {
        this.nav = nav;
        this.popoverCtrl = popoverCtrl;
        this.dashboardService = dashboardService;
        this.encaissementService = encaissementService;
        this.helperService = helperService;
        this.isLoading = true;
        this.raccourci_date = 'annee';
        this.localDateDebut = new Date();
        this.localDateFin = new Date();
        this.dateDebut = null;
        this.dateFin = null;
        this.encaissementData = [];
        this.dateNow = new Date();
        this.BACKGROUND_COLOR = ['rgb(255, 0, 255)', 'rgb(255, 0, 0)', 'rgb(128, 0, 0)', 'rgb(128, 0, 128)', 'rgb(0, 0, 255)', 'rgb(0, 128, 128)', 'rgb(0, 255, 0)', 'rgb(0, 128, 0)', 'rgb(25, 128, 50)'];
        this.footerState = __WEBPACK_IMPORTED_MODULE_5_ionic_pullup__["a" /* IonPullUpFooterState */].Collapsed;
    }
    DashboardPage.prototype.ngOnInit = function () {
        this.localDateDebut.setFullYear(this.localDateDebut.getFullYear() - 1);
        var dateNow = new Date();
        this.margeChart = this.createEmptyChart(this.margeCanvas);
        this.totalAchatChart = this.createEmptyChart(this.totaleAchatCanvas);
        this.totaleVenteChart = this.createEmptyChart(this.totaleVenteCanvas);
        this.setDataChart();
    };
    DashboardPage.prototype.footerExpanded = function () {
        console.log('Footer expanded!');
    };
    DashboardPage.prototype.footerCollapsed = function () {
        console.log('Footer collapsed!');
    };
    DashboardPage.prototype.toggleFooter = function () {
        this.footerState = this.footerState == __WEBPACK_IMPORTED_MODULE_5_ionic_pullup__["a" /* IonPullUpFooterState */].Collapsed ? __WEBPACK_IMPORTED_MODULE_5_ionic_pullup__["a" /* IonPullUpFooterState */].Expanded : __WEBPACK_IMPORTED_MODULE_5_ionic_pullup__["a" /* IonPullUpFooterState */].Collapsed;
    };
    DashboardPage.prototype.onRaccourciDateChange = function () {
        this.localDateDebut = new Date();
        this.localDateFin = new Date();
        switch (this.raccourci_date) {
            case 'jour': {
                console.log("Jour :  date debut", this.localDateDebut);
                console.log("Jour : date fin ", this.localDateFin);
                break;
            }
            case 'mois': {
                this.localDateDebut.setMonth(this.localDateDebut.getMonth() - 1);
                console.log("Mois : dateDebut    ", this.localDateDebut);
                console.log("Mois : dateFin   ", this.localDateFin);
                break;
            }
            case 'annee': {
                this.localDateDebut.setFullYear(this.localDateDebut.getFullYear() - 1);
                console.log("Anne :    ", this.localDateDebut);
                console.log("Anne :    ", this.localDateFin);
                break;
            }
        }
        this.setDataChart();
    };
    DashboardPage.prototype.setDateDebut = function (date) {
        this.localDateDebut = date;
        this.dateDebut = this.localDateDebut.toISOString();
        this.setDataChart();
    };
    DashboardPage.prototype.setDateFin = function (date) {
        this.localDateFin = date;
        this.dateFin = this.localDateFin.toISOString();
        this.setDataChart();
    };
    DashboardPage.prototype.setDataChart = function () {
        var _this = this;
        this.isLoading = true;
        console.log("data debut", this.localDateDebut.toISOString());
        console.log("data Fin", this.localDateFin.toISOString());
        this.dashboardService.getMargeParVendeur(this.localDateDebut, this.localDateFin).subscribe(function (data) {
            var dataChartMarge = new __WEBPACK_IMPORTED_MODULE_7__models_datachart__["a" /* DataChart */](data, "CREATED_BY", "Sum_MARGE");
            var dataChartTotaleVente = new __WEBPACK_IMPORTED_MODULE_7__models_datachart__["a" /* DataChart */](data, "CREATED_BY", "Sum_TOTAL_VENTE");
            var dataChartTotaleAchat = new __WEBPACK_IMPORTED_MODULE_7__models_datachart__["a" /* DataChart */](data, "CREATED_BY", "Sum_TOTAL_ACHAT");
            //Marge par Vendeur
            _this.updateChart(_this.margeChart, dataChartMarge);
            // Totale Vente
            _this.updateChart(_this.totaleVenteChart, dataChartTotaleVente);
            // totale achat
            _this.updateChart(_this.totalAchatChart, dataChartTotaleAchat);
            _this.isLoading = false;
        }, function (error) {
            _this.helperService.showNotifError('dashboard MargeVendeur :' + error);
        });
    };
    DashboardPage.prototype.updateChart = function (chart, dataChart) {
        var _this = this;
        this.removeDataChart(chart);
        chart.data.labels = dataChart.labels;
        chart.data.datasets.forEach(function (dataset) {
            dataset.data = dataChart.values;
            dataset.backgroundColor = _this.BACKGROUND_COLOR;
        });
        console.log("the chart after updated ", chart);
        chart.update();
    };
    DashboardPage.prototype.removeDataChart = function (chart) {
        chart.data.labels.pop();
        chart.data.datasets.forEach(function (dataset) {
            dataset.data.pop();
        });
    };
    DashboardPage.prototype.createChart = function (canvas, dataChart) {
        return new __WEBPACK_IMPORTED_MODULE_3_chart_js__["Chart"](canvas.nativeElement, {
            type: 'horizontalBar',
            data: {
                labels: (dataChart.labels.length > 0) ? dataChart.labels : ["none"],
                datasets: [{
                        data: (dataChart.values.length > 0) ? dataChart.values : [0],
                        backgroundColor: this.BACKGROUND_COLOR
                    }]
            }, options: {
                legend: {
                    display: false,
                }
            }
        });
    };
    DashboardPage.prototype.createEmptyChart = function (canvas) {
        return new __WEBPACK_IMPORTED_MODULE_3_chart_js__["Chart"](canvas.nativeElement, {
            type: 'horizontalBar',
            data: {
                labels: ["none"],
                datasets: [{
                        data: [0],
                        backgroundColor: this.BACKGROUND_COLOR
                    }]
            }, options: {
                legend: {
                    display: false,
                }
            }
        });
    };
    DashboardPage.prototype.ionViewDidLoad = function () {
    };
    return DashboardPage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["ViewChild"])('margeCanvas'),
    __metadata("design:type", Object)
], DashboardPage.prototype, "margeCanvas", void 0);
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["ViewChild"])('totaleVenteCanvas'),
    __metadata("design:type", Object)
], DashboardPage.prototype, "totaleVenteCanvas", void 0);
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["ViewChild"])('totaleAchatCanvas'),
    __metadata("design:type", Object)
], DashboardPage.prototype, "totaleAchatCanvas", void 0);
DashboardPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Component"])({
        selector: 'page-dashboard',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\dashboard\dashboard.html"*/'<!-- -->\n<ion-header>\n  <ion-navbar color="primary">\n    <button ion-button menuToggle>\n      <ion-icon name="menu"></ion-icon>\n    </button>\n    <ion-title>\n      <strong>Dashboard</strong>\n    </ion-title>\n    <ion-buttons end>\n      \n      <button ion-button tappable>\n        <ion-icon  *ngIf="!isLoading" name="sync" (click)="setDataChart()"></ion-icon>\n        <ion-spinner *ngIf="isLoading"  color="light" name="crescent">\n          </ion-spinner>\n      </button>\n    </ion-buttons>\n  </ion-navbar>\n</ion-header>\n<ion-content>\n  \n  <div style="margin-top:20px;display:block;" text-center>\n    <h5 align-center>Marge par vendeur</h5>\n    <canvas width="800" height="600" #margeCanvas ></canvas>\n  </div>\n  <div style="display:block;" text-center>\n    <h5 align-center>Totale Vente</h5>\n    <canvas width="800" height="600" #totaleVenteCanvas></canvas>\n  </div>\n  <div style="display:block;" text-center>\n    <h5 align-center>Totale Achat</h5>\n    <canvas width="800" height="600" #totaleAchatCanvas></canvas>\n  </div>\n\n\n\n  <ion-row class="hidden-div">\n\n  </ion-row>\n</ion-content>\n<ion-pullup (onExpand)="footerExpanded()" (onCollapse)="footerCollapsed()" [(state)]="footerState" class="tab-footer">\n  <!--  <ion-pullup-tab [footer]="pullup" color="white" class="tab-footer" (tap)="toggleFooter()">\n    <ion-icon color="primary" name="arrow-up" *ngIf="footerState == 0"></ion-icon>\n    <ion-icon color="primary" name="arrow-down" *ngIf="footerState == 1"></ion-icon>\n  </ion-pullup-tab> -->\n  <ion-toolbar color="white">\n    <ion-slides>\n      <ion-slide>\n        <ion-segment no-border-top [(ngModel)]="raccourci_date" color="secondary" (ionChange)="onRaccourciDateChange()">\n          <ion-segment-button value="annee">\n            annee\n          </ion-segment-button>\n          <ion-segment-button value="mois">\n            mois\n          </ion-segment-button>\n          <ion-segment-button value="jour">\n            jour\n          </ion-segment-button>\n        </ion-segment>\n      </ion-slide>\n      <ion-slide>\n        <ion-row>\n          <ion-col>\n            <span ion-datepicker [modalOptions]="" [localeStrings]="{weekdays: [\'Dim\', \'Lun\', \'Mar\', \'Mer\', \'Jeu\', \'Vend\', \'Sam\'],\n            months: [\'Janvier\', \'Février\', \'Mars\', \'Avril\', \'Mai\', \'Juin\', \'Juillet\', \'Août\', \'Septembre\', \'Octobre\', \'Novembre\', \'Décembre\']}"\n              (ionChanged)="setDateDebut($event);" [(value)]="localDateDebut" clear class="ScheduleDate">\n\n              <span class="xpert-date" float-right> {{localDateDebut | date : \'dd-MM-y\'}} </span>\n            </span>\n          </ion-col>\n          <ion-col>\n            <span ion-button color="primary">\n              <ion-icon name="arrow-round-forward"></ion-icon>\n            </span>\n          </ion-col>\n          <ion-col>\n            <span ion-datepicker [modalOptions]="" [okText]="Ok" [cancelText]="Annuler" [locale]="fr-FR" (ionChanged)="setDateFin($event);"\n              [min]="localDateDebut" [(value)]="localDateFin" clear class="ScheduleDate">\n              <span class="xpert-date" float-left> {{localDateFin | date : \'dd-MM-y\'}}</span>\n            </span>\n          </ion-col>\n        </ion-row>\n      </ion-slide>\n\n    </ion-slides>\n  </ion-toolbar>\n</ion-pullup>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\dashboard\dashboard.html"*/,
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["PopoverController"],
        __WEBPACK_IMPORTED_MODULE_4__providers_dashboard_service_dashboard_service__["a" /* DashboardServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_0__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_6__providers_helper_service_helper_service__["a" /* HelperServiceProvider */]])
], DashboardPage);

//# sourceMappingURL=dashboard.js.map

/***/ }),

/***/ 411:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return XpertCurrencyPipe; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(30);
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

/***/ 412:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TiersServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__Module_httpInterceptor__ = __webpack_require__(56);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__ = __webpack_require__(26);
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
  Generated class for the TiersServiceProvider provider.

  See https://angular.io/guide/dependency-injection for more info on providers
  and Angular DI.
*/
var TiersServiceProvider = (function () {
    function TiersServiceProvider(http, helperService) {
        this.http = http;
        this.helperService = helperService;
        this.BASE_URL = "api";
        this.TIERS_URL = "tiers";
    }
    TiersServiceProvider.prototype.getTiers = function (codeTiers) {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.TIERS_URL, codeTiers))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    return TiersServiceProvider;
}());
TiersServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__Module_httpInterceptor__["a" /* HttpInterceptor */],
        __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__["a" /* HelperServiceProvider */]])
], TiersServiceProvider);

//# sourceMappingURL=tiers-service.js.map

/***/ }),

/***/ 413:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MenuEncaissementListComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__pages_form_encaissement_form_encaissement__ = __webpack_require__(91);
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
        // on recupere la reference de la page qui appele le composant menu
        this.encaissementsPage = navParams.get('parent');
    }
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
                            _this.navCtrl.pop();
                            // on met a jour la page des encaissments a partir du composant menu 
                            _this.encaissementsPage.getEncaissementSPerPage();
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
    Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["Component"])({
        template: "\n    <ion-list>\n      <button ion-item  (click)=\"updateEncaissement()\">Modifier</button>\n      <button ion-item (click) =\"deleteEncaissement()\">Supprimer</button>\n    </ion-list>\n  "
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

/***/ 414:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Tiers; });
var Tiers = (function () {
    function Tiers() {
        this.CODE_TIERS = "all";
        this.NOM_TIERS = "Tous";
        this.NOM_TIERS1 = "Tous";
        this.SOLDE_TIERS = 0;
    }
    return Tiers;
}());

//# sourceMappingURL=tiers.js.map

/***/ }),

/***/ 415:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Motif; });
var Motif = (function () {
    function Motif() {
        this.CODE_MOTIF = "all";
        this.DESIGN_MOTIF = "Tous";
    }
    return Motif;
}());

//# sourceMappingURL=motif.js.map

/***/ }),

/***/ 416:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MenuFilterComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_tiers__ = __webpack_require__(414);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_motif__ = __webpack_require__(415);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__providers_helper_service_helper_service__ = __webpack_require__(26);
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
 * Generated class for the MenuFilterComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
var MenuFilterComponent = (function () {
    function MenuFilterComponent(encaisseService, helperService, viewCtrl, navCtrl, navParams) {
        this.encaisseService = encaisseService;
        this.helperService = helperService;
        this.viewCtrl = viewCtrl;
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.localDateDebut = new Date();
        this.localDateFin = new Date();
        this.dateDebut = null;
        this.dateFin = null;
        this.tiers = new __WEBPACK_IMPORTED_MODULE_1__models_tiers__["a" /* Tiers */]();
        this.motif = new __WEBPACK_IMPORTED_MODULE_2__models_motif__["a" /* Motif */]();
        this.tiersAll = new __WEBPACK_IMPORTED_MODULE_1__models_tiers__["a" /* Tiers */]();
        this.filterActivated = false;
        this.compteList = [];
        this.codeCompte = "all";
    }
    MenuFilterComponent.prototype.ionViewDidEnter = function () {
    };
    MenuFilterComponent.prototype.ngOnInit = function () {
        this.getMotifs();
        this.getTiers();
        this.getCaisses();
        this.localDateDebut = this.navParams.get('dateDebut');
        this.localDateFin = this.navParams.get('dateFin');
        this.motif = this.navParams.get('motif');
        this.tiers = this.navParams.get('tiers');
        this.codeCompte = this.navParams.get('codeCompte');
    };
    MenuFilterComponent.prototype.getTiers = function () {
        this.listTiers = this.encaisseService.tiersListForFilter;
        this.tiers = this.encaisseService.tiersFilter;
    };
    MenuFilterComponent.prototype.getMotifs = function () {
        this.listMotifs = this.encaisseService.motifListForFilter;
        this.motif = this.encaisseService.motifFilter;
        console.log(this.listMotifs);
    };
    MenuFilterComponent.prototype.getCaisses = function () {
        this.compteList = this.encaisseService.caissesList;
    };
    MenuFilterComponent.prototype.setDateDebut = function (date) {
        this.localDateDebut = date;
        this.dateDebut = this.localDateDebut.toISOString();
    };
    MenuFilterComponent.prototype.setDateFin = function (date) {
        this.localDateFin = date;
        this.dateFin = this.localDateFin.toISOString();
    };
    MenuFilterComponent.prototype.close = function () {
        this.navCtrl.pop();
    };
    MenuFilterComponent.prototype.desactiver = function () {
        this.viewCtrl.dismiss({
            filter: false,
        });
    };
    MenuFilterComponent.prototype.filter = function () {
        this.viewCtrl.dismiss({
            filter: true,
            dateDebut: this.localDateDebut,
            dateFin: this.localDateFin,
            tiers: this.tiers,
            motif: this.motif,
            compte: this.codeCompte
        });
    };
    MenuFilterComponent.prototype.tiersChange = function (event) {
    };
    return MenuFilterComponent;
}());
MenuFilterComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'menu-filter',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\menu-filter\menu-filter.html"*/'<!-- Generated template for the MenuFilterComponent component -->\n\n<div class="container">\n  <button ion-button small outline class="btnCorner right-corner" color="danger" float-right (click)="close()">\n    <ion-icon ios="ios-close" md="md-close"></ion-icon>\n  </button>\n  <ion-list>\n    <ion-item>\n      <span ion-datepicker [modalOptions]="" [localeStrings]="{weekdays: [\'Dim\', \'Lun\', \'Mar\', \'Mer\', \'Jeu\', \'Vend\', \'Sam\'],\n      months: [\'Janvier\', \'Février\', \'Mars\', \'Avril\', \'Mai\', \'Juin\', \'Juillet\', \'Août\', \'Septembre\', \'Octobre\', \'Novembre\', \'Décembre\']}"\n        (ionChanged)="setDateDebut($event);" [(value)]="localDateDebut" clear class="ScheduleDate">\n        <ion-icon name="calendar"></ion-icon>\n        <span float-right> {{localDateDebut | date : \'dd-MM-y\'}}</span>\n      </span>\n    </ion-item>\n    <ion-item>\n      <span ion-datepicker [modalOptions]="" [okText]="Ok" [cancelText]="Annuler" [locale]="fr-FR" (ionChanged)="setDateFin($event);"\n        [min]="localDateDebut" [(value)]="localDateFin" clear class="ScheduleDate">\n        <ion-icon name="calendar"></ion-icon>\n        <span float-right> {{localDateFin | date : \'dd-MM-y\'}}</span>\n      </span>\n    </ion-item>\n    <ion-item>\n      <ion-label floating>Compte</ion-label>\n      <ion-select name="codeCompte" [(ngModel)]="codeCompte" interface="popover" required>\n        <ion-option value="all">\n          Tous\n        </ion-option>\n        <ion-option *ngFor="let compte of compteList" [value]="compte.CODE_COMPTE">\n          {{ compte.DESIGN_COMPTE }}\n        </ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label floating>Motif</ion-label>\n      <select-searchable item-content searchPlaceholder="motif" [focusSearchbar]="true" name="Motif" [(ngModel)]="motif"\n        [items]="listMotifs" itemValueField="CODE_MOTIF" itemTextField="DESIGN_MOTIF" [canSearch]="true">\n      </select-searchable>\n    </ion-item>\n    <ion-item>\n      <ion-label floating>Tiers</ion-label>\n      <select-searchable item-content name="tiers" [(ngModel)]="tiers" [items]="listTiers" itemValueField="CODE_TIERS"\n        itemTextField="NOM_TIERS1" [canSearch]="true" (onChange)="tiersChange($event)">\n      </select-searchable>\n    </ion-item>\n    <ion-item text-center>\n      <button ion-button (click)="filter()">\n        Appliquer\n      </button>\n      <button ion-button (click)="desactiver()">\n        Desactiver\n      </button>\n    </ion-item>\n  </ion-list>\n</div>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\menu-filter\menu-filter.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_5__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_4_ionic_angular__["ViewController"],
        __WEBPACK_IMPORTED_MODULE_4_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_4_ionic_angular__["NavParams"]])
], MenuFilterComponent);

//# sourceMappingURL=menu-filter.js.map

/***/ }),

/***/ 424:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser_dynamic__ = __webpack_require__(425);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_module__ = __webpack_require__(429);



// this is the magic wand
Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["enableProdMode"])();
Object(__WEBPACK_IMPORTED_MODULE_0__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 429:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__ = __webpack_require__(56);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_dashboard_service_dashboard_service__ = __webpack_require__(145);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__ = __webpack_require__(274);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__ = __webpack_require__(411);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__components_menu_sommaire_menu_sommaire__ = __webpack_require__(779);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__pages_form_encaissement_form_encaissement__ = __webpack_require__(91);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__components_menu_encaissement_list_menu_encaissement_list__ = __webpack_require__(413);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__pages_encaissements_encaissements__ = __webpack_require__(146);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__angular_platform_browser__ = __webpack_require__(48);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__angular_common_http__ = __webpack_require__(780);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14_ng2_charts__ = __webpack_require__(781);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14_ng2_charts___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_14_ng2_charts__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__ionic_native_status_bar__ = __webpack_require__(417);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__ionic_native_splash_screen__ = __webpack_require__(419);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__ionic_native_keyboard__ = __webpack_require__(420);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__app_component__ = __webpack_require__(786);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__pages_settings_settings__ = __webpack_require__(789);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__pages_home_home__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__pages_login_login__ = __webpack_require__(152);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_22__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23__providers_auth_service_auth_service__ = __webpack_require__(93);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_24__angular_http__ = __webpack_require__(144);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_25_ionic_select_searchable__ = __webpack_require__(790);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_25_ionic_select_searchable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_25_ionic_select_searchable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_26_ionic3_datepicker__ = __webpack_require__(421);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_27__Module_httpFactory__ = __webpack_require__(791);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_28__components_xpert_chart_xpert_chart__ = __webpack_require__(792);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_29__components_encaiss_encaiss__ = __webpack_require__(793);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_30__components_menu_filter_menu_filter__ = __webpack_require__(416);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_31_ionic_selectable__ = __webpack_require__(794);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_32_ionic_pullup__ = __webpack_require__(408);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_33__providers_tiers_service_tiers_service__ = __webpack_require__(412);
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
    Object(__WEBPACK_IMPORTED_MODULE_9__angular_core__["NgModule"])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_18__app_component__["a" /* MyApp */],
            __WEBPACK_IMPORTED_MODULE_19__pages_settings_settings__["a" /* SettingsPage */],
            __WEBPACK_IMPORTED_MODULE_20__pages_home_home__["a" /* HomePage */],
            __WEBPACK_IMPORTED_MODULE_21__pages_login_login__["a" /* LoginPage */],
            __WEBPACK_IMPORTED_MODULE_8__pages_encaissements_encaissements__["a" /* EncaissementsPage */],
            __WEBPACK_IMPORTED_MODULE_7__components_menu_encaissement_list_menu_encaissement_list__["a" /* MenuEncaissementListComponent */],
            __WEBPACK_IMPORTED_MODULE_6__pages_form_encaissement_form_encaissement__["a" /* FormEncaissementPage */],
            __WEBPACK_IMPORTED_MODULE_5__components_menu_sommaire_menu_sommaire__["a" /* MenuSommaireComponent */],
            __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
            __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__["a" /* DashboardPage */],
            __WEBPACK_IMPORTED_MODULE_28__components_xpert_chart_xpert_chart__["a" /* XpertChartComponent */],
            __WEBPACK_IMPORTED_MODULE_29__components_encaiss_encaiss__["a" /* EncaissComponent */],
            __WEBPACK_IMPORTED_MODULE_30__components_menu_filter_menu_filter__["a" /* MenuFilterComponent */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_11__angular_platform_browser__["a" /* BrowserModule */],
            __WEBPACK_IMPORTED_MODULE_12__angular_common_http__["a" /* HttpClientModule */],
            __WEBPACK_IMPORTED_MODULE_25_ionic_select_searchable__["SelectSearchableModule"],
            __WEBPACK_IMPORTED_MODULE_32_ionic_pullup__["b" /* IonPullupModule */],
            __WEBPACK_IMPORTED_MODULE_10_ionic_angular__["IonicModule"].forRoot(__WEBPACK_IMPORTED_MODULE_18__app_component__["a" /* MyApp */], {
                scrollPadding: false,
                scrollAssist: true,
                autoFocusAssist: false
            }, {
                links: []
            }),
            __WEBPACK_IMPORTED_MODULE_31_ionic_selectable__["a" /* IonicSelectableModule */],
            __WEBPACK_IMPORTED_MODULE_13__ionic_storage__["a" /* IonicStorageModule */].forRoot({
                name: '__mydb',
                driverOrder: ['indexeddb', 'sqlite', 'websql']
            }),
            __WEBPACK_IMPORTED_MODULE_24__angular_http__["d" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_14_ng2_charts__["ChartsModule"],
            __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
            __WEBPACK_IMPORTED_MODULE_26_ionic3_datepicker__["b" /* DatePickerModule */],
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_10_ionic_angular__["IonicApp"]],
        entryComponents: [
            __WEBPACK_IMPORTED_MODULE_18__app_component__["a" /* MyApp */],
            __WEBPACK_IMPORTED_MODULE_19__pages_settings_settings__["a" /* SettingsPage */],
            __WEBPACK_IMPORTED_MODULE_20__pages_home_home__["a" /* HomePage */],
            __WEBPACK_IMPORTED_MODULE_21__pages_login_login__["a" /* LoginPage */],
            __WEBPACK_IMPORTED_MODULE_8__pages_encaissements_encaissements__["a" /* EncaissementsPage */],
            __WEBPACK_IMPORTED_MODULE_7__components_menu_encaissement_list_menu_encaissement_list__["a" /* MenuEncaissementListComponent */],
            __WEBPACK_IMPORTED_MODULE_6__pages_form_encaissement_form_encaissement__["a" /* FormEncaissementPage */],
            __WEBPACK_IMPORTED_MODULE_5__components_menu_sommaire_menu_sommaire__["a" /* MenuSommaireComponent */],
            __WEBPACK_IMPORTED_MODULE_2__pages_dashboard_dashboard__["a" /* DashboardPage */],
            __WEBPACK_IMPORTED_MODULE_29__components_encaiss_encaiss__["a" /* EncaissComponent */],
            __WEBPACK_IMPORTED_MODULE_30__components_menu_filter_menu_filter__["a" /* MenuFilterComponent */]
        ],
        providers: [
            {
                provide: __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__["a" /* HttpInterceptor */],
                useFactory: __WEBPACK_IMPORTED_MODULE_27__Module_httpFactory__["a" /* HttpFactory */],
                deps: [__WEBPACK_IMPORTED_MODULE_24__angular_http__["f" /* XHRBackend */], __WEBPACK_IMPORTED_MODULE_24__angular_http__["e" /* RequestOptions */], __WEBPACK_IMPORTED_MODULE_23__providers_auth_service_auth_service__["a" /* AuthServiceProvider */]]
            },
            __WEBPACK_IMPORTED_MODULE_15__ionic_native_status_bar__["a" /* StatusBar */],
            __WEBPACK_IMPORTED_MODULE_16__ionic_native_splash_screen__["a" /* SplashScreen */],
            __WEBPACK_IMPORTED_MODULE_17__ionic_native_keyboard__["a" /* Keyboard */],
            __WEBPACK_IMPORTED_MODULE_22__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_23__providers_auth_service_auth_service__["a" /* AuthServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_3__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_4__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
            __WEBPACK_IMPORTED_MODULE_1__providers_dashboard_service_dashboard_service__["a" /* DashboardServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_22__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
            __WEBPACK_IMPORTED_MODULE_33__providers_tiers_service_tiers_service__["a" /* TiersServiceProvider */],
        ],
        schemas: [__WEBPACK_IMPORTED_MODULE_9__angular_core__["CUSTOM_ELEMENTS_SCHEMA"]]
    })
], AppModule);

//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 51:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EncaisseServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__ = __webpack_require__(56);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__helper_service_helper_service__ = __webpack_require__(26);
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



var EncaisseServiceProvider = (function () {
    function EncaisseServiceProvider(http, helperService) {
        this.http = http;
        this.helperService = helperService;
        this.BASE_URL = "api";
        this.ENCAISSEMENT_URL = "Encaissements";
        this.ENCAISSEMENT_Per_Page_URL = "EncaissementsPerPage";
        this.ADD_ENCAISSEMENT_URL = "addEncaissement";
        this.DELETE_ENCAISSEMENT_URL = "deleteEncaissement";
        this.MOTIFS_URL = "motifs";
        this.CAISSES_URL = "caisses";
        this.COMPTES_URL = "comptes";
        this.TIERS_URL = "tiers";
        this.STATISTIC_URL = "statistic";
        this.SESSION_URL = "session";
        this.motifsList = [];
        this.motifFilter = {
            CODE_MOTIF: "all",
            DESIGN_MOTIF: "Tous"
        };
        this.motifListForFilter = [];
        this.tiersList = [];
        this.tiersListForForm = [];
        this.tiersForm = {
            CODE_TIERS: null,
            NOM_TIERS: "  ",
            NOM_TIERS1: "  ",
            SOLDE_TIERS: 0
        };
        this.tiersListForFilter = [];
        this.tiersFilter = {
            CODE_TIERS: "all",
            NOM_TIERS: "Tous",
            NOM_TIERS1: "Tous",
            SOLDE_TIERS: 0
        };
        this.caissesList = [];
        this.loadCaisses();
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
    }
    EncaisseServiceProvider.prototype.getStatisticEncaiss = function (date_start, date_end) {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.STATISTIC_URL, date_start, date_end))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getComptes = function () {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.COMPTES_URL))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getEncaissements = function (type) {
        return this.http.get(this.helperService.createLink(this.BASE_URL, type))
            .map(this.helperService.extractData)
            .do(this.helperService.logResponse)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getEncaissementsPerPage = function (type, page, idCaisse, dateDebut, dateFin, codeTiers, codeMotif, codeCompte) {
        var url;
        if (dateDebut == null && dateFin == null) {
            url = this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_Per_Page_URL, type, page, idCaisse);
        }
        else {
            url = this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_Per_Page_URL, type, page, idCaisse, dateDebut, dateFin, codeTiers, codeMotif, codeMotif, codeCompte);
        }
        return this.http.get(url)
            .map(this.helperService.extractData)
            .do(this.helperService.logResponse)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getMotifs = function (type) {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.MOTIFS_URL, type))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getTiers = function () {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.TIERS_URL))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getCaisses = function () {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.CAISSES_URL))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.getSessions = function () {
        var url = this.helperService.createLink(this.BASE_URL, this.SESSION_URL);
        console.log("url session get Sessions :  ", url);
        console.log("http://localhost/xpertpharm.rest.api/api/session/");
        return this.http.get(url)
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
        return this.http.post(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.ADD_ENCAISSEMENT_URL), body)
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
        return this.http.put(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL), body)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.deleteEncaissement = function (codeEncaiss) {
        var body = {
            "CODE_ENCAISS": codeEncaiss
        };
        return this.http.post(this.helperService.createLink(this.BASE_URL, this.ENCAISSEMENT_URL, this.DELETE_ENCAISSEMENT_URL), body)
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(this.helperService.catchError);
    };
    EncaisseServiceProvider.prototype.loadMotifs = function () {
        var _this = this;
        this.getMotifs("ENC").subscribe(function (data) {
            _this.motifsList = data.slice();
            _this.motifListForFilter = data.slice();
            _this.motifListForFilter.unshift(_this.motifFilter);
            _this.loadTiers();
        }, function (error) {
            _this.helperService.showNotifError(error);
        });
    };
    EncaisseServiceProvider.prototype.loadTiers = function () {
        var _this = this;
        this.getTiers().subscribe(function (data) {
            _this.tiersListForFilter = data.slice();
            _this.tiersListForForm = data.slice();
            _this.tiersListForFilter.unshift(_this.tiersFilter);
            _this.tiersListForForm.unshift(_this.tiersForm);
        }, function (error) {
            _this.helperService.showNotifError(error);
        });
    };
    EncaisseServiceProvider.prototype.loadCaisses = function () {
        var _this = this;
        this.getCaisses().subscribe(function (data) {
            _this.caissesList = data;
            _this.loadMotifs();
        }, function (error) {
            _this.helperService.showNotifError(error);
        });
    };
    return EncaisseServiceProvider;
}());
EncaisseServiceProvider = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_2__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__Module_httpInterceptor__["a" /* HttpInterceptor */],
        __WEBPACK_IMPORTED_MODULE_1__helper_service_helper_service__["a" /* HelperServiceProvider */]])
], EncaisseServiceProvider);

//# sourceMappingURL=encaiss-service.js.map

/***/ }),

/***/ 56:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HttpInterceptor; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_auth_service_auth_service__ = __webpack_require__(93);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(144);
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

/***/ 758:
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"./af": 285,
	"./af.js": 285,
	"./ar": 286,
	"./ar-dz": 287,
	"./ar-dz.js": 287,
	"./ar-kw": 288,
	"./ar-kw.js": 288,
	"./ar-ly": 289,
	"./ar-ly.js": 289,
	"./ar-ma": 290,
	"./ar-ma.js": 290,
	"./ar-sa": 291,
	"./ar-sa.js": 291,
	"./ar-tn": 292,
	"./ar-tn.js": 292,
	"./ar.js": 286,
	"./az": 293,
	"./az.js": 293,
	"./be": 294,
	"./be.js": 294,
	"./bg": 295,
	"./bg.js": 295,
	"./bm": 296,
	"./bm.js": 296,
	"./bn": 297,
	"./bn.js": 297,
	"./bo": 298,
	"./bo.js": 298,
	"./br": 299,
	"./br.js": 299,
	"./bs": 300,
	"./bs.js": 300,
	"./ca": 301,
	"./ca.js": 301,
	"./cs": 302,
	"./cs.js": 302,
	"./cv": 303,
	"./cv.js": 303,
	"./cy": 304,
	"./cy.js": 304,
	"./da": 305,
	"./da.js": 305,
	"./de": 306,
	"./de-at": 307,
	"./de-at.js": 307,
	"./de-ch": 308,
	"./de-ch.js": 308,
	"./de.js": 306,
	"./dv": 309,
	"./dv.js": 309,
	"./el": 310,
	"./el.js": 310,
	"./en-au": 311,
	"./en-au.js": 311,
	"./en-ca": 312,
	"./en-ca.js": 312,
	"./en-gb": 313,
	"./en-gb.js": 313,
	"./en-ie": 314,
	"./en-ie.js": 314,
	"./en-il": 315,
	"./en-il.js": 315,
	"./en-nz": 316,
	"./en-nz.js": 316,
	"./eo": 317,
	"./eo.js": 317,
	"./es": 318,
	"./es-do": 319,
	"./es-do.js": 319,
	"./es-us": 320,
	"./es-us.js": 320,
	"./es.js": 318,
	"./et": 321,
	"./et.js": 321,
	"./eu": 322,
	"./eu.js": 322,
	"./fa": 323,
	"./fa.js": 323,
	"./fi": 324,
	"./fi.js": 324,
	"./fo": 325,
	"./fo.js": 325,
	"./fr": 326,
	"./fr-ca": 327,
	"./fr-ca.js": 327,
	"./fr-ch": 328,
	"./fr-ch.js": 328,
	"./fr.js": 326,
	"./fy": 329,
	"./fy.js": 329,
	"./gd": 330,
	"./gd.js": 330,
	"./gl": 331,
	"./gl.js": 331,
	"./gom-latn": 332,
	"./gom-latn.js": 332,
	"./gu": 333,
	"./gu.js": 333,
	"./he": 334,
	"./he.js": 334,
	"./hi": 335,
	"./hi.js": 335,
	"./hr": 336,
	"./hr.js": 336,
	"./hu": 337,
	"./hu.js": 337,
	"./hy-am": 338,
	"./hy-am.js": 338,
	"./id": 339,
	"./id.js": 339,
	"./is": 340,
	"./is.js": 340,
	"./it": 341,
	"./it.js": 341,
	"./ja": 342,
	"./ja.js": 342,
	"./jv": 343,
	"./jv.js": 343,
	"./ka": 344,
	"./ka.js": 344,
	"./kk": 345,
	"./kk.js": 345,
	"./km": 346,
	"./km.js": 346,
	"./kn": 347,
	"./kn.js": 347,
	"./ko": 348,
	"./ko.js": 348,
	"./ky": 349,
	"./ky.js": 349,
	"./lb": 350,
	"./lb.js": 350,
	"./lo": 351,
	"./lo.js": 351,
	"./lt": 352,
	"./lt.js": 352,
	"./lv": 353,
	"./lv.js": 353,
	"./me": 354,
	"./me.js": 354,
	"./mi": 355,
	"./mi.js": 355,
	"./mk": 356,
	"./mk.js": 356,
	"./ml": 357,
	"./ml.js": 357,
	"./mn": 358,
	"./mn.js": 358,
	"./mr": 359,
	"./mr.js": 359,
	"./ms": 360,
	"./ms-my": 361,
	"./ms-my.js": 361,
	"./ms.js": 360,
	"./mt": 362,
	"./mt.js": 362,
	"./my": 363,
	"./my.js": 363,
	"./nb": 364,
	"./nb.js": 364,
	"./ne": 365,
	"./ne.js": 365,
	"./nl": 366,
	"./nl-be": 367,
	"./nl-be.js": 367,
	"./nl.js": 366,
	"./nn": 368,
	"./nn.js": 368,
	"./pa-in": 369,
	"./pa-in.js": 369,
	"./pl": 370,
	"./pl.js": 370,
	"./pt": 371,
	"./pt-br": 372,
	"./pt-br.js": 372,
	"./pt.js": 371,
	"./ro": 373,
	"./ro.js": 373,
	"./ru": 374,
	"./ru.js": 374,
	"./sd": 375,
	"./sd.js": 375,
	"./se": 376,
	"./se.js": 376,
	"./si": 377,
	"./si.js": 377,
	"./sk": 378,
	"./sk.js": 378,
	"./sl": 379,
	"./sl.js": 379,
	"./sq": 380,
	"./sq.js": 380,
	"./sr": 381,
	"./sr-cyrl": 382,
	"./sr-cyrl.js": 382,
	"./sr.js": 381,
	"./ss": 383,
	"./ss.js": 383,
	"./sv": 384,
	"./sv.js": 384,
	"./sw": 385,
	"./sw.js": 385,
	"./ta": 386,
	"./ta.js": 386,
	"./te": 387,
	"./te.js": 387,
	"./tet": 388,
	"./tet.js": 388,
	"./tg": 389,
	"./tg.js": 389,
	"./th": 390,
	"./th.js": 390,
	"./tl-ph": 391,
	"./tl-ph.js": 391,
	"./tlh": 392,
	"./tlh.js": 392,
	"./tr": 393,
	"./tr.js": 393,
	"./tzl": 394,
	"./tzl.js": 394,
	"./tzm": 395,
	"./tzm-latn": 396,
	"./tzm-latn.js": 396,
	"./tzm.js": 395,
	"./ug-cn": 397,
	"./ug-cn.js": 397,
	"./uk": 398,
	"./uk.js": 398,
	"./ur": 399,
	"./ur.js": 399,
	"./uz": 400,
	"./uz-latn": 401,
	"./uz-latn.js": 401,
	"./uz.js": 400,
	"./vi": 402,
	"./vi.js": 402,
	"./x-pseudo": 403,
	"./x-pseudo.js": 403,
	"./yo": 404,
	"./yo.js": 404,
	"./zh-cn": 405,
	"./zh-cn.js": 405,
	"./zh-hk": 406,
	"./zh-hk.js": 406,
	"./zh-tw": 407,
	"./zh-tw.js": 407
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
webpackContext.id = 758;

/***/ }),

/***/ 778:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DataChart; });
var DataChart = (function () {
    function DataChart(tab, champLabel, champValue) {
        var _this = this;
        this.values = [];
        this.labels = [];
        var value;
        var label;
        tab.forEach(function (element) {
            if (Reflect.get(element, champLabel)) {
                label = Reflect.get(element, champLabel);
                _this.labels.push(label);
            }
            if (Reflect.get(element, champValue)) {
                value = Reflect.get(element, champValue);
                _this.values.push(Reflect.get(element, champValue));
            }
        });
    }
    return DataChart;
}());

//# sourceMappingURL=datachart.js.map

/***/ }),

/***/ 779:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MenuSommaireComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__ = __webpack_require__(26);
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
        selector: 'menu-sommaire',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\menu-sommaire\menu-sommaire.html"*/'<!-- Generated template for the MenuSommaireComponent component -->\n<div>\n  <form #f="ngForm" class="list-form" (ngSubmit)="add()">\n    <ion-list>\n      <ion-item>\n        <ion-label>Date debut</ion-label>\n        <ion-datetime displayFormat="DD-MM-YYYY" pickerFormat="DD-MM-YYYY" value="" ng-model [(ngModel)]="dateDebut" name="dateDebut"\n          required>\n        </ion-datetime>\n      </ion-item>\n      <ion-item>\n        <ion-label>Date Fin</ion-label>\n        <ion-datetime displayFormat="DD-MM-YYYY" pickerFormat="DD-MM-YYYY" value="" ng-model [(ngModel)]="dateFin" name="dateFin"\n          required>\n        </ion-datetime>\n      </ion-item>\n      <button ion-button icon-start block color="dark" type="submit" [disabled]="!f.valid">\n        <ion-icon name="add"></ion-icon>\n      </button>\n    </ion-list>\n  </form>\n</div>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\menu-sommaire\menu-sommaire.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_0_ionic_angular__["NavController"]])
], MenuSommaireComponent);

//# sourceMappingURL=menu-sommaire.js.map

/***/ }),

/***/ 786:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyApp; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__providers_dashboard_service_dashboard_service__ = __webpack_require__(145);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__pages_dashboard_dashboard__ = __webpack_require__(274);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__pages_encaissements_encaissements__ = __webpack_require__(146);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__ionic_native_status_bar__ = __webpack_require__(417);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__ionic_native_splash_screen__ = __webpack_require__(419);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__ionic_native_keyboard__ = __webpack_require__(420);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__pages_home_home__ = __webpack_require__(149);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__pages_login_login__ = __webpack_require__(152);
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
    function MyApp(platform, statusBar, splashScreen, keyboard, helperService, alertCtrl, dashBoardService, app, events) {
        var _this = this;
        this.platform = platform;
        this.statusBar = statusBar;
        this.splashScreen = splashScreen;
        this.keyboard = keyboard;
        this.helperService = helperService;
        this.alertCtrl = alertCtrl;
        this.dashBoardService = dashBoardService;
        this.app = app;
        this.events = events;
        this.rootPage = __WEBPACK_IMPORTED_MODULE_10__pages_login_login__["a" /* LoginPage */];
        this.username = '';
        this.role = '';
        this.initializeApp();
        this.appMenuItems = [
            { title: 'Accueil', component: __WEBPACK_IMPORTED_MODULE_9__pages_home_home__["a" /* HomePage */], icon: 'home' },
            { title: 'Journal', component: __WEBPACK_IMPORTED_MODULE_3__pages_encaissements_encaissements__["a" /* EncaissementsPage */], icon: 'logo-usd' },
            { title: 'Analyse statistique', component: __WEBPACK_IMPORTED_MODULE_1__pages_dashboard_dashboard__["a" /* DashboardPage */], icon: 'stats' },
        ];
        this.events.subscribe('user:username', function (username) {
            console.log("we recieved username");
            _this.username = username;
        });
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
                            });
                            //*** Control Status Bar
                            _this.statusBar.styleDefault();
                            _this.statusBar.overlaysWebView(false);
                            //*** Control Keyboard
                            //this.keyboard.disableScroll(true);
                        })];
                    case 1:
                        _a.sent();
                        this.platform.registerBackButtonAction(function () {
                            // Catches the active view
                            var nav = _this.app.getActiveNavs()[0];
                            var activeView = nav.getActive();
                            // Checks if can go back before show up the alert
                            if (activeView.name === 'HomePage' || activeView.name === 'LoginPage') {
                                if (nav.canGoBack()) {
                                    nav.pop();
                                }
                                else {
                                    var alert_1 = _this.alertCtrl.create({
                                        title: 'App',
                                        message: 'Voulez vous quiter l\'application?',
                                        buttons: [{
                                                text: 'non',
                                                role: 'cancel',
                                                handler: function () {
                                                    _this.nav.setRoot('HomePage');
                                                }
                                            }, {
                                                text: 'oui',
                                                handler: function () {
                                                    _this.logout();
                                                    _this.platform.exitApp();
                                                }
                                            }]
                                    });
                                    alert_1.present();
                                }
                            }
                            else {
                                nav.pop();
                            }
                        });
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
        this.nav.setRoot(__WEBPACK_IMPORTED_MODULE_10__pages_login_login__["a" /* LoginPage */]);
    };
    return MyApp;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_4__angular_core__["ViewChild"])(__WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Nav"]),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Nav"])
], MyApp.prototype, "nav", void 0);
MyApp = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_4__angular_core__["Component"])({template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\app\app.html"*/'<ion-menu side="left" id="authenticated" [content]="content" >\n  <ion-header >\n    <ion-toolbar class="user-profile" >\n      <ion-grid >\n        <ion-row >\n          <ion-col padding-top col-8 >\n            <img src="assets/img/logo_v_tl.png">\n            <h3 ion-text class="no-margin bold text-white">\n              {{ username }}\n            </h3>\n          </ion-col>\n        </ion-row>       \n      </ion-grid>\n    </ion-toolbar>\n  </ion-header>\n  <ion-content color="primary" >\n    <ion-list class="user-list">\n      <button ion-item menuClose class="text-1x" *ngFor="let menuItem of appMenuItems" (click)="openPage(menuItem)">\n        <ion-icon item-left [name]="menuItem.icon" color="primary"></ion-icon>\n        <span ion-text color="primary">{{menuItem.title}}</span>\n      </button>\n    </ion-list>\n    <ion-footer no-shadow>\n      <button ion-button icon-left small full color="primary" menuClose (click)="logout()">\n        <ion-icon name="log-out"></ion-icon>\n        Deconnexion\n      </button>\n    </ion-footer> \n  </ion-content>\n</ion-menu>\n<ion-nav [root]="rootPage" #content swipeBackEnabled="false" ></ion-nav>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\app\app.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Platform"],
        __WEBPACK_IMPORTED_MODULE_6__ionic_native_status_bar__["a" /* StatusBar */],
        __WEBPACK_IMPORTED_MODULE_7__ionic_native_splash_screen__["a" /* SplashScreen */],
        __WEBPACK_IMPORTED_MODULE_8__ionic_native_keyboard__["a" /* Keyboard */],
        __WEBPACK_IMPORTED_MODULE_2__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["AlertController"],
        __WEBPACK_IMPORTED_MODULE_0__providers_dashboard_service_dashboard_service__["a" /* DashboardServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["App"],
        __WEBPACK_IMPORTED_MODULE_5_ionic_angular__["Events"]])
], MyApp);

//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 789:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SettingsPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__login_login__ = __webpack_require__(152);
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
        selector: 'page-settings',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\settings\settings.html"*/'<!-- -->\n<ion-header class="no-shadow">\n\n  <ion-navbar class="no-border">\n    <ion-title>\n      <ion-icon name="cog" class="text-primary"></ion-icon>\n      <span class="text-primary">Settings</span>\n    </ion-title>\n  </ion-navbar>\n\n</ion-header>\n\n<ion-content class="common-bg">\n  <!-- User settings-->\n  <ion-item-group>\n    <ion-item-divider color="secondary" class="bold">User Settings</ion-item-divider>\n    <ion-item>\n      <ion-label>Language</ion-label>\n      <ion-select  cancelText="Cancel" okText="OK">\n        <ion-option value="en-US" selected="true">English (US)</ion-option>\n        <ion-option value="en-GB">English (UK)</ion-option>\n        <ion-option value="en-CA">English (CA)</ion-option>\n        <ion-option value="en-AU">English (AU)</ion-option>\n        <ion-option value="en-IN">English (IN)</ion-option>\n        <ion-option value="pt-BR">Portuguese (BR)</ion-option>\n        <ion-option value="pt-PT">Portuguese (PT)</ion-option>\n        <ion-option value="es-ES">Spanish (ES)</ion-option>\n        <ion-option value="es-AR">Spanish (AR)</ion-option>\n        <ion-option value="es-CO">Spanish (CO)</ion-option>\n        <ion-option value="es-CL">Spanish (CL)</ion-option>\n        <ion-option value="es-MX">Spanish (MX)</ion-option>\n        <ion-option value="zh-CN">Chinese (CN)</ion-option>\n        <ion-option value="zh-TW">Chinese (TW)</ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label>Currency</ion-label>\n      <ion-select cancelText="Cancel" okText="OK">\n        <ion-option value="USD" selected="true">U.S Dollar (US$)</ion-option>\n        <ion-option value="EUR">Euro (€)</ion-option>\n        <ion-option value="GBP">Pound (£)</ion-option>\n        <ion-option value="BRL">Brazilian Real (R$)</ion-option>\n        <ion-option value="CNY">Chinese Yuan</ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label>Units</ion-label>\n      <ion-select  cancelText="Cancel" okText="OK">\n        <ion-option value="M" selected="true">Miles (ft²)</ion-option>\n        <ion-option value="K">Kilometers (m²)</ion-option>\n      </ion-select>\n    </ion-item>\n    <ion-item>\n      <ion-label>Notifications?</ion-label>\n      <ion-toggle checked="true"></ion-toggle>\n    </ion-item>\n  </ion-item-group>\n  <!-- App settings-->\n  <ion-item-group>\n    <ion-item-divider color="secondary" class="bold">App Settings</ion-item-divider>\n    <ion-item>\n      <span>Clear Private Data</span>\n    </ion-item>\n    <ion-item>\n      <ion-label>Push Notifications?</ion-label>\n      <ion-toggle checked="false"></ion-toggle>\n    </ion-item>\n    <ion-item>\n      <span>Privacy Policy</span>\n    </ion-item>\n  </ion-item-group>  \n\n  <!--sign out button-->\n  <button ion-button color="primary" full tappable (click)="logout()">LOG OUT</button>\n\n</ion-content>\n'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\settings\settings.html"*/
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1_ionic_angular__["NavController"]])
], SettingsPage);

//# sourceMappingURL=settings.js.map

/***/ }),

/***/ 791:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = HttpFactory;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__httpInterceptor__ = __webpack_require__(56);

function HttpFactory(xhrBackend, requestOptions, authService) {
    return new __WEBPACK_IMPORTED_MODULE_0__httpInterceptor__["a" /* HttpInterceptor */](xhrBackend, requestOptions, authService);
}
//# sourceMappingURL=httpFactory.js.map

/***/ }),

/***/ 792:
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
        selector: 'xpert-chart',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\xpert-chart\xpert-chart.html"*/'<!-- Generated template for the XpertChartComponent component -->\n<div style="display: block">\n <canvas baseChart   [datasets]="barChartData" [labels]="barChartLabels" [options]="barChartOptions" [legend]="barChartLegend"\n    [chartType]="barChartType" ></canvas>\n  <h1 color="black" text-center class="text-1x">\n    <strong>{{ title }}</strong>\n  </h1>\n</div>\n'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\xpert-chart\xpert-chart.html"*/
    }),
    __metadata("design:paramtypes", [])
], XpertChartComponent);

//# sourceMappingURL=xpert-chart.js.map

/***/ }),

/***/ 793:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EncaissComponent; });
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
 * Generated class for the EncaissComponent component.
 *
 * See https://angular.io/api/core/Component for more info on Angular
 * Components.
 */
var EncaissComponent = (function () {
    function EncaissComponent() {
    }
    return EncaissComponent;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])("encaissement"),
    __metadata("design:type", Array)
], EncaissComponent.prototype, "encaissement", void 0);
EncaissComponent = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'encaiss',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\encaiss\encaiss.html"*/'<!-- Generated template for the EncaissComponent component -->\n<ion-row>\n  <ion-col>\n      <span ion-text class="text-big" color="black">{{encaissement.DESIGN_MOTIF | titlecase}} </span>\n  </ion-col>\n  <ion-col float-right>\n      <strong ion-text class="text-big" color="danger" float-right>{{encaissement.TOTAL_ENCAISS |\n          xpertCurrency}}\n      </strong>\n  </ion-col>\n</ion-row>\n<ion-row>\n  <ion-col>\n      <strong class="text-sm">{{ encaissement.DESIGN_COMPTE | titlecase }} </strong>\n  </ion-col>\n  <ion-col float-right>\n      <strong class="text-sm" float-right>{{encaissement.DATE_ENCAISS | date : "dd-MM-y,HH:mm:ss"\n          }}</strong>\n  </ion-col>\n</ion-row>\n<ion-row>\n  <ion-col col-8>\n      <strong class="text-sm">{{encaissement.NOM_TIERS | titlecase}} </strong>\n  </ion-col>\n  <ion-col>\n      <strong ion-text class="text-sm" float-right>{{encaissement.CREATED_BY | titlecase}}</strong>\n  </ion-col>\n  <hr size=1 align="center" width="80%">\n</ion-row>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\components\encaiss\encaiss.html"*/
    }),
    __metadata("design:paramtypes", [])
], EncaissComponent);

//# sourceMappingURL=encaiss.js.map

/***/ }),

/***/ 91:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FormEncaissementPage; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__providers_encaiss_service_encaiss_service__ = __webpack_require__(51);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_ionic_angular__ = __webpack_require__(13);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__providers_helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__(23);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__pipes_xpert_currency_xpert_currency__ = __webpack_require__(411);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__providers_tiers_service_tiers_service__ = __webpack_require__(412);
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
    function FormEncaissementPage(navCtrl, navParams, encaisseService, helperService, montantInput, xpertPipe, tiersService) {
        this.navCtrl = navCtrl;
        this.navParams = navParams;
        this.encaisseService = encaisseService;
        this.helperService = helperService;
        this.montantInput = montantInput;
        this.xpertPipe = xpertPipe;
        this.tiersService = tiersService;
        this.update = false;
        this.method = "Ajouter";
        this.caissesList = [];
        this.dateEncaiss = new Date().toISOString();
        this.hours = new Date().getHours();
        this.localDate = new Date();
        this.encaissementsPage = this.navParams.get('encaissementsPage');
        this.el = this.montantInput.nativeElement;
    }
    FormEncaissementPage.prototype.ionViewDidLoad = function () {
    };
    FormEncaissementPage.prototype.ionViewWillEnter = function () {
    };
    FormEncaissementPage.prototype.setDate = function (date) {
        this.localDate = date;
        this.dateEncaiss = this.localDate.toISOString();
        this.dateEncaiss = this.localDate.toISOString();
    };
    FormEncaissementPage.prototype.getMotifs = function () {
        this.listMotifs = this.encaisseService.motifsList;
    };
    FormEncaissementPage.prototype.getTiers = function () {
        this.listTiers = this.encaisseService.tiersListForForm;
        this.tiers = this.encaisseService.tiersForm;
        console.log("tier list : ", this.listTiers);
    };
    FormEncaissementPage.prototype.getCaisses = function () {
        this.caissesList = this.encaisseService.caissesList;
    };
    FormEncaissementPage.prototype.goBack = function () {
        this.navCtrl.pop();
    };
    FormEncaissementPage.prototype.updateEncaissement = function () {
        var _this = this;
        this.encaisseService.updateEncaissement(this.encaissement.CODE_ENCAISS, this.dateEncaiss, this.codeCompte, this.totalEncaiss, this.codeType, this.motif.CODE_MOTIF, this.tiers.CODE_TIERS).subscribe(function (data) {
            if (_this.encaissementsPage) {
                _this.encaissementsPage.getEncaissementSPerPage();
            }
            _this.helperService.showNotifSuccess("l'encaissement a bien etait modifier");
            _this.navCtrl.pop();
        }, function (error) {
            _this.form.form.enable();
            _this.helperService.showNotifError(error);
        });
    };
    FormEncaissementPage.prototype.addEncaissement = function () {
        var _this = this;
        this.encaisseService.addEncaissement(this.dateEncaiss, this.codeCompte, this.totalEncaiss, this.codeType, this.motif.CODE_MOTIF, this.tiers.CODE_TIERS).subscribe(function (data) {
            if (_this.encaissementsPage)
                _this.encaissementsPage.getEncaissementSPerPage();
            _this.helperService.showNotifSuccess("l'encaissement a bien etait ajouter");
            _this.navCtrl.pop();
        }, function (error) {
            _this.form.form.enable();
            _this.helperService.showNotifError(error);
        });
    };
    FormEncaissementPage.prototype.ngOnInit = function () {
        /// test if we are in the update or add Encaissment
        this.update = this.navParams.get('update');
        this.initForm();
        if (this.update) {
            this.initUpdateForm();
        }
        else {
            this.codeType = this.navParams.get('type');
            this.tiers = {
                CODE_TIERS: "",
                NOM_TIERS: "",
                NOM_TIERS1: "",
                SOLDE_TIERS: 0
            };
            //this.initAddForm();
        }
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
        this.codeType = this.encaissement.CODE_TYPE;
        this.dateEncaiss = this.encaissement.DATE_ENCAISS;
        this.totalEncaiss = this.encaissement.TOTAL_ENCAISS;
        this.tiers = {
            CODE_TIERS: this.encaissement.CODE_TIERS,
            NOM_TIERS: this.encaissement.NOM_TIERS,
            NOM_TIERS1: this.encaissement.NOM_TIERS1,
            SOLDE_TIERS: this.encaissement.SOLDE_TIERS
        };
        this.codeCompte = this.encaissement.CODE_COMPTE;
        this.motif = {
            CODE_MOTIF: this.encaissement.CODE_MOTIF,
            DESIGN_MOTIF: this.encaissement.DESIGN_MOTIF
        };
        if (this.tiers.CODE_TIERS != "")
            this.getSoldeTiers(this.tiers.CODE_TIERS);
    };
    FormEncaissementPage.prototype.getSoldeTiers = function (codeTiers) {
        var _this = this;
        this.tiersService.getTiers(codeTiers).subscribe(function (data) {
            _this.tiers = data;
        }, function (error) {
            _this.helperService.showNotifError(error);
        });
    };
    FormEncaissementPage.prototype.initTitle = function () {
        if (this.codeType == 'ENC') {
            this.title = "Encaissement";
        }
        else {
            this.title = "Decaissement";
        }
    };
    FormEncaissementPage.prototype.updateAmount = function (event) {
        console.log("update ", this.xpertPipe.transform(event.target.value));
        event.target.value = this.xpertPipe.transform(event.target.value);
    };
    FormEncaissementPage.prototype.validate = function (f) {
        this.form.form.disable();
        console.log(this.form);
        if (this.update) {
            this.updateEncaissement();
        }
        else {
            this.addEncaissement();
        }
    };
    FormEncaissementPage.prototype.tiersChange = function (event) {
        console.log(this.tiers.SOLDE_TIERS);
    };
    return FormEncaissementPage;
}());
__decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('f'),
    __metadata("design:type", __WEBPACK_IMPORTED_MODULE_4__angular_forms__["NgForm"])
], FormEncaissementPage.prototype, "form", void 0);
FormEncaissementPage = __decorate([
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'page-form-encaissement',template:/*ion-inline-start:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\form-encaissement\form-encaissement.html"*/'<!--\n  Generated template for the EncaissementsPage page.\n\n  See http://ionicframework.com/docs/components/#navigation for more info on\n  Ionic pages and navigation.\n-->\n<ion-header>\n    <ion-navbar hideBackButton>\n        <ion-title>{{ method }} {{ title }}</ion-title>\n        <ion-buttons left>\n            <button ion-button (click)="goBack()">\n                <ion-icon name="arrow-back"></ion-icon>\n            </button>\n        </ion-buttons>\n    </ion-navbar>\n</ion-header>\n<ion-content padding>\n    <form #f="ngForm" (ngSubmit)="validate(encaissForm)">\n        <ion-list>\n            <ion-item>\n                <span ion-datepicker [cancelText]="Annuler" [modalOptions]="" [localeStrings]="{weekdays: [\'Dim\', \'Lun\', \'Mar\', \'Mer\', \'Jeu\', \'Vend\', \'Sam\'],\n                months: [\'Janvier\', \'Février\', \'Mars\', \'Avril\', \'Mai\', \'Juin\', \'Juillet\', \'Août\', \'Septembre\', \'Octobre\', \'Novembre\', \'Décembre\']}"\n                    (ionChanged)="setDate($event);" [(value)]="localDate" clear class="ScheduleDate">\n                    <span float-left>\n                        {{localDate | date : \'dd-MM-y\'}}\n                    </span>\n                    <ion-icon name="calendar" float-right></ion-icon>\n                </span>\n            </ion-item>\n            <ion-item>\n                <ion-label floating>Motif</ion-label>\n                <ionic-selectable item-content searchPlaceholder="motif" [focusSearchbar]="true" name="Motif"\n                    [(ngModel)]="motif" [items]="listMotifs" itemValueField="CODE_MOTIF" itemTextField="DESIGN_MOTIF"\n                    [canSearch]="true">\n                </ionic-selectable>\n            </ion-item>\n            <ion-item>\n                <ion-label floating>Compte</ion-label>\n                <ion-select name="codeCompte" [(ngModel)]="codeCompte" interface="popover" required>\n                    <ion-option *ngFor="let caisse of caissesList" [value]="caisse.CODE_COMPTE">\n                        {{ caisse.DESIGN_COMPTE }}\n                    </ion-option>\n                </ion-select>\n            </ion-item>\n            <ion-item>\n                <ion-label floating>Tiers</ion-label>\n                <ionic-selectable item-content searchPlaceholder="tiers" [focusSearchbar]="true" name="tiers"\n                    [(ngModel)]="tiers" [items]="listTiers" itemValueField="CODE_TIERS" itemTextField="NOM_TIERS"\n                    [canSearch]="true"  (onChange)="tiersChange($event)">\n                </ionic-selectable>\n            </ion-item>\n            \n            <ion-item>\n                <ion-input name="totalEncaiss" placeholder="veuillez saisir le montant" [(ngModel)]="totalEncaiss"></ion-input>\n            </ion-item>\n            <br>\n            <div text-center>\n                <button ion-button icon-start color="dark" type="submit" [disabled]="!f.valid">\n                    {{ method }}\n                </button>\n                <button ion-button icon-start color="dark" (click)="goBack()">\n                    Annuler\n                </button>\n            </div>\n            <br><br>\n            <div bottom>\n                Solde Tiers : <strong>{{ tiers.SOLDE_TIERS | xpertCurrency }}</strong>\n            </div>\n        </ion-list>\n    </form>\n</ion-content>'/*ion-inline-end:"C:\Users\PC\Desktop\XpertDatable datatable\src\pages\form-encaissement\form-encaissement.html"*/,
    }),
    __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavController"],
        __WEBPACK_IMPORTED_MODULE_2_ionic_angular__["NavParams"],
        __WEBPACK_IMPORTED_MODULE_1__providers_encaiss_service_encaiss_service__["a" /* EncaisseServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_3__providers_helper_service_helper_service__["a" /* HelperServiceProvider */],
        __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"],
        __WEBPACK_IMPORTED_MODULE_5__pipes_xpert_currency_xpert_currency__["a" /* XpertCurrencyPipe */],
        __WEBPACK_IMPORTED_MODULE_6__providers_tiers_service_tiers_service__["a" /* TiersServiceProvider */]])
], FormEncaissementPage);

//# sourceMappingURL=form-encaissement.js.map

/***/ }),

/***/ 93:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthServiceProvider; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__ionic_storage__ = __webpack_require__(75);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs__ = __webpack_require__(431);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__helper_service_helper_service__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__(144);
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
        this.BASE_URL = "api";
        this.TOKEN_URL = "token";
        this.TEST_URL = "test";
        this.ACCOUNT_DETAIL_URL = "getAccountDetail";
        this.GRANT_TYPE = "password";
        this.token = "token";
        this.TOKEN_KEY = 'token';
    }
    AuthServiceProvider.prototype.getAuthentification = function (username, password) {
        this.headers = new __WEBPACK_IMPORTED_MODULE_4__angular_http__["b" /* Headers */]();
        this.headers.append('Content-Type', 'application/json');
        return this.http.post(this.helperService.networkAddress + this.BASE_URL + "/" + this.TOKEN_URL, 'username=' + username + '&password=' + password + '&grant_type=' + this.GRANT_TYPE).do(this.helperService.logResponse)
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
                        console.log('Token set ', this.getToken());
                        return [4 /*yield*/, this.storage.set(this.TOKEN_KEY, this.token)];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    AuthServiceProvider.prototype.testConnexion = function () {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.TEST_URL))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(function (error) {
            console.log(error);
            return __WEBPACK_IMPORTED_MODULE_1_rxjs__["Observable"].throw(error.json().error || "Server error");
        });
    };
    AuthServiceProvider.prototype.getAccountDetail = function () {
        return this.http.get(this.helperService.createLink(this.BASE_URL, this.ACCOUNT_DETAIL_URL))
            .do(this.helperService.logResponse)
            .map(this.helperService.extractData)
            .catch(function (error) {
            console.log(error);
            return __WEBPACK_IMPORTED_MODULE_1_rxjs__["Observable"].throw(error.json().error || "Server error");
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

/***/ })

},[424]);
//# sourceMappingURL=main.js.map