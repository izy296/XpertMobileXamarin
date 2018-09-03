import { AuthServiceProvider } from './../providers/auth-service/auth-service';
import { InventairePage } from '../pages/inventaire/inventaire';
import { DashboardServiceProvider } from '../providers/dashboard-service/dashboard-service';
import { DashboardPage } from '../pages/dashboard/dashboard';
import { HelperServiceProvider } from '../providers/helper-service/helper-service';
import { EncaissementsPage } from '../pages/encaissements/encaissements';
import { Component, ViewChild } from "@angular/core";
import { Platform, Nav } from "ionic-angular";
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Keyboard } from '@ionic-native/keyboard';
import { HomePage } from "../pages/home/home";
import { LoginPage } from "../pages/login/login";
export interface MenuItem {
  title: string;
  component: any;
  icon: string;
}
@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;
  rootPage: any = LoginPage;
  appMenuItems: Array<MenuItem>;
  constructor(
    public platform: Platform,
    public statusBar: StatusBar,
    public splashScreen: SplashScreen,
    public keyboard: Keyboard,
    public helperService: HelperServiceProvider,
    public dashBoardService: DashboardServiceProvider

  ) {
    this.initializeApp();
    this.appMenuItems = [
      { title: 'Home', component: HomePage, icon: 'home' },
      { title: 'Encaissements', component: EncaissementsPage, icon: 'logo-usd' },
      { title: 'Dashboard', component: DashboardPage, icon: 'stats' },
      { title:'Inventaire',component:InventairePage,icon : 'clipboard'}

    ];
  }

  async initializeApp() {
    await this.platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      //*** Control Splash Screen
      //this.splashScreen.show();
      // this.splashScreen.hide();
      this.helperService.getNetworkAdress().then(() => {
        this.dashBoardService.initDashBoard();
      }
      );
      //*** Control Status Bar
      this.statusBar.styleDefault();
      this.statusBar.overlaysWebView(false);
      //*** Control Keyboard
      this.keyboard.disableScroll(true);
    });
  }

  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.nav.setRoot(page.component);
  }

  logout() {
    this.nav.setRoot(LoginPage);
  }

}
