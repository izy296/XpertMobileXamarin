import { HttpInterceptor } from '../Module/httpInterceptor';
import { DashboardServiceProvider } from '../providers/dashboard-service/dashboard-service';
import { DashboardPage } from '../pages/dashboard/dashboard';
import { EncaisseServiceProvider } from '../providers/encaiss-service/encaiss-service';
import { XpertCurrencyPipe } from '../pipes/xpert-currency/xpert-currency';
import { MenuSommaireComponent } from '../components/menu-sommaire/menu-sommaire';
import { FormEncaissementPage } from '../pages/form-encaissement/form-encaissement';
import { MenuEncaissementListComponent } from '../components/menu-encaissement-list/menu-encaissement-list';
import { EncaissementsPage } from '../pages/encaissements/encaissements';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
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
import { HttpModule, XHRBackend, RequestOptions } from "@angular/http";
import { SelectSearchableModule } from 'ionic-select-searchable';
import { DatePickerModule } from 'ionic3-datepicker';
import { HttpFactory } from '../Module/httpFactory';
import { XpertChartComponent } from '../components/xpert-chart/xpert-chart';
import { EncaissComponent } from '../components/encaiss/encaiss'
import { MenuFilterComponent } from '../components/menu-filter/menu-filter';
import { IonicSelectableModule } from 'ionic-selectable';
import { IonPullupModule } from 'ionic-pullup';
import { TiersServiceProvider } from '../providers/tiers-service/tiers-service';

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
    MenuEncaissementListComponent,
    FormEncaissementPage,
    MenuSommaireComponent,
    XpertCurrencyPipe,
    DashboardPage,
    XpertChartComponent,
    EncaissComponent,
    MenuFilterComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SelectSearchableModule,
    IonPullupModule,
    IonicModule.forRoot(MyApp, {
      scrollPadding: false,
      scrollAssist: true,
      autoFocusAssist: false
    }),
    IonicSelectableModule,
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
    MenuEncaissementListComponent,
    FormEncaissementPage,
    MenuSommaireComponent,
    DashboardPage,
    EncaissComponent,
    MenuFilterComponent
  ],
  providers: [
    {
      provide: HttpInterceptor,
      useFactory: HttpFactory,
      deps: [XHRBackend, RequestOptions, AuthServiceProvider]
    },
    StatusBar,
    SplashScreen,
    Keyboard,
    HelperServiceProvider,
    AuthServiceProvider,
    EncaisseServiceProvider,
    XpertCurrencyPipe,
    DashboardServiceProvider,
    HelperServiceProvider,
    TiersServiceProvider,
  ],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]

})

export class AppModule {
}
