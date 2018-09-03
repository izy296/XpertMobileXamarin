import { HttpInterceptor } from '../Module/httpInterceptor';
import { DashboardServiceProvider } from '../providers/dashboard-service/dashboard-service';
import { DashboardPage } from '../pages/dashboard/dashboard';
import { EncaisseServiceProvider } from '../providers/encaiss-service/encaiss-service';
import { XpertCurrencyPipe } from '../pipes/xpert-currency/xpert-currency';
import { MenuSommaireComponent } from '../components/menu-sommaire/menu-sommaire';
import { FormEncaissementPage } from '../pages/form-encaissement/form-encaissement';
import { MenuEncaissementListComponent } from '../components/menu-encaissement-list/menu-encaissement-list';
import { EncaissementPage } from '../pages/encaissements/encaissement/encaissement';
import { EncaissementsPage } from '../pages/encaissements/encaissements';
import { NgModule } from "@angular/core";
import { IonicApp, IonicModule } from "ionic-angular";
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { IonicStorageModule } from '@ionic/storage';
import { ChartsModule } from 'ng2-charts';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Keyboard } from '@ionic-native/keyboard';
import { MyApp } from "./app.component";
import { SettingsPage } from "../pages/settings/settings";
import { HomePage } from "../pages/home/home";
import { LoginPage } from "../pages/login/login";
import { HelperServiceProvider } from '../providers/helper-service/helper-service';
import { AuthServiceProvider } from '../providers/auth-service/auth-service';
import { HttpModule, XHRBackend, RequestOptions} from "@angular/http";
import { SelectSearchableModule } from 'ionic-select-searchable';
import { DatePickerModule } from 'ionic3-datepicker';
import { BarcodeScanner } from '@ionic-native/barcode-scanner';
import { InventairePage } from '../pages/inventaire/inventaire';
import { HttpFactory } from '../Module/httpFactory';
import { XpertChartComponent } from '../components/xpert-chart/xpert-chart';
// end import services
// end import services

// import pages
// end import pages

@NgModule({
  declarations: [
    MyApp,
    SettingsPage,
    HomePage,
    LoginPage,
    EncaissementsPage,
    EncaissementPage,
    MenuEncaissementListComponent,
    FormEncaissementPage,
    MenuSommaireComponent,
    XpertCurrencyPipe,
    DashboardPage,
    InventairePage,
    XpertChartComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SelectSearchableModule,
    IonicModule.forRoot(MyApp, {
      scrollPadding: false,
      scrollAssist: true,
      autoFocusAssist: false
    }),
    IonicStorageModule.forRoot({
      name: '__mydb',
      driverOrder: ['indexeddb', 'sqlite', 'websql']
    }),
    HttpModule,
    ChartsModule,
    XpertCurrencyPipe,
    DatePickerModule,
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    SettingsPage,
    HomePage,
    LoginPage,
    EncaissementsPage,
    EncaissementPage,
    MenuEncaissementListComponent,
    FormEncaissementPage,
    MenuSommaireComponent,
    DashboardPage,
    InventairePage
  ],
  providers: [
    {
      provide : HttpInterceptor,
      useFactory : HttpFactory,
      deps : [XHRBackend, RequestOptions,AuthServiceProvider]
    } ,
    BarcodeScanner,
    StatusBar,
    SplashScreen,
    Keyboard,
    HelperServiceProvider,
    AuthServiceProvider,
    EncaisseServiceProvider,
    XpertCurrencyPipe,
    DashboardServiceProvider,
    HelperServiceProvider,
  ]
})

export class AppModule {
}
